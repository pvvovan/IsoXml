using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IsoXmlGui
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

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                txtBoxSource.Text = fbd.SelectedPath;

            setConvertEnabled();

            //OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.FileName = "";
            //openFileDialog1.CheckPathExists = true;
            //openFileDialog1.ShowReadOnly = false;
            //openFileDialog1.ReadOnlyChecked = true;
            //openFileDialog1.CheckFileExists = false;
            //openFileDialog1.ValidateNames = false;

            //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    // openFileDialog1.FileName should contain the folder and a dummy filename
            //}
        }

        private void btnOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                txtBoxOutput.Text = fbd.SelectedPath;

            setConvertEnabled();
        }

        void setConvertEnabled()
        {
            if (txtBoxSource.Text != "" && txtBoxOutput.Text != "")
            {
                btnToAgGps.IsEnabled = true;
                btnToIsoXml.IsEnabled = true;
            }
            else
            {
                btnToAgGps.IsEnabled = false;
                btnToIsoXml.IsEnabled = false;
            }
        }

        private void btnToAgGps_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("AgGPS.exe", "-i \"" + txtBoxSource.Text + "\" -o \"" + txtBoxOutput.Text + "\"");
            string isoXmlDir = txtBoxSource.Text;
            string agGpsDir = txtBoxOutput.Text;
            AgGPS.Form1.ConvertToAgGPS(isoXmlDir, agGpsDir);
            Process.Start("explorer.exe", agGpsDir + "\\AgGPS");
        }

        private void btnToIsoXml_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("IsoXml.exe", "-i \"" + txtBoxSource.Text + "\" -o \"" + txtBoxOutput.Text + "\"");
            string agGpsDir = txtBoxSource.Text;
            string isoXmlDir = txtBoxOutput.Text;
            IsoXml.Program.ConvertToIsoXml(agGpsDir, isoXmlDir);
            Process.Start("explorer.exe", isoXmlDir);
        }
    }
}
