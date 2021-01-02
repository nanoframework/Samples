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
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using nanoFramework.UI.Threading;
using nanoFramework.Runtime.Events;
using nanoFramework.Presentation;

namespace Tetris.Presentation
{
    /// <summary>
    /// ListBoxItem with score data
    /// </summary>
    public class ScoreItem : ListBoxItem    
    {
        /// <summary>
        /// Maximum length of name displayed
        /// </summary>
        const int NAME_LENGTH = 3;
        /// <summary>
        /// Maximum length of score displayed
        /// </summary>
        const int SCORE_LENGTH = 4;

        // Property fields
        private string _name;
        private readonly int _score;
        private int _selectedLetter;
        private bool _highlite;
        readonly Text[] nameLetters = new Text[NAME_LENGTH];
        Text scoreLabel;
        StackPanel nameStack;
        DispatcherTimer blinkTimer;
        readonly Color normalColor = ColorUtility.ColorFromRGB(206, 206, 206);
        readonly Color selectedColor = Color.Black;    

        /// <summary>
        /// Creates new listbox score item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        public ScoreItem(string name, int score)
            : base()
        {
            // make name empty.string if null
            this._name = (name != null) ? name : string.Empty;
            this._score = score;

            InitializeComponents();
        }

        /// <summary>
        /// Creates all WPF controls of the window
        /// </summary>
        private void InitializeComponents()
        {
            blinkTimer = new DispatcherTimer(this.Dispatcher)
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            blinkTimer.Tick += new EventHandler(BlinkTimer_Tick);

            #region Name Characters
            nameStack = new StackPanel(Orientation.Horizontal)
            {
                HorizontalAlignment = HorizontalAlignment.Left
            };
            nameStack.SetMargin(0, 0, 20, 5);

            for (int i = 0; i < NAME_LENGTH; i++)
            {
                nameLetters[i] = new Text
                {
                    Font = nfResource.GetFont(nfResource.FontResources.Consolas23)
                };
                nameStack.Children.Add(nameLetters[i]);
            }

            UpdateLetters();
            #endregion

            #region Score Label
            scoreLabel = new Text(ScoreToString(this._score))
            {
                ForeColor = normalColor,
                Font = nfResource.GetFont(nfResource.FontResources.Consolas23),
                TextAlignment = TextAlignment.Right
            };
            #endregion

            StackPanel mainStack = new StackPanel(Orientation.Horizontal)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainStack.Children.Add(nameStack);
            mainStack.Children.Add(scoreLabel);

            this.Child = mainStack;
        }

        /// <summary>
        /// Timer handler - blinks active letter
        /// </summary>
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {            
            if (_selectedLetter >= 0 && _selectedLetter < NAME_LENGTH)
                nameLetters[_selectedLetter].ForeColor = (nameLetters[_selectedLetter].ForeColor == selectedColor) ? normalColor : selectedColor;            
        }

        /// <summary>
        /// Updates name and score according to conditions
        /// </summary>
        private void UpdateLetters()
        {
            // Fill Label with Name letters
            for(int i = 0; i < NAME_LENGTH; i ++)
            {
                nameLetters[i].ForeColor = normalColor;
                if (i < _name.Length)
                    nameLetters[i].TextContent = _name[i].ToString();
            }

            // Start timer if highlite is enabled
            if (Highlite)
            {
                blinkTimer.Start();
                nameLetters[SelectedLetter].ForeColor = selectedColor;
            }
            else
                blinkTimer.Stop();
        }

        /// <summary>
        /// Converts score number to string
        /// </summary>
        /// <param name="scoreToString">Number</param>
        /// <returns>String</returns>
        private string ScoreToString(int scoreToString)
        {
            string scoreString = scoreToString.ToString();
            int len = scoreString.Length;
            string zeroes = (len < SCORE_LENGTH) ?
                new string('0', SCORE_LENGTH - len) : string.Empty;

            return zeroes + scoreToString;
        }

        /// <summary>
        /// Gets or sets name to be displayed
        /// </summary>
        public string Name
        {
            get { return Name; }
            set 
            { 
                _name = value;
                UpdateLetters();
            }
        }

        /// <summary>
        /// Gets or sets highlite mode
        /// </summary>
        public bool Highlite
        {
            get { return _highlite; }
            set 
            { 
                _highlite = value;
                UpdateLetters();
            }
        }

        /// <summary>
        /// Gets or sets highlited letter
        /// </summary>
        public int SelectedLetter
        {
            get { return _selectedLetter; }
            set
            {
                _selectedLetter = value;
                UpdateLetters();
            }
        }
    }
}
