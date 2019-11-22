using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace jvh.winEx.Controls.WinEx
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var info = value as WinExDisplayItem;

            if (info == null) return null;

            try
            {
                switch (info.ItemType)
                {
                    case WinExDisplayItemType.DRIVE:
                        var driveUri = new Uri("pack://application:,,,/Assets/Img/diskdrive.png");
                        var driveImg = new BitmapImage(driveUri);
                        return driveImg;

                    case WinExDisplayItemType.FOLDER:
                        var fileUri = new Uri("pack://application:,,,/Assets/Img/folder.png");
                        var fileImg = new BitmapImage(fileUri);
                        return fileImg;

                    case WinExDisplayItemType.FILE:
                        return IconManager.FindIconForFilename(info.Path, false);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}