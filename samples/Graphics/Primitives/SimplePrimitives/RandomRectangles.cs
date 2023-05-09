// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Drawing;

namespace Primitives.SimplePrimitives
{
    public class RandomRectangles
    {
        public RandomRectangles(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            int xCornerRadius;
            int yCornerRadius;

            for (int i = 0; i < 100; i++)
            {
                Color fillColor = Color.FromArgb(random.Next(0xFFFFFF));

                if (i % 2 == 0)
                {
                    xCornerRadius = random.Next(2, 40);
                    yCornerRadius = random.Next(2, 40);
                }
                else
                {
                    xCornerRadius = 0;
                    yCornerRadius = 0;
                }

                fullScreenBitmap.DrawRectangle(Color.FromArgb(random.Next(0xFFFFFF)), random.Next(1),
                    random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), xCornerRadius, yCornerRadius, fillColor, 0, 0, fillColor, 0, 0, (ushort)random.Next(256));
                InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Rectangle Number {i}");
                fullScreenBitmap.Flush();
            }
        }
    }
}

