using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bound.Tablet.Services
{
    public class OnlineDateTimeService
    {
        private static async Task<OnlineDateResult> GetCurrentDateAsync()
        {

            HttpClient httpClient = new HttpClient();

            string result = await httpClient.GetStringAsync("http://worldtimeapi.org/api/timezone/Europe/Stockholm");

            var myDeserializedClass = JsonConvert.DeserializeObject<OnlineDateResult>(result);

            return myDeserializedClass;
        }

        private static DateTime now;

        public static DateTime Now
        {
            get
            {
                now = GetCurrentDateAsync().GetAwaiter().GetResult().datetime;
                return now;
            }
        }


        private static DateTime utcNow;

        public static DateTime UtcNow
        {
            get
            {
                utcNow = GetCurrentDateAsync().GetAwaiter().GetResult().utc_datetime.ToUniversalTime();
                return utcNow;
            }
        }

    }

    public class OnlineDateResult
    {
        public string abbreviation { get; set; }
        public string client_ip { get; set; }
        public DateTime datetime { get; set; }
        
        public int day_of_week { get; set; }
        public int day_of_year { get; set; }
        public bool dst { get; set; }
        public DateTime dst_from { get; set; }
        public int dst_offset { get; set; }
        public DateTime dst_until { get; set; }
        public int raw_offset { get; set; }
        public string timezone { get; set; }
        public int unixtime { get; set; }
        public DateTime utc_datetime { get; set; }
        public string utc_offset { get; set; }
        public int week_number { get; set; }
    }
}