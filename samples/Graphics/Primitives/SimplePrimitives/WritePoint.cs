// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using System.Drawing;
using System.Threading;

namespace Primitives.SimplePrimitives
{
    public class WritePoint
    {
        public WritePoint()
        {
            DrawWithushort();
            Thread.Sleep(1000);
            DisplayControl.Clear();
            DrawWithColor();
        }

        private void DrawWithushort()
        {
            ushort[] toDraw = new ushort[100];
            var blue = Color.Blue.ToBgr565();
            var red = Color.Red.ToBgr565();
            var green = Color.Green.ToBgr565();
            var white = Color.White.ToBgr565();

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = blue;
            }

            DisplayControl.Write(0, 0, 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = red;
            }

            DisplayControl.Write((ushort)(DisplayControl.ScreenWidth - 10), (ushort)(DisplayControl.ScreenHeight - 10), 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = green;
            }

            DisplayControl.Write((ushort)(DisplayControl.ScreenWidth - 10), 0, 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = white;
            }

            DisplayControl.Write(0, (ushort)(DisplayControl.ScreenHeight - 10), 10, 10, toDraw);
        }

        private void DrawWithColor()
        {
            Color[] toDraw = new Color[100];
            var blue = Color.Blue;
            var red = Color.Red;
            var green = Color.Green;
            var white = Color.White;

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = red;
            }

            DisplayControl.Write(0, 0, 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = green;
            }

            DisplayControl.Write((ushort)(DisplayControl.ScreenWidth - 10), (ushort)(DisplayControl.ScreenHeight - 10), 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = white;
            }

            DisplayControl.Write((ushort)(DisplayControl.ScreenWidth - 10), 0, 10, 10, toDraw);

            for (int i = 0; i < toDraw.Length; i++)
            {
                toDraw[i] = blue;
            }

            DisplayControl.Write(0, (ushort)(DisplayControl.ScreenHeight - 10), 10, 10, toDraw);
        }
    }
}
