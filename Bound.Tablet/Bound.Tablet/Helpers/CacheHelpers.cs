using Bound.NFC;
using Bound.Tablet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bound.Tablet.Helpers
{
    public static class CacheHelpers
    {

        public static bool GetCachedUserForLogin()
        {
            Debug.WriteLine("Authentication started");

            var savedUser = Xamarin.Essentials.Preferences.Get("user", "");
            var savedTokens = Xamarin.Essentials.Preferences.Get("tokens", "");

            if (!string.IsNullOrEmpty(savedUser) && !string.IsNullOrEmpty(savedTokens))
            {
                App.User = JsonConvert.DeserializeObject<User>(savedUser);
                var token = JsonConvert.DeserializeObject<Tokens>(savedTokens);
                App.User.Tokens = token;

                Debug.WriteLine("User found on mobile");

                return true;
            }

            return false;
        }

        public static User GetCachedUser()
        {
            var savedUser = Xamarin.Essentials.Preferences.Get("user", "");

            var user = new User();

            if (!string.IsNullOrEmpty(savedUser))
            {
                user = JsonConvert.DeserializeObject<User>(savedUser);

                Debug.WriteLine("User found on mobile");
            }
            else
            {
                Debug.WriteLine("No User found on mobile");

            }

            return user;
        }

        public static void SaveCachedUser()
        {
            var userAsJSON = JsonConvert.SerializeObject(App.User);
            var tokensAsJSON = JsonConvert.SerializeObject(App.User.Tokens);

            Xamarin.Essentials.Preferences.Set("user", userAsJSON);
            Xamarin.Essentials.Preferences.Set("tokens", tokensAsJSON);
        }

    }
}
