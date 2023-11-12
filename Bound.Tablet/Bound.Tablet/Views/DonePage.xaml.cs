using Bound.NFC;
using Bound.Tablet.Services;
using Bound.Tablet.ViewModels;
using Devicemanager.API.Managers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonePage : ContentPage
    {
        public DonePage()
        {
            JWTHttpClient.SendDebugTextToTablet("Showing DonePage...");

            App.CurrentPage = this;
            App.UpTime = 0;
            InitializeComponent();
        }

        private void ButtonBack_Clicked(object sender, System.EventArgs e)
        {

        }

        private void ButtonShoulders_Clicked(object sender, System.EventArgs e)
        {

        }

        private void ButtonFeet_Clicked(object sender, System.EventArgs e)
        {
        }

        private async void buttonDone_Clicked(object sender, System.EventArgs e)
        {
            App.UpTime = 0;

            var ioTHubManager = new IoTHubManager();

            if (ExercisePageViewModel.timer != null) ExercisePageViewModel.timer.Stop();
            
            JWTHttpClient.SendDebugTextToTablet("User done exercising... restarting device");
            await ioTHubManager.SendTextToIoTHubDevice("restartDevice");
            JWTHttpClient.ResetUserInfoToTablet();
            App.User.DeviceData.Weight = 0;
            ExercisePageViewModel.weightAsString = string.Empty;
            ExercisePageViewModel.hasBeenStarted = false;

            Application.Current.MainPage = new ExercisePage();

        }
    }
}