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
using nanoFramework.UI.Input;
using Tetris.GameLogic;
using Tetris;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation;
using nanoFramework.Presentation.Media;

namespace Tetris.Presentation
{
    /// <summary>
    /// Windows with the high score table
    /// </summary>
    public class HighScoreWindow : Window
    {
        /// <summary>
        /// Length of the name to be edited
        /// </summary>
        const int NAME_LENGTH = 3;

        /// <summary>
        /// Allowed chars in names
        /// </summary>
        private readonly char[] allowedChars = new char[26] { 'A', 'B', 'C', 'D',
                                                              'E', 'F', 'G', 'H',
                                                              'I', 'J', 'K', 'L',
                                                              'M', 'N', 'O', 'P',
                                                              'Q', 'R', 'S', 'T',
                                                              'U', 'V', 'W', 'X',
                                                              'Y', 'Z'};

        private int selectedItem, selectedLetter;
        private readonly int[] letterIndexes;
        private bool editMode;

        readonly TetrisApp parentApp;
        ListBox scoreListBox;
        TextFlow hintTextFlow;

        /// <summary>
        /// Crates new HighScoreWindow
        /// </summary>
        /// <param name="parentApp">Parent application</param>
        public HighScoreWindow(TetrisApp parentApp)
        {
            this.parentApp = parentApp;

            editMode = false;
            letterIndexes = new int[NAME_LENGTH];

            InitializeComponents();
        }

        /// <summary>
        /// Start edit mode for given high score item
        /// </summary>
        /// <param name="index">Index in high to edit</param>
        public void EditItem(int index)
        {
            if (index < scoreListBox.Items.Count)
            {
                editMode = true;
                selectedItem = index;
                selectedLetter = 0;
                ScoreItem scoreItem = (ScoreItem)scoreListBox.Items[selectedItem];
                scoreItem.Highlite = true;
                UpdateName();
                UpdateHint();
            }
        }

        /// <summary>
        /// Creates all WPF controls of the window
        /// </summary>
        private void InitializeComponents()
        {
            this.Width = DisplayControl.ScreenWidth;
            this.Height = DisplayControl.ScreenHeight;
            this.Background = new SolidColorBrush(Color.Black);

            #region Caption
            Text caption = new Text(nfResource.GetString(nfResource.StringResources.HighScore))
            {
                Font = nfResource.GetFont(nfResource.FontResources.Consolas23),
                ForeColor = Color.Red,
                TextAlignment = TextAlignment.Center
            };
            caption.SetMargin(0, 10, 0, 15);
            #endregion

            #region Score ListBox
            scoreListBox = new ListBox()
            {
                Background = this.Background,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            foreach (ScoreRecord scoreRecord in parentApp.HighScore.Table)
            {
                ScoreItem scoreItem = new ScoreItem(scoreRecord.Name, scoreRecord.Score)
                {
                    Background = scoreListBox.Background,
                };
                scoreListBox.Items.Add(scoreItem);
            }
            #endregion

            #region HintLabel
            hintTextFlow = new TextFlow();
            hintTextFlow.SetMargin(0, 15, 0, 0);
            hintTextFlow.TextAlignment = TextAlignment.Center;
            UpdateHint();
            #endregion

            StackPanel mainStack = new StackPanel(Orientation.Vertical)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainStack.Children.Add(caption);
            mainStack.Children.Add(scoreListBox);
            mainStack.Children.Add(hintTextFlow);

            this.Child = mainStack;

            this.Visibility = Visibility.Visible;
            Buttons.Focus(this);
        }

        /// <summary>
        /// Button down handler
        /// </summary>        
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            // Close windows if not editing
            if (!editMode)
            {
                this.Close();
                parentApp.SetFocus();
            }
            else
            {
                switch (e.Button)
                {
                    case Button.VK_UP:
                        letterIndexes[selectedLetter]++;
                        break;
                    case Button.VK_DOWN:
                        letterIndexes[selectedLetter]--;
                        break;
                    case Button.VK_LEFT:
                        selectedLetter--;
                        break;
                    case Button.VK_RIGHT:
                        selectedLetter++;
                        break;
                    case Button.VK_SELECT:
                        SaveItem();
                        break;

                }
                UpdateName();
            }
        }

        /// <summary>
        /// Saves edited item into HighScore Table.
        /// Ends the editing mode.
        /// </summary>
        private void SaveItem()
        {
            editMode = false;
            ScoreItem scoreItem = (ScoreItem)scoreListBox.Items[selectedItem];
            scoreItem.Highlite = false;
            parentApp.HighScore.Table[selectedItem].Name = LettersToString();
            parentApp.PersistHighScore();
            UpdateHint();
        }

        /// <summary>
        /// Updates edited name
        /// </summary>
        private void UpdateName()
        {
            // Test selected letter value
            if (selectedLetter >= NAME_LENGTH)
                selectedLetter = NAME_LENGTH - 1;
            else if (selectedLetter < 0)
                selectedLetter = 0;

            // Test char index value
            if (letterIndexes[selectedLetter] >= allowedChars.Length)
                letterIndexes[selectedLetter] = 0;
            else if (letterIndexes[selectedLetter] < 0)
                letterIndexes[selectedLetter] = allowedChars.Length - 1;

            // Update scoreItem
            ScoreItem scoreItem = (ScoreItem)scoreListBox.Items[selectedItem];
            scoreItem.SelectedLetter = selectedLetter;
            scoreItem.Name = LettersToString();
        }

        /// <summary>
        /// Updates hints according to editMode value
        /// </summary>
        private void UpdateHint()
        {
            Font hintFont = nfResource.GetFont(nfResource.FontResources.NinaB);
            Color hintColor = ColorUtility.ColorFromRGB(206, 206, 206);

            hintTextFlow.TextRuns.Clear();

            // Print editing hints
            if (editMode)
            {
                hintTextFlow.TextRuns.Add(
                            nfResource.GetString(nfResource.StringResources.UseArrows),
                            hintFont,
                            hintColor);
                hintTextFlow.TextRuns.Add(TextRun.EndOfLine);
                hintTextFlow.TextRuns.Add(
                            nfResource.GetString(nfResource.StringResources.PressSelect),
                            hintFont,
                            hintColor);
            }
            else
            {
                hintTextFlow.TextRuns.Add(
                            nfResource.GetString(nfResource.StringResources.PressAnyKey),
                            hintFont,
                            hintColor);
            }

            hintTextFlow.Invalidate();
        }

        /// <summary>
        /// Converts letter array into string
        /// </summary>
        /// <returns>String</returns>
        private string LettersToString()
        {
            string output = string.Empty;
            foreach (int letter in letterIndexes)
                output += allowedChars[letter].ToString();

            return output;
        }
    }
}
