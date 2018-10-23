using Base64TestingShared;
using System.Threading;

namespace Base64Test
{
    public class Program
    {
        public static void Main()
        {
            TestCode.Output();

            for (; ; )
            {
                Thread.Sleep(10000);
            }
        }
    }
}
