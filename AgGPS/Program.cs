using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;

namespace AgGPS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] inputArguments)
        {
            try
            {
                string currentDir = null;
                string agGpsDir = null;

                var res = IsoXml.ConsoleNativeMethods.GetDirectories(inputArguments);

                if (res.ShouldConvert)
                {
                    if (res.OutputDirectory == null)
                    {
                        currentDir = Directory.GetCurrentDirectory();
                        agGpsDir = currentDir;
                    }
                    else
                    {
                        currentDir = res.InputDirectory;
                        agGpsDir = res.OutputDirectory;
                    }
                }
                else
                    return;
                

                if (!Directory.Exists(agGpsDir))
                    Directory.CreateDirectory(agGpsDir);

                Form1.ConvertToAgGPS(currentDir, agGpsDir);

                if (res.ShouldOpenDirectory)
                    Process.Start("explorer.exe", agGpsDir + "\\AgGPS");
            }
            catch (Exception ex)
            {
                StreamWriter wr = new StreamWriter("errorAg.txt");
                wr.WriteLine(ex.Message);
                wr.WriteLine(ex.StackTrace);
                wr.Flush();
                wr.Close();
            }


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }


    

}
