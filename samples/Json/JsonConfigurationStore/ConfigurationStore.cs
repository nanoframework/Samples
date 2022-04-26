//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.IO;
using System.Text;
using nanoFramework.Json;

namespace JsonConfigurationStore
{
    public class ConfigurationStore
    {
        private string configFilePath { get; set; }

        public ConfigurationStore(string path = "I:\\configuration.json")
        {
            configFilePath = path;
        }

        public bool ClearConfig()
        {
            return WriteConfig(new Configuration());
        }

        public Configuration GetConfig()
        {
            var configFile = Directory.GetFiles(configFilePath);

            var json = new FileStream(configFile[0], FileMode.Open);
            Configuration config = (Configuration)JsonConvert.DeserializeObject(json, typeof(Configuration));

            return config;
        }
        public bool WriteConfig(Configuration config)
        {
            try
            {
                var configJson = JsonConvert.SerializeObject(config);

                var json = new FileStream(configFilePath, FileMode.OpenOrCreate);

                byte[] buffer = Encoding.UTF8.GetBytes(configJson);
                json.Write(buffer, 0, buffer.Length);
                json.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
