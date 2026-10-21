//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using nanoFramework.UI;

namespace ManagedResources.Graphics
{
    public class Program
    {
        public static void Main()
        {
            // images and fonts in managed resources require a reference to nanoFramework.Graphics
            // and a target running a firmware image with graphics support

            // images declared as System.Drawing.Bitmap (newer resource editor)
            PrintBitmap("GIF", Resource.GetBitmap(Resource.BitmapResources.SoccerBall));
            PrintBitmap("JPG", Resource.GetBitmap(Resource.BitmapResources.Waterfall));
            PrintBitmap("BMP", Resource.GetBitmap(Resource.BitmapResources.Checker));

            // image declared as System.Byte[] (legacy resource editor)
            PrintBitmap("JPG (legacy declaration)", Resource.GetBitmap(Resource.BitmapResources.WaterfallLegacy));

            // fonts
            Debug.WriteLine($"Font has height {Resource.GetFont(Resource.FontResources.Small).Height}");
            Debug.WriteLine($"Font (stream declaration) has height {Resource.GetFont(Resource.FontResources.SmallNew).Height}");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void PrintBitmap(string description, Bitmap bitmap)
        {
            Debug.WriteLine($"{description} image is {bitmap.Width}x{bitmap.Height}");
        }
    }
}
