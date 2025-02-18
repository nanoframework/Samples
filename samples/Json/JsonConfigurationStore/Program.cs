//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;

namespace JsonConfigurationStore
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello world!");

            ConfigurationStore configurationStore = new ConfigurationStore();
            Configuration configuration = new Configuration()
            {
                Setting1 = "Setting 1 value",
                Setting2 = "Setting 2 value",
                Setting3 = "Setting 3 value"
            };

            Debug.WriteLine($"A configuration file does {(configurationStore.IsConfigFileExisting ? string.Empty : "not ")} exists.");
            configurationStore.ClearConfig();
            Debug.WriteLine("The configuration file has been deleted.");
            Debug.WriteLine($"A configuration file does {(configurationStore.IsConfigFileExisting ? string.Empty : "not ")} exists.");

            Debug.WriteLine("Saving configuration file");
            var writeResult = configurationStore.WriteConfig(configuration);
            Debug.WriteLine($"Configuration file {(writeResult ? "" : "not ")} saved properly.");

            var newConfig = configurationStore.GetConfig();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
