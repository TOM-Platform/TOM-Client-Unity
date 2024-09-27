using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Storage;
using System.Linq;
#endif

namespace TOM.Common.Config
{

    [System.Serializable]
    public class ConfigData
    {
        // set default values
        public string host = "127.0.0.1";
        public string port = "8090";
    }


    // Note: This one runs before the ChatCommunication script (Edit > Project Settings > Script Execution Order)
    public class ConfigLoader : MonoBehaviour
    {
        public const string FILE_NAME = "tom_config.json";
        public const string FILE_DIRECTORY = "TOM";

        private static string host = null;
        private static string port = null;

        private static bool isLoaded = false;

        // Start is called before the first frame update
        void Start()
        {
            if (isLoaded)
            {
                return;
            }

            UpdateConfigInfo();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public string GetHost()
        {
            return host;
        }

        public string GetPort()
        {
            return port;
        }

        public async Task UpdateConfigInfo()
        {
            ConfigData config;
#if WINDOWS_UWP
            config = await readConfigDataAsync();
#else
            config = readConfigData();
#endif
            // for testing on remote device
            host = config.host;
            // for local development on same device
            // host = "localhost";
            port = config.port;

            isLoaded = true;

            //VisualLog.Log(host + ":" + port);
        }


        public bool IsLoaded()
        {
            return isLoaded;
        }

        private string getConfigDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + Path.DirectorySeparatorChar +
                   FILE_DIRECTORY;
            //return (System.Environment.ExpandEnvironmentVariables("%userprofile%") + Path.DirectorySeparatorChar + "Videos") + Path.DirectorySeparatorChar + FILE_DIRECTORY;
        }

        private string getConfigFilePath()
        {
            return (getConfigDirectory() + Path.DirectorySeparatorChar + FILE_NAME);
            //return Path.Combine(Application.persistentDataPath, FILE_NAME);
        }

        private void writeConfigData()
        {
            ConfigData config = new ConfigData();

            string jsonString = JsonUtility.ToJson(config);
            string directory = getConfigDirectory();
            string path = getConfigFilePath();

            try
            {
                if (!Directory.Exists(directory))
                {
                    Debug.Log("Creating a directory: " + directory);
                    Directory.CreateDirectory(directory);
                }

                Debug.Log("Write config file: " + path);
                File.WriteAllText(path, jsonString);
            }
            catch
            {
                Debug.LogError("Failed to write config file: " + path);
            }
        }

        private ConfigData readConfigData()
        {
            string path = getConfigFilePath();

            if (!File.Exists(path))
            {
                writeConfigData();
            }

            ConfigData config = new ConfigData();
            try
            {
                Debug.Log("Read config file: " + path);
                string jsonString = File.ReadAllText(path);
                config = JsonUtility.FromJson<ConfigData>(jsonString);
            }
            catch
            {
                Debug.LogError("Failed to read config file: " + path);
            }

            return config;
        }

#if WINDOWS_UWP
        private async Task<ConfigData> readConfigDataAsync()
        {
            ConfigData config = new ConfigData();

            try{
                Debug.Log("[UWP] Read config file: " + FILE_NAME);
                StorageFolder docLib = await KnownFolders.VideosLibrary.GetFolderAsync(FILE_DIRECTORY);
                StorageFile docFile = await docLib.GetFileAsync(FILE_NAME);

                string jsonString;
                using (Stream fs = await docFile.OpenStreamForReadAsync())
                {
                    byte[] byData = new byte[fs.Length];
                    fs.Read(byData, 0, (int)fs.Length);
                    jsonString = System.Text.Encoding.UTF8.GetString(byData);
                }
                config = JsonUtility.FromJson<ConfigData>(jsonString);
            }
            catch
            {
                Debug.LogError("[UWP] Failed to read config file: " + FILE_NAME);
            }
            return config;
        }
#endif
    }

}
