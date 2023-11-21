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
            LabelWeight = "0 kg";
        }

        internal async void RestartRPI_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("restartRPI");
            JWTHttpClient.SendDebugTextToTablet("[RestartRPI_Clicked] restartRPI");
            Application.Current.MainPage = new MainPage();
        }

        internal async void ShutdownRPI_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("shutdownRPI");
            JWTHttpClient.SendDebugTextToTablet("[ShutdownRPI_Clicked] shutdownRPI");
            Application.Current.MainPage = new MainPage();

        }

        internal async void ButtonRestartDevice_Clicked()
        {
            CommonMethods.Vibrate();
            await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
            JWTHttpClient.SendDebugTextToTablet("[ButtonRestartDevice_Clicked] restartDevice");
            Application.Current.MainPage = new MainPage();
        }



        internal async Task ButtonOK_Clicked()
        {
            CommonMethods.Vibrate();
            LabelText = "Starting excercise, please wait...";

            await ioTHubManager.SendStartTextToIoTHubDevice(App.User);


            JWTHttpClient.SendDebugTextToTablet("[ButtonOK_Clicked] Starting excercise, please wait...");
            App.User.DeviceData.Weight = 0;
            Thread.Sleep(3000);

            hasBeenStarted = true;
            JWTHttpClient.SendDebugTextToTablet("[ButtonOK_Clicked] User added weight and workout started.");

            hasBeenStarted = false;
            Application.Current.MainPage = new DonePage();

        }


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
            App.User.DeviceData.Weight--;
            LabelWeight = App.User.DeviceData.Weight.ToString();

        }
    }
}