using Bound.Tablet;
using Bound.Tablet.ViewModels;
using Bound.Tablet.Views;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.StartListeningForNFC();
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.OnBackButtonPressed();
            return base.OnBackButtonPressed();
        }

        private void ImageButtonContinue_Clicked(object sender, System.EventArgs e)
        {
            viewModel.ImageButtonContinue_Clicked();
        }

        private void ImageButtonHeatmap_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new HeatMapPage();
        }

        private void ImageButtonNFCSettings_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new NFCSettingsPage();
        }
    }
}
