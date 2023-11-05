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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IotHubConnectionStringBuilder = Microsoft.Azure.Devices.Client.IotHubConnectionStringBuilder;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Devicemanager.API.Managers
{
    public class IoTHubManager : IIoTHubManager
    {
        public IoTHubManager()
        {
            IoTHubConnectionString = Constants.ioTHubConnectionString;
            RegistryManager = RegistryManager.CreateFromConnectionString(IoTHubConnectionString);
        }

        private RegistryManager RegistryManager { get; set; }

        private string IoTHubConnectionString { get; }

        private IotHubConnectionStringBuilder IotHubConnectionStringBuilder { get; set; }

        public async Task<IoTHubDevice> Get(string deviceId)
        {
            Device device = await this.RegistryManager.GetDeviceAsync(deviceId);

            if (device == null)
            {
                return null; // throw new ArgumentException("IoThub device with that name does not exist");
            }

            await RegistryManager.CloseAsync();

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


        public async Task SendTextToIoTHubDevice(string messageToSend)
        {
            var serviceClient = ServiceClient.CreateFromConnectionString(IoTHubConnectionString);

            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(messageToSend));
            await serviceClient.SendAsync(App.User.DeviceData.MachineName, commandMessage);
          
        }

        public async Task SendStartTextToIoTHubDevice(User user)
        {
            var userAsJson = JsonConvert.SerializeObject(user);

            var serviceClient = ServiceClient.CreateFromConnectionString(IoTHubConnectionString);

            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes("start****"+userAsJson));
            await serviceClient.SendAsync(App.User.DeviceData.MachineName, commandMessage);

        }

        public async Task<HttpStatusCode> SendStartRequestToDevice(User user)
        {
            CloudToDeviceMethodResult result;
            
            App.User.Device = await Get(App.User.DeviceData.MachineName);
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(user.Device.ConnectionString, TransportType.Mqtt);
            var serviceClient = ServiceClient.CreateFromConnectionString(this.IoTHubConnectionString);

            try
            {
                var methodInvocation = new CloudToDeviceMethod("start");
                string json = JsonConvert.SerializeObject(user);

                methodInvocation.SetPayloadJson(json);
                result = await serviceClient.InvokeDeviceMethodAsync(user.DeviceData.MachineName, methodInvocation);

                if (result.Status == 200)
                {
                    Debug.WriteLine("Response from device");
                }

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

                user.Device.IsRunning = false;
            }

            return (HttpStatusCode)result.Status;
        }
        
        public async Task<HttpStatusCode> SendOnlineRequestToDevice(User user)
        {
            CloudToDeviceMethodResult result;
            
            App.User.Device = await Get(App.User.DeviceData.MachineName);
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(user.Device.ConnectionString, TransportType.Mqtt);
            var serviceClient = ServiceClient.CreateFromConnectionString(this.IoTHubConnectionString);

            try
            {
                var methodInvocation = new CloudToDeviceMethod("online");
                string json = JsonConvert.SerializeObject(user);

                methodInvocation.SetPayloadJson(json);
                result = await serviceClient.InvokeDeviceMethodAsync(user.DeviceData.MachineName, methodInvocation);

                if (result.Status == 200)
                {
                    Debug.WriteLine("Response from device");
                }

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

                user.Device.IsRunning = false;
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

        public string StartReceivingMessages()
        {
            var lastReceivedMessage = string.Empty;

            var deviceClient = DeviceClient.CreateFromConnectionString(Constants.ioTHubConnectionString, "Mobile");

            //await Task.Run(async () =>
            //{
            //    //// Initialize the DeviceClient with the connection string.
            //    //string connectionString = "YourDeviceAConnectionString";
            //    //deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

            //    //Debug.WriteLine

            //    //var receivedMessageLabel = new Xamarin.Forms.Label();

            //    //sendMessageButton.Clicked += async (sender, args) =>
            //    //{
            //    //    string data = "Data from Device A to Device B";
            //    //    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(data));
            //    //    message.Properties.Add("targetDevice", "DeviceB");
            //    //    await deviceClient.SendEventAsync(message);
            //    //    receivedMessageLabel.Text = "Message sent to Device B: " + data;
            //    //};
            //}
            // Set up message reception.






            return lastReceivedMessage;
        }




        public Task<List<IoTHubDevice>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task SendDataToIoTHubDevice(IoTHubDevice ioTHubDevice, string messageToSend)
        {
            throw new NotImplementedException();
        }

        Task<string> IIoTHubManager.StartReceivingMessages()
        {
            throw new NotImplementedException();
        }

        public Task SendStartTextToIoTHubDevice(string messageToSend)
        {
            throw new NotImplementedException();
        }
    }
}