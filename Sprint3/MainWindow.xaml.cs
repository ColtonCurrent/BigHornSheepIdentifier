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
        //Initialize our three user inputs
        string studyArea = "";
        string siteName = "";
        string directory = "";
        System.ComponentModel.BackgroundWorker bighornWorker = new System.ComponentModel.BackgroundWorker();// create an instance of the BackgroundWorker class
        public MainWindow()
        {
           
            InitializeComponent();
            bighornWorker.DoWork += bighornWorker_DoWork;//assosiate our do work function with the DoWork method of our BackgroundWorker class
        }
        
        private void upload_Click(object sender, RoutedEventArgs e)
        {

            Completion.Content = "";
            OpenFolderDialog folder = new OpenFolderDialog(); //creates new OpenFolderDialog class
            if (folder.ShowDialog() == true)//checks if the folder path exists 
            {
                URLBlock.Text = folder.FolderName;//displays folder path in text box
            }
            directory = folder.FolderName; //set string directory to folder path 

        }

        private void Submit_clicked(object sender, RoutedEventArgs e)
        {
            
            studyArea = StudyArea.Text;//grabs the StudyArea 
            siteName = SiteName.Text;//grabs the SiteName
            bighornWorker.RunWorkerAsync();//starts a new thread and calls the DoWork method
            

        }

        private void bighornWorker_DoWork(object Sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new ThreadStart(() => Completion.Content = "Please enter a site name and study area"));
            if (studyArea != "" || siteName != "")//Only runs the python file if the use has entered a site name and study area
            {
                Dispatcher.BeginInvoke(new ThreadStart(() => Completion.Content = "In Progress"));
                string pythonModel = @"3_class_predictions_and_file_writing.exe";


                Process p = new Process();//creates a new process


                //p.StartInfo = new ProcessStartInfo(@"C:\\Python311\\python", fileName)
                p.StartInfo = new ProcessStartInfo(pythonModel)
                //graps an instance of python and starts the process given in fileName
                {

                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    //sends variables to the python script via cmd line argument
                    Arguments = string.Format("{0} {1} {2} {3}",
                                pythonModel, directory, 
                                studyArea.Replace(" ", "_"), 
                                siteName.Replace(" ", "_"))//replaces any spaces with a _ 
                };

                p.Start();//stants the process
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                Console.WriteLine(output);
                Console.WriteLine(error);
                p.WaitForExit();//waits until the process has exited
                
                Dispatcher.BeginInvoke(new ThreadStart(() => Completion.Content = "Model Complete"));//Grabs the label from the owning thread and modifies it
            }

        }
        
    }
}
