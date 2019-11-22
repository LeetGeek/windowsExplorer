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

namespace jvh.winEx.Controls.WinEx
{
    /// <summary>
    /// Interaction logic for WinExItemViewerDataGridControl.xaml
    /// </summary>
    public partial class WinExItemViewerDataGridControl : UserControl
    {
        WinExViewModel ViewModel
        {
            get => DataContext as WinExViewModel;
            set => DataContext = value;
        }

        public WinExItemViewerDataGridControl()
        {
            InitializeComponent();
        }

        private void Control_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var dg = sender as DataGrid;
                var item = dg.SelectedItem as WinExDisplayItem;
                ViewModel.Open(item);
            }
        }
    }
}
