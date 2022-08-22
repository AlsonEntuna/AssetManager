using System.Windows;

namespace AssetManager.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for NameDialog.xaml
    /// </summary>
    public partial class NameDialog : Window
    {
        public string InputName => Txt_Value.Text;
        public NameDialog(string title = "", string description = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title))
                Title = title;
            if (!string.IsNullOrEmpty(description))
                Description.Header = description;
        }

        private void Btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
