using Android.Content;
using Bound.NFC;
using Bound.Tablet.Services;
using Bound.Tablet.Views;
using Devicemanager.API.Managers;
using Plugin.NFC;
using System.Diagnostics;
using System.Linq;
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

        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            var machineNameFromTag = tagInfo.Records.First();
            if (string.IsNullOrEmpty(App.User.DeviceData.MachineName))
            {
                App.User.DeviceData.MachineName = machineNameFromTag.Message;
                await ioTHubManager.SendTextToIoTHubDevice("online");
                JWTHttpClient.SendUserInfoToTablet();
                Application.Current.MainPage = new ExercisePage();
            }
            else
            {
                Debug.WriteLine("User already logged in");
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