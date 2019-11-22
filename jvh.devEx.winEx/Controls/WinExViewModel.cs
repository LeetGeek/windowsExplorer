using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;

namespace jvh.devEx.winEx.Controls
{
    class WinExViewModel : ViewModelBase
    {
        private string _TargetDirectory;
        public string TargetDirectory
        {
            get => _TargetDirectory;
            set
            {
                if (SetProperty(ref _TargetDirectory, value, nameof(TargetDirectory)))
                    OnDirectoryChange(value);
            } 
        }

        private IEnumerable<WinExDisplayItem> _DisplayItems;
        public IEnumerable<WinExDisplayItem> DisplayItems
        {
            get => _DisplayItems;
            set => SetProperty(ref _DisplayItems, value, nameof(DisplayItems));
        }



        public WinExViewModel()
        {
            TargetDirectory = "";
        }

        public void Open(WinExDisplayItem item)
        {
            if(item.ItemType != WinExDisplayItemType.FILE)
                TargetDirectory = item.Path;
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(item.Path);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        private void OnDirectoryChange(string directory)
        {
            UpdateView(directory);
        }

        private void UpdateView(string currentDirectory)
        {
            var list = new List<WinExDisplayItem>();
            try
            {
                if (currentDirectory == "")
                {
                    var drives = DriveInfo.GetDrives();
                    foreach (var drive in drives)
                    {
                        var item = WinExDisplayItem.CreateDrive(drive);
                        list.Add(item);
                    }
                }
                else
                {
                    var root = new DirectoryInfo(currentDirectory);
                    list.AddRange(root.GetDirectories().Select(WinExDisplayItem.CreateFolder));
                    list.AddRange(root.GetFiles().Select(WinExDisplayItem.CreateFile));
                }

                this.DisplayItems = list;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
            }
        }
    }

    enum WinExDisplayItemType
    {
        DRIVE,
        FILE,
        FOLDER
    }

    class WinExDisplayItem
    {
        public WinExDisplayItemType ItemType { get; }
        public string Name { get; }
        public string Path { get; }
        public string CreationDateTime { get; }

        public WinExDisplayItem(WinExDisplayItemType itemType, string name, string path, DateTime? creationDateTime)
        {
            ItemType = itemType;
            Name = name;
            Path = path;
            CreationDateTime = creationDateTime.ToString();
        }

        public static WinExDisplayItem Create(object info)
        {
            switch (info)
            {
                case DriveInfo drive:
                    return CreateDrive(drive);
                case DirectoryInfo folder:
                    return CreateFolder(folder);
                case FileInfo file:
                    return CreateFile(file);
                
                default: throw new ArgumentException("info object is not a valid type");
            }
        }

        public static WinExDisplayItem CreateDrive(DriveInfo info)
        {
            return new WinExDisplayItem(WinExDisplayItemType.DRIVE, info.Name, info.Name, null);
        }

        public static WinExDisplayItem CreateFile(FileInfo info)
        {
            return new WinExDisplayItem(WinExDisplayItemType.FILE, info.Name, info.FullName, info.CreationTime);
        }

        public static WinExDisplayItem CreateFolder(DirectoryInfo info)
        {
            return new WinExDisplayItem(WinExDisplayItemType.FOLDER, info.Name, info.FullName, info.CreationTime);
        }
    }
}