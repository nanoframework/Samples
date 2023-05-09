// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Drawing;

namespace Primitives.SimplePrimitives
{
    public class BouncingBalls
    {
        struct Rectangle
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public Rectangle(int x, int y, int width, int height)
            {
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
            }
        }
        
        struct Point { public int X; public int Y; };
        private Rectangle[] BallLocation;
        private Point[] BallVelocity;
        private Bitmap ScreenBitmap { get; set; }

        public BouncingBalls(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            ScreenBitmap = fullScreenBitmap;
            SetupBalls();

            for (int iCount = 0; iCount < 180; iCount++)
            {
                MoveBalls();
                DrawBalls();
            }

        }

        private void SetupBalls()
        {
            Random rand = new Random();
            const int num_balls = 12;
            int vx = 0;
            int vy = 0;

            BallLocation = new Rectangle[num_balls];
            BallVelocity = new Point[num_balls];

            for (int iBall = 0; iBall < num_balls; iBall++)
            {
                int width = rand.Next(3, 50);
                BallLocation[iBall] = new Rectangle
                {
                    X = rand.Next(0, ScreenBitmap.Width - 2 * width),
                    Y = rand.Next(0, ScreenBitmap.Height - 2 * width),
                    Width = width,
                    Height = width
                };
                // Setup 1/2 the balls with different speeds
                if (iBall % 2 == 0)
                {
                    vx = rand.Next(2, 10);
                    vy = rand.Next(2, 10);
                }
                else
                {
                    vx = rand.Next(12, 25);
                    vy = rand.Next(12, 25);
                }

                // Setup random directions
                if (rand.Next(0, 2) == 0) vx = -vx;
                if (rand.Next(0, 2) == 0) vy = -vy;
                BallVelocity[iBall] = new Point { X = vx, Y = vy };
            }
        }

        private void MoveBalls()
        {
            for (int ball_num = 0;
                ball_num < BallLocation.Length;
                ball_num++)
            {
                // Move the ball.
                int new_x = BallLocation[ball_num].X +
                    BallVelocity[ball_num].X;
                int new_y = BallLocation[ball_num].Y +
                    BallVelocity[ball_num].Y;
                if (new_x < 0)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                }
                else if (new_x + BallLocation[ball_num].Width > ScreenBitmap.Width)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                }
                if (new_y < 0)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                }
                else if (new_y + BallLocation[ball_num].Height > ScreenBitmap.Height)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                }

                BallLocation[ball_num] = new Rectangle(new_x, new_y,
                                                       BallLocation[ball_num].Width,
                                                       BallLocation[ball_num].Height);
            }
        }

        private void DrawBalls()
        {
            ScreenBitmap.Clear();
            for (int i = 0; i < BallLocation.Length; i++)
            {
                ScreenBitmap.DrawEllipse(Color.Yellow, 1, BallLocation[i].X, BallLocation[i].Y, BallLocation[i].Width, BallLocation[i].Height,
                                              Color.Black, 0, 0, Color.Black, 0, 0, Bitmap.OpacityOpaque);
            }
            ScreenBitmap.Flush();
        }
    }
}


