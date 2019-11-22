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
using Prism.Mvvm;

namespace jvh.WindowsExplorer.UI2.Controls
{
    /// <summary>
    /// Interaction logic for WindowsExplorer.xaml
    /// </summary>
    public partial class WindowsExplorer : UserControl
    {
        ViewModel VM { get => DataContext as ViewModel;}
        public string TargetPath { get; private set; }
        public List<FileSystemDisplayItem> FileItems { get; private set; } = new List<FileSystemDisplayItem>();

        private object dummyNode = null;
        public WindowsExplorer()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            Loaded += WindowsExplorerControl_Loaded;
            this.DataContext = new ViewModel();
        }

        private void WindowsExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<TreeViewItem>();

            foreach (string s in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;

                item.Expanded += new RoutedEventHandler(Folder_Expanded);
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
            var list = new List<FileSystemDisplayItem>();
            try
            {
                var directories = Directory.GetDirectories(currentDirectory);
                foreach (var d in directories)
                {
                    var info = new DirectoryInfo(d);
                    var item = FileSystemDisplayItem.CreateFolder(d, info.CreationTime.ToString()); 
                    list.Add(item);
                }

                var files = Directory.GetFiles(currentDirectory);
                foreach (var s in files)
                {
                    var info = new FileInfo(s);
                    var item = FileSystemDisplayItem.CreateFile(s, IconManager.FindIconForFilename(s, false), info.CreationTime.ToString());
                    list.Add(item);
                }

                FileItems = list;

                VM.TargetDirectory = currentDirectory;
                VM.DisplayItems = FileItems;
                //ListBoxMain.ItemsSource = FileItems;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
            }
        }
        private void Folder_Expanded(object sender, RoutedEventArgs e)
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
                        subitem.Expanded += new RoutedEventHandler(Folder_Expanded);
                        item.Items.Add(subitem);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }



        }
        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                var g = sender as Grid;
                var d = g.DataContext as FileSystemDisplayItem;

                if (d.ItemType == FileSystemItemType.FILE)
                {

                }
                else
                {
                    UpdateItemView(d.FullPath);
                }
            }
        }
    }
    public enum FileSystemItemType
    {
        FILE,
        FOLDER,
    }

    public class FileSystemDisplayItem
    {
        public string Name { get; set; }
        public ImageSource Img { get; set; }
        public string FullPath { get; }
        public string LastUpdate { get; set; }

        public FileSystemItemType ItemType { get; set; }

        public FileSystemDisplayItem(string name, FileSystemItemType itemType, string fullPath, ImageSource img, string lastUpdate)
        {
            Name = name;
            ItemType = itemType;
            FullPath = fullPath;
            Img = img;
            LastUpdate = lastUpdate;
        }

        public static FileSystemDisplayItem CreateFile(string path, ImageSource img, string lastUpdate)
        {
            return new FileSystemDisplayItem(path.Substring(path.LastIndexOf("\\") + 1), FileSystemItemType.FILE, path, img, lastUpdate);
        }

        public static FileSystemDisplayItem CreateFolder(string path, string lastUpdate)
        {
            Uri uri = new Uri("pack://application:,,,/Assets/Img/folder.png");
            BitmapImage source = new BitmapImage(uri);
            return new FileSystemDisplayItem(path.Substring(path.LastIndexOf("\\") + 1), FileSystemItemType.FOLDER, path, source, lastUpdate);
        }
    }
    class ViewModel : BindableBase
    {
        private string _TargetDirectory;
        public string TargetDirectory
        {
            get => _TargetDirectory;
            set => SetProperty(ref _TargetDirectory, value);
        }

        private IEnumerable<FileSystemDisplayItem> _DisplayItems;

        public IEnumerable<FileSystemDisplayItem> DisplayItems
        {
            get => _DisplayItems;
            set => SetProperty(ref _DisplayItems, value);
        }
    }
}

