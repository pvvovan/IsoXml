using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IsoXml
{
    public static class ConsoleNativeMethods
    {
        // http://msdn.microsoft.com/en-us/library/ms681944(VS.85).aspx
        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// A process can be associated with only one console,
        /// so the function fails if the calling process already has a console.
        /// </remarks>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        private const int ATTACH_PARENT_PROCESS = -1;

        // http://msdn.microsoft.com/en-us/library/ms683150(VS.85).aspx
        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// If the calling process is not already attached to a console,
        /// the error code returned is ERROR_INVALID_PARAMETER (87).
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int FreeConsole();

        static void AllocConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }

        public class InteractionResult
        {
            public string InputDirectory;
            public string OutputDirectory;
            public bool ShouldConvert;
            public bool ShouldOpenDirectory;
        }

        public static InteractionResult GetDirectories(string[] inputArguments)
        {
            InteractionResult res = new InteractionResult();

            if (inputArguments.Count() == 1)
                if (inputArguments[0].ToLower() == "-h")
                {
                    AllocConsole();
                    Console.WriteLine("AgGPS –i input_directory –o output_directory");
                    FreeConsole();
                    res.ShouldConvert = false;
                    return res;
                }

            res.ShouldConvert = true;
            res.ShouldOpenDirectory = true;
            if (inputArguments.Count() == 4)
            {
                if (inputArguments[0].ToLower() == "-i" && inputArguments[2].ToLower() == "-o")
                {
                    res.InputDirectory = inputArguments[1];
                    res.OutputDirectory = inputArguments[3];
                }
                else if (inputArguments[0].ToLower() == "-o" && inputArguments[2].ToLower() == "-i")
                {
                    res.InputDirectory = inputArguments[3];
                    res.OutputDirectory = inputArguments[1];
                }
                res.ShouldOpenDirectory = false;
            }

            return res;
        }
    }
}
