using Bound.NFC;
using Plugin.NFC;
using System.Linq;
using Xamarin.Forms;

namespace Bound.Tablet.Views
{
    public partial class NFCSettingsPage : ContentPage
    {

        bool _eventsAlreadySubscribed = false;
        public const string MIME_TYPE = "application/com.companyname.Bound.NFC";

        public NFCSettingsPage()
        {
            App.CurrentPage = this;
            App.UpTime = 0;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_eventsAlreadySubscribed)
                return;

            _eventsAlreadySubscribed = true;
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            CrossNFC.Current.StartListening();

            DisplayAlert("Tap", "Tap the tag you want to rename with your mobile phone.", "OK");

        }

        protected override bool OnBackButtonPressed()
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
            CrossNFC.Current.StopListening();

            return base.OnBackButtonPressed();
        }

        void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            var machineNameFromTag = tagInfo.Records.First();
            App.User.DeviceData.MachineName = machineNameFromTag.Message;
        }

        private void ButtonSetNFCTagName_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var record = new NFCNdefRecord
                {
                    TypeFormat = NFCNdefTypeFormat.WellKnown,
                    MimeType = MIME_TYPE,
                    Payload = NFCUtils.EncodeToByteArray(EntryNFCTagName.Text),
                    LanguageCode = "en"
                };

                var tagInfo = new TagInfo();

                tagInfo.Records = new[] { record };

                CrossNFC.Current.PublishMessage(tagInfo, false);
                DisplayAlert("Name", "Tag name set to: " + EntryNFCTagName.Text, "OK");

                Application.Current.MainPage = new MainPage();
            }
            catch (System.Exception)
            {
                DisplayAlert("test", "Put you mobile on the tag before you press the button", "OK");
            }
        }
    }
}
