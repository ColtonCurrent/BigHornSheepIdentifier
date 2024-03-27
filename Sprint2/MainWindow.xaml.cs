using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Python.Runtime;

namespace AppDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

     
    public partial class MainWindow : Window
    {
        string studyArea = "";
        string siteName = "";
        string directory = "";
        System.ComponentModel.BackgroundWorker bighornWorker = new System.ComponentModel.BackgroundWorker();
        public MainWindow()
        {
           
            InitializeComponent();
            bighornWorker.DoWork += bighornWorker_DoWork;
        }
        
        private void upload_Click(object sender, RoutedEventArgs e)
        {
            
           
            OpenFolderDialog folder = new OpenFolderDialog(); //creates new OpenFolderDialog class
            if (folder.ShowDialog() == true)//checks if the folder path exists 
            {
                URLBlock.Text = folder.FolderName;//displays folder path in text box
            }
            directory = folder.FolderName; //set string directory to folder path 

        }

        private void Submit_clicked(object sender, RoutedEventArgs e)
        {
            studyArea = StudyArea.Text;
            siteName = SiteName.Text;
            bighornWorker.RunWorkerAsync();

        }

        private void bighornWorker_DoWork(object Sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //studyArea = StudyArea.Text.Replace(" ", "_");
            if(studyArea == "") { studyArea = "Weird but ok"; }
            //studyArea = "Weird but ok";
            if (studyArea != "" && siteName != "")
            {
                string fileName = @"E:\\CS495\\FInalProject\\CS495Project\\3_class_predictions_and_file_writing.py"; //sets file path for python script(test.py is our testing script)
                                                                                            //string fileName = @"E:\\CS495\\FInalProject\\CS495Project\\test.py";
                Process p = new Process();


                //p.StartInfo = new ProcessStartInfo(@"C:\\Users\\roomk\\AppData\\Local\\Programs\\Python\\Python312\\python", fileName)//sets python.exe file path and unites with the script file path


                p.StartInfo = new ProcessStartInfo(@"C:\\Python311\\python", fileName)
                {

                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    //Arguments = string.Format("{0} {1}", fileName, directory)
                    Arguments = string.Format("{0} {1} {2} {3}", fileName, directory, studyArea.Replace(" ", "_"), siteName.Replace(" ", "_"))//sends directory variable to the python script via cmd line argument
                };

                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                Console.WriteLine(output);
                Console.WriteLine(error);
                p.WaitForExit();

            }
        }
    }
}
