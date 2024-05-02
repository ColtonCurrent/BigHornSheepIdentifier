using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Python.Runtime;
using System.ComponentModel;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Windows.Forms;

namespace AppDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class ImageNameCSV //class to structure data from CSV
    {
        [Name("Image Name")]
        public string? name { get; set; }

        [Name("Identification")]
        public string? identification { get; set; }
    }
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
            bighornWorker.WorkerReportsProgress = true;
            bighornWorker.DoWork += bighornWorker_DoWork;//assosiate our do work function with the DoWork method of our BackgroundWorker class
            

            bighornWorker.RunWorkerCompleted += bighornWorker_RunWorkerCompleted;

            
        }




        private void upload_Click(object sender, RoutedEventArgs e)
        {

            Completion.Content = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog(); //creates new OpenFolderDialog class
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)//checks if the folder path exists 
            {
                URLBlock.Text = folderBrowserDialog1.SelectedPath;//displays folder path in text box
            }
            directory = folderBrowserDialog1.SelectedPath; //set string directory to folder path 

        }

        private void Submit_clicked(object sender, RoutedEventArgs e)
        {
            
            studyArea = StudyArea.Text.Trim().Replace(" ", "");//grabs the StudyArea 
            siteName = SiteName.Text.Trim().Replace(" ", "");//grabs the SiteName
            //ModelProgress.Value = 0;
            
            if (studyArea != "" && siteName != "" && directory != "")
            {
                submit.IsEnabled = false;
                SpinningBar.Visibility = Visibility.Visible;
                Completion.Content = "In Progress";
                bighornWorker.RunWorkerAsync();//starts a new thread and calls the DoWork method
            }
            else
            {
                Completion.Content = "Missing Information";
            }
                
            
            


        }

        private void ImageGallery()
        {
            var csvName = $@"{studyArea}-{siteName}.csv"; //stores the lastest csv file name 
            var directoryCSV = $@"{directory}" + "\\" + $"{csvName}"; //creates the directory for the csv
            var imagesList = new List<string>();//stores image names from the clients folder
            string[] folder = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly); //grabs images from folder
            int count = 0;
            if (File.Exists(directoryCSV))
            {
                try
                {
                    int index = 0;
                    using (var reader = new StreamReader(directoryCSV))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) //reads csv data
                        {
                            var records = csv.GetRecords<ImageNameCSV>().ToList(); //enumerates through csv
                            foreach (var record in records)
                            {
                                if (record.identification == "Bighorn Sheep" && imagesList.Count < 10) { imagesList.Add(record.name); } //adds the desired data to imagesList
                            }
                            foreach (var i in imagesList)
                            {
                                if (index != 10)
                                {
                                    foreach (string file in folder)
                                    {
                                        string[] temp = file.Split("\\");
                                        var tempImageName = temp.Last();
                                        //Trace.WriteLine(imageName);
                                        if (tempImageName == i) //compares the name of the current image in the folder to the name of the image from imagesList.
                                        {
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                BitmapImage bitmap = new BitmapImage();
                                                bitmap.BeginInit();
                                                bitmap.UriSource = new Uri(file);
                                                bitmap.EndInit();
                                                if (count == 0) { image1.Source = bitmap; }
                                                if (count == 1) { image2.Source = bitmap; }
                                                if (count == 2) { image3.Source = bitmap; }
                                                if (count == 3) { image4.Source = bitmap; }
                                                if (count == 4) { image5.Source = bitmap; }
                                                if (count == 5) { image6.Source = bitmap; }
                                                if (count == 6) { image7.Source = bitmap; }
                                                if (count == 7) { image8.Source = bitmap; }
                                                if (count == 8) { image9.Source = bitmap; }
                                                if (count == 9) { image10.Source = bitmap; }
                                            });
                                            count++;
                                        }
                                    }

                                }
                                index++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else { Trace.WriteLine("file not found"); }
        }
    

    private void bighornWorker_DoWork(object Sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            
                
                
            string pythonExe = "CNN_predict_finale.exe";
            string modelName = "armas_gigas.h5";

            









            Process p = new Process();//creates a new process
            p.StartInfo = new ProcessStartInfo(pythonExe)
            //graps an instance of python and starts the process given in fileName
            {

                RedirectStandardOutput = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true,
                
               
                //sends variables to the python script via cmd line argument
                Arguments = string.Format("{0} {1} {2} {3}",
                            modelName,
                            directory, 
                            studyArea, 
                            siteName)//replaces any spaces  
            };
            p.Start();//stants the process

            //p.OutputDataReceived += bighornWorker_ProgressChanged;

            //p.BeginOutputReadLine();
            /* var reader = p.StandardOutput;
             while (!reader.EndOfStream)
             {
                 // the point is that the stream does not end until the process has 
                 // finished all of its output.
                 Dispatcher.Invoke(new ThreadStart(() => ModelProgress.Value = Double.Parse(reader.ReadLine())));

             }*/

            p.WaitForExit();//waits until the process has exited
            






        }

        /*private void bighornWorker_ProgressChanged(object sender, DataReceivedEventArgs e)
        {
            
            if (!String.IsNullOrEmpty(e.Data))
            {
                
                Trace.WriteLine(e.Data);

                
                System.Windows.Application.Current.Dispatcher.Invoke(new ThreadStart(() => ModelProgress.Value = Double.Parse(e.Data)));
               
            }
            
        }*/

        void bighornWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SpinningBar.Visibility = Visibility.Collapsed;
            Completion.Content = "MODEL COMPLETE";
            ImageGallery();
            submit.IsEnabled = true;
            
        }

        
       


    }
}
