//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Reflection;

namespace Reflection.CustomAttributes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the type of MyClass1.
            Type myType = typeof(MyClass1);

            // Display the attributes of MyClass1.
            object[] myAttributes = myType.GetCustomAttributes(true);
            if (myAttributes.Length > 0)
            {
                Console.WriteLine($"\nThe attributes for the class '{myType.Name}' are:");
                for (int j = myAttributes.Length - 1; j >= 0; j--)
                {
                    Console.WriteLine($"  {myAttributes[j]}");
                }
            }

            // Get the methods associated with MyClass1.
            MemberInfo[] myMethods = myType.GetMethods();

            Console.WriteLine("");
            Console.WriteLine($"'{myType.Name}' type has '{myMethods.Length}' methods");

            // Display the attributes for each of the methods of MyClass1.
            for (int i = 0; i < myMethods.Length; i++)
            {
                Console.WriteLine("");
                Console.WriteLine($"Getting custom attributes for '{myMethods[i].Name}'");

                myAttributes = myMethods[i].GetCustomAttributes(true);

                Console.WriteLine("");
                Console.WriteLine($"'{myMethods[i].Name}' method has {myAttributes.Length} custom attributes");

                if (myAttributes.Length > 0)
                {
                    Console.WriteLine($"\nThe attributes for the method '{myMethods[i].Name}' of class '{myType.Name}' are:");

                    for (int j = 0; j < myAttributes.Length; j++)
                    {
                        Console.WriteLine($"  {myAttributes[j]}");
                    }

                    foreach (var attrib in myAttributes)
                    {
                        // check if the method has Attribute1
                        if (attrib is Attribute1Attribute)
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'Attribute1' attribute");
                        }

                        // check if the method has IgnoreAttribute
                        if (attrib is IgnoreAttribute)
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'IgnoreAttribute' attribute");
                        }

                        // check if the method has DataRowAttribute
                        if (attrib is DataRowAttribute)
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'DataRowAttribute' attribute");

                            DataRowAttribute attDataRow = (DataRowAttribute)attrib;
                            
                            int index = 0;

                            foreach(var dataRow in attDataRow.Arguments)
                            {
                                Console.WriteLine($"          DataRowAttribute.Arg[{index++}] has: {dataRow}");
                            }
                        }

                        // check if the method has ComplexAttribute
                        if (attrib is ComplexAttribute)
                        {
                            Console.WriteLine($"  >>>>>>> {myMethods[i].Name} has 'ComplexAttribute' attribute");

                            ComplexAttribute attDataRow = (ComplexAttribute)attrib;

                            Console.WriteLine($"          ComplexAttribute.Max is {attDataRow.Max}");
                            Console.WriteLine($"          ComplexAttribute.B   is {attDataRow.B}");
                            Console.WriteLine($"          ComplexAttribute.S   is {attDataRow.S}");
                        }
                    }
                }
            }


            // Get the methods associated with MyClass1.
            FieldInfo[] myFields = myType.GetFields();

            // Display the attributes for each of MyClass1 fields.
            for (int i = 0; i < myFields.Length; i++)
            {
                myAttributes = myFields[i].GetCustomAttributes(true);
                if (myAttributes.Length > 0)
                {
                    Console.WriteLine($"\nThe attributes for field '{myFields[i].Name}' of class '{myType.Name}' are:");

                    for (int j = 0; j < myFields.Length; j++)
                    {
                        Console.WriteLine($"  {myFields[j]}");
                    }

                    var attributeData = myFields[i].GetCustomAttributesData();
                }
            }

            // display the custom attributes with constructor
            var myClass = new MyClass1();

            var myFieldAttributes = myClass.GetType().GetField("MyPackedField").GetCustomAttributes(true);

            Console.WriteLine($"\nThe custom attributes of field 'MyPackedField' are:");

            MaxAttribute attMax = (MaxAttribute)myFieldAttributes[0];
            Console.WriteLine($"MaxAttribute value is: 0x{attMax.Max.ToString("X8")}");

            AuthorAttribute attAuthor = (AuthorAttribute)myFieldAttributes[1];
            Console.WriteLine($"AuthorAttribute value is: '{attAuthor.Author}'");

        }
    }
}
