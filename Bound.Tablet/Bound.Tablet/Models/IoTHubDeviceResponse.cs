using Microsoft.Azure.Devices;

namespace Bound.Tablet.Models
{
    public class IoTHubDevice
    {
        public Device AzureIoTHubDevice { get; set; } = new Device();

        public bool IsRunning { get; set; }

        public string DeviceName { get; set; }

        public string ConnectionString { get; set; }
    }
}

