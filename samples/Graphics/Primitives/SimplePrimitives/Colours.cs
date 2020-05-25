using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Reflection;
using System.Threading;

namespace Primitives.SimplePrimitives
{
    public class Colours
    {
        private struct ColourAndName
        {
            public string Name;
            public Color Colour;
            public ColourAndName(string Name, Color Colour)
            {
                this.Name = Name;
                this.Colour = Colour;
            }
        }
        public Colours(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            fullScreenBitmap.Clear();
            Font fnt12 = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);
            ColourAndName[] ColourTest = new ColourAndName[] { new ColourAndName("AliceBlue",Color.AliceBlue),
                                         new ColourAndName("AntiqueWhite",Color.AntiqueWhite),
                                         new ColourAndName("Aqua",Color.Aqua),
                                         new ColourAndName("Aquamarine",Color.Aquamarine),
                                         new ColourAndName("Azure",Color.Azure),
                                         new ColourAndName("Beige",Color.Beige),
                                         new ColourAndName("Bisque",Color.Bisque),
                                         new ColourAndName("Black",Color.Black),
                                         new ColourAndName("BlanchedAlmond",Color.BlanchedAlmond),
                                         new ColourAndName("Blue",Color.Blue),
                                         new ColourAndName("BlueViolet",Color.BlueViolet),
                                         new ColourAndName("Brown",Color.Brown),
                                         new ColourAndName("BurlyWood",Color.BurlyWood),
                                         new ColourAndName("CadetBlue",Color.CadetBlue),
                                         new ColourAndName("Chartreuse",Color.Chartreuse),
                                         new ColourAndName("Chocolate",Color.Chocolate),
                                         new ColourAndName("Coral",Color.Coral),
                                         new ColourAndName("CornflowerBlue",Color.CornflowerBlue),
                                         new ColourAndName("Cornsilk",Color.Cornsilk),
                                         new ColourAndName("Crimson",Color.Crimson),
                                         new ColourAndName("Cyan",Color.Cyan),
                                         new ColourAndName("DarkBlue",Color.DarkBlue),
                                         new ColourAndName("DarkCyan",Color.DarkCyan),
                                         new ColourAndName("DarkGoldenrod",Color.DarkGoldenrod),
                                         new ColourAndName("DarkGray",Color.DarkGray),
                                         new ColourAndName("DarkGreen",Color.DarkGreen),
                                         new ColourAndName("DarkKhaki",Color.DarkKhaki),
                                         new ColourAndName("DarkMagenta",Color.DarkMagenta),
                                         new ColourAndName("DarkOliveGreen",Color.DarkOliveGreen),
                                         new ColourAndName("DarkOrange",Color.DarkOrange),
                                         new ColourAndName("DarkOrchid",Color.DarkOrchid),
                                         new ColourAndName("DarkRed",Color.DarkRed),
                                         new ColourAndName("DarkSalmon",Color.DarkSalmon),
                                         new ColourAndName("DarkSeaGreen",Color.DarkSeaGreen),
                                         new ColourAndName("DarkSlateBlue",Color.DarkSlateBlue),
                                         new ColourAndName("DarkSlateGray",Color.DarkSlateGray),
                                         new ColourAndName("DarkTurquoise",Color.DarkTurquoise),
                                         new ColourAndName("DarkViolet",Color.DarkViolet),
                                         new ColourAndName("DeepPink",Color.DeepPink),
                                         new ColourAndName("DeepSkyBlue",Color.DeepSkyBlue),
                                         new ColourAndName("DimGray",Color.DimGray),
                                         new ColourAndName("DodgerBlue",Color.DodgerBlue),
                                         new ColourAndName("Firebrick",Color.Firebrick),
                                         new ColourAndName("FloralWhite",Color.FloralWhite),
                                         new ColourAndName("ForestGreen",Color.ForestGreen),
                                         new ColourAndName("Gainsboro",Color.Gainsboro),
                                         new ColourAndName("GhostWhite",Color.GhostWhite),
                                         new ColourAndName("Gold",Color.Gold),
                                         new ColourAndName("Goldenrod",Color.Goldenrod),
                                         new ColourAndName("Gray",Color.Gray),
                                         new ColourAndName("Green",Color.Green),
                                         new ColourAndName("GreenYellow",Color.GreenYellow),
                                         new ColourAndName("Honeydew",Color.Honeydew),
                                         new ColourAndName("HotPink",Color.HotPink),
                                         new ColourAndName("IndianRed",Color.IndianRed),
                                         new ColourAndName("Indigo",Color.Indigo),
                                         new ColourAndName("Ivory",Color.Ivory),
                                         new ColourAndName("Khaki",Color.Khaki),
                                         new ColourAndName("Lavender",Color.Lavender),
                                         new ColourAndName("LavenderBlush",Color.LavenderBlush),
                                         new ColourAndName("LawnGreen",Color.LawnGreen),
                                         new ColourAndName("LemonChiffon",Color.LemonChiffon),
                                         new ColourAndName("LightBlue",Color.LightBlue),
                                         new ColourAndName("LightCoral",Color.LightCoral),
                                         new ColourAndName("LightCyan",Color.LightCyan),
                                         new ColourAndName("LightGoldenrodYellow",Color.LightGoldenrodYellow),
                                         new ColourAndName("LightGray",Color.LightGray),
                                         new ColourAndName("LightGreen",Color.LightGreen),
                                         new ColourAndName("LightPink",Color.LightPink),
                                         new ColourAndName("LightSalmon",Color.LightSalmon),
                                         new ColourAndName("LightSeaGreen",Color.LightSeaGreen),
                                         new ColourAndName("LightSkyBlue",Color.LightSkyBlue),
                                         new ColourAndName("LightSlateGray",Color.LightSlateGray),
                                         new ColourAndName("LightSteelBlue",Color.LightSteelBlue),
                                         new ColourAndName("LightYellow",Color.LightYellow),
                                         new ColourAndName("Lime",Color.Lime),
                                         new ColourAndName("LimeGreen",Color.LimeGreen),
                                         new ColourAndName("Linen",Color.Linen),
                                         new ColourAndName("Magenta",Color.Magenta),
                                         new ColourAndName("Maroon",Color.Maroon),
                                         new ColourAndName("MediumAquamarine",Color.MediumAquamarine),
                                         new ColourAndName("MediumBlue",Color.MediumBlue),
                                         new ColourAndName("MediumOrchid",Color.MediumOrchid),
                                         new ColourAndName("MediumPurple",Color.MediumPurple),
                                         new ColourAndName("MediumSeaGreen",Color.MediumSeaGreen),
                                         new ColourAndName("MediumSlateBlue",Color.MediumSlateBlue),
                                         new ColourAndName("MediumSpringGreen",Color.MediumSpringGreen),
                                         new ColourAndName("MediumTurquoise",Color.MediumTurquoise),
                                         new ColourAndName("MediumVioletRed",Color.MediumVioletRed),
                                         new ColourAndName("MidnightBlue",Color.MidnightBlue),
                                         new ColourAndName("MintCream",Color.MintCream),
                                         new ColourAndName("MistyRose",Color.MistyRose),
                                         new ColourAndName("Moccasin",Color.Moccasin),
                                         new ColourAndName("NavajoWhite",Color.NavajoWhite),
                                         new ColourAndName("Navy",Color.Navy),
                                         new ColourAndName("OldLace",Color.OldLace),
                                         new ColourAndName("Olive",Color.Olive),
                                         new ColourAndName("OliveDrab",Color.OliveDrab),
                                         new ColourAndName("Orange",Color.Orange),
                                         new ColourAndName("OrangeRed",Color.OrangeRed),
                                         new ColourAndName("Orchid",Color.Orchid),
                                         new ColourAndName("PaleGoldenrod",Color.PaleGoldenrod),
                                         new ColourAndName("PaleGreen",Color.PaleGreen),
                                         new ColourAndName("PaleTurquoise",Color.PaleTurquoise),
                                         new ColourAndName("PaleVioletRed",Color.PaleVioletRed),
                                         new ColourAndName("PapayaWhip",Color.PapayaWhip),
                                         new ColourAndName("PeachPuff",Color.PeachPuff),
                                         new ColourAndName("Peru",Color.Peru),
                                         new ColourAndName("Pink",Color.Pink),
                                         new ColourAndName("Plum",Color.Plum),
                                         new ColourAndName("PowderBlue",Color.PowderBlue),
                                         new ColourAndName("Purple",Color.Purple),
                                         new ColourAndName("Red",Color.Red),
                                         new ColourAndName("RosyBrown",Color.RosyBrown),
                                         new ColourAndName("RoyalBlue",Color.RoyalBlue),
                                         new ColourAndName("SaddleBrown",Color.SaddleBrown),
                                         new ColourAndName("Salmon",Color.Salmon),
                                         new ColourAndName("SandyBrown",Color.SandyBrown),
                                         new ColourAndName("SeaGreen",Color.SeaGreen),
                                         new ColourAndName("SeaShell",Color.SeaShell),
                                         new ColourAndName("Sienna",Color.Sienna),
                                         new ColourAndName("Silver",Color.Silver),
                                         new ColourAndName("SkyBlue",Color.SkyBlue),
                                         new ColourAndName("SlateBlue",Color.SlateBlue),
                                         new ColourAndName("SlateGray",Color.SlateGray),
                                         new ColourAndName("Snow",Color.Snow),
                                         new ColourAndName("SpringGreen",Color.SpringGreen),
                                         new ColourAndName("SteelBlue",Color.SteelBlue),
                                         new ColourAndName("Tan",Color.Tan),
                                         new ColourAndName("Teal",Color.Teal),
                                         new ColourAndName("Thistle",Color.Thistle),
                                         new ColourAndName("Tomato",Color.Tomato),
                                         new ColourAndName("Turquoise",Color.Turquoise),
                                         new ColourAndName("Violet",Color.Violet),
                                         new ColourAndName("Wheat",Color.Wheat),
                                         new ColourAndName("White",Color.White),
                                         new ColourAndName("WhiteSmoke",Color.WhiteSmoke),
                                         new ColourAndName("Yellow",Color.Yellow),
                                         new ColourAndName("YellowGreen",Color.YellowGreen),
            };
            int displayFourAtATime = -1;
            int xCoord = 0;
            int yCoord = 0;
            int spacing = 10;
            int width = (fullScreenBitmap.Width / 2) - spacing / 2;
            int height = (fullScreenBitmap.Height / 2) - spacing / 2;
            int textWidth;
            int textHeight;
            int xCoordText = 0;
            int yCoordText = 0;

            foreach (ColourAndName can in ColourTest)
            {
                fnt12.ComputeExtent(can.Name, out textWidth, out textHeight);

                int remainder = displayFourAtATime % 4;
                switch (remainder)
                {
                    case 0:
                        xCoord = 0;
                        xCoordText = (width - textWidth) / 2;
                        yCoord = 0;
                        yCoordText = (height - textHeight) / 2;
                        break;
                    case 1:
                        xCoord = fullScreenBitmap.Width - width;
                        xCoordText = fullScreenBitmap.Width - (width + textWidth) / 2;
                        yCoord = 0;
                        yCoordText = (height - textHeight) / 2;
                        break;
                    case 2:
                        xCoord = 0;
                        xCoordText = (width - textWidth) / 2;
                        yCoord = fullScreenBitmap.Height - height;
                        yCoordText = fullScreenBitmap.Height - (height + textHeight) / 2;
                        break;
                    case 3:
                        xCoord = fullScreenBitmap.Width - width;
                        xCoordText = fullScreenBitmap.Width - (width + textWidth) / 2;
                        yCoord = fullScreenBitmap.Height - height;
                        yCoordText = fullScreenBitmap.Height - (height + textHeight) / 2;
                        break;
                }
                fullScreenBitmap.DrawRectangle(can.Colour,                      // outline color
                                                   1,                           // outline thickness
                                                   xCoord, yCoord,              // x and y of top left corner
                                                   width,
                                                   height,
                                                   0, 0,                        // x and y corner radius
                                                   can.Colour,                  // gradient start color
                                                   0, 0,                        // gradient start coordinates  
                                                   can.Colour,                  // gradient end color
                                                   0, 0,                        // gradient end coordinates
                                                   Bitmap.OpacityOpaque);       // opacity
                Color contrastTextColour = ColorUtility.ColorFromRGB((byte)(255 - ColorUtility.GetRValue(can.Colour)),
                                                                     (byte)(255 - ColorUtility.GetGValue(can.Colour)),
                                                                     (byte)(255 - ColorUtility.GetBValue(can.Colour)));
                fullScreenBitmap.DrawText(can.Name, fnt12, contrastTextColour, xCoordText, yCoordText);

                if (remainder == 3)
                {
                    fullScreenBitmap.Flush();
                    Thread.Sleep(700);
                }
                displayFourAtATime++;
            }
        }
    }
}