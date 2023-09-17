namespace Bound.Tablet.Settings
{
    public static class Constants
    {
        public const string IDPLoginUri = "https://bound2023-idp.azurewebsites.net/api/v1/auth/login";
        
        public const string ioTHubConnectionString = "HostName=boundiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Ytv0bSvktmjBB0pL2Do1SPZLbkCl2QbpYAIoTMJr7v0=";

        public const string Content_Type = "application/json";

        public const string StartDeviceTopic = "StartDeviceTopic";

        public const string HostMqttBroker = "test.mosquitto.org";

        public const string HostMqttBrokerLocal = "192.168.1.9";
    }
}
