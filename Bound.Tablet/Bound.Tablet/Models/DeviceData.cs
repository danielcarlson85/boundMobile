using Bound.Tablet.Models;
using Microsoft.Azure.Devices;
using System.Collections.Generic;

namespace Bound
{
    public class DeviceData
    {
        public string MachineName { get; set; }
        public string ObjectId { get; set; }

        public int Weight { get; set; }

        public IoTHubDevice Device { get; set; } = new IoTHubDevice();
    }
}
