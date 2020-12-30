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
using nanoFramework.Presentation;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using Tetris.GameLogic;

namespace Tetris.Presentation
{
    /// <summary>
    /// Preview of the next tetris block
    /// </summary>
    public class NextBlockView : UIElement
    {
        const int COLUMNS = 7;
        const int ROWS = 6;
        const int COLUMN_WIDTH = 8;
        const int ROW_HEIGHT = 8;        

        GameUniverse _universe;
        Pen _blockPen, _linePen;
        Brush _fillBrush;

        /// <summary>
        /// Creates new NextBlockView element for given GameUniverse
        /// </summary>
        /// <param name="gameUniverse">GameUniverse to visualize</param>
        public NextBlockView(GameUniverse gameUniverse)
        {
            this._universe = gameUniverse;

            _blockPen = new Pen(Color.Black);
            _linePen = new Pen(Color.White, 4);
            _fillBrush = new SolidColorBrush(ColorUtility.ColorFromRGB(164, 164, 164));
        }

        public override void OnRender(nanoFramework.Presentation.Media.DrawingContext dc)
        {
            // Draw outline rectangle
            dc.DrawRectangle(_fillBrush, _linePen, 0, 0, COLUMN_WIDTH * COLUMNS, ROW_HEIGHT * ROWS);            

            // Performance tuning - save all property calls to variables
            int blockCols = _universe.NextBlock.Columns;
            int blockRows = _universe.NextBlock.Rows;

            int offsetX = (Width - (blockCols * COLUMN_WIDTH) + 2) / 2;
            int offsetY = (Height - (blockRows * ROW_HEIGHT) + 2) / 2;

            // Draw block
            for (int row = 0; row < blockRows; row++)
                for (int col = 0; col < blockCols; col++)
                {
                    int brushType = _universe.NextBlock.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        dc.DrawRectangle(BlockBrushes.Instance.GetBrush(brushType),
                                         _blockPen,
                                         (col * COLUMN_WIDTH) + offsetX,
                                         (row * ROW_HEIGHT) + offsetY,
                                         COLUMN_WIDTH - 1,
                                         ROW_HEIGHT - 1);
                }

            base.OnRender(dc);
        }

        protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight)
        {
            // Set desired dimensions
            desiredHeight = Height;
            desiredWidth = Width;
        }

        /// <summary>
        /// Gets Width of the element
        /// </summary>
        public new int Width
        {
            get { return COLUMN_WIDTH * COLUMNS; }
        }

        /// <summary>
        /// Gets Height of the element
        /// </summary>
        public new int Height
        {
            get { return ROW_HEIGHT * ROWS; }
        }
    }
}
