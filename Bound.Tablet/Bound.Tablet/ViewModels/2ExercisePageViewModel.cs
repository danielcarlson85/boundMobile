using Bound.NFC;
using Bound.Tablet.Models;
using Bound.Tablet.Views;
using Devicemanager.API.Managers;
using Microsoft.Azure.Devices;
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
        System.Timers.Timer timer;

        public ExercisePageViewModel()
        {
            ioTHubManager = new IoTHubManager();
            ImageCurrentMachine = App.User.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.User.DeviceData.MachineName;
            LabelDeviceStatus = Color.Red;
            LabelDeviceIsRunning = Color.Red;

            InitStatusTask(true);
            //InitCounterTimer(false);
        }

        public void InitStatusTask(bool isRunning)
        {
            if (!isRunning)
            {
                return;
            }

            Task.Run(async () =>
            {
                while (isRunning)
                {
                    var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);
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

        int time = 5;

        //public void InitCounterTimer(bool isRunning)
        //{
        //    if (isRunning)
        //    {
        //        return;
        //    }

        //    Task.Run(async () =>
        //    {
        //        while (isRunning)
        //        {
        //            time--;
        //            Debug.WriteLine(time);

        //            LabelWeight = "Starting device " + time;

        //            if (time <= 0)
        //            {


    //            }
    //            await Task.Delay(1000);
    //        }
    //    });
    //}
    private CancellationTokenSource timerCancellationTokenSource;


        private async Task SendDataToDeviceAsync()
        {

            Debug.WriteLine("Send");
            var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

            if (device == null)
            {
                LabelWeight = "This machine is not yet registred, please use another one.";
                return;
            }

            LabelWeight = "Device started.";

            App.User.Device = device;

            _ = await ioTHubManager.SendStartRequestToDevice(App.User);
        }

        string weightAsString = string.Empty;

        public async Task ButtonAddWeight_ClickedAsync(string weightToAdd)
        {
            CommonMethods.Vibrate();

            if (weightToAdd != "CE")
            {
                weightAsString += weightToAdd;
                var weight = long.Parse(weightAsString);
                App.User.DeviceData.Weight = weight;

                LabelWeight = $"{App.User.DeviceData.Weight} kg";
                Debug.WriteLine("Add " + LabelWeight.ToString());

                timerCancellationTokenSource?.Cancel(); // Cancel the previous timer (if any)

                // Start a new timer
                timerCancellationTokenSource = new CancellationTokenSource();
                await Task.Delay(TimeSpan.FromSeconds(5), timerCancellationTokenSource.Token);

                if (!timerCancellationTokenSource.Token.IsCancellationRequested)
                {
                    await SendDataToDeviceAsync();
                }
            }
            else
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
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