using System;
using System.IO;

namespace LoggerLib
{
    public class Logger
    {
        public static void WriteLine(object obj)
        {
            using var streamWriter = File.AppendText("log.txt");
            streamWriter.WriteLine(obj);
        }
    }
}
