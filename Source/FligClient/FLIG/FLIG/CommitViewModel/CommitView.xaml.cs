using System;
using System.Collections;
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
using System.Windows.Shapes;
using FligClient.Git;

namespace FligClient.CommitViewModel
{
    /// <summary>
    /// Interaction logic for CommitView.xaml
    /// </summary>
    public partial class CommitView : Window
    {
        public CommitView() 
        {
            InitializeComponent();
        }

        public CommitView(GitViewModel gitViewModel, IList selectedFiles)
        {
            var commitViewModel = new CommitViewModel();
            commitViewModel._gitViewModel = gitViewModel;
            commitViewModel._selectedFiles = selectedFiles;
            this.DataContext = commitViewModel;
            InitializeComponent();
        }
    }
}
