using Android.App;
using Android.Content;
using Android.OS;
using Bound.NFC;
using Bound.Tablet.Settings;
using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Threading.Tasks;

namespace Bound.Tablet.Services
{
    [Service]
    public class MyBackgroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(Constants.ioTHubConnectionString, "Mobile");
            var lastReceivedMessage = string.Empty;

            Task.Run(async () =>
            {
                while (true)
                {
                    Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage != null)
                    {
                        string receivedData = Encoding.UTF8.GetString(receivedMessage.GetBytes());
                        if (receivedMessage.Properties.ContainsKey("user") && receivedMessage.Properties["user"] == App.User.ObjectId)
                        {
                            System.Diagnostics.Debug.WriteLine(receivedData);
                        }
                        await deviceClient.CompleteAsync(receivedMessage);
                    }
                }
            });

            return StartCommandResult.Sticky;

        }
    }
}
