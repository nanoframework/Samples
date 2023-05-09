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
using nanoFramework.Presentation;
using Tetris.GameLogic;
using nanoFramework.UI.Input;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation.Controls;
using nanoFramework.UI.Threading;
using nanoFramework.Runtime.Events;
using System.Drawing;

namespace Tetris.Presentation
{
    /// <summary>
    /// Main gaming window with tetris grid 
    /// </summary>
    public class GameWindow : Window
    {
        readonly GameUniverse gameUniverse = new GameUniverse();
        UniverseView universeView;
        DispatcherTimer gameTimer;
        readonly TetrisApp parentApp;

        /// <summary>
        /// Close window delegate
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="gameStatistics">Game statistics</param>
        public delegate void CloseDelegate(object sender, GameStatistics gameStatistics);
        /// <summary>
        /// Event occurs when window is closed
        /// </summary>
        public event CloseDelegate OnClose;

        /// <summary>
        /// Creates new GameWindow
        /// </summary>
        /// <param name="parentApp">Parent application</param>
        public GameWindow(TetrisApp parentApp)
        {
            this.parentApp = parentApp;
            InitializeComponents();
        }

        /// <summary>
        /// Starts game on specified level
        /// </summary>
        /// <param name="startLevel">Level to start game</param>
        public void StartGame(int startLevel)
        {
            // Prepare universe
            gameUniverse.Init();
            gameUniverse.StartLevel(startLevel);
            gameUniverse.StepUniverse();

            // Start tick timer
            gameTimer = new DispatcherTimer(this.Dispatcher)
            {
                Interval = new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval)
            };
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            gameTimer.Start();
        }

        /// <summary>
        ///  Creates all WPF controls of the window
        /// </summary>
        private void InitializeComponents()
        {
            this.Height = DisplayControl.ScreenHeight;
            this.Width = DisplayControl.ScreenWidth;
            this.Background = new SolidColorBrush(Color.Black);

            // Tetris grid
            universeView = new UniverseView(gameUniverse);

            // Stack panel with next block and score
            GradientStackPanel statusStack = new GradientStackPanel(Orientation.Vertical, Color.Black, Color.White)
            {
                Width = this.Width - universeView.Width,
                Height = this.Height
            };

            // Next block control
            NextBlockView nextBlockView = new NextBlockView(gameUniverse);
            nextBlockView.SetMargin(8);
            nextBlockView.HorizontalAlignment = HorizontalAlignment.Right;

            // Score panel
            StatisticsPanel statsPanel = new StatisticsPanel(gameUniverse.Statistics);
            statsPanel.SetMargin(8);
            statsPanel.HorizontalAlignment = HorizontalAlignment.Right;
            
            statusStack.Children.Add(nextBlockView);
            statusStack.Children.Add(statsPanel);

            // Main stack on the screen
            StackPanel mainStack = new StackPanel(Orientation.Horizontal);
            mainStack.Children.Add(universeView);
            mainStack.Children.Add(statusStack);
            this.Child = mainStack;

            // Set the window visibility to visible.
            this.Visibility = Visibility.Visible;

            // Attach the button focus to the window.
            Buttons.Focus(this);
        }

        /// <summary>
        /// Game timer tick handler
        /// </summary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameUniverse.StepUniverse();

            // If next level then update timer speed and update statsPanel
            if (gameUniverse.Statistics.NextLevel)
            {
                gameTimer.Interval = new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval);
                gameUniverse.Statistics.NextLevel = false;
            }

            // If game is over then stop the counter
            if (gameUniverse.Statistics.GameOver)
            {
                gameTimer.Stop();
            }

            universeView.Invalidate();
        }

        /// <summary>
        /// Button handler
        /// </summary>        
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            // If game is over then every key close the window
            if (gameUniverse.Statistics.GameOver)
            {
                this.Close();
                parentApp.SetFocus();

                if (OnClose != null)
                    OnClose(this, gameUniverse.Statistics);
            }
            // else handle the keys
            else
            {
                switch (e.Button)
                {
                    case Button.VK_LEFT:
                        gameUniverse.StepLeft();
                        break;
                    case Button.VK_RIGHT:
                        gameUniverse.StepRight();
                        break;
                    case Button.VK_UP:
                        gameUniverse.Rotate();
                        break;
                    case Button.VK_SELECT:
                        gameUniverse.DropDown();
                        break;
                }
            }

            universeView.Invalidate();
        }
    }
}
