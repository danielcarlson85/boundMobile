using Bound.NFC;
using Bound.Tablet.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        private readonly SignInPageViewModel viewModel;

        public SignInPage()
        {
            App.UpTime = 0;
            InitializeComponent();
            viewModel = new SignInPageViewModel();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        protected override void OnAppearing()
        {

        }

        private async void ImageButtonBack_ClickedAsync(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            await viewModel.Authentication();
        }
    }
}