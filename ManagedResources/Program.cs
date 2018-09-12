//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;

namespace ManagedResources
{
    public class Program
    {
        public static void Main()
        {
            // to use managed resources:
            // 1. add a reference to nanoFramework.Runtime.Native
            // 2. Add a managed resource file (.resx extension)
            // 3. Access the resource using the appropriate getter as shown bellow


            Console.WriteLine(Resources.GetString(Resources.StringResources.String1));

            Thread.Sleep(2000);

            Console.WriteLine(Resources.GetString(Resources.StringResources.String2));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
