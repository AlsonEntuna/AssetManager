using System.Collections.ObjectModel;
using System.IO;

namespace AssetManager.Data
{
    internal class ObjectsHandler
    {
        public ObservableCollection<ObjectDisplayWrapper> Objects { get; private set; }
        public ObjectsHandler() 
        {
            Objects = new ObservableCollection<ObjectDisplayWrapper>();
        }

        public void GenerateObjectWrapper(string path)
        {
            string ext = Path.GetExtension(path);
            EObjType type;
            switch (ext.ToLower())
            {
                case ".fbx": type = EObjType.Fbx; break;
                case ".obj": type = EObjType.Obj; break;
                case ".blend": type = EObjType.Blender; break;
                case ".ma": type = EObjType.Maya; break;
                case ".mb": type = EObjType.Maya; break;
                case ".max": type = EObjType.Max; break;
                case ".gltf": type = EObjType.Gltf; break;
                case ".dae": type = EObjType.Collada; break;
                default: type = EObjType.Unknown; break;
            }

            // Ignore unknown types...
            if (type == EObjType.Unknown)
            {
                return;
            }

            ObjectDisplayWrapper wrapper = new ObjectDisplayWrapper(Path.GetFileName(path), path, type);
            if (!Objects.Contains(wrapper))
                Objects.Add(wrapper);
        }
    }
}
