using Bound.Tablet.Helpers;
using Bound.Tablet.Models;
using Bound.Tablet.Views;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User = new User();

        public static bool IsOn { get; internal set; }

        public App()
        {
            InitializeComponent();

            if (CacheHelpers.GetCachedUserForLogin())
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new SignInPage();
            }
        }



        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
