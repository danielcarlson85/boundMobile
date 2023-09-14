using Bound.Tablet.Models;
using Devicemanager.API.Managers;
using Microsoft.Azure.Devices;
using Bound.NFC;
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
                if (device.Device.ConnectionState == DeviceConnectionState.Connected)
                {
                    LabelTime++;
                }
                else
                {
                    LabelTime = 0;
                }
            };
        }

        public async Task ButtonStart_Clicked()
        {
            CommonMethods.Vibrate();
            device = await ioTHubManager.Get(App.DeviceData.MachineName);
            if (device.Device.ConnectionState == DeviceConnectionState.Connected)
            {
                timer.Start();
                LabelDeviceIsRunning = Color.GreenYellow;
            }
            Debug.WriteLine("Device started: " + App.DeviceData.MachineName);
            await ioTHubManager.SendStartRequestToDevice(device);
        }
        public async Task ButtonStop_Clicked()
        {
            CommonMethods.Vibrate();
            timer.Stop();
            LabelTime = 0;
            device = await ioTHubManager.Get(App.DeviceData.MachineName);
            LabelDeviceIsRunning = Color.Red;
            _ = await ioTHubManager.SendStopRequestToDevice(device);
            Debug.WriteLine("Device stopped: " + App.DeviceData.MachineName);
        }
    }
}