using Bound.NFC;
using Bound.Tablet.Models;
using Devicemanager.API.Managers;
using Microsoft.Azure.Devices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class ExercisePageViewModel : BaseViewModel
    {
        readonly IoTHubManager ioTHubManager;
        IoTHubDevice device;
        System.Timers.Timer timer;
        DateTime timeOfStart;

        public ExercisePageViewModel()
        {
            ioTHubManager = new IoTHubManager();
            ImageCurrentMachine = App.User.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.User.DeviceData.MachineName;
            LabelDeviceStatus = Color.Red;
            LabelDeviceIsRunning = Color.Red;

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

        int time = 0;

        public void InitCounterTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += async (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                time++;
                Debug.WriteLine(time);
                if (time >= 5)
                {

                    Debug.WriteLine("Send");
                    timer.Stop();
                    App.User.DeviceData.Device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

                    _ = await ioTHubManager.SendStartRequestToDevice(App.User);
                }
            };
        }

        public async Task ButtonAddWeight_Clicked()
        {
            time = 0;
            App.User.DeviceData.Weight++;
            LabelWeight = App.User.DeviceData.Weight;
            Debug.WriteLine("Add");
            timer.Start();

            //var time = GetTimeDifference(timeOfStart, DateTime.Now);

            //if (device.Device.ConnectionState == DeviceConnectionState.Connected && !device.IsRunning && time)
            //{
            //    timeOfStart = DateTime.Now;

            CommonMethods.Vibrate();
            //    if (device.Device.ConnectionState == DeviceConnectionState.Connected)
            //    {
            //        timer.Start();
            //        device.IsRunning = true;
            //    }
            //    Debug.WriteLine("Device started: " + App.User.DeviceData.MachineName);
            //    await ioTHubManager.SendStartRequestToDevice(App.User);
            //}
        }
        public async Task ButtonRemoveWeight_Clicked()
        {
            //if (device.Device.ConnectionState == DeviceConnectionState.Connected && device.IsRunning)
            //{
            CommonMethods.Vibrate();
            //    timer.Stop();
            //    device.IsRunning = false;
            //    LabelWeight = 0;
            //    LabelDeviceIsRunning = Color.Red;
            //    _ = await ioTHubManager.SendStopRequestToDevice(device);
            //    Debug.WriteLine("Device stopped: " + App.User.DeviceData.MachineName);
            //}
            Debug.WriteLine("remove");
            time = 0;
            App.User.DeviceData.Weight--;
            LabelWeight = App.User.DeviceData.Weight;
        }

        bool GetTimeDifference(DateTime start, DateTime stop)
        {
            TimeSpan timeDifference = stop - start;
            double seconds = timeDifference.TotalSeconds;

            if (seconds > 10)
            {
                return true;
            }

            return false;
        }
    }
}