using AssetManager.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.View
{
    /// <summary>
    /// Interaction logic for PerforceLoginSetup.xaml
    /// </summary>
    public partial class PerforceLoginSetup : Window
    {
        public PerforceLoginSetup()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((PerforceLoginViewModel)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
