using Bound.NFC;
using Bound.Tablet.Helpers;
using Bound.Tablet.Services;
using Bound.Tablet.Views;
using Devicemanager.API.Managers;
using Plugin.NFC;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        bool _eventsAlreadySubscribed = false;
        private IoTHubManager ioTHubManager;
        public const string MIME_TYPE = "application/com.companyname.Bound.NFC";
        public string BaseText = "Put your mobile phone on the NFC tag that you can find on the machine.";


        public MainPageViewModel()
        {
            MainPageTextLabel = BaseText;
            ioTHubManager = new IoTHubManager();
        }

        public void StartListeningForNFC()
        {
            if (_eventsAlreadySubscribed)
                return;

            _eventsAlreadySubscribed = true;
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            CrossNFC.Current.StartListening();
        }

        public void OnBackButtonPressed()
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
            CrossNFC.Current.StopListening();
        }

        static string machineNameFromTag = String.Empty;

        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            App.UpTime = 0;
            machineNameFromTag = tagInfo.Records.First().Message;
            App.User.DeviceData.MachineName = machineNameFromTag;

            MainPageTextLabel = ($"Connecting to device {machineNameFromTag}, please wait...");

            try
            {
                if (!App.IsOn)
                {
                    JWTHttpClient.SendDebugTextToTablet("[Current_OnMessageReceived] NFC recognized logging in user...");

                    App.IsOn = true;
                    
                    var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);
                    if (device.AzureIoTHubDevice.ConnectionState == Microsoft.Azure.Devices.DeviceConnectionState.Connected)
                    {
                        await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
                        JWTHttpClient.SendDebugTextToTablet("[Current_OnMessageReceived] 'restartDevice' sent to device.");

                        Thread.Sleep(1000);
                        await ioTHubManager.SendLoginTextToIoTHubDevice(App.User);
                        JWTHttpClient.SendDebugTextToTablet("[Current_OnMessageReceived] 'login text' sent to device.");

                        JWTHttpClient.SendUserInfoToTablet();
                        CacheHelpers.SaveCachedUser();

                        JWTHttpClient.SendDebugTextToTablet("[Current_OnMessageReceived] NFC user logged in.");


                        Application.Current.MainPage = new NavigationPage(new ExercisePage());
                    }
                    else
                    {
                        Debug.WriteLine("This device is not online");
                        //InitUITimer("This device is not online, connecting...", 5);
                    }
                }

                App.IsOn = false;

            }
            catch (System.Exception ex)
            {
                //InitUITimer($"Something happend, cannot connect to device, {ex.Message}", 5);
            }
        }

        private void InitUITimer(string text, int time)
        {
            MainPageTextLabel = text;

            System.Timers.Timer timer = new System.Timers.Timer(time * 1000);
            timer.Start();
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MainPageTextLabel = BaseText;
                    });
                    timer.Stop();
                };
        }



        public void ImageButtonContinue_Clicked()
        {
            CommonMethods.Vibrate();
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
            CrossNFC.Current.StopListening();
            Application.Current.MainPage = new MachinesPage();
        }
    }
}