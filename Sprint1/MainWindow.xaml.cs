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
            directory = path.FileName; //set string directory to file path
            
        }
        private void Study_Area(object sender, TextChangedEventArgs e)
        {
            //will grab study area
        }

        private void Submit_clicked(object sender, RoutedEventArgs e)
        {

            string fileName = @"C:\\Users\\nigel\\source\\repos\\test.py"; //sets file path for python script(test.py is our testing script)
            Process p = new Process();

            p.StartInfo = new ProcessStartInfo(@"C:\\Users\\nigel\\AppData\\Local\\Programs\\Python\\Python37\\python", fileName)//sets python.exe file path and unites with the script file path
            {

                RedirectStandardOutput = true, 
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = string.Format("{0} {1}", fileName, directory)//sends directory variable to the python script via cmd line argument
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
