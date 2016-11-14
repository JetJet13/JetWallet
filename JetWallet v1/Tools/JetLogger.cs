using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JetWallet.Tools
{
    public class JetLogger
    {
        public static void Log(string logMessage)
        {
            string path = LoggerTools.GetFilePath();

            using (StreamWriter writer = File.AppendText(path))
            {
                writer.Write("\r\nLog Entry : ");
                writer.WriteLine("{0} {1}:", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                writer.Write(Environment.NewLine);
                writer.WriteLine(logMessage);
                writer.Write(Environment.NewLine);
                writer.WriteLine("-------------------------------");
            }
        }

        public static void LogException(Exception e)
        {
            string path = LoggerTools.GetFilePath();

            using (StreamWriter writer = File.AppendText(path))
            {
                writer.Write("\r\nLog Entry : ");
                writer.WriteLine("{0} {1}:", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                writer.WriteLine(Environment.NewLine);
                writer.WriteLine("Exception: {0}", e.ToString());
                writer.Write(Environment.NewLine);
                writer.WriteLine("Message: {0}", e.Message);
                writer.Write(Environment.NewLine);
                writer.WriteLine("StackTrace: {0}", e.StackTrace);
                writer.Write(Environment.NewLine);
                writer.WriteLine("-------------------------------");
            }
        }
    }
}
