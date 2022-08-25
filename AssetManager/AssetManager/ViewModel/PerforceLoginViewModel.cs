using AssetManager.Perforce;
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
            set => SetProperty(ref _server, value);
        }

        private string _user;
        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand SetupAndConnectCommand => new RelayCommand<Window>(SetupAndConnect);
        public PerforceLoginViewModel()
        {

        }

        private void SetupAndConnect(Window window)
        {
            // TODO: Setup perforce connection here....
            bool connected = PerforceTools.Connect(Server, User, Password);
            Console.WriteLine($"Connection result: {connected}");
            window?.Close();
        }
    }
}
