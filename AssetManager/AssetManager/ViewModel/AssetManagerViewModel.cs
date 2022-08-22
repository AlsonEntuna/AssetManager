using AssetManager.Data;
using AssetManager.Perforce;
using AssetManager.View;
using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AssetManager.ViewModel
{
    class AssetManagerViewModel : ViewModelBase
    {
        private ObservableCollection<ObjectDisplayWrapper> _objectDisplay;
        public ObservableCollection<ObjectDisplayWrapper> ObjectDisplay
        { 
            get => _objectDisplay;
            set => SetProperty(ref _objectDisplay, value);
        }

        private ObservableCollection<string> _workspaces;
        public ObservableCollection<string> Workspaces
        {
            get => _workspaces;
            set => SetProperty(ref _workspaces, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

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

        private void SetupPerforceDependencies()
        {
            Workspaces = Utils.Utils.ToObservableCollection(PerforceTools.GetUserWorkspaces());
        }

        private void PerforceLoginAndSetup()
        {
            PerforceLoginSetup p4Win = new PerforceLoginSetup();
            PerforceLoginViewModel vm = new PerforceLoginViewModel();
            p4Win.DataContext = vm;
            p4Win.ShowDialog();

            // TODO: this is temporary. Find a good way to do this update...
            IsConnected = PerforceTools.Connection.connectionEstablished();
            if (IsConnected)
                SetupPerforceDependencies();
        }
    }
}
