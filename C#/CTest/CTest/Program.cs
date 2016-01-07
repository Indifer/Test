using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int b = 1;
            var bytes = File.ReadAllBytes("reader.txt");
            int p = PosterMatch(bytes);
            
            Console.WriteLine(p);

            //Process myProcess = new Process();
            //string fileName = @"ffmpeg.exe";
            //string para = @"-i 1.amr 1.mp3";
            //ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(fileName, para);
            //myProcess.StartInfo = myProcessStartInfo;
            //myProcess.Start();
            //while (!myProcess.HasExited)
            //{

            //    myProcess.WaitForExit();

            //}

            //int returnValue = myProcess.ExitCode;
        }

        [DllImport("PosterDll.dll", EntryPoint = "PosterMatch", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PosterMatch(byte[] imgData);

        //[DllImport("Win32Project1.dll", EntryPoint = "fnWin32Project1", CallingConvention = CallingConvention.Cdecl)]
        //private static extern int fnWin32Project1();
    }
}
