using AssetManager.Wpf;

namespace AssetManager.Data
{
    internal class ObjectDisplayWrapper : ViewModelBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public EObjType Type { get; set; }

        public ObjectDisplayWrapper(string name, string path, EObjType type) 
        {
            Name = name;
            Path = path;
            Type = type;
        }
    }
}
