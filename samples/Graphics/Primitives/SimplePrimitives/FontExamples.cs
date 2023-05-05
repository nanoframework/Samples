// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System.Drawing;

namespace Primitives.SimplePrimitives
{
    public class FontExamples
    {
       public FontExamples(Bitmap fullScreenBitmap)
        {
            Font fntCourierRegular10 = Resource.GetFont(Resource.FontResources.CourierRegular10);
            Font fntComicSansMS16 = Resource.GetFont(Resource.FontResources.ComicSansMS16);
            Font fntNinaB = Resource.GetFont(Resource.FontResources.NinaB);
            Font fntSegoeUIRegular12 = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);
            Font fntSmall = Resource.GetFont(Resource.FontResources.small);


            string strSmallFont = Resource.GetString(Resource.StringResources.strnSmallFont);
            string strNinaFont = Resource.GetString(Resource.StringResources.strNinaFont);
            string strComicSansMS16 = Resource.GetString(Resource.StringResources.strComicSansMS16);
            string strCourierRegular10 = Resource.GetString(Resource.StringResources.strCourierRegular10);
            string strSegoeUIRegular12 = Resource.GetString(Resource.StringResources.strSegoeUIRegular12);

            Color red = Color.Red;
            Color green = Color.Green;
            Color blue = Color.Blue;

            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            fullScreenBitmap.DrawText(strSmallFont, fntSmall, red, 30, 10);
            fullScreenBitmap.DrawText(strSegoeUIRegular12, fntSegoeUIRegular12, blue, 30, 30);
            fullScreenBitmap.DrawText(strNinaFont, fntNinaB, green, 30, 60);
            fullScreenBitmap.DrawText(strComicSansMS16, fntComicSansMS16, red, 30, 120);
            fullScreenBitmap.DrawText(strCourierRegular10, fntCourierRegular10, blue, 30, 180);
            fullScreenBitmap.Flush();
        }
    }
}
