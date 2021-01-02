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

namespace Tetris.GameLogic
{
    /// <summary>
    /// Class represents whole game of tetris. 
    /// Game logic and presentation layer are completely separated
    /// </summary>
    public class GameUniverse
    {
        // Game field dimensions
        const int FIELD_COLS = 10;
        const int FIELD_ROWS = 15;

        // Current block coords
        private int blockX, blockY;        
        private ByteMatrix currentBlock, nextBlock;
        // Game field
        private readonly ByteMatrix field = new ByteMatrix(FIELD_ROWS, FIELD_COLS);
        // Game statistics
        private readonly GameStatistics gameStats = new GameStatistics();

        private Random random;
        /// <summary>
        /// Inits the game - clears game statistics        
        /// </summary>
        public void Init()
        {
            random = new Random();
         //   Randomize();      
             
            gameStats.Score = 0;
            gameStats.Level = 1;
            gameStats.LinesCompleted = 0;
            gameStats.GameOver = false;
            gameStats.NextLevel = false;
        }

        /// <summary>
        /// Start game at given level
        /// </summary>
        /// <param name="initLevel">Level to start</param>
        public void StartLevel(int initLevel)
        {            
            gameStats.Level = initLevel;
            gameStats.NextLevel = true;
            gameStats.Interval = (int)(800 / (gameStats.Level * 0.80));
            field.Clear();
            NewBlock();            
        }

        /// <summary>
        /// Method is called whenever game-step should be done.
        /// This method is called by timer from presentation layer.
        /// </summary>
        public void StepUniverse()
        {
            StepDown();
            gameStats.GameOver = !Check(currentBlock, blockX, blockY);
        }

        /// <summary>
        /// Move current block left
        /// </summary>
        public void StepLeft()
        {
            if (Check(currentBlock, BlockX - 1, BlockY))
                blockX--;
        }

        /// <summary>
        /// Move current block right
        /// </summary>
        public void StepRight()
        {
            if (Check(currentBlock, BlockX + 1, BlockY))
                blockX++;
        }

        /// <summary>
        /// Rotate current block
        /// </summary>
        public void Rotate()
        {
            ByteMatrix rotated = new ByteMatrix(currentBlock.Columns, currentBlock.Rows);

            // Rotate the object
            for (int row = 0; row < currentBlock.Rows; row++)
                for (int col = 0; col < currentBlock.Columns; col++)
                    rotated.SetCell(rotated.Rows - col - 1, row, currentBlock.GetCell(row, col));

            // Check whether rotate object fits the field
            if (Check(rotated, blockX, blockY))
                currentBlock = new ByteMatrix(rotated);
        }

        /// <summary>
        /// Drop current block down
        /// </summary>
        public void DropDown()
        {
            // This loop allows to drop down the block and still move it when it's down
            while (Check(currentBlock, blockX, blockY + 1))
            {
                StepDown();
            }
        }

        /// <summary>
        /// Creates new block from 'next-block' and generates new nex-block
        /// </summary>
        private void NewBlock()
        {
            int next;

            // nextBlock is null on first execution of the game
            if (nextBlock == null)
            {
                next = random.Next(7);
                nextBlock = GameBlocks.Instance.GetBlock(next);
            }

            currentBlock = new ByteMatrix(nextBlock);

            next = random.Next(7);
            nextBlock = GameBlocks.Instance.GetBlock(next);

            blockX = 4;
            blockY = 0;
        }

        /// <summary>
        /// Moves current block down
        /// </summary>
        private void StepDown()
        {
            // Check whether block can fall down
            if (Check(currentBlock, blockX, blockY + 1))
            {
                blockY++;
                return;
            }

            // We reached the field-bottom
            // Set block as part of the field
            for (int row = 0; row < currentBlock.Rows; row++)
                for (int col = 0; col < currentBlock.Columns; col++)
                {
                    byte value = currentBlock.GetCell(row, col);
                    if (value != 0)
                        field.SetCell(row + blockY, col + blockX, value);
                }

            // Find and remove all full lines
            ProcessFullLines();

            // Get new block
            NewBlock();
        }

        /// <summary>
        /// Checks whether the given tetrisBlock can be on given coords
        /// </summary>
        /// <param name="block">GameBlock</param>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <returns>True if block can be on given coords</returns>
        private bool Check(ByteMatrix block, int x, int y)
        {
            for (int row = 0; row < block.Rows; row++)
                for (int col = 0; col < block.Columns; col++)
                {
                    int newX = col + x;
                    int newY = row + y;

                    // if block is out of the field then return false
                    if ((newX < 0 || newX > FIELD_COLS - 1 || newY < 0 || newY > FIELD_ROWS - 1) && block.GetCell(row, col) != 0)
                        return false;

                    // if block is in the field but there is no free place to fall, return false
                    if (!(newX < 0 || newX > FIELD_COLS - 1 || newY < 0 || newY > FIELD_ROWS - 1))
                        if (field.GetCell(newY, newX) != 0 && block.GetCell(row, col) != 0) 
                            return false;
                }

            return true;
        }

        /// <summary>
        /// Remove completed lines and adjust score and level
        /// </summary>
        private void ProcessFullLines()
        {
            int count = 0;
            for (int row = 1; row < FIELD_ROWS; row++)
            {
                bool fullLine = true;
                for (int col = 0; col < FIELD_COLS; col++)
                {
                    if (field.GetCell(row, col) == 0) 
                        fullLine = false;
                }

                // If line is full - remove it                
                if (fullLine)
                {                    
                    count++;
                    for (int drow = row; drow > 0; drow--)
                        for (int dcol = 0; dcol < FIELD_COLS; dcol++)
                            field.SetCell(drow, dcol, field.GetCell(drow - 1, dcol));
                }
            }

            // If there were some completed line - recount score
            if (count > 0)
            {
                gameStats.Score += count * (gameStats.Level * 10);
                gameStats.LinesCompleted += count;

                // Start new level when enough completed lines is reached
                int nextLevel = (gameStats.LinesCompleted / FIELD_ROWS) + 1;
                if (nextLevel > gameStats.Level)
                {
                    gameStats.Level = nextLevel;
                    StartLevel(gameStats.Level);
                }
            }
        }

        #region Properties
        /// <summary>
        /// Current block
        /// </summary>
        public ByteMatrix CurrentBlock
        {
            get { return currentBlock; }
        }

        /// <summary>
        /// Next block
        /// </summary>
        public ByteMatrix NextBlock
        {
            get { return nextBlock; }
        }

        /// <summary>
        /// Game field
        /// </summary>
        public ByteMatrix Field
        {
            get { return field; }
        }

        /// <summary>
        /// X coord of current block
        /// </summary>
        public int BlockX
        {
            get { return blockX; }
        }

        /// <summary>
        /// Y coord of current block
        /// </summary>
        public int BlockY
        {
            get { return blockY; }
        }

        /// <summary>
        /// Game statistic
        /// </summary>
        public GameStatistics Statistics
        {
            get { return gameStats; }
        }
        #endregion
    }
}
