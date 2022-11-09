using AssetManager.Data;
using AssetManager.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace AssetManager.Settings
{
    internal class AssetManagerObjectsCache
    {
        public static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AssetManager");

        [NonSerialized]
        public static readonly string ObjectsCachePath = Path.Combine(AppDataPath, Constants.ObjectsDisplayCacheFileName);

        public List<ObjectDisplayWrapper> ObjectCache;
        public AssetManagerObjectsCache() { }

        public void SaveObjectsCache()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            JsonUtils.Serialize(ObjectsCachePath, this);
        }

        public void ClearCache()
        {
            ObjectCache.Clear();
            SaveObjectsCache();
        }

        public static AssetManagerObjectsCache LoadCache()
        {
            if (!File.Exists(AssetManagerObjectsCache.ObjectsCachePath))
            {
                return null;
            }
            AssetManagerObjectsCache cache = JsonUtils.Deserialize<AssetManagerObjectsCache>(AssetManagerObjectsCache.ObjectsCachePath);
            return cache;
        }
    }
}
