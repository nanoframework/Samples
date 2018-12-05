using Base64TestingShared;
using System.Globalization;
using System.Threading;

namespace DesktopBase64Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change current culture to Invariant culture to match nanoFramework culture setting
            CultureInfo dft = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            TestCode.Output();

            for (; ; )
            {
                Thread.Sleep(10000);
            }
        }
    }
}
