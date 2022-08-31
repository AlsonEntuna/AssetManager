using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssetManager.Data
{
    internal class ObjectDisplayWrapper : ViewModelBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public EObjType Type { get; set; }

        #region Commands
        public ICommand PreviewCommand => new RelayCommand(Preview);

        #endregion

        public ObjectDisplayWrapper(string name, string path, EObjType type) 
        {
            Name = name;
            Path = path;
            Type = type;
        }

        private void Preview()
        {
            throw new NotImplementedException();
        }
    }
}
