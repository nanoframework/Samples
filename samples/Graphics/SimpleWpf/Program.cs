using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using SimpleWPF;
using System;
using System.Threading;

namespace SimpleWpf
{
    /// <summary>
    /// Demonstrates the Windows Presentation Foundation functionality in .NET 
    /// NanoFramework.
    /// </summary>
    public class MySimpleWPFApplication : Application
    {
        // Fields to hold the fonts used by this demo.   
        static public Font NinaBFont;
        static public Font SmallFont;
        static public Bitmap Leaf;

        // Declare the main window.
        static Window mainWindow;


        
        /// <summary>
        /// The executable entry point.
        /// </summary>
        public static void Main()
        {
            // Create application object
            MySimpleWPFApplication myApplication = new MySimpleWPFApplication();

            // Add main window to application
            mainWindow = new MainMenuWindow(myApplication);

            // Create the object that handles the buttons presses on GPIO pins.
            GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);


            // Uncomment/Change required GPIO pins used for Keys

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


            // Load the fonts resources used by demo.
            NinaBFont = Resource.GetFont(Resource.FontResources.NinaB);
            SmallFont = Resource.GetFont(Resource.FontResources.small);

            // Start the application running.
            myApplication.Run(mainWindow);
        }

        /// <summary>
        /// Sets focus on the main window.
        /// </summary>
        public void GoHome()
        {
            Buttons.Focus(MySimpleWPFApplication.mainWindow);
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
