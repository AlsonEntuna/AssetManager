using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AssetManager.ViewModel
{
    class AssetManagerViewModel : ViewModelBase
    {
        public ICommand SyncCommand => new RelayCommand(Sync);
        public AssetManagerViewModel()
        {

        }

        private void Sync()
        {

        }
    }
}
