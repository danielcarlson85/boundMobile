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

        public MainPageViewModel()
        {
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

            try
            {
                if (!App.IsOn)
                {
                    App.IsOn = true;
                    machineNameFromTag = tagInfo.Records.First().Message;

                    App.User.DeviceData.MachineName = machineNameFromTag;
                    var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);
                    if (device.AzureIoTHubDevice.ConnectionState == Microsoft.Azure.Devices.DeviceConnectionState.Connected)
                    {
                        await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
                        Thread.Sleep(1000);
                        await ioTHubManager.SendLoginTextToIoTHubDevice(App.User);

                        JWTHttpClient.SendUserInfoToTablet();
                        CacheHelpers.SaveCachedUser();

                        Application.Current.MainPage = new ExercisePage();
                    }
                    else
                    {
                        Debug.WriteLine("This device is not online");
                        await Application.Current.MainPage.DisplayAlert("This machine is not online ", "Machine not online", "OK");
                    }
                }

                App.IsOn = false;

            }
            catch (System.Exception)
            {
                await Application.Current.MainPage.DisplayAlert("This machine is not online, try again.", "Machine not online", "OK");
            }
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