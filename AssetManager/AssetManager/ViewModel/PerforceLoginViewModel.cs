using AssetManager.Encryption;
using AssetManager.Perforce;
using AssetManager.Settings;
using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace AssetManager.ViewModel
{
    public class PerforceLoginViewModel : ViewModelBase
    {
        private string _server;
        public string Server
        {
            get => _server;
            set
            {
                SetProperty(ref _server, value);
                AssetManagerSettings.Instance.PerforceServer = value;
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                SetProperty(ref _user, value);
                AssetManagerSettings.Instance.PerforceUser = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                AssetManagerSettings.Instance.PerforcePassword = value;
            }
        }

        public ICommand SetupAndConnectCommand => new RelayCommand<Window>(SetupAndConnect);
        public PerforceLoginViewModel()
        {
            Server = AssetManagerSettings.Instance.PerforceServer;
            User = AssetManagerSettings.Instance.PerforceUser;
        }

        private void SetupAndConnect(Window window)
        {
            bool connected = PerforceTools.Connect(Server, User, Password);
            Console.WriteLine($"Connection result: {connected}");
            window.DialogResult = true;
            window?.Close();
        }
    }
}
