// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Bound.Tablet.Models;
using Microsoft.Azure.Devices.Shared;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Devicemanager.API.Interfaces
{
    public interface IIoTHubManager
    {
        Task<IoTHubDevice> Create(string deviceName);

        Task<IoTHubDevice> Get(string deviceId);

        Task<List<IoTHubDevice>> GetAll();

        Task<List<Twin>> GetAllIoTDevices();

        Task SendDataToIoTHubDevice(IoTHubDevice ioTHubDevice, string messageToSend);

        Task<HttpStatusCode> SendStartRequestToDevice(IoTHubDevice deviceName);
        Task<HttpStatusCode> SendStopRequestToDevice(IoTHubDevice deviceName);
    }
}