using AssetManager.Data;
using AssetManager.Perforce;
using AssetManager.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        private bool UserSearchFilter(object item)
        {
            if (string.IsNullOrEmpty(Txt_Search.Text))
            {
                return true;
            }
            else
            {
                return (item as ObjectDisplayWrapper).Name.IndexOf(Txt_Search.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void Search()
        {
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataGrid_Objects.ItemsSource);
            collectionView.Filter = UserSearchFilter;
            CollectionViewSource.GetDefaultView(DataGrid_Objects.ItemsSource).Refresh();
        }

        private void Combo_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWorkspace = ((ComboBox)sender).SelectedItem as string;
            PerforceTools.Connection.SetClient(selectedWorkspace);
        }

        private void Txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private bool ObjTypeFilter(object item)
        {
            if (_vm.SelectedObjType == EObjType.All)
            {
                return true;
            }
            else
            {
                return (item as ObjectDisplayWrapper).Type == _vm.SelectedObjType;
            }
        }

        private void FilterDataGrid()
        {
            _vm.IsLoading = true;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataGrid_Objects.ItemsSource);
            collectionView.Filter = ObjTypeFilter;
            CollectionViewSource.GetDefaultView(DataGrid_Objects.ItemsSource).Refresh();
            _vm.IsLoading = false;
        }

        private void Combo_ObjType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread filterThread = new Thread(() =>
            {
                Action action = () => FilterDataGrid();
                Dispatcher.Invoke(action);
            })
            { IsBackground = true };
            filterThread.Start();
        }
    }
}
