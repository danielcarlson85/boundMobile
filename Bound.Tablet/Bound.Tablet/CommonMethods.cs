using Bound.NFC;
using Xamarin.Essentials;

namespace Bound.Tablet
{
    internal class CommonMethods
    {
        public static void Vibrate()
        {
            App.UpTime = 0;
            Vibration.Vibrate(20);
        }
    }
}
