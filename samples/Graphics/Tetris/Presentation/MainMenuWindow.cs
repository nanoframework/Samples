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
using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using System.Drawing;

namespace Tetris.Presentation
{
    /// <summary>
    /// Game main menu window
    /// </summary>
    public class MainMenuWindow : Window
    {
        GameWindow gameWindow;
        ListBox menuListBox;
        readonly TetrisApp parentApp;

        /// <summary>
        /// Creates new MainMenuWindow
        /// </summary>
        /// <param name="parentApp">Parent application</param>
        public MainMenuWindow(TetrisApp parentApp)
        {
            this.parentApp = parentApp;

            InitializeComponents();
        }

        /// <summary>
        /// Creates all WPF controls of the window
        /// </summary>
        private void InitializeComponents()
        {
            this.Width = DisplayControl.ScreenWidth;
            this.Height = DisplayControl.ScreenHeight;
            this.Background = new SolidColorBrush(Color.Black);

            Image logoImage = new Image(nfResource.GetBitmap(nfResource.BitmapResources.Logo));

            #region ListBox event handler
            Color selectedItemColor = Color.White;
            Color unselectedItemColor = Color.FromArgb(206, 206, 206);
            Brush selectedBackground = new SolidColorBrush(Color.FromArgb(0, 148, 255));

            menuListBox = new ListBox()
            {
                Background = this.Background,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Event handler for menu items
            menuListBox.SelectionChanged += delegate (object sender, SelectionChangedEventArgs e)
            {
                int previousSelectedIndex = e.PreviousSelectedIndex;
                if (previousSelectedIndex != -1)
                {
                    // Change previously-selected listbox item color to unselected color
                    ((Text)menuListBox.Items[previousSelectedIndex].Child).ForeColor = unselectedItemColor;
                    menuListBox.Items[previousSelectedIndex].Background = menuListBox.Background;
                }

                // Change newly-selected listbox item color to selected color and background
                ((Text)menuListBox.Items[e.SelectedIndex].Child).ForeColor = selectedItemColor;
                menuListBox.Items[e.SelectedIndex].Background = selectedBackground;
            };
            #endregion

            #region Menu Items
            // Menu items from nfResource
            string[] menuItems = new string[4] { nfResource.GetString(nfResource.StringResources.RookieLevel),
                                                 nfResource.GetString(nfResource.StringResources.AdvancedLevel),
                                                 nfResource.GetString(nfResource.StringResources.ExtremeLevel),
                                                 nfResource.GetString(nfResource.StringResources.ViewHighScore)};
            // Add items into listbox
            foreach (string item in menuItems)
            {
                Text itemText = new Text(nfResource.GetFont(nfResource.FontResources.NinaB), item)
                {
                    Width = this.Width - 40,
                    ForeColor = unselectedItemColor,
                    TextAlignment = TextAlignment.Center
                };
                itemText.SetMargin(5);

                ListBoxItem listBoxItem = new ListBoxItem()
                {
                    Background = menuListBox.Background,
                    Child = itemText
                };

                menuListBox.Items.Add(listBoxItem);
            }

            menuListBox.SelectedIndex = 0;
            #endregion

            // Add all controls to stack panel
            StackPanel mainStackPanel = new StackPanel(Orientation.Vertical);
            mainStackPanel.Children.Add(logoImage);
            mainStackPanel.Children.Add(menuListBox);

            this.Child = mainStackPanel;

            this.Visibility = Visibility.Visible;
            Buttons.Focus(menuListBox);
        }

        /// <summary>
        /// Button event handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (e.Button == Button.VK_SELECT)
            {
                switch (menuListBox.SelectedIndex)
                {
                    case 0:
                        StartGame(1);
                        break;
                    case 1:
                        StartGame(5);
                        break;
                    case 2:
                        StartGame(10);
                        break;
                    case 3:
                        ViewHighScore(-1);
                        break;
                }
            }
        }

        /// <summary>
        /// Focus event handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(FocusChangedEventArgs e)
        {
            // Whenever this window gets focus, it gives it to listbox
            Buttons.Focus(menuListBox);
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Start new game at specified level
        /// </summary>
        /// <param name="startLevel">Starting level</param>
        private void StartGame(int startLevel)
        {
            gameWindow = new GameWindow(parentApp);
            gameWindow.OnClose += new GameWindow.CloseDelegate(GameWindow_OnGameOver);
            gameWindow.StartGame(startLevel);
        }

        /// <summary>
        /// Shows HighScore table
        /// </summary>
        /// <param name="position"></param>
        private void ViewHighScore(int position)
        {
            HighScoreWindow scoreWindow = new HighScoreWindow(parentApp);
        }

        /// <summary>
        /// Event handler for GameWindws close event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="gameStatistics">Game statistics</param>
        private void GameWindow_OnGameOver(object sender, GameStatistics gameStatistics)
        {
            ScoreRecord scoreRecord = new ScoreRecord()
            {
                Score = gameStatistics.Score
            };

            // Add score into HighScore table
            int scoreIndex = parentApp.HighScore.AddRecord(scoreRecord);

            // Show high score window
            HighScoreWindow scoreWindow = new HighScoreWindow(parentApp);

            // if high score has been reached then edit name
            if (scoreIndex > -1)
                scoreWindow.EditItem(scoreIndex);
        }
    }
}
