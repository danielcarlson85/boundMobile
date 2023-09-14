// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace Devicemanager.API.Dtos.IoTHubDevices
{
    public class IoTHubDevicesResponse
    {
        public Device Device { get; set; }

        public List<Twin> Devices { get; set; }

        public string DeviceName { get; set; }

        public string Value { get; set; }

        public string ConnectionString { get; set; }
    }
}
