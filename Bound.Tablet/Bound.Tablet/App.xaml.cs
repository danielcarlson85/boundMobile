using Bound.Tablet.Helpers;
using Bound.Tablet.Models;
using Bound.Tablet.Views;
using System.Diagnostics;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User = new User();

        public static bool IsOn { get; internal set; }
        public static int UpTime = 0;


        public App()
        {
            InitializeComponent();
            InitStartUpTimeTimer();

            if (CacheHelpers.GetCachedUserForLogin())
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new SignInPage();
            }
        }


        private void InitStartUpTimeTimer()
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpTime++;

                    if (UpTime > 60)
                    {
                        Current.MainPage = new MainPage();
                        UpTime = 0;
                    }

                    Debug.WriteLine(UpTime);
                });
            };

            timer.Start();
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
