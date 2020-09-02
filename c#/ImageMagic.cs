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
using System.Drawing;
using System.IO;
using Microsoft.Win32;

namespace ImageMagic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    // Создать библиотеку класса для этих функций, добавить загрузку из файла в этот же класс
    static class Func
    {
        public static BitmapImage ToImage(this byte[] Array)
        {
            using (MemoryStream ms = new MemoryStream(Array, 0, Array.Length))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static byte[] FileToByte(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            var ImageConverter = System.Drawing.Image.FromFile(path);
            using (MemoryStream ms = new MemoryStream())
            {
                ImageConverter.Save(ms, bitmap.RawFormat);
                return ms.ToArray();
            }
        }
    }
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        byte[] Image;
        private void uploadImage_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog()
            {

            };
            if ((bool)op.ShowDialog())
            {
                image.Source = new BitmapImage(new Uri(op.FileName));
                Image = Func.FileToByte(op.FileName);
            }


        }

        private void returnImage_btn_Click(object sender, RoutedEventArgs e)
        {
            image2.Source = Func.ToImage(Image);
        }
        
    }
}
