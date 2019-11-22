using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace jvh.devEx.winEx.Controls
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
        }

        private void SimpleButtonGo_OnClick(object sender, RoutedEventArgs e)
        {
            var text = TextEditUrl.Text;

            if (Directory.Exists(text) || File.Exists(text))
                ViewModel.TargetDirectory = text;
            else
            {
                MessageBox.Show("Bad URL");
            }
        }

        private void SimpleButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.TargetDirectory)) return;

            try
            {
                var di = new DirectoryInfo(ViewModel.TargetDirectory).Parent;
                if (di != null)
                    ViewModel.TargetDirectory = di.FullName;
                else
                {
                    ViewModel.TargetDirectory = "";
                }
            }
            catch (Exception exception)
            {
                ViewModel.TargetDirectory = "";
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.TargetDirectory)) return;

            try
            {
                var di = new DirectoryInfo(ViewModel.TargetDirectory).Parent;
                if (di != null)
                    ViewModel.TargetDirectory = di.FullName;
                else
                {
                    ViewModel.TargetDirectory = "";
                }
            }
            catch (Exception exception)
            {
                ViewModel.TargetDirectory = "";
            }
        }
    }
}
