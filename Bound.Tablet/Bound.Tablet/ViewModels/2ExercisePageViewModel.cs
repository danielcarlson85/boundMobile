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
            ImageCurrentMachine = App.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.DeviceData.MachineName;
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
                    device = await ioTHubManager.Get(App.DeviceData.MachineName);
                    if (device.Device.ConnectionState == DeviceConnectionState.Connected)
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

        public void InitCounterTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                if (device.Device.ConnectionState == DeviceConnectionState.Connected && device.IsRunning)
                {
                    LabelTime++;
                    LabelDeviceIsRunning = Color.GreenYellow;
                }
                else
                {
                    LabelTime = 0;
                    LabelDeviceIsRunning = Color.Red;
                }
            };
        }

        public async Task ButtonStart_Clicked()
        {
            var time = GetTimeDifference(timeOfStart, DateTime.Now);

            if (device.Device.ConnectionState == DeviceConnectionState.Connected && !device.IsRunning && time)
            {
                timeOfStart = DateTime.Now;

                CommonMethods.Vibrate();
                if (device.Device.ConnectionState == DeviceConnectionState.Connected)
                {
                    timer.Start();
                    device.IsRunning = true;
                }
                Debug.WriteLine("Device started: " + App.DeviceData.MachineName);
                await ioTHubManager.SendStartRequestToDevice(device);
            }
        }
        public async Task ButtonStop_Clicked()
        {
            if (device.Device.ConnectionState == DeviceConnectionState.Connected && device.IsRunning)
            {
                CommonMethods.Vibrate();
                timer.Stop();
                device.IsRunning = false;
                LabelTime = 0;
                LabelDeviceIsRunning = Color.Red;
                _ = await ioTHubManager.SendStopRequestToDevice(device);
                Debug.WriteLine("Device stopped: " + App.DeviceData.MachineName);
            }
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