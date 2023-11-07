using Bound.NFC;
using Bound.Tablet.Models;
using Bound.Tablet.Services;
using Bound.Tablet.Views;
using Devicemanager.API.Managers;
using Microsoft.Azure.Devices;
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
        bool hasBeenStarted = false;


        public ExercisePageViewModel()
        {
            ioTHubManager = new IoTHubManager();
            LabelText = "Please choose your weight.";
            ImageCurrentMachine = App.User.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.User.DeviceData.MachineName;
            LabelDeviceStatus = Color.Red;
            LabelDeviceIsRunning = Color.Red;
            string weightAsString = string.Empty;
            App.User.DeviceData.Weight = 0;
            if (timer != null) timer.Stop();

            InitStatusTask();
            InitCounterTimer();
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

        internal async void RestartRPI_Clicked()
        {
            await ioTHubManager.SendTextToIoTHubDevice("restartRPI");
            JWTHttpClient.ResetUserInfoToTablet();
            Application.Current.MainPage = new MainPage();
        }

        internal async void ShutdownRPI_Clicked()
        {
            await ioTHubManager.SendTextToIoTHubDevice("shutdownRPI");
            JWTHttpClient.ResetUserInfoToTablet();
            Application.Current.MainPage = new MainPage();

        }

        internal async void ButtonRestartDevice_Clicked()
        {
            await ioTHubManager.SendTextToIoTHubDevice("restart");
            Application.Current.MainPage = new ExercisePage();
        }

        internal async Task ButtonReset_Clicked()
        {
            if (timer != null) timer.Stop();
            LabelText = "Resetting machine, please wait...";
            await ioTHubManager.SendTextToIoTHubDevice("restart");
            JWTHttpClient.ResetUserInfoToTablet();
            App.User.DeviceData.Weight = 0;
            Thread.Sleep(3000);
            LabelText = "Please choose your weight.";
            LabelWeight = "0 kg";
            weightAsString = string.Empty;
            await ioTHubManager.SendTextToIoTHubDevice("login");
            hasBeenStarted = false;
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
                    Debug.WriteLine("Device started...");
                    timer.Stop();
                    var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

                    if (device == null)
                    {
                        LabelText = "This machine is not yet registred, please use another one.";
                        return;
                    }

                    LabelText = "Device started...";

                    App.User.Device = device;
                    JWTHttpClient.SendUserInfoToTablet();
                    hasBeenStarted = true;
                    await ioTHubManager.SendStartTextToIoTHubDevice(App.User);

                }
            };
        }



        public async void ButtonAddWeight_Clicked(string weightToAdd)
        {
            timer.Stop();

            if (device.AzureIoTHubDevice.ConnectionState != DeviceConnectionState.Connected)
            {
                await Application.Current.MainPage.DisplayAlert("This machine is not online ", "Machine not online", "OK");
                timer.Stop();
                return;
            }

            if (hasBeenStarted)
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
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