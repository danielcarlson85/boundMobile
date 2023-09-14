// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Microsoft.Azure.Devices;

namespace Devicemanager.API.Dtos.IoTHubDevice
{
    public class IoTHubDeviceResponse
    {
        public Device Device { get; set; }

        public string DeviceName { get; set; }

        public string Value { get; set; }

        public string ConnectionString { get; set; }
    }
}
