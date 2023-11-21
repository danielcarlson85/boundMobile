﻿using Bound.NFC;
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
        //IoTHubDevice device;
        //public static System.Timers.Timer timer;
        public static string weightAsString = string.Empty;
        public static bool hasBeenStarted = false;


        public ExercisePageViewModel()
        {
            ioTHubManager = new IoTHubManager();
            LabelText = "Please choose your weight.";
            ImageCurrentMachine = App.User.DeviceData.MachineName;
            LabelMachineName = "Current machine: " + App.User.DeviceData.MachineName;
            LabelDeviceStatus = Color.Red;
            LabelDeviceIsRunning = Color.Red;
            App.User.DeviceData.Weight = 0;
            //if (timer != null) timer.Stop();

            //InitStatusTask();
            //InitCounterTimer();
        }

        //public void InitStatusTask()
        //{
        //    Task.Run(async () =>
        //    {
        //        while (true)
        //        {
        //            device = await ioTHubManager.Get(App.User.DeviceData.MachineName);
        //            if (device == null)
        //            {
        //                LabelDeviceIsRunning = Color.Red;
        //                return;
        //            }

        //            if (device.AzureIoTHubDevice.ConnectionState == DeviceConnectionState.Connected)
        //            {
        //                LabelDeviceStatus = Color.GreenYellow;
        //                LabelText = "Machine ready, please choose your weight...";
        //            }
        //            else
        //            {
        //                LabelDeviceStatus = Color.Red;
        //                LabelDeviceIsRunning = Color.Red;
        //                LabelText = "Waiting for machine to be ready...";
        //            }
        //            await Task.Delay(1000);
        //        }
        //    });
        //}

        internal async void RestartRPI_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("restartRPI");
            JWTHttpClient.ResetUserInfoToTablet("[RestartRPI_Clicked] restartRPI");
            Application.Current.MainPage = new MainPage();
        }

        internal async void ShutdownRPI_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("shutdownRPI");
            JWTHttpClient.ResetUserInfoToTablet("[ShutdownRPI_Clicked] shutdownRPI");
            Application.Current.MainPage = new MainPage();

        }

        internal async void ButtonRestartDevice_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
            JWTHttpClient.ResetUserInfoToTablet("[ButtonRestartDevice_Clicked] restartDevice");
            Application.Current.MainPage = new MainPage();
        }



        internal async Task ButtonOK_Clicked()
        {
            CommonMethods.Vibrate();

            await ioTHubManager.SendStartTextToIoTHubDevice(App.User);
            LabelText = "Starting excercise, please wait...";


            await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
            JWTHttpClient.ResetUserInfoToTablet("[ButtonOK_Clicked] Starting excercise, please wait...");
            App.User.DeviceData.Weight = 0;
            Thread.Sleep(3000);

            hasBeenStarted = true;
            JWTHttpClient.SendUserInfoToTablet();
            JWTHttpClient.SendDebugTextToTablet("[ButtonOK_Clicked] User added weight and workout started.");

            hasBeenStarted = false;
            Application.Current.MainPage = new DonePage();

        }

        //public void InitCounterTimer()
        //{
        //    //timer = new System.Timers.Timer(1000);
        //    //timer.Elapsed += async (object sender, System.Timers.ElapsedEventArgs e) =>
        //    {
        //        time--;
        //        Debug.WriteLine(time);

        //        LabelWeight = "Starting device in: " + time;

        //        if (time <= 0)
        //        {
        //            time = 5;
        //            Debug.WriteLine("Device started...");
        //            timer.Stop();
        //            var device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

        //            if (device == null)
        //            {
        //                LabelWeight = "This machine is not yet registred, please use another one.";
        //                return;
        //            }

        //            App.User.Device = device;
        //            hasBeenStarted = true;
        //            await ioTHubManager.SendStartTextToIoTHubDevice(App.User);
        //            JWTHttpClient.SendUserInfoToTablet();
        //            JWTHttpClient.SendDebugTextToTablet("[InitCounterTimer] User added weight and workout started.");


        //            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
        //            {
        //                // Your UI update code here
        //                // For example, updating a label text
        //                Application.Current.MainPage = new DonePage();
        //            });
        //        }
        //    };
        //}



        public async Task ButtonAddWeight_ClickedAsync(string weightToAdd)
        {
            CommonMethods.Vibrate();
            JWTHttpClient.SendDebugTextToTablet("[ButtonAddWeight_Clicked] Button AddWeight_clicked: " + weightToAdd);
            if (App.User.Device == null)
            {
                App.User.Device = await ioTHubManager.Get(App.User.DeviceData.MachineName);

            }

            if (weightToAdd != "CE")
            {
                weightAsString += weightToAdd;
                var weight = int.Parse(weightAsString);
                App.User.DeviceData.Weight = weight;
                LabelWeight = App.User.DeviceData.Weight.ToString() + " kg";
                Debug.WriteLine("Add " + LabelWeight.ToString());
            }
            else
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
                LabelText = "Please choose your weight.";
                LabelWeight = "0 kg";
            }

            if (App.User.DeviceData.Weight > 300)
            {
                App.User.DeviceData.Weight = 0;
                weightAsString = string.Empty;
                LabelText = "Too much weight.";
                LabelWeight = "0 kg";
                return;
            }
        }

        public void ButtonRemoveWeight_Clicked()
        {
            CommonMethods.Vibrate();

            Debug.WriteLine("remove");
            ////timer.Start();
            App.User.DeviceData.Weight--;
            LabelWeight = App.User.DeviceData.Weight.ToString();

        }
    }
}