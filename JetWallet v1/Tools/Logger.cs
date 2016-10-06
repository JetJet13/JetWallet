using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Tools
{
    public static class Logger
    {
        private static object writing = new object();
        private static FileStream fs;
        private static StreamWriter writer;
        private static string FilePath
        {
            get;
            set;
        }

        
        public static void StartLogging(string walletFolderPath)
        {
            SetFilePath(walletFolderPath);
            fs = File.Open(FilePath, FileMode.Append);
            writer = new StreamWriter(fs);
        }

        public static void SetFilePath(string walletFolderPath)
        {
            string logpath = Path.Combine(walletFolderPath, "debug.log");
            FilePath = logpath;
        }

        public static void StopLogging()
        {            
            writer.Dispose();
            writer = null;
            fs.Dispose();            
        }

        public static void WriteLine(string content)
        {
                if (writer == null)
                {
                    return;
                }
                writer.WriteLine(content);                       
        }
    }
}
