using Autofac;
using Bound.Common.Helpers;
using Bound.Tablet.Interfaces;
using Bound.Tablet.Services.Interfaces;
using Bound.Tablet.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bound.Tablet.Views
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachinePage : ContentPage
    {
        private readonly IAuthenticationService _authenticationService;
        private MachinePageViewModel viewModel;

        public MachinePage()
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        var internet = InternetHelpers.CheckIfBoundBusinessIsOnline();

            //        if (!internet)
            //        {
            //            await DisplayAlert($"No internet connection found on you device", "Check you connection on your device", "Ok");
            //        }
            //    });


            var notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.ScheduleNotification("title", "test");

            InitializeComponent();

            _authenticationService = DependencyService.Get<IAuthenticationService>();


            BindingContext = viewModel = new MachinePageViewModel(_authenticationService);
        }
        protected async override void OnAppearing()
        {
            await viewModel.Authentication();

            //Application.Current.MainPage = new HeatMapPage();


            //var configuration = new MqttConfiguration();
            //_client = await MqttClient.CreateAsync(Constants.HostMqttBroker, configuration);
            //await _client.ConnectAsync();

            //await _client.SubscribeAsync(Constants.StartDeviceTopic, MqttQualityOfService.AtMostOnce);
            //_client.MessageStream.Subscribe(message => SendMessageToStartDeviceAsync(message));
        }

        //private void SendMessageToStartDeviceAsync(MqttApplicationMessage message)
        //{
        //    string messageAsString = Encoding.UTF8.GetString(message.Payload);

        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        await DisplayAlert($"Message recieved on topic: {message.Topic}", messageAsString, "Ok");
        //    });
        //}

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        private void ImageButtonNFC_Clicked(object sender, System.EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            App.DeviceData.MachineName = button.Source.ToString().Substring(6);

            Debug.WriteLine("Machine selected: " + App.DeviceData.MachineName);

            Application.Current.MainPage = new ExercisePage();
        }
    }
}