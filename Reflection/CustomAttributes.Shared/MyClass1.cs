
using System;
using System.Diagnostics;
using System.Reflection;

namespace Reflection.CustomAttributes
{
    // Define a class that has the custom attribute associated with one of its members.
    [Attribute2]
    [Attribute4]
    public class MyClass1
    {
        [Attribute1]
        [Attribute3]
        public void MyMethod1(int i)
        {
            return;
        }
    }
}
