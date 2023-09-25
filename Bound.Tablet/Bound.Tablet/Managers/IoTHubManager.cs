// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Bound.NFC;
using Bound.Tablet.Models;
using Bound.Tablet.Settings;
using Devicemanager.API.Interfaces;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IotHubConnectionStringBuilder = Microsoft.Azure.Devices.Client.IotHubConnectionStringBuilder;
using Message = Microsoft.Azure.Devices.Client.Message;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Devicemanager.API.Managers
{
    public class IoTHubManager : IIoTHubManager
    {
        public IoTHubManager()
        {
            this.IoTHubConnectionString = Constants.ioTHubConnectionString;
            this.RegistryManager = RegistryManager.CreateFromConnectionString(IoTHubConnectionString);
        }

        private RegistryManager RegistryManager { get; set; }

        private string IoTHubConnectionString { get; }

        private IotHubConnectionStringBuilder IotHubConnectionStringBuilder { get; set; }

        public async Task<IoTHubDevice> Get(string deviceId)
        {
            Device device = await this.RegistryManager.GetDeviceAsync(deviceId);

            if (device == null)
            {
                throw new ArgumentException("IoThub device with that name does not exist");
            }

            var ioTHubDevice = this.CreateIoTHubDeviceObject(deviceId, device);
            return ioTHubDevice;
        }

        private IoTHubDevice CreateIoTHubDeviceObject(string deviceId, Device device)
        {
            var ioTHubDeviceAsObject = new IoTHubDevice();
            this.IotHubConnectionStringBuilder = IotHubConnectionStringBuilder.Create(this.IoTHubConnectionString + ";DeviceId=" + deviceId);
            ioTHubDeviceAsObject.AzureIoTHubDevice = device;
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

        public async Task<IoTHubDevice> Create(string deviceName)
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

        public async Task SendDataToIoTHubDevice(IoTHubDevice ioTHubDevice, string messageToSend)
        {
            using (DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(ioTHubDevice.ConnectionString, TransportType.Mqtt))
            {
                var message = new Message(Encoding.ASCII.GetBytes(messageToSend));
                message.Properties.Add("Device", ioTHubDevice.DeviceName);
                await deviceClient.SendEventAsync(message);
            }

            Console.WriteLine("Data sent to iothubdevice");
        }

        public async Task<HttpStatusCode> SendStartRequestToDevice(User user)
        {
            CloudToDeviceMethodResult result;
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(user.DeviceData.Device.ConnectionString, TransportType.Mqtt);
            var serviceClient = ServiceClient.CreateFromConnectionString(this.IoTHubConnectionString);

            try
            {
                var methodInvocation = new CloudToDeviceMethod("start");
                string json = JsonConvert.SerializeObject(user.DeviceData);

                methodInvocation.SetPayloadJson(json);
                result = await serviceClient.InvokeDeviceMethodAsync(user.DeviceData.MachineName, methodInvocation);

            }
            catch (Exception ex)
            {
                return HttpStatusCode.NotFound;
            }
            finally
            {
                if (deviceClient != null)
                {
                    await deviceClient.CloseAsync();
                }

                user.DeviceData.Device.IsRunning = false;
            }

            return (HttpStatusCode)result.Status;
        }

        public async Task<HttpStatusCode> SendStopRequestToDevice(IoTHubDevice deviceName)
        {
            CloudToDeviceMethodResult result;
            var serviceClient = ServiceClient.CreateFromConnectionString(this.IoTHubConnectionString);

            try
            {

                var methodInvocation = new CloudToDeviceMethod("stop");
                string json = JsonConvert.SerializeObject(App.User.DeviceData);
                methodInvocation.SetPayloadJson(json);

                result = await serviceClient.InvokeDeviceMethodAsync(deviceName.DeviceName, methodInvocation);
            }
            catch (Exception)
            {
                return HttpStatusCode.NotFound;
            }
            finally
            {
                if (serviceClient != null)
                {
                    await serviceClient.CloseAsync();
                }
            }

            return (HttpStatusCode)result.Status;
        }

        public Task<List<IoTHubDevice>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}