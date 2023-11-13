using Bound.Tablet.Helpers;
using Bound.Tablet.Models;
using Bound.Tablet.Services;
using Bound.Tablet.Views;
using System.Diagnostics;
using System.Timers;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User;

        public static Page CurrentPage;
        public static bool IsOn { get; internal set; }
        public static int UpTime = 0;

        public Timer timer = new Timer(1000);

        public App()
        {

            InitializeComponent();
            InitStartUpTimeTimer();
            JWTHttpClient.SendDebugTextToTablet("[App] StartListening for NFC started...");


            if (User == null)
            {
                if (CacheHelpers.GetCachedUserForLogin())
                {
                    MainPage = new MainPage();
                }
                else
                {
                    MainPage = new SignInPage();
                }
            }
            else
            {
                MainPage = CurrentPage;
            }

        }


        private void InitStartUpTimeTimer()
        {
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpTime++;

                    if (UpTime > 3000)
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
            timer.Start();
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("Sleep mode");
            timer.Stop();
            CurrentPage = MainPage;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            UpTime = 0;
            Debug.WriteLine("On resume");
            MainPage = CurrentPage;
            timer.Start();
        }
    }
}
