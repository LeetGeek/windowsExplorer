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

namespace jvh.WindowsExplorer.UI2.Controls
{
    /// <summary>
    /// Interaction logic for WindowsExplorer.xaml
    /// </summary>
    public partial class WindowsExplorer : UserControl
    {
        public string TargetPath { get; private set; }
        public List<FileItem> FileItems { get; private set; } = new List<FileItem>();

        private object dummyNode = null;
        public WindowsExplorer()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            Loaded += WindowsExplorerControl_Loaded;
        }

        private void WindowsExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<TreeViewItem>();

            foreach (string s in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;

                item.Expanded += new RoutedEventHandler(folder_Expanded);
                item.Items.Add(dummyNode);
                list.Add(item);
            }

            TreeViewMain.ItemsSource = list;
            TreeViewMain.SelectedItemChanged += TreeViewMain_SelectedItemChanged;
        }

        private void TreeViewMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = (TreeView)sender;
            var temp = ((TreeViewItem)tree.SelectedItem);

            if (temp == null)
                return;

            var temp1 = temp.Header.ToString();


            while (temp.Parent != null)
            {
                temp = temp.Parent as TreeViewItem;
                temp1 = $"{temp.Header.ToString().TrimEnd('\\')}\\{temp1}";
            }

            TargetPath = temp1;
            UpdateItemView(TargetPath);
        }

        private void UpdateItemView(string currentDirectory)
        {
            var list = new List<FileItem>();
            try
            {
                var file = Directory.GetFiles(currentDirectory);
                foreach (var s in file)
                {
                    var fi = new FileItem(s.Substring(s.LastIndexOf("\\") + 1), IconManager.FindIconForFilename(s, true));
                    list.Add(fi);
                }

                FileItems = list;
                ItemView.ItemsSource = null;
                ItemView.ItemsSource = FileItems;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
                
            }
            
        }
        private void folder_Expanded(object sender, RoutedEventArgs e)
        {



            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {

                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;

                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }



        }

        public class FileItem
        {
            public string Name { get; set; }
            public ImageSource Img { get; set; }

            public FileItem(string name, ImageSource img)
            {
                Name = name;
                Img = img;
            }
        }


    }
}

