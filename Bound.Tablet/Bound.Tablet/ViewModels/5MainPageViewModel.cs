using Bound.Tablet.Views;
using Bound.NFC;
using Plugin.NFC;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        bool _eventsAlreadySubscribed = false;
        public const string MIME_TYPE = "application/com.companyname.Bound.NFC";

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

        void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            var machineNameFromTag = tagInfo.Records.First();
            App.DeviceData.MachineName = machineNameFromTag.Message;
            Application.Current.MainPage = new ExercisePage();
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