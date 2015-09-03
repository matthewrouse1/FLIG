using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FligClient.FileBrowsing;
using FligClient.MasterViewModel;
using RestSharp.Extensions;

namespace FLIG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MasterViewModel();
        }



        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((MasterViewModel) DataContext).SelectedFolder = (Folder)e.NewValue;
        }

        private List<File> selectedFiles = new List<File>();

        private void CheckoutClicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedFiles = new List<File>();
            foreach (var cell in e.AddedCells)
            {
                selectedFiles.Add((File)cell.Item);
            }
        }
    }
}
