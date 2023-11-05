using Bound.NFC;
using Bound.Tablet.Models;
using Bound.Tablet.Services;
using Bound.Tablet.Views;
using Devicemanager.API.Managers;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class ExercisePageViewModel : BaseViewModel
    {
        readonly IoTHubManager ioTHubManager;
        IoTHubDevice device;
        System.Timers.Timer timer;
        int time = 5;
        string weightAsString = string.Empty;

        public ExercisePageViewModel()
        {
            ioTHubManager = new IoTHubManager();
            ResetPage();
            LabelText = "Please choose your weight.";


            InitStatusTask();
            InitCounterTimer();
        }

        public void ResetPage()
        {
            ImageCurrentMachine = App.User.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.User.DeviceData.MachineName;
            LabelDeviceStatus = Color.Red;
            LabelDeviceIsRunning = Color.Red;
            if (timer != null) timer.Stop();
        }

        public void InitStatusTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    device = await ioTHubManager.Get(App.User.DeviceData.MachineName);
                    if (device == null)
                    {
                        LabelDeviceIsRunning = Color.Red;
                        return;
                    }

                    if (device.AzureIoTHubDevice.ConnectionState == DeviceConnectionState.Connected)
                    {
                        LabelDeviceStatus = Color.GreenYellow;
                    }
                    else
                    {
                        LabelDeviceStatus = Color.Red;
                        LabelDeviceIsRunning = Color.Red;
                    }
                    await Task.Delay(1000);
                }
            });
        }

        internal async Task ButtonReset_Clicked()
        {
            ResetPage();
            await ioTHubManager.SendRestartTextToIoTHubDevice(App.User.DeviceData.MachineName);
            JWTHttpClient.ResetUserInfoToTablet();

            await ioTHubManager.SendTextToIoTHubDevice("online");
            hasBeenStarted = false;
            Application.Current.MainPage = new ExercisePage();
        }

        public void InitCounterTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += async (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                time--;
                Debug.WriteLine(time);

                LabelText = "Starting device in: " + time;

                if (time <= 0)
                {
                    time = 5;
                    Debug.WriteLine("Starting device");
                    timer.Stop();
                    var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

                    if (device == null)
                    {
                        LabelText = "This machine is not yet registred, please use another one.";
                        return;
                    }

                    LabelText = "Device started.";

                    App.User.Device = device;
                    JWTHttpClient.SendUserInfoToTablet();
                    hasBeenStarted = true;
                    await ioTHubManager.SendStartTextToIoTHubDevice(App.User);
                    
                }
            };
        }

        bool hasBeenStarted = false;


        public void ButtonAddWeight_Clicked(string weightToAdd)
        {
            timer.Stop();

            if (hasBeenStarted)
            {
                LabelWeight = "Device is already running";
                Debug.WriteLine("Device is already running");
                return;
            }

            CommonMethods.Vibrate();
            if (weightToAdd != "CE")
            {
                weightAsString += weightToAdd;
                var weight = long.Parse(weightAsString);
                App.User.DeviceData.Weight = weight;
                time = 5;
                timer.Start();
                LabelWeight = App.User.DeviceData.Weight.ToString() + " kg";
                Debug.WriteLine("Add " + LabelWeight.ToString());


            }
            else
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
                LabelText = "Please choose your weight.";
                LabelWeight = "0 kg";
                timer.Stop();
            }

            if (App.User.DeviceData.Weight > 300)
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
                LabelText = "Too much weight.";
                LabelWeight = "0 kg";
                timer.Stop();
                return;
            }

        }

        public void ButtonRemoveWeight_Clicked()
        {
            CommonMethods.Vibrate();

            Debug.WriteLine("remove");
            time = 0;
            timer.Start();
            App.User.DeviceData.Weight--;
            LabelWeight = App.User.DeviceData.Weight.ToString();

        }
    }
}