// -------------------------------------------------------------------------------------------------
// Copyright (c) Bound Technologies AB. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Bound.Tablet.Settings;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mqtt;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bound.Common.Helpers
{
    public static class InternetHelpers
    {
        /// <summary>
        /// Checks if Bound Business is online or not.
        /// </summary>
        public static bool CheckIfBoundBusinessIsOnline()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(Constants.IDPLoginUri, 10000);

                    if (reply.Status != IPStatus.Success)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the first local ip address.
        /// </summary>
        /// <returns>String of IpAddress.</returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            Console.WriteLine("No network adapters with an IPv4 address in the system!");
            Environment.Exit(0);
            return null;
        }

        /// <summary>
        /// Prin out all Ipv4 addresses that can be found.
        /// </summary>
        public static void PrintAllIpAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                }
            }
        }
    }
}
