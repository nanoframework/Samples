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
using Tetris;
using Tetris.GameLogic;
using Tetris.Presentation;
using nanoFramework.Runtime.Events;
using System.Threading;

namespace Tetris
{
    /// <summary>
    /// Tetris Application
    /// </summary>
    public class TetrisApp : Application
    {        
        /// <summary>
        /// Game HighScoreTable
        /// </summary>
        public HighScoreTable HighScore;
        private static ExtendedWeakReference highScoreEWD;

        private TetrisApp()
        {
            // Create the object that configures the GPIO pins to buttons.
            GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);

            // Assign GPIO / Key functions to GPIOButtonInputProvider
            // Esp32
            inputProvider.AddButton(12, Button.VK_LEFT, true);
            inputProvider.AddButton(13, Button.VK_RIGHT, true);
            inputProvider.AddButton(34, Button.VK_UP, true);
            inputProvider.AddButton(35, Button.VK_SELECT, true);
            inputProvider.AddButton(36, Button.VK_DOWN, true);

            // STM32
            //inputProvider.AddButton(PinNumber('A', 0), Button.VK_LEFT, true);
            //inputProvider.AddButton(PinNumber('A', 1), Button.VK_RIGHT, true);
            //inputProvider.AddButton(PinNumber('A', 2), Button.VK_UP, true);
            //inputProvider.AddButton(PinNumber('A', 3), Button.VK_SELECT, true);
            //inputProvider.AddButton(PinNumber('A', 4), Button.VK_DOWN, true);


            // Create ExtendedWeakReference for high score table
            highScoreEWD = ExtendedWeakReference.RecoverOrCreate(
                                                    typeof(TetrisApp), 
                                                    0, 
                                                    ExtendedWeakReference.c_SurvivePowerdown);
            // Set persistence priority
            highScoreEWD.Priority = (int)ExtendedWeakReference.PriorityLevel.Important;

            // Try to recover previously saved HighScore
            HighScore = (HighScoreTable)highScoreEWD.Target;
            
            // If nothing was recovered - create new
            if (HighScore == null)
                HighScore = new HighScoreTable();
        }

        /// <summary>
        /// OnStartUp event handler
        /// </summary>        
        protected override void OnStartup(EventArgs e)
        {
            this.MainWindow = new MainMenuWindow(this);
            base.OnStartup(e);
        }

        /// <summary>
        /// Sets focus to MainWindow
        /// </summary>
        public void SetFocus()
        {            
            Buttons.Focus(this.MainWindow);
        }

        /// <summary>
        /// Persists high score to the FLASH memory
        /// </summary>
        public void PersistHighScore()
        {
            // Persist HighScore by setting the Target property
            // of ExtendedWeakReference
            highScoreEWD.Target = HighScore;
        }

        public static void Main()
        {
            new TetrisApp().Run();
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }

    }
}
