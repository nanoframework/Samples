using System;
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

            var clearResult = configurationStore.ClearConfig();

            var writeResult = configurationStore.WriteConfig(configuration);

            var newConfig = configurationStore.GetConfig();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
