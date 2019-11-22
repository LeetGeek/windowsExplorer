using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using File = System.IO.File;

namespace jvh.winEx.Controls.WinEx
{
    /// <summary>
    /// Interaction logic for WinExUrlBarControl.xaml
    /// </summary>
    public partial class WinExUrlBarControl : UserControl
    {
        WinExViewModel ViewModel
        {
            get => DataContext as WinExViewModel;
            set => DataContext = value;
        }

        public WinExUrlBarControl()
        {
            InitializeComponent();
            DataContextChanged += WinExUrlBarControl_DataContextChanged;
        }

        private void WinExUrlBarControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.OldValue is WinExViewModel oldVM)
                oldVM.PropertyChanged -= VMPropertyChanged;
            
            if(e.NewValue is WinExViewModel newVM)
                newVM.PropertyChanged += VMPropertyChanged;
        }

        private void VMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "TargetDirectory")
            {
                TextBoxTargetDirectory.Text = ViewModel.TargetDirectory;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var text = TextBoxTargetDirectory.Text;

            if (Directory.Exists(text) || File.Exists(text))
                ViewModel.TargetDirectory = text;
            else
            {
                MessageBox.Show("Bad URL");
            }
        }

        private void ButtonGoBack_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var di = new DirectoryInfo(ViewModel.TargetDirectory).Parent;
                if (di != null)
                    ViewModel.TargetDirectory = di.FullName;
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
