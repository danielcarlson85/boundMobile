using Bound.Tablet.Views;
using Bound.NFC;
using System.Diagnostics;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class MachinePageViewModel : BaseViewModel
    {
        public MachinePageViewModel()
        {

        }

        public void ImageButton_Clicked(object sender)
        {
            CommonMethods.Vibrate();
            ImageButton button = (ImageButton)sender;
            App.User.DeviceData.MachineName = button.Source.ToString().Substring(6);

            Debug.WriteLine("Machine selected: " + App.User.DeviceData.MachineName);

            Application.Current.MainPage = new ExercisePage();
        }
    }
}