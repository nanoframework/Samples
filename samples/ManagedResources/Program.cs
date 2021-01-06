//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
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


            Debug.WriteLine(Resources.GetString(Resources.StringResources.String1));

            Thread.Sleep(2000);

            Debug.WriteLine(Resources.GetString(Resources.StringResources.String2));

            Debug.WriteLine($"PNG image has {Resources.GetBytes(Resources.BinaryResources.nano_Framework_logo_32_border_trans).Length} bytes");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
