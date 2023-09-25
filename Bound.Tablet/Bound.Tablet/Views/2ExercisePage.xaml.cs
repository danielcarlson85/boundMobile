using Bound.Tablet.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercisePage : ContentPage
    {
        readonly ExercisePageViewModel viewModel;

        public ExercisePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ExercisePageViewModel();
        }

        private async void ButtonAddWeight_Clicked(object sender, System.EventArgs e)
        {
            viewModel.ButtonAddWeight_Clicked();
        }

        public async void ButtonRemoveWeight_Clicked(object sender, System.EventArgs e)
        {
            viewModel.ButtonRemoveWeight_Clicked();
        }

        private void ImageButtonBack_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new MachinesPage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}