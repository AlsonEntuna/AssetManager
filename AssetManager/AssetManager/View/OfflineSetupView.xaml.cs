using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssetManager.View
{
    /// <summary>
    /// Interaction logic for OfflineSetupView.xaml
    /// </summary>
    public partial class OfflineSetupView : Window
    {
        public string FolderPath => Txt_FolderPath.Text;
        public OfflineSetupView()
        {
            InitializeComponent();
        }

        private void Btn_Browse_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fd = new FolderBrowserDialog() { Description = "Select Folder Path", ShowNewFolderButton = true, SelectedPath = "C:\\" })
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    Txt_FolderPath.Text = fd.SelectedPath;
            }
        }

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {
            FinishDialog();
        }

        private void FinishDialog()
        {
            DialogResult = true;
            Close();
        }
    }
}
