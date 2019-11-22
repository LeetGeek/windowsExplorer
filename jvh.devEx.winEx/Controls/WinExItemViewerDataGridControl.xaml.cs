using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Grid;

namespace jvh.devEx.winEx.Controls
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

        

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var dg = e.Source.DataControl as GridControl;
                var item = dg.SelectedItem as WinExDisplayItem;
                ViewModel.Open(item);
            }
        }
    }
}
