using System.Windows;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using JetWallet.Tools;
using JetWallet.ViewModel;
using System.Diagnostics;
using JetWallet.View;
using System.Threading;
using JetWallet.Model;
using JetWallet.Controller;

/// <summary>
///    JetWallet is a Bitcoin wallet that let's users hold, send and receive bitcoins.
///
///    JetWallet Copyright(C) 2016  Johny Georges
///
///    This program is free software: you can redistribute it and/or modify
///    it under the terms of the GNU General Public License as published by
///    the Free Software Foundation, either version 3 of the License, or
///    (at your option) any later version.
///
///    This program is distributed in the hope that it will be useful,
///    but WITHOUT ANY WARRANTY; without even the implied warranty of
///    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
///    GNU General Public License for more details.
///
///    You should have received a copy of the GNU General Public License
///    along with this program.If not, see http://www.gnu.org/licenses/
///    
///    For contact, jgeorges371@gmail.com
/// </summary>


namespace JetWallet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public static string AppDir
        {
            get { return System.AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string WalletsDir
        {
            get
            {
                var path = Path.Combine(AppDir, @"..\Wallets");
                var fullPath = Path.GetFullPath(path);
                return fullPath;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ExistingInstance();
            base.OnStartup(e);
        }

        private void ExistingInstance()
        {
            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If ther is more than one, than it is already running.
                MessageBox.Show("JetWallet is already running.");
                Application.Current.Shutdown();
                return;
            }
        }

        static App()
        {            
            DispatcherHelper.Initialize();
            CheckAppDirectory();
            CheckWalletsDirectory();

        }

        private static void CheckAppDirectory()
        {
            if (!Directory.Exists(AppDir))
            {
                Directory.CreateDirectory(AppDir);
            }
        }

        private static void CheckWalletsDirectory()
        {
            if (!Directory.Exists(WalletsDir))
            {
                Directory.CreateDirectory(WalletsDir);
            }
        }

        public static object Saving = new object();
        protected override void OnExit(ExitEventArgs e)
        {
            
            lock (Saving)
            {
                if (Global.ActiveWallet != null)
                {
                    
                    Global.ActiveWallet.Stop();
                    
                }
                base.OnExit(e);
            }
        }


    }
}
