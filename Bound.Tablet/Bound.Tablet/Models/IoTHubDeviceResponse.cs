using Microsoft.Azure.Devices;

namespace Bound.Tablet.Models
{
    public class IoTHubDevice
    {
        public Device Device { get; set; }

        public string DeviceName { get; set; }

        public string Value { get; set; }

        public string ConnectionString { get; set; }
    }
}

