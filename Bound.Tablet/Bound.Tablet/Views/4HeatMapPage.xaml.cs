using Bound.Tablet.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeatMapPage : ContentPage
    {
        public HeatMapPage()
        {
            InitializeComponent();
            BindingContext = new HeatMapPageViewModel();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        private void ButtonBack_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new MachinesPage();
        }

        private void ButtonShoulders_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new MachinesPage();
        }

        private void ButtonFeet_Clicked(object sender, System.EventArgs e)
        {
        }
    }
}