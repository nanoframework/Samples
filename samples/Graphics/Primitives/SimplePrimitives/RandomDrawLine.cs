// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomDrawLine
    {
        public RandomDrawLine(Bitmap fullScreenBitmap,  Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            fullScreenBitmap.DrawText("Random Line Drawing", DisplayFont, Color.AliceBlue, 0, 0);

            for (int i = 100; i > 0; i--)
            {
                int thickness = random.Next(8);
                fullScreenBitmap.DrawLine((Color)random.Next(0xFFFFFF),
                                           thickness,
                                           random.Next(fullScreenBitmap.Width),
                                           random.Next(fullScreenBitmap.Height - 22),
                                           random.Next(fullScreenBitmap.Width),
                                           random.Next(fullScreenBitmap.Height));
                InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Line Number {i}");
                fullScreenBitmap.Flush();
            }
        }
    }
}
