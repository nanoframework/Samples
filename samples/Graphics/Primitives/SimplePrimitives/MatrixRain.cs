using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Threading;

//
// This sample was adapted from work by Andreoli Carlo who published his original work on CodeProject at this URL
// https://www.codeproject.com/Articles/5164199/Matrix-style-Rain-in-Csharp-with-WPF
//

//
// The Matrix font was downloaded from www.norfok.com.
// The font came with a license.txt file with the following contents
//-----------------------------------------------------------
// License Agreement
// =
// 
// This font is freeware
// You can use this font for all your private purposes.
// 
// If you want to use this font for commercial use,
// you may purchase the font for U$ 25
// 
// Thank You.
// 
// =
// website:
// http://www.norfok.com
//-----------------------------------------------------------

namespace Primitives
{
    public class MatrixRain
    {
        internal class PointAndChar
        {
            public int x { get; set; }
            public int y { get; set; }
            public string Character { get; set; }
            public PointAndChar() { }
            public PointAndChar(int x, int y, string Character)
            {
                this.x = x;
                this.y = y;
                this.Character = Character;
            }
        }
        //  private PointAndChar BaselineOrigin;               // letter baseline origin
        private int[] Drops;                       // Array that keep track of rain 'drops' position
        private Color BackgroundColour;
        private Color TextColour;
        private int LetterAdvanceWidth;          //single letter height calculate from glyph typeface
        private int LetterAdvanceHeight;         // single letter height calculate from glyph typeface

        private int xOffset;
        private int yOffset;

        private Random random = new Random();
        private Bitmap FullScreenBitmap;            //render current visualization for animation needs
        private Color MatrixTextColour { get; set; }
        System.Threading.Timer timer;

        // The character used for the rain will be randomly choose from this string
        private String AvailableLetterChars = "abcdefghijklmnopqrstuvwxyz1234567890";
        private Font MatrixFont { get; set; }
        public MatrixRain(Bitmap fullScreenBitmap)
        {
            int timerInterval = 50;                     // The number of mSec between each frame.
            FullScreenBitmap = fullScreenBitmap;
            FullScreenBitmap.Clear();
            Initialize();

            timer = new Timer(new TimerCallback(_animationTimer_Tick), null, 0, timerInterval);
        }
        private void Initialize()
        {
            MatrixFont = Primitives.Resource.GetFont(Resource.FontResources.MatrixFont);
            BackgroundColour = Color.Black;
            TextColour = ColorUtility.ColorFromRGB(0x66, 0xff, 00);
            LetterAdvanceWidth = MatrixFont.CharWidth('c');
            LetterAdvanceHeight = MatrixFont.Height;
            xOffset = 1;
            yOffset = 1;
            //BaselineOrigin = new PointAndChar(2, 2, " ");

            Drops = new int[(int)(FullScreenBitmap.Width / LetterAdvanceWidth)];
            for (var x = 0; x < Drops.Length; x++)
            {
                Drops[x] = 1;
            }
        }
        private void _animationTimer_Tick(object state)
        {
            if (Drops != null & Drops.Length > 0)
            {
                // Black background with opacity to fade characters
                FullScreenBitmap.DrawRectangle(colorOutline: BackgroundColour, thicknessOutline: 0,
                                                x: 0, y: 0,
                                                width: FullScreenBitmap.Width, height: FullScreenBitmap.Height,
                                                xCornerRadius: 0, yCornerRadius: 0,
                                                colorGradientStart: Color.Black,
                                                xGradientStart: 0, yGradientStart: 0,
                                                colorGradientEnd: Color.Black,
                                                xGradientEnd: FullScreenBitmap.Width, yGradientEnd: FullScreenBitmap.Height,
                                                opacity: 25);

                PointAndChar pac = new PointAndChar();
                for (var i = 0; i < Drops.Length; i++)         // looping over drops
                {
                    // new drop position
                    double x = xOffset + LetterAdvanceWidth * i;
                    double y = yOffset + LetterAdvanceHeight * Drops[i];

                    // check if new letter does not go outside the image
                    if (y + LetterAdvanceHeight < FullScreenBitmap.Height)
                    {
                        char randomLetter = AvailableLetterChars[random.Next(AvailableLetterChars.Length - 1)];
                        pac.x = (int)x;
                        pac.y = (int)y;
                        pac.Character = randomLetter.ToString();
                    }
                    else
                    {
                        pac.Character = " ";
                    }
                    //sending the drop back to the top randomly after it has crossed the image
                    //adding a randomness to the reset to make the drops scattered on the Y axis
                    if (Drops[i] * LetterAdvanceHeight > FullScreenBitmap.Height && random.Next(1000) > 850)
                    {
                        Drops[i] = 0;
                    }
                    FullScreenBitmap.DrawText(pac.Character, MatrixFont, TextColour, pac.x, pac.y);
                    //incrementing Y coordinate
                    Drops[i]++;
                }
                FullScreenBitmap.Flush();
            }
        }
    }
}
