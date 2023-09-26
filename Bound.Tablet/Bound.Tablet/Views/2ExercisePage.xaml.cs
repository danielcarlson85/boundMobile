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
            Button clickedButton = (Button)sender; // Get the button that was clicked

            string weightToAdd = clickedButton.Text;
            viewModel.ButtonAddWeight_ClickedAsync(weightToAdd);
        }

        public async void ButtonRemoveWeight_Clicked(object sender, System.EventArgs e)
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

        private void Button_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}