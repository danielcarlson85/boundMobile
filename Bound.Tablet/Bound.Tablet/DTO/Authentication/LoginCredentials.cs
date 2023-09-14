using Newtonsoft.Json;

namespace Bound.Tablet.Dtos.Authentication
{
    public class LoginCredentials
    {
        public string Email { get; set;}

        public string Password { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
