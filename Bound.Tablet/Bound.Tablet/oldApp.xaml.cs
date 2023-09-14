//using Autofac;
//using Bound.Tablet.Models;
//using Bound.Tablet.Services;
//using Bound.Tablet.Services.Interfaces;
//using Bound.Tablet.Views;
//using Xamarin.Forms;

//namespace Bound.Tablet
//{
//    public partial class App : Application
//    {
//        public static string IoTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R9Vrz6beac9GeyTqYtL+e9YqklvZ4GPpRPmGejPzkdA=";

//        public static User User = new User();

//        public static DeviceData DeviceData = new DeviceData();

//        public static Tokens Tokens = new Tokens();

//        public App()
//        {
//            InitializeComponent();

//            var builder = new ContainerBuilder();
//            builder.RegisterInstance<IAuthenticationService>(new AuthenticationService()).SingleInstance();

//            //DependencyService.Get<INotification>().Send("App.xaml.cs", "Welcome to Bound Mobile App :-D");
//            //DependencyService.Get<INotificationManager>().Initialize();


//            MainPage = new MachinesPage();
//        }


//        protected override void OnStart()
//        {
//        }

//        protected override void OnSleep()
//        {
//        }

//        protected override void OnResume()
//        {
//        }
//    }
//}
