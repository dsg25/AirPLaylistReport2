using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace AirPLaylistReport
{
    /// <summary>
    /// Логика взаимодействия для Xdcam.xaml
    /// </summary>
    public partial class Xdcam : Window
    {
        public Xdcam()
        {
            InitializeComponent();
        }

        string selectpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new VistaFolderBrowserDialog();
                dialog.Description = "Выбери папку.";
                dialog.UseDescriptionForTitle = true;
                dialog.RootFolder = Environment.SpecialFolder.Desktop; //Environment.SpecialFolder.DesktopDirectory;
                
                if ((bool)dialog.ShowDialog(this))
                {
                    selectpath = dialog.SelectedPath;
                    Textblock1.Text = selectpath;
                }


            }

            catch (Exception error)
            {
               MessageBox.Show(error.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string directorywritepath = selectpath + "\\Clip";

            // *********** Создать папку Clip ****************

            try
            {
                 if (!Directory.Exists(directorywritepath)) // Проверить, существует ли папка
                {
                    Directory.CreateDirectory(directorywritepath);  // Создать новую папку
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ошибка при создании папки: {error.Message}");
            }
        
        // ********* END *********

        Directory.CreateDirectory(directorywritepath); // Cоздаем директорю

            string filewritepath = selectpath+ "\\MEDIAPRO.XML";

            var directory = new DirectoryInfo(selectpath);

            // ********** Перемещение файлов в Clip ***************

            FileInfo[] filesall = directory.GetFiles("*.MXF");

            try
            {
                string[] files = Directory.GetFiles(selectpath);
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);   // Получение имени файла без пути
                    string destinationPath = Path.Combine(directorywritepath, fileName);   // Полный путь к целевой папке
                    File.Move(filePath, destinationPath);   // Перемещение файла
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при перемещении файлов: {ex.Message}");
            }

            // ********* END *********

            

            try
            {
               StreamWriter sw = new StreamWriter(filewritepath);

               string line;

               line = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                        "<MediaProfile xmlns=\"http://xmlns.sony.net/pro/metadata/mediaprofile\" createdAt=\"2019-09-15T22:30:56Z\" version=\"2.10\">\r\n" +
                        "\t<Properties>\r\n" +
                        "\t\t<System systemId=\"0800460202A38A7F\" systemKind=\"PXW-FS7M2 ver.1.220\" masterVersion=\"XAVC-XD@1.10.00\"/>\r\n" +
                        "\t\t<Attached mediaId=\"0491D4D8358705DF8A7F0800460202A3\" mediaKind=\"ProfessionalDisc\"\r\n" +
                        "\t\t\tmediaName=\"\"/>\r\n" +
                        "\t</Properties>\r\n" +
                        "\t<Contents>";

                sw.WriteLine(line);

                var directory1 = new DirectoryInfo(directorywritepath);

                FileInfo[] files = directory1.GetFiles("*.MXF");
                
                foreach (FileInfo file in files)
                {
                    // ********* Вычмсляем имя файла без разрешения *********
                    string filenamewithoutext = file.Name;
                    int lastIndex = filenamewithoutext.LastIndexOf(".");
                    if (lastIndex != -1)
                    {
                        filenamewithoutext = filenamewithoutext.Substring(0, lastIndex);
                    }
                    // ********* END *********


                    line = "\t\t<Material uri=\"./Clip/" + filenamewithoutext + ".MXF" + "\" type=\"MXF\" offset=\"0\" dur=\"277\" fps=\"25p\" aspectRatio=\"16:9\" ch=\"4\"\r\n" +
                            "\t\t\tvideoType=\"AVC50_1920_1080_H422P@L41\" audioType=\"LPCM24\"\r\n" +
                            "\t\t\tumid=\"060A2B340101010501010D431300000031C2E406378705D70800460202A38A7F\"\r\n" +
                            "\t\t\tstatus=\"none\">\r\n" +
                            "\t\t\t<RelevantInfo uri=\"./Clip/" + filenamewithoutext + "M01.XML\" type=\"XML\"/>\r\n" +
                            "\t\t\t<RelevantInfo uri=\"./Clip/" + filenamewithoutext + "R01.BIM\" type=\"BiM\"/>\r\n" +
                            "\t\t</Material>";

                    sw.WriteLine(line);

                }

                line = "\t</Contents>\r\n</MediaProfile>";

                sw.WriteLine(line);


                sw.Close();


            }
            catch (Exception error)
            {
               MessageBox.Show("Exception: " + error.Message);
            }
            finally
            {
                MessageBox.Show("Выполнено успешно!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
