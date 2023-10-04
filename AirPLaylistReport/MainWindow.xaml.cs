using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml;

namespace AirPLaylistReport
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int numberfiles;
        string[] path;

        private void OnLoad (object sender, EventArgs e) 
        {
            textblock.Text = "Выбери файлы Air Playlist";
        }

        public void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Air Playlist | *.MCRlist";   // Фильтр расширения файлов
                fileDialog.InitialDirectory = @"C:\REC\Playlist";           // Директория по умолчанию
                fileDialog.Title = "Выбери Air Playlist файл(ы)";
                fileDialog.Multiselect = true;

                bool? success = fileDialog.ShowDialog();

                if (success == true)
                {
                    // ************ Окно списка файлов ***********

                    string[] readpath = fileDialog.FileNames;
                    string[] writepath = fileDialog.FileNames;
                    string[] filenames = fileDialog.SafeFileNames;

                    numberfiles = filenames.Length;
                    path = readpath;
                    
                    for (int i = 0; i < numberfiles; i++)
                    {
                        textbox.Text = ("Выбрано файлов: " + path[i] ); //path[i]
                    }
                   
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
           


        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string idmainplaylist = "";
            string iddeleteplaylist = "";

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Air Playlist| *.MCRlist";   // Фильтр расширения файлов
            fileDialog.InitialDirectory = "C:\\REC";           // Директория по умолчанию
            fileDialog.Title = "Выбери файл для сохранения";
            fileDialog.FileName = "WeekPlaylist.MCRlist"; //Название файла по умолчанию
            fileDialog.DefaultExt = "MCRlist"; // Расширение файла по умолчанию

            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                // ********** Вычисляем ID первого плэйлиста
                using (StreamReader srid = new StreamReader(path[0]))
                {
                    string lineid;
                    while ((lineid = srid.ReadLine()) != null)
                    {
                        Match match = Regex.Match(lineid, "<mcrs_playlist><guid>(.*?)</guid>");
                        if (match.Success)
                        {
                            idmainplaylist = match.Groups[1].Value;
                        }
                    }
                }

                // ********** Запмсываем первый строчки плейлиста
                Stream file = fileDialog.OpenFile();
                
                StreamWriter fileWriter = new StreamWriter(file);
                
                string line;

                line = "<?xml version=\"1.0\"?> \n" +
                            "<!--cinegy air control playlist--> \n" +
                            "<mcrs_playlist><guid>" + idmainplaylist + "</guid>" +
                            "<version>3</version><TV_Format>1920x1080 16:9 25i</TV_Format>";
                fileWriter.WriteLine(line);

                
                    
                

                    // ********** Перебираем все открытые файлы
                    for (int i = 0; i < numberfiles; i++)
                {
                    using (StreamReader reader = new StreamReader(path[i]))
                    {
                        // ********** Вычисляем плэйлист ID у всех файлов для удаления
                        
                            string linedeleteid;
                            while ((linedeleteid = reader.ReadLine()) != null)
                            {
                                Match match = Regex.Match(linedeleteid, "<mcrs_playlist><guid>(.*?)</guid>");
                                if (match.Success)
                                {
                                    iddeleteplaylist = match.Groups[1].Value;
                                }
                            }
                        

                        MessageBox.Show("RRR");

            // ********** Удаляем первые и последнюю строчки плэйлиста у всех последующих файлов

                        while ((line = reader.ReadLine()) != null)
                        {

                            line = line.Replace("<?xml version=\"1.0\"?>", "");

                            line = line.Replace("<!--cinegy air control playlist-->", "");

                            line = line.Replace("<mcrs_playlist><guid>" + iddeleteplaylist + 
                                "</guid><version>3</version><TV_Format>1920x1080 16:9 25i</TV_Format>", "");
                            
                            line = line.Replace("</mcrs_playlist>", "");

                            fileWriter.WriteLine(line);         // Записываем содержимое в файл
                                                                
                        }

                    }

                }
                // ********** дописываем в конец файла финальную строчку плэйлиста
                line = "</mcrs_playlist>";
                fileWriter.WriteLine(line);

                fileWriter.Close();
                file.Close();

                MessageBox.Show("Готово");
                Application.Current.Shutdown();

                // StartAir();
            }
        }
        
        void StartAir()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"C:\REC\Playlist\Пятница.MCRlist";
            p.Start();
        }     
    }
}

