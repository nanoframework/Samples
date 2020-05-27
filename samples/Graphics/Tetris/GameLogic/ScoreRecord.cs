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
    /// Struct representing score record in high score table
    /// </summary>
    [Serializable]
    public struct ScoreRecord
    {
        public string Name;
        public int Score;
    }
}
