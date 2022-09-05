using AssetManager.Encryption;
using AssetManager.Settings;
using AssetManager.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.View
{
    /// <summary>
    /// Interaction logic for PerforceLoginSetup.xaml
    /// </summary>
    public partial class PerforceLoginSetup : Window
    {
        private PerforceLoginViewModel _vm;
        public PerforceLoginSetup()
        {
            InitializeComponent();
            
        }

        public void Setup()
        {
            _vm = DataContext as PerforceLoginViewModel;

            if (!string.IsNullOrEmpty(AssetManagerSettings.Instance.PerforcePassword))
            {
                try
                {
                    string decryptedPass = Encryptor.Decrypt(AssetManagerSettings.Instance.PerforcePassword);
                    AssetManagerSettings.Instance.PerforcePassword = decryptedPass;
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            _vm.Password = AssetManagerSettings.Instance.PerforcePassword;
            PwdBox_P4Password.Password = _vm.Password;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((PerforceLoginViewModel)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
