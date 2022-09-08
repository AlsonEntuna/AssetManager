using AssetManager.Encryption;
using AssetManager.Utils;
using System;
using System.IO;

namespace AssetManager.Settings
{
    internal class AssetManagerSettings
    {
        public static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AssetManager");
        
        [NonSerialized]
        public readonly string SettingsSavePath = Path.Combine(AppDataPath, Constants.SettingsFileName);

        public string FolderPath;

        // Perforce
        public string PerforceServer;
        public string PerforceUser;
        public string PerforcePassword;
        public string PerforceClient;
        public string DepotPath;

        #region Singleton
        private static AssetManagerSettings _instance;
        public static AssetManagerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetManagerSettings();
                }
                return _instance;
            }
        }
        #endregion

        private AssetManagerSettings() { }

        public void SaveSettings()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            // Encrypt Password
            if (!string.IsNullOrEmpty(PerforcePassword))
            {
                string encrypted = Encryptor.Encrypt(PerforcePassword);
                PerforcePassword = encrypted;
            }

            JsonUtils.Serialize(Instance.SettingsSavePath, Instance);
        }
    }
}
