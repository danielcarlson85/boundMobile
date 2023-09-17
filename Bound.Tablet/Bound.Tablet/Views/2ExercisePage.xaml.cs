using Bound.NFC;
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

        private async void ButtonStart_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.ButtonStart_Clicked();
        }

        public async void ButtonStop_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.ButtonStop_Clicked();
        }

        private void ImageButtonBack_Clicked(object sender, System.EventArgs e)
        {
            CommonMethods.Vibrate();
            Application.Current.MainPage = new MachinesPage();
        }

        protected override async void OnDisappearing()
        {
            await viewModel.ButtonStop_Clicked();
            base.OnDisappearing();
        }
    }
}