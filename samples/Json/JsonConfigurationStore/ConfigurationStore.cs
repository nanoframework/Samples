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
        private string _configFile { get; set; }

        public ConfigurationStore(string configFile = "I:\\configuration.json")
        {
            _configFile = configFile;
        }

        public void ClearConfig()
        {
            if (File.Exists(_configFile))
            {
                File.Delete(_configFile);
            }
        }

        public bool IsConfigFileExisting => File.Exists(_configFile);

        public Configuration GetConfig()
        {
            var json = new FileStream(_configFile, FileMode.Open);
            Configuration config = (Configuration)JsonConvert.DeserializeObject(json, typeof(Configuration));

            return config;
        }
        public bool WriteConfig(Configuration config)
        {
            try
            {
                var configJson = JsonConvert.SerializeObject(config);

                var json = new FileStream(_configFile, FileMode.Create);

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
