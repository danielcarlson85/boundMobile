using Bound.Tablet.Models;
using Bound.Tablet.Services;
using Newtonsoft.Json;
using Bound.NFC;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bound.Tablet.ViewModels
{
    public class SignInPageViewModel : BaseViewModel
    {
        readonly AuthenticationService _authenticationService;

        public SignInPageViewModel()
        {
            _authenticationService = new AuthenticationService();

            Email = "info@danielcarlson.net";
            Password = "Bound2023";
        }

        public async Task Authentication()
        {
            CommonMethods.Vibrate();
            IsBusy = true;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "AppResources.AuthenticationNoUsernamePasswordAlertText", "AppResources.OK");
                Debug.WriteLine("Some input is wrong");
            }

            var AuthenticationResult = await _authenticationService.AuthenticationAsync(Email, Password);
            await Application.Current.MainPage.DisplayAlert("Welcome ", AuthenticationResult.FirstName, "OK");

            CreateObjects(AuthenticationResult);
            SaveCachedUser();

            Debug.WriteLine("Authentication finnished");
            Debug.WriteLine("User is saved on device");

            Application.Current.MainPage = new MainPage();

            IsBusy = false;
        }

        private static void SaveCachedUser()
        {
            var userAsJSON = JsonConvert.SerializeObject(App.User);
            var tokensAsJSON = JsonConvert.SerializeObject(App.Tokens);

            Xamarin.Essentials.Preferences.Set("user", userAsJSON);
            Xamarin.Essentials.Preferences.Set("tokens", tokensAsJSON);
        }

        private static void CreateObjects(Dots.Authentication.CleanADUserResponse AuthenticationResult)
        {
            App.User = new User()
            {
                Email = AuthenticationResult.Email,
                FirstName = AuthenticationResult.FirstName,
                LastName = AuthenticationResult.LastName,
                ObjectId = AuthenticationResult.ObjectId,
                Role = AuthenticationResult.Role
            };

            App.Tokens = new Tokens()
            {
                AccessToken = AuthenticationResult.AccessToken,
                RefreshToken = AuthenticationResult.RefreshToken
            };
        }
    }
}