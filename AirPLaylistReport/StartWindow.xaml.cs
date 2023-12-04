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
using System.Windows.Shapes;

namespace AirPLaylistReport
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window

    {

       public static MainWindow Playlist;
       public static Xdcam XdcamConvert;
        public StartWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnPlaylist_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Playlist = new MainWindow();
            Playlist.ShowDialog();
           
            //if (Playlist == null)
            //{
            //    Playlist = new MainWindow();
            //    Playlist.Show();

            //}
            //else { Playlist.Activate(); }
        }

        private void btnXdcam_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();

            XdcamConvert = new Xdcam();
            XdcamConvert.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
