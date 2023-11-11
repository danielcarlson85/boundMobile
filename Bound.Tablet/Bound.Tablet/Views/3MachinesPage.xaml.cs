using Bound.Tablet.ViewModels;
using Bound.NFC;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachinesPage : ContentPage
    {
        private readonly MachinePageViewModel viewModel;

        public MachinesPage()
        {
            App.UpTime = 0;
            InitializeComponent();

            viewModel = new MachinePageViewModel();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.ImageButton_Clicked(sender);
        }

        private void ImageButtonBack_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new MainPage();
        }
    }
}