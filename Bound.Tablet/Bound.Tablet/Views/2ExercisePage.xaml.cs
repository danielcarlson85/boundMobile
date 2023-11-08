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

        private void ButtonAddWeight_Clicked(object sender, System.EventArgs e)
        {
            Button clickedButton = (Button)sender; // Get the button that was clicked

            string weightToAdd = clickedButton.Text;
            viewModel.ButtonAddWeight_Clicked(weightToAdd);
        }

        public void ButtonRemoveWeight_Clicked(object sender, System.EventArgs e)
        {
            Button clickedButton = (Button)sender; // Get the button that was clicked

            string buttonText = clickedButton.Text;
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

        private async void ButtonReset_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.ButtonReset_Clicked();

        }

        private void ButtonRestartDevice_Clicked(object sender, System.EventArgs e)
        {
            if (!App.IsOn)
            {
                App.IsOn = true;
                viewModel.ButtonRestartDevice_Clicked();
            }
        }

        private void ShutdownRPI_Clicked(object sender, System.EventArgs e)
        {
            viewModel.ShutdownRPI_Clicked();
        }

        private void RestartRPI_Clicked(object sender, System.EventArgs e)
        {
            viewModel.RestartRPI_Clicked();
        }
    }
}