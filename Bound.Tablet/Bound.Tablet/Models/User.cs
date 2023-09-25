namespace Bound.Tablet.Models
{
    public class User
    {
        public DeviceData DeviceData { get; set; } = new DeviceData();
        public Tokens Tokens { get; set; } = new Tokens();
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ObjectId { get; set; }
        public string Role { get; set; }
    }
}
