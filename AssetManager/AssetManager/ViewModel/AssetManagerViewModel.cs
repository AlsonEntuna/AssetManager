using AssetManager.Perforce;
using AssetManager.View;
using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AssetManager.ViewModel
{
    class AssetManagerViewModel : ViewModelBase
    {
        public ICommand SyncCommand => new RelayCommand(Sync);
        public ICommand OpenRootFolderCommand => new RelayCommand(OpenRootFolder);
        public ICommand PerforceSetupCommand => new RelayCommand(PerforceLoginAndSetup);
        public AssetManagerViewModel()
        {

        }

        private void Sync()
        {
            //PerforceTools.Sync();
        }

        private void OpenRootFolder()
        {

        }

        private void PerforceLoginAndSetup()
        {
            PerforceLoginSetup p4Win = new PerforceLoginSetup();
            PerforceLoginViewModel vm = new PerforceLoginViewModel();
            p4Win.DataContext = vm;
            p4Win.ShowDialog();
        }
    }
}
