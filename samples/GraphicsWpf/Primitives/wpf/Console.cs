using nanoFramework.UI;
using nanoFramework.UI.Console;
using System;
using System.Threading;

namespace UAM.InformatiX.SPOT
{
    public static class Console
    {
        private static ConsoleWindow _consoleWindow;

        static Console()
        {
            Thread = new Thread(
                delegate ()
                {
                    _consoleWindow = new ConsoleWindow();
                    new Application().Run(_consoleWindow);
                });

            Thread.Start();
        }

        //public static void WriteLine(string text)
        //{
        //    WriteLine(text, ReportTimeStamps);
        //}

//        public static void WriteLine(string text, bool timeStamp)
        public static void WriteLine(string text)
        {
            while (_consoleWindow == null)
                Thread.Sleep(10);

            //if (timeStamp)
            //{
            //    text = (UseRelativeTime ? (Utility.GetMachineTime() - Utility.GetLastBootTime()) : Utility.GetMachineTime()) + " " + text;
            //}
            lock (_consoleWindow)
                _consoleWindow.WriteLine(text);
        }

        public static Thread Thread;
        public static bool ReportTimeStamps;
        public static bool UseRelativeTime;
    }
}
