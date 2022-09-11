//-----------------------------------------------------------------------------
// 
//  Tetris game for .NET nanoFramework
//
//  Original source from http://bansky.net/blog (no longer available).
// 
// This code was written by Pavel Bansky. It is released under the terms of 
// the Creative Commons "Attribution NonCommercial ShareAlike 2.5" license.
// http://creativecommons.org/licenses/by-nc-sa/2.5/
//-----------------------------------------------------------------------------


// !!!----------- SAMPLE - ENSURE YOU CHOOSE THE CORRECT TARGET HERE --------------!!!
//#define STM32F769I_DISCO // Comment this in if for the target!
//#define ESP32 // Comment this in if for the target platform!
// !!!-----------------------------------------------------------------------------!!!


using System;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using Tetris.GameLogic;
using Tetris.Presentation;

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
#if ESP32   // This is an example mapping, work them out for your needs!
            inputProvider.AddButton(12, Button.VK_LEFT, true);
            inputProvider.AddButton(13, Button.VK_RIGHT, true);
            inputProvider.AddButton(34, Button.VK_UP, true);
            inputProvider.AddButton(35, Button.VK_SELECT, true);
            inputProvider.AddButton(36, Button.VK_DOWN, true);

            DisplayControl.Initialize(new SpiConfiguration(1, 22, 21, 18, 5), new ScreenConfiguration(0, 0, 320, 240));

#elif STM32F769I_DISCO // This is an example (working) button map, work the actual pins out for your need!
            //WARNING: Invalid pin mappings will never be returned, and may need you to reflash the device!
            inputProvider.AddButton(PinNumber('J', 0), Button.VK_LEFT, true);
            inputProvider.AddButton(PinNumber('J', 1), Button.VK_RIGHT, true);
            inputProvider.AddButton(PinNumber('J', 3), Button.VK_UP, true);
            inputProvider.AddButton(PinNumber('J', 4), Button.VK_DOWN, true);
            inputProvider.AddButton(PinNumber('A', 6), Button.VK_SELECT, true);

            DisplayControl.Initialize(new SpiConfiguration(), new ScreenConfiguration()); //TODO: surely this should "actually" be I2C?!
#else
            throw new System.Exception("Unknown button and/or display mapping!");
#endif

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
            MainWindow = new MainMenuWindow(this);
            base.OnStartup(e);
        }

        /// <summary>
        /// Sets focus to MainWindow
        /// </summary>
        public void SetFocus()
        {            
            Buttons.Focus(MainWindow);
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

        private static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }

    }
}
