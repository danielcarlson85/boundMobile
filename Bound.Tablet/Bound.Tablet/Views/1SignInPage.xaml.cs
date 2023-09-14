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
            InitializeComponent();
            viewModel = new SignInPageViewModel();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        protected async override void OnAppearing()
        {

        }

        private async void ImageButtonBack_ClickedAsync(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            await viewModel.Authentication();
        }
    }
}