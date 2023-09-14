using Newtonsoft.Json;

namespace Bound.Tablet.Dots.Authentication
{
    public class CleanADUserResponse
    {
        public string ObjectId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
