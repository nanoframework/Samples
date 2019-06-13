using nanoFramework.Json;
using System;
using System.Text;
using Windows.Storage;

namespace JsonConfigurationStore
{
    public class ConfigurationStore
    {
        private StorageFolder configFolder { get; set; }
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
            var InternalDevices = Windows.Storage.KnownFolders.InternalDevices;
            var flashDevices = InternalDevices.GetFolders();
            var configFolder = flashDevices[0];

            var configFile = StorageFile.GetFileFromPath(configFilePath);

            string json = FileIO.ReadText(configFile);
            Configuration config = (Configuration)JsonSerializer.DeserializeString(json);
            return config;
        }
        public bool WriteConfig(Configuration config)
        {
            try
            {
                var configJson = JsonSerializer.SerializeObject(config);
                StorageFile configFile = configFolder.CreateFile("configuration.json", CreationCollisionOption.ReplaceExisting);
                FileIO.WriteText(configFile, configJson);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
