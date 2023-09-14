// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Devicemanager.API.Dtos.IoTHubDevice;
using Devicemanager.API.Interfaces;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Device = Microsoft.Azure.Devices.Device;
using IotHubConnectionStringBuilder = Microsoft.Azure.Devices.Client.IotHubConnectionStringBuilder;
using Message = Microsoft.Azure.Devices.Client.Message;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Devicemanager.API.Managers
{
    public class IoTHubManager
    {
        public IoTHubManager(string ioTHubConnectionString)
        {
            this.IoTHubConnectionString = ioTHubConnectionString;
            this.RegistryManager = RegistryManager.CreateFromConnectionString(ioTHubConnectionString);
        }

        private RegistryManager RegistryManager { get; set; }

        private static ServiceClient ServiceClient { get; set; }

        private string IoTHubConnectionString { get; }

        private IotHubConnectionStringBuilder IotHubConnectionStringBuilder { get; set; }

        public async Task<IoTHubDeviceResponse> Get(string deviceId)
        {
            Device device = await this.RegistryManager.GetDeviceAsync(deviceId);

            if (device == null)
            {
                throw new ArgumentException("IoThub device with that name does not exist");
            }

            var ioTHubDevice = this.CreateIoTHubDeviceObject(deviceId, device);
            return ioTHubDevice;
        }

        private IoTHubDeviceResponse CreateIoTHubDeviceObject(string deviceId, Device device)
        {
            var ioTHubDeviceAsObject = new IoTHubDeviceResponse();
            this.IotHubConnectionStringBuilder = IotHubConnectionStringBuilder.Create(this.IoTHubConnectionString + ";DeviceId=" + deviceId);
            ioTHubDeviceAsObject.Device = device;
            ioTHubDeviceAsObject.DeviceName = device.Id;
            ioTHubDeviceAsObject.ConnectionString = "HostName=" + this.IotHubConnectionStringBuilder.HostName + ";DeviceId=" + deviceId + ";SharedAccessKey=" + device.Authentication.SymmetricKey.PrimaryKey;
            return ioTHubDeviceAsObject;
        }

        public async Task<List<Twin>> GetAllIoTDevices()
        {
            IQuery searchForAllDevices = this.RegistryManager.CreateQuery("Select * from devices");
            IEnumerable<Twin> allDevices = await searchForAllDevices.GetNextAsTwinAsync();

            return allDevices.ToList();
        }

        public async Task<IoTHubDeviceResponse> Create(string deviceName)
        {
            Device newIoTHubDevice;

            try
            {
                newIoTHubDevice = await this.RegistryManager.AddDeviceAsync(new Device(deviceName));
            }
            catch
            {
                throw new ArgumentException("IoTHubDevice with that name already exists");
            }

            var ioTHubDevice = this.CreateIoTHubDeviceObject(deviceName, newIoTHubDevice);

            return ioTHubDevice;
        }

        public async Task SendDataToIoTHubDevice(IoTHubDeviceResponse ioTHubDevice, string messageToSend)
        {
            using (DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(ioTHubDevice.ConnectionString, TransportType.Mqtt))
            {
                var message = new Message(Encoding.ASCII.GetBytes(messageToSend));
                message.Properties.Add("Device", ioTHubDevice.DeviceName);
                await deviceClient.SendEventAsync(message);
            }

            Console.WriteLine("Data sent to iothubdevice");
        }

        public async Task<HttpStatusCode> SendStartRequestToDevice(IoTHubDeviceResponse deviceName)
        {
            CloudToDeviceMethodResult result;

            try
            {
                ServiceClient = ServiceClient.CreateFromConnectionString(this.IoTHubConnectionString);

                var methodInvocation = new CloudToDeviceMethod("Start") { ResponseTimeout = TimeSpan.FromSeconds(30) };
                methodInvocation.SetPayloadJson("10");

                result = await ServiceClient.InvokeDeviceMethodAsync(deviceName.DeviceName, methodInvocation);
            }
            catch (Exception)
            {
                return HttpStatusCode.NotFound;
            }

            return (HttpStatusCode)result.Status;
        }

        public Task<List<IoTHubDeviceResponse>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}