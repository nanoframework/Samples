//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Reflection;
using System.Threading;

namespace Reflection.CustomAttributes
{
    public class Program
    {
        public static void Main()
        {
            // Get the type of MyClass1.
            Type myType = typeof(MyClass1);

            // Display the attributes of MyClass1.
            Object[] myAttributes = myType.GetCustomAttributes(true);
            if (myAttributes.Length > 0)
            {
                Console.WriteLine($"\nThe attributes for the class '{myType.Name}' are:");
                for (int j = 0; j < myAttributes.Length; j++)
                    Console.WriteLine($"  {myAttributes[j]}");
            }

            // Get the methods associated with MyClass1.
            MemberInfo[] myMethods = myType.GetMethods();

            // Display the attributes for each of the methods of MyClass1.
            for (int i = 0; i < myMethods.Length; i++)
            {
                myAttributes = myMethods[i].GetCustomAttributes(true);
                if (myAttributes.Length > 0)
                {
                    Console.WriteLine($"\nThe attributes for the method '{myMethods[i].Name}' of class '{myType.Name}' are:");
                    for (int j = 0; j < myAttributes.Length; j++)
                    {
                        Console.WriteLine($"  {myAttributes[j]}");

                        // check if the method has Attribute1
                        if (typeof(Attribute1Attribute).Equals(myAttributes[j]))
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} as 'Attribute1' attribute");
                        }

                        // check if the method has IgnoreAttribute
                        if (typeof(IgnoreAttribute).Equals(myAttributes[j]))
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} as 'IgnoreAttribute' attribute");

                            //var attributeData = myMethods[i].GetCustomAttributesData();

                        }
                    }
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
