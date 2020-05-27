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

namespace Tetris.GameLogic
{
    /// <summary>
    /// Class representing two dimensional byte array
    /// </summary>
    public class ByteMatrix
    {
        private byte[] _baseArray;
        private int _rows, _columns;

        /// <summary>
        /// Creates new matrix
        /// </summary>
        /// <param name="rows">Rows cont</param>
        /// <param name="columns">Columns count</param>
        public ByteMatrix(int rows, int columns)
        {
            this._baseArray = new byte[rows * columns];
            this._columns = columns;
            this._rows = rows;
        }

        /// <summary>
        /// Clonning constructor
        /// </summary>
        /// <param name="sourceMatrix">Matrix to make copy from</param>
        public ByteMatrix(ByteMatrix sourceMatrix)
        {
            int length = sourceMatrix.BaseArray.Length;
            this._baseArray = new byte[length];

            Array.Copy(sourceMatrix.BaseArray, this._baseArray, length);            
            this._rows = sourceMatrix.Rows;
            this._columns = sourceMatrix.Columns;
        }

        /// <summary>
        /// Gets cell value on specific location
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="column">Column</param>
        /// <returns>Cell value</returns>
        public byte GetCell(int row, int column)
        {
            return _baseArray[row * Columns + column];
        }

        /// <summary>
        /// Sets cell value on specific location
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="column">Columns</param>
        /// <param name="value">Value</param>
        public void SetCell(int row, int column, byte value)
        {
            _baseArray[row * Columns + column] = value;
        }

        /// <summary>
        /// Adds array into matrix, begginig on given location
        /// </summary>
        /// <param name="row">Row where to start</param>
        /// <param name="column">Column where to start</param>
        /// <param name="inputArray">Array to add</param>
        public void SetCells(int row, int column, byte[] inputArray)
        {
            Array.Copy(inputArray, 0, _baseArray, row * Columns + column, inputArray.Length);
        }

        /// <summary>
        /// Fills whole matrix with zeroes
        /// </summary>
        public void Clear()
        {
            int length = _baseArray.Length;
            for (int i = 0; i < length; i++)
                _baseArray[i] = 0;    
        }

        #region Properties
        /// <summary>
        /// Matrix rows
        /// </summary>
        public int Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Matrix columns
        /// </summary>
        public int Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Metrix cells
        /// </summary>
        public int Length
        {
            get { return _baseArray.Length; }
        }

        /// <summary>
        /// Base array of matrix
        /// </summary>
        public byte[] BaseArray
        {
            get { return _baseArray; }
        }
        #endregion
    }
}
