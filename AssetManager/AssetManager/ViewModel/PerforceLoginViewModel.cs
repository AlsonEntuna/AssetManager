using AssetManager.Perforce;
using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssetManager.ViewModel
{
    class PerforceLoginViewModel : ViewModelBase
    {
        private string _server;
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        private string _user;
        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ICommand SetupAndConnectCommand => new RelayCommand(SetupAndConnect);
        public PerforceLoginViewModel()
        {

        }

        private void SetupAndConnect()
        {
            // TODO: Setup perforce connection here....
            PerforceTools.Connect(Server, User);
        }
    }
}
