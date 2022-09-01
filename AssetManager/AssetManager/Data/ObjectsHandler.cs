using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            switch (ext)
            {
                case ".fbx": type = EObjType.Fbx; break;
                case ".obj": type = EObjType.Obj; break;
                case ".blend": type = EObjType.Blender; break;
                case ".ma": type = EObjType.Maya; break;
                case ".mb": type = EObjType.Maya; break;
                case ".max": type = EObjType.Max; break;
                default: type = EObjType.Unknown; break;
            }

            ObjectDisplayWrapper wrapper = new ObjectDisplayWrapper(Path.GetFileName(path), path, type);
            Objects.Add(wrapper);
        }
    }
}
