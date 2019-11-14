using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Collections;
using System.Threading;

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
        private PointAndChar BaselineOrigin;               // letter baseline origin
        private int[] RainDrops;                       // Array that keep track of rain 'drops' position
        private Brush backgroundBrush;
        private Brush textBrush;
        private double LetterAdvanceWidth;          //single letter height calculate from glyph typeface
        private double LetterAdvanceHeight;         // single letter height calculate from glyph typeface
        private int _animationStep = 0;
        private long _lastTick = 0;

        private Random random = new Random();
        private Bitmap FullScreenBitmap;            //render current visualization for animation needs

        System.Threading.Timer timer;

        // The character used for the rain will be randomly choose from this string
        private String AvailableLetterChars = "abcdefghijklmnopqrstuvwxyz1234567890";
        private Font MatrixFont { get; set; }
        public MatrixRain(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            int timerInterval = 60;                     // The number of mSec between each frame.
            FullScreenBitmap = fullScreenBitmap;
            timer = new Timer(new TimerCallback(_animationTimer_Tick), null, 0, timerInterval);
            Initialize(width, height);
        }
        private void Initialize(int width, int height)
        {
            MatrixFont = Primitives.Resource.GetFont(Resource.FontResources.MatrixFont);
            backgroundBrush = new SolidColorBrush(ColorUtility.ColorFromRGB(255, 0, 0));
            textBrush = new SolidColorBrush(ColorUtility.ColorFromRGB(255, 0, 255));
            LetterAdvanceWidth = MatrixFont.CharWidth('c');
            LetterAdvanceHeight = MatrixFont.Height;
            BaselineOrigin = new PointAndChar(2, 2, " ");
            backgroundBrush.Opacity = 1;

            RainDrops = new int[(int)(FullScreenBitmap.Width / LetterAdvanceWidth)];
            for (var x = 0; x < RainDrops.Length; x++)
            {
                RainDrops[x] = 1;
            }
        }
        /**
         * <summary>
         * Method that occurs when the timer interval has elapsed. This method refresh the control to perform the animation
         * </summary>
         */
        private void _animationTimer_Tick(object state)
        {
            if (RainDrops != null & RainDrops.Length > 0)
            {
                //Black BG for the canvas to fade away letter
                FullScreenBitmap.DrawRectangle(colorOutline: Color.Black, thicknessOutline: 0,
                                                x: 0, y: 0,
                                                width: FullScreenBitmap.Width, height: FullScreenBitmap.Height,
                                                xCornerRadius: 0, yCornerRadius: 0,
                                                colorGradientStart: Color.Black,
                                                xGradientStart: 0, yGradientStart: 0,
                                                colorGradientEnd: Color.Black,
                                                xGradientEnd: FullScreenBitmap.Width, yGradientEnd: FullScreenBitmap.Height,
                                                opacity: 255);

                ArrayList glyphPointOffsets = new ArrayList();
                PointAndChar pac = new PointAndChar();

                for (var i = 0; i < RainDrops.Length; i++)         // looping over drops
                {
                    // new drop position
                    double x = BaselineOrigin.x + LetterAdvanceWidth * i;
                    double y = BaselineOrigin.y + LetterAdvanceHeight * RainDrops[i];

                    // check if new letter does not go outside the image
                    if (y + LetterAdvanceHeight < FullScreenBitmap.Height)
                    {
                        char randomLetter = AvailableLetterChars[random.Next(AvailableLetterChars.Length - 1)];
                        pac.x = (int)x;
                        pac.y = (int)y;
                        pac.Character = randomLetter.ToString();
                    }
                    //sending the drop back to the top randomly after it has crossed the image
                    //adding a randomness to the reset to make the drops scattered on the Y axis
                    if (RainDrops[i] * LetterAdvanceHeight > FullScreenBitmap.Height && random.NextDouble() > 0.775)
                    {
                        RainDrops[i] = 0;
                    }
                    FullScreenBitmap.DrawText(pac.Character, MatrixFont, Color.Green, pac.x, pac.y);


      //              FullScreenBitmap.DrawTextInRect(pac.Character, pac.x, pac.y, width: MatrixFont.AverageWidth, MatrixFont.Height, Bitmap.DT_AlignmentCenter, Color.Green, MatrixFont);
                    Console.WriteLine($"{pac.Character}:[{pac.x},{pac.y}]");
                    //incrementing Y coordinate
                    RainDrops[i]++;
                    FullScreenBitmap.Flush();
                }
            }
        }
    }
}
