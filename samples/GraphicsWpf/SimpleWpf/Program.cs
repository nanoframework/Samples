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
    /// Micro Framework.
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
            MySimpleWPFApplication myApplication = new MySimpleWPFApplication();

            mainWindow = new MainMenuWindow(myApplication);

            // Create the object that configures the GPIO pins to buttons.
            GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);

            // Load the fonts.
            NinaBFont = Resource.GetFont(Resource.FontResources.NinaB);
            SmallFont = Resource.GetFont(Resource.FontResources.small);

            // Start the application.
            myApplication.Run(mainWindow);
        }

        /// <summary>
        /// Sets focus on the main window.
        /// </summary>
        public void GoHome()
        {
            Buttons.Focus(MySimpleWPFApplication.mainWindow);
        }
    }
}
