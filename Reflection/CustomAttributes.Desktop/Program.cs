using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.CustomAttributes
{
    class Program
    {
        static void Main(string[] args)
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
                        Console.WriteLine($"  {myAttributes[j]}");
                }
            }
        }
    }
}
