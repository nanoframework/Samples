// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Drawing;

namespace Primitives.SimplePrimitives
{
    public class ColourGradient
    {
        public ColourGradient(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            fullScreenBitmap.DrawRectangle(Color.White,           // outline color
                              1,                     // outline thickness
                              100, 100,              // x and y of top left corner
                              200, 100,              // width and height
                              0, 0,                  // x and y corner radius
                              Color.White,           // gradient start color
                              100, 100,              // gradient start coordinates  
                              Color.Black,           // gradient end color
                              100 + 200, 100 + 100,  // gradient end coordinates
                              Bitmap.OpacityOpaque); // opacity
            fullScreenBitmap.Flush();
        }
    }
}
