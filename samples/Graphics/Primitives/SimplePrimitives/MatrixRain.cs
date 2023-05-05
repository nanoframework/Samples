using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Drawing;
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
        // The character used for the rain will be randomly choose from this string
        const string AvailableLetterChars = "abcdefghijklmnopqrstuvwxyz1234567890";

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
        private int[] _drops;                       // Array that keep track of rain 'drops' position
        private Color _backgroundColour;
        private Color _textColour;
        private int _letterAdvanceWidth;          //single letter height calculate from glyph typeface
        private int _letterAdvanceHeight;         // single letter height calculate from glyph typeface

        private int _xOffset;
        private int _yOffset;

        private Random _random = new Random();
        private Bitmap _fullScreenBitmap;            //render current visualization for animation needs
        private Timer _timer;

        private Font _matrixFont;

        public MatrixRain(Bitmap fullScreenBitmap)
        {
            int timerInterval = 50;                     // The number of mSec between each frame.
            _fullScreenBitmap = fullScreenBitmap;
            _fullScreenBitmap.Clear();
            Initialize();

            _timer = new Timer(new TimerCallback(AnimationTimerTick), null, 0, timerInterval);
        }

        public void Stop()
        {
            _timer.Change(0, 0);
            _timer.Dispose();
        }

        private void Initialize()
        {
            _matrixFont = Primitives.Resource.GetFont(Resource.FontResources.MatrixFont);
            _backgroundColour = Color.Black;
            _textColour = Color.FromArgb(0x66, 0xff, 00);
            _letterAdvanceWidth = _matrixFont.CharWidth('c');
            _letterAdvanceHeight = _matrixFont.Height;
            _xOffset = 1;
            _yOffset = 1;
            //BaselineOrigin = new PointAndChar(2, 2, " ");

            _drops = new int[(int)(_fullScreenBitmap.Width / _letterAdvanceWidth)];
            for (var x = 0; x < _drops.Length; x++)
            {
                _drops[x] = 1;
            }
        }

        private void AnimationTimerTick(object state)
        {
            if (_drops != null & _drops.Length > 0)
            {
                // Black background with opacity to fade characters
                _fullScreenBitmap.DrawRectangle(colorOutline: _backgroundColour, thicknessOutline: 0,
                                                x: 0, y: 0,
                                                width: _fullScreenBitmap.Width, height: _fullScreenBitmap.Height,
                                                xCornerRadius: 0, yCornerRadius: 0,
                                                colorGradientStart: Color.Black,
                                                xGradientStart: 0, yGradientStart: 0,
                                                colorGradientEnd: Color.Black,
                                                xGradientEnd: _fullScreenBitmap.Width, yGradientEnd: _fullScreenBitmap.Height,
                                                opacity: 25);

                PointAndChar pac = new PointAndChar();
                for (var i = 0; i < _drops.Length; i++)         // looping over drops
                {
                    // new drop position
                    double x = _xOffset + _letterAdvanceWidth * i;
                    double y = _yOffset + _letterAdvanceHeight * _drops[i];

                    // check if new letter does not go outside the image
                    if (y + _letterAdvanceHeight < _fullScreenBitmap.Height)
                    {
                        char randomLetter = AvailableLetterChars[_random.Next(AvailableLetterChars.Length - 1)];
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
                    if (_drops[i] * _letterAdvanceHeight > _fullScreenBitmap.Height && _random.Next(1000) > 850)
                    {
                        _drops[i] = 0;
                    }

                    _fullScreenBitmap.DrawText(pac.Character, _matrixFont, _textColour, pac.x, pac.y);
                    //incrementing Y coordinate
                    _drops[i]++;
                }

                _fullScreenBitmap.Flush();
            }
        }
    }
}
