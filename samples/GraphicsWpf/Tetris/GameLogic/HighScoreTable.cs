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
    /// Class to keep and work with high score results
    /// </summary>
    [Serializable]
    public class HighScoreTable
    {
        /// <summary>
        /// HighScore Table
        /// </summary>
        public ScoreRecord[] Table;

        /// <summary>
        /// Creates new high score table filled with zero results
        /// </summary>
        public HighScoreTable()
        {
            Table = new ScoreRecord[5];

            for (int i = 0; i < Table.Length; i++)
            {
                Table[i] = new ScoreRecord();
                Table[i].Score = 0;
                Table[i].Name = "AAA";
            }
        }

        /// <summary>
        /// Adds new record to score table
        /// </summary>
        /// <param name="scoreRecord">ScoreRecord to add</param>
        /// <returns>Position where score was placed. Returns -1 when high score was not reached</returns>
        public int AddRecord(ScoreRecord scoreRecord)
        {
            int highScorePos = -1;

            // Find high score position to insert
            for (int i = 0; i < Table.Length; i++)
            {
                if (scoreRecord.Score > Table[i].Score)
                {
                    highScorePos = i;
                    break;
                }
            }

            // Move results down and insert new score
            if (highScorePos > -1)
            {
                for (int i = Table.Length - 2; i >= highScorePos; i--)
                {
                    Table[i + 1] = Table[i];
                }

                Table[highScorePos] = scoreRecord;
            }

            return highScorePos;
        }
    }
}
