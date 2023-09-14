using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bound.Tablet.Dots.Authentication;
using Bound.Tablet.Dtos.Authentication;
using Bound.Tablet.Services.Interfaces;
using Bound.Tablet.Settings;
using Newtonsoft.Json;

namespace Bound.Tablet.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JWTHttpClient _httpClient = new JWTHttpClient();

        /// <summary>
        /// Calls the backend to validate credentials for Authentication.
        /// </summary>
        /// <param name="username">The supplied user name</param>
        /// <param name="password">The supplied password</param>
        /// <returns></returns>
        public async Task<CleanADUserResponse> AuthenticationAsync(string username, string password)
        {
            try
            {
                var AuthenticationModel = new LoginCredentials { Email = username, Password = password }; //HashFactory.Encrypt(password) };

                var authModelAsJson = AuthenticationModel.ToJson();
                var content = new StringContent(authModelAsJson, Encoding.UTF8, Constants.Content_Type);
                var response = await _httpClient.PostAsync(Constants.IDPLoginUri, content);
            
                return JsonConvert.DeserializeObject<CleanADUserResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}
