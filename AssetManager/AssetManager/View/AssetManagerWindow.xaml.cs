using AssetManager.Perforce;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AssetManagerWindow : Window
    {
        public AssetManagerWindow()
        {
            InitializeComponent();
        }

        private void Combo_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWorkspace = ((ComboBox)sender).SelectedItem as string;
            PerforceTools.Connection.SetClient(selectedWorkspace);
        }
    }
}
