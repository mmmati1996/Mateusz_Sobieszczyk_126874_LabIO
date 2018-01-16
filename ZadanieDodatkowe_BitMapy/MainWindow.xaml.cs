using Microsoft.Win32;
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
using System.Drawing.Imaging;

namespace ZadanieDodatkowe_BitMapy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Wybierz obraz",
                Filter = "BMP (*.bmp)|*.bmp",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                image.Source = imgSource;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //TAP
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Wybierz obraz",
                Filter = "BMP (*.bmp)|*.bmp",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {

                //asynchronicznie
                Dispatcher.Invoke(new Action(() => {
                    ImageSource imgSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                    image.Source = imgSource;
                }));
            }
        }

    }
}
