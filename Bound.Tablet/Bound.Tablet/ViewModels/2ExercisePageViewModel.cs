using Bound.NFC;
using Bound.Tablet.Models;
using Bound.Tablet.Views;
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
        string weightAsString = string.Empty;

        public void ButtonAddWeight_Clicked(string weightToAdd)
        {
            if (weightToAdd != "CE")
            {
                weightAsString += weightToAdd;
                var weight = long.Parse(weightAsString);
                App.User.DeviceData.Weight = weight;
            }
            else
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
            }

            LabelWeight = App.User.DeviceData.Weight;

            Debug.WriteLine("Add " + LabelWeight.ToString());

            time = 0;
            timer.Start();

            CommonMethods.Vibrate();
        }

        public void ButtonRemoveWeight_Clicked()
        {
            CommonMethods.Vibrate();

            Debug.WriteLine("remove");
            time = 0;
            timer.Start();
            App.User.DeviceData.Weight--;
            LabelWeight = App.User.DeviceData.Weight;
        }
    }
}