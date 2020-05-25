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
using System.Collections;

namespace Tetris.GameLogic
{
    /// <summary>
    /// Singleton class representing all Tetris blocks
    /// </summary>
    public class GameBlocks
    {
        private static GameBlocks _instance = new GameBlocks();
        private ByteMatrix[] _blocks;        

        public GameBlocks()
        {
            _blocks = new ByteMatrix[7];

            // O
            _blocks[0] = new ByteMatrix(2, 2);
            _blocks[0].SetCells(0, 0, new byte[4] { 1, 1, 
                                                    1, 1});
            // J
            _blocks[1] = new ByteMatrix(3, 2);
            _blocks[1].SetCells(0, 0, new byte[6] { 2, 2, 
                                                    2, 0, 
                                                    2, 0});
            // L
            _blocks[2] = new ByteMatrix(3, 2);
            _blocks[2].SetCells(0, 0, new byte[6] { 3, 0,
                                                    3, 0,
                                                    3, 3});
            // T
            _blocks[3] = new ByteMatrix(2, 3);
            _blocks[3].SetCells(0, 0, new byte[6] { 0, 4, 0,
                                                    4, 4, 4});
            // Z
            _blocks[4] = new ByteMatrix(2, 3);
            _blocks[4].SetCells(0, 0, new byte[6] { 5, 5, 0,
                                                    0, 5, 5});
            // S
            _blocks[5] = new ByteMatrix(2, 3);
            _blocks[5].SetCells(0, 0, new byte[6] { 0, 6, 6,
                                                    6, 6, 0});
            // I
            _blocks[6] = new ByteMatrix(4, 1);
            _blocks[6].SetCells(0, 0, new byte[4] { 7, 
                                                    7,
                                                    7,
                                                    7,});
        }

        /// <summary>
        /// Returns new instance of ByteMatrix representing tetris block
        /// </summary>
        /// <param name="blockId">Block to return</param>
        /// <returns>New instanc of ByteMatrix</returns>
        public ByteMatrix GetBlock(int blockId)
        {
            return new ByteMatrix(_blocks[blockId]);
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static GameBlocks Instance
        {
            get { return _instance; }
        }
    }
}
