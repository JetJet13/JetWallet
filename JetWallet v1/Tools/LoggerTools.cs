using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Tools
{
    class LoggerTools
    {
        public static string GetFilePath()
        {
            string relativePath = Path.Combine(App.AppDir, @"../data.log");

            string path = Path.GetFullPath(relativePath);
            return path;
        }

        public static void ClearFile()
        {
            string path = GetFilePath();
            File.WriteAllText(path, string.Empty);
        }

        public static void CreateFile()
        {
            string path = GetFilePath();
            File.Create(path).Dispose();
        }
    }
}
