using AssetManager.Data;
using AssetManager.Perforce;
using AssetManager.Settings;
using AssetManager.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AssetManagerWindow : Window
    {
        private readonly AssetManagerViewModel _vm;
        public AssetManagerWindow()
        {
            InitializeComponent();
            _vm = DataContext as AssetManagerViewModel;
            _vm.Dispatcher = Dispatcher;
            Combo_ObjType.ItemsSource = Enum.GetValues(typeof(EObjType)).Cast<EObjType>();
        }

        private void Combo_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWorkspace = ((ComboBox)sender).SelectedItem as string;
            PerforceTools.Connection.SetClient(selectedWorkspace);
        }
    }
}
