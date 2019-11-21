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
    /// Class to keep gameStatistics
    /// </summary>
    public class GameStatistics
    {
        public int Score;
        public int LinesCompleted;
        public int Level;
        public int Interval;
        public bool GameOver;
        public bool NextLevel;
    }
}
