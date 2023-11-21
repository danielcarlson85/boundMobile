﻿using System;
using System.Net.Http;
using Bound.NFC;
using Bound.Tablet.Settings;

namespace Bound.Tablet.Services
{
    public class JWTHttpClient : HttpClient
    {
        public static string Token
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(Token), "");
            set => Xamarin.Essentials.Preferences.Set(nameof(Token), value);
        }

        public static string RefreshToken
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(RefreshToken), "");
            set => Xamarin.Essentials.Preferences.Set(nameof(RefreshToken), value);
        }

        public JWTHttpClient()
        {
            BaseAddress = new Uri(Constants.IDPLoginUri);
            if (!string.IsNullOrEmpty(Token))
            {
                DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            }
        }

        public void ReAddToken()
        {
            DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        }

        public static void SendDebugTextToTablet(string debugText)
        {
            string fromdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            new HttpClient().GetAsync($"https://boundhub.azurewebsites.net/send?name=" + App.User?.Email + "&machinename=" + App.User?.DeviceData.MachineName + "&weight=" + App.User?.DeviceData.Weight + "&status=offline&reps="+ App.User?.DeviceData.TotalReps + "&DebugText=" + fromdate + "_MOBILE:_" + debugText);
        }
    }
}
