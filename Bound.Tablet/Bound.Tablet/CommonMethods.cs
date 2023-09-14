using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Bound.Tablet
{
    internal class CommonMethods
    {
        public static void Vibrate()
        {
            Vibration.Vibrate(20);
        }
    }
}
