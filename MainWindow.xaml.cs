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
        public MainWindow()
        {
            InitializeComponent();
        }
        string directory;
        private async void upload_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog path = new OpenFileDialog(); //creates new OpenFileDialog class
            path.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; //creates custom file filter

            if (path.ShowDialog() == true)
            { //checks if the file path exists 
                URLBlock.Text = path.FileName; //displays file path in text box
            }
            //executepython(path.FileName);
            directory = path.FileName;
            //return path.FileName;
        }
        private void Study_Area(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Submit_clicked(object sender, RoutedEventArgs e)
        {
            //executepython(directory);
            ProcessStartInfo startInfo = new ProcessStartInfo(); //initializes new instance of process
            startInfo.FileName = "C:\\Users\\nigel\\AppData\\Local\\Programs\\Python\\Python37\\python.exe"; //gets the application to start
            startInfo.Arguments = $"test.py \"{directory}\""; // Pass the selected directory as an argument to the Python script
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
        }
        /*
        private void executepython(string directory)
        {
            

           
        }
        */
    }
}
