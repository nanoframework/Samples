//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
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
            object[] myAttributes = myType.GetCustomAttributes(true);
            if (myAttributes.Length > 0)
            {
                Debug.WriteLine($"\nThe attributes for the class '{myType.Name}' are:");
                for (int j = 0; j < myAttributes.Length; j++)
                {
                    Debug.WriteLine($"  {myAttributes[j]}");
                }
            }

            // Get the methods associated with MyClass1.
            MemberInfo[] myMethods = myType.GetMethods();

            // Display the attributes for each of the methods of MyClass1.
            for (int i = 0; i < myMethods.Length; i++)
            {
                myAttributes = myMethods[i].GetCustomAttributes(true);

                if (myAttributes.Length > 0)
                {
                    Debug.WriteLine($"\nThe attributes for the method '{myMethods[i].Name}' of class '{myType.Name}' are:");

                    for (int j = 0; j < myAttributes.Length; j++)
                    {
                        Debug.WriteLine($"  {myAttributes[j]}");

                        // check if the method has Attribute1
                        if (myAttributes[j] is Attribute1Attribute)
                        {
                            Debug.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'Attribute1' attribute");
                        }

                        // check if the method has IgnoreAttribute
                        if (myAttributes[j] is IgnoreAttribute)
                        {
                            Debug.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'IgnoreAttribute' attribute");
                        }

                        // check if the method has DataRowAttribute
                        if (myAttributes[j] is DataRowAttribute)
                        {
                            Debug.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'DataRowAttribute' attribute");

                            DataRowAttribute attDataRow = (DataRowAttribute)myAttributes[j];

                            int index = 0;

                            foreach (var dataRow in attDataRow.Arguments)
                            {
                                Console.WriteLine($"          DataRowAttribute.Arg[{index++}] has: {dataRow}");
                            }
                        }

                        // check if the method has ComplexAttribute
                        if (myAttributes[j] is ComplexAttribute)
                        {
                            Debug.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'ComplexAttribute' attribute");

                            ComplexAttribute attDataRow = (ComplexAttribute)myAttributes[j];

                            Console.WriteLine($"          ComplexAttribute.Max is {attDataRow.Max}");
                            Console.WriteLine($"          ComplexAttribute.B   is {attDataRow.B}");
                            Console.WriteLine($"          ComplexAttribute.S   is {attDataRow.S}");
                        }

                    }
                }
            }

            // display the custom attributes with constructor
            var myClass = new MyClass1();

            var myFieldAttributes = myClass.GetType().GetField("MyPackedField").GetCustomAttributes(true);

            Debug.WriteLine($"\nThe custom attributes of field 'MyPackedField' are:");

            MaxAttribute attMax = (MaxAttribute)myFieldAttributes[0];
            Debug.WriteLine($"MaxAttribute value is: 0x{attMax.Max.ToString("X8")}");

            AuthorAttribute attAuthor = (AuthorAttribute)myFieldAttributes[1];
            Debug.WriteLine($"AuthorAttribute value is: '{attAuthor.Author}'");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
