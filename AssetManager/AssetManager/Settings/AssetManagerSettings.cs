using AssetManager.Utils;
using System;
using System.IO;

namespace AssetManager.Settings
{
    internal class AssetManagerSettings
    {
        public string FolderPath { get; set; }
        public static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AssetManager");
        [NonSerialized]
        public readonly string SettingsSavePath = Path.Combine(AppDataPath, Constants.SettingsFileName);

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

            JsonUtils.Serialize(Instance.SettingsSavePath, Instance);
        }
    }
}
