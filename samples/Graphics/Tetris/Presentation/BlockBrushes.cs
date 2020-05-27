//-----------------------------------------------------------------------------
// 
//  Tetris game for .NET Micro Framework
//
//  http://bansky.net/blog
// 
// This code was written by Pavel Bansky. It is released under the terms of 
// the Creative Commons "Attribution NonCommercial ShareAlike 2.5" license.
// http://creativecommons.org/licenses/by-nc-sa/2.5/
//-----------------------------------------------------------------------------

using System;
using nanoFramework.Presentation.Media;


namespace Tetris.Presentation
{
    /// <summary>
    /// Singleton class representing color brushes for game blocks
    /// </summary>
    public class BlockBrushes
    {
        private static BlockBrushes _instance = new BlockBrushes();
        private Brush[] _blockBrush;

        public BlockBrushes()
        {
            _blockBrush = new SolidColorBrush[7];

            // O
            _blockBrush[0] = new SolidColorBrush(ColorUtility.ColorFromRGB(240, 240, 0));
            // J
            _blockBrush[1] = new SolidColorBrush(ColorUtility.ColorFromRGB(0, 0, 240));
            // L
            _blockBrush[2] = new SolidColorBrush(ColorUtility.ColorFromRGB(240, 160, 0));
            // T
            _blockBrush[3] = new SolidColorBrush(ColorUtility.ColorFromRGB(160, 0, 240));
            // Z
            _blockBrush[4] = new SolidColorBrush(ColorUtility.ColorFromRGB(240, 0, 0));
            // S
            _blockBrush[5] = new SolidColorBrush(ColorUtility.ColorFromRGB(0, 216, 0));
            // I
            _blockBrush[6] = new SolidColorBrush(ColorUtility.ColorFromRGB(0, 240, 240));
        }

        /// <summary>
        /// Returns brush by given index
        /// </summary>
        /// <param name="brushId">Brush index</param>
        /// <returns>Brush</returns>
        public Brush GetBrush(int brushId)
        {
            return _blockBrush[brushId];
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static BlockBrushes Instance
        {
            get { return _instance; }
        }
    }
}
