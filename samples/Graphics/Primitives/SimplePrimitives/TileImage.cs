﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System.Drawing;

namespace Primitives.SimplePrimitives
{
    public class TileImage
    {
        public TileImage(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            //Bitmap btn = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
            Bitmap btn = new Bitmap(PandaGif.Panda, Bitmap.BitmapImageType.Gif);

            fullScreenBitmap.DrawImage(10, 10, btn, 0, 0, btn.Width, btn.Height);

            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();

            fullScreenBitmap.DrawRectangle(Color.White, 0, 0, 0, fullScreenBitmap.Width, fullScreenBitmap.Height, 0, 0, Color.White, 0, 0, Color.Black, 0, 0, 256);
            Font fntComicSansMS16 = Resource.GetFont(Resource.FontResources.ComicSansMS16);
            fullScreenBitmap.DrawText("Tile Image Example", fntComicSansMS16, Color.Black, 10, 0);
            fullScreenBitmap.TileImage(20, 20, btn, fullScreenBitmap.Width - 50, fullScreenBitmap.Height - 50, 256);
            InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, "Tile Image");
            fullScreenBitmap.Flush();
            btn.Dispose();
        }
    }
}
