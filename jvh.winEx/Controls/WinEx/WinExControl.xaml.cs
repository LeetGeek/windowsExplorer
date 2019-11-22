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
    /// Interaction logic for WinExControl.xaml
    /// </summary>
    public partial class WinExControl : UserControl
    {
        WinExViewModel ViewModel
        {
            get => DataContext as WinExViewModel;
            set => DataContext = value;
        }

        public WinExControl()
        {
            InitializeComponent();
            Loaded += WinExControl_Loaded;
        }

        private void WinExControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new WinExViewModel();
        }
    }
}
