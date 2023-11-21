namespace Bound.Tablet.Models
{
    public class User
    {
        public DeviceData DeviceData { get; set; } = new DeviceData();
        public Tokens Tokens { get; set; } = new Tokens();
        public IoTHubDevice Device { get; set; } = new IoTHubDevice();

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ObjectId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

    }
}
