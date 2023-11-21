using Microsoft.Azure.Devices;

namespace Bound.Tablet.Models
{
    public class IoTHubDevice
    {
        public Device AzureIoTHubDevice { get; set; } = new Device();

        public bool IsRunning { get; set; } = false;

        public string DeviceName { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;
    }
}

