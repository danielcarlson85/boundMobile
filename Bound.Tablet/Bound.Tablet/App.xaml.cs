using Autofac;
using Bound;
using Bound.Tablet.Models;
using Bound.Tablet.Services.Interfaces;
using Bound.Tablet.Views;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User = new User();

        public static DeviceData DeviceData = new DeviceData();

        public static Tokens Tokens = new Tokens();

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
                Tokens = JsonConvert.DeserializeObject<Tokens>(savedTokens);

                DeviceData = new DeviceData()
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
