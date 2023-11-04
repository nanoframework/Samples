//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace nanoFramework.Simple
{
    public class Program
    {
        public static void Main()
        {
            // registering services 
            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(ServiceObject))
                .AddSingleton(typeof(RootObject))
                .BuildServiceProvider();

            // create a service provider to get access to the RootObject
            var service = (RootObject)serviceProvider.GetService(typeof(RootObject));
            service.ServiceObject.Three = "3";

            // create an updated instance of the root object  
            var instance = (RootObject)ActivatorUtilities.CreateInstance(serviceProvider, typeof(RootObject), 1, "2");

            Debug.WriteLine($"One: {instance.One}");
            Debug.WriteLine($"Two: {instance.Two}");
            Debug.WriteLine($"Three: {instance.ServiceObject.Three}");
            Debug.WriteLine($"Name: {instance.ServiceObject.GetType().Name}");
        }

        public class ServiceObject
        {
            public string Three { get; set; }
        }

        public class RootObject
        {
            public int One { get; }

            public string Two { get; }

            public ServiceObject ServiceObject { get; protected set; }

            public RootObject(ServiceObject serviceObject)
            {
                ServiceObject = serviceObject;
            }

            // constructor with the most parameters will be used for activation
            public RootObject(ServiceObject serviceObject, int one, string two)
            {
                ServiceObject = serviceObject;
                One = one;
                Two = two;
            }
        }
    }
}
