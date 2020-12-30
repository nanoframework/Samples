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
using nanoFramework.UI;
using Tetris;
using Tetris.GameLogic;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation;

namespace Tetris.Presentation
{
    /// <summary>
    /// Game universe visualization element
    /// </summary>
    public class UniverseView : UIElement
    {
        const int COLUMN_WIDTH = 16;
        const int ROW_HEIGHT = 16;

        GameUniverse gameUniverse;
        Pen blockPen, gridPen;

        /// <summary>
        /// Creates new UniverseView for specified GameUniverse
        /// </summary>
        /// <param name="gameUniverse">GameUniverse to visualize</param>
        public UniverseView(GameUniverse gameUniverse)
        {
            this.gameUniverse = gameUniverse;

            blockPen = new Pen(Color.Black);
            gridPen = new Pen(ColorUtility.ColorFromRGB(25, 25, 25));
        }

        public override void OnRender(DrawingContext dc)
        {
            // Performance tuning
            // storing properties to variables for faster reuse
            int fieldCols = gameUniverse.Field.Columns;
            int fieldRows = gameUniverse.Field.Rows;

            #region Draw Grid
            int final = fieldRows * ROW_HEIGHT;
            for(int col = 0; col <= fieldCols; col++)
            {
                int x = col * COLUMN_WIDTH;                
                dc.DrawLine(gridPen, x, 0, x, final);
            }

            final = fieldCols * COLUMN_WIDTH;
            for (int row = 0; row <= fieldRows; row++)
            {
                int y = row * ROW_HEIGHT;
                dc.DrawLine(gridPen, 0, y, final, y);
            }
            #endregion

            #region Draw the game field
            for (int row = 0; row < fieldRows; row++)
                for (int col = 0; col < fieldCols; col++)
                {
                    int brushType = gameUniverse.Field.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        dc.DrawRectangle(BlockBrushes.Instance.GetBrush(brushType), 
                                         blockPen,
                                         col * COLUMN_WIDTH + 1,
                                         row * ROW_HEIGHT + 1,
                                         COLUMN_WIDTH - 1,
                                         ROW_HEIGHT - 1);
                }
            #endregion

            #region Draw the current object
            // Performance tunning
            fieldCols = gameUniverse.CurrentBlock.Columns;
            fieldRows = gameUniverse.CurrentBlock.Rows;

            for (int row = 0; row < fieldRows; row++)
                for(int col = 0; col < fieldCols; col++)
                {
                    int brushType = gameUniverse.CurrentBlock.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        dc.DrawRectangle(BlockBrushes.Instance.GetBrush(brushType), 
                                         blockPen, 
                                         (col + gameUniverse.BlockX) * COLUMN_WIDTH + 1, 
                                         (row + gameUniverse.BlockY) * ROW_HEIGHT + 1, 
                                         COLUMN_WIDTH - 1, 
                                         ROW_HEIGHT - 1);
                }
            #endregion

            #region Draw Game Over banner
            if (gameUniverse.Statistics.GameOver)
            {                
                string gameOverStr = nfResource.GetString(nfResource.StringResources.GameOver);
                Font gameOverFont = nfResource.GetFont(nfResource.FontResources.Consolas23);

                int gameOverY = (this.Height - gameOverFont.Height) / 2;
                int gameOverHeight = gameOverFont.Height + 3;

                dc.DrawRectangle(new LinearGradientBrush(Color.Gray, Color.White, 0, 0, this.Width, gameOverFont.Height * 2),
                                 new Pen(Color.White),
                                 2,
                                 gameOverY - 5,
                                 this.Width - 4,
                                 gameOverHeight + 5);

                dc.DrawText(ref gameOverStr,
                            gameOverFont,
                            Color.Red, 
                            0,
                            gameOverY, 
                            this.Width, 
                            gameOverHeight,
                            TextAlignment.Center,
                            TextTrimming.WordEllipsis);
            }
            #endregion

            base.OnRender(dc);
        }

        protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight)
        {
            // Set desired dimensions
            desiredWidth = Width;
            desiredHeight = Height;
        }

        /// <summary>
        /// Gets Width of the element
        /// </summary>
        public new int Width
        {
            get { return COLUMN_WIDTH * gameUniverse.Field.Columns + gridPen.Thickness; }
        }

        /// <summary>
        /// Get Height of the element
        /// </summary>
        public new int Height
        {
            get { return ROW_HEIGHT * gameUniverse.Field.Rows + gridPen.Thickness; }
        }
    }
}
