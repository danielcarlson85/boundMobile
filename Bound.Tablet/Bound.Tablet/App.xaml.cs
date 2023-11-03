using Bound.Tablet.Models;
using Bound.Tablet.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User = new User();


        public App()
        {
            InitializeComponent();

            if (GetCachedUser())
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new SignInPage();
            }
        }


        private bool GetCachedUser()
        {
            Debug.WriteLine("Authentication started");

            var savedUser = Xamarin.Essentials.Preferences.Get("user", "");
            var savedTokens = Xamarin.Essentials.Preferences.Get("tokens", "");

            if (!string.IsNullOrEmpty(savedUser) && !string.IsNullOrEmpty(savedTokens))
            {
                User = JsonConvert.DeserializeObject<User>(savedUser);
                User.Tokens = JsonConvert.DeserializeObject<Tokens>(savedTokens);

                User.DeviceData = new DeviceData()
                {
                    ObjectId = User.ObjectId
                };

                Debug.WriteLine("User found on device");

                return true;
            }

            return false;
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
