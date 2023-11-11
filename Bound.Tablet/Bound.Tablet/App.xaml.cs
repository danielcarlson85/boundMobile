using Bound.Tablet.Helpers;
using Bound.Tablet.Models;
using Bound.Tablet.Views;
using System.Diagnostics;
using System.Timers;
using Xamarin.Forms;

namespace Bound.NFC
{
    public partial class App : Application
    {
        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

        public static User User = new User();

        public static bool IsOn { get; internal set; }
        public static int UpTime = 0;

        public Timer timer = new Timer(1000);

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
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpTime++;

                    if (UpTime > 300)
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
            Debug.WriteLine("Sleep mode");
            timer.Stop();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            UpTime = 0;
            Debug.WriteLine("On resume");
            timer.Start();
            // Handle when your app resumes
        }
    }
}
