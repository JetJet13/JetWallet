using System.Windows;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using JetWallet_v1.Tools;
using JetWallet_v1.ViewModel;
using System.Diagnostics;
using JetWallet_v1.View;
using System.Threading;

/// <summary>
///    JetWallet is a Bitcoin wallet that let's users hold, send and receive bitcoins.
///    At the time of writing, JetWallet is only available on Windows.
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


namespace JetWallet_v1
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

        private static Mutex myMutex;
        public static void ExistingInstance()
        {
            bool aIsNewInstance = false;
            myMutex = new Mutex(true, "JetWallet", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                MessageBox.Show("JetWallet is already running");
                App.Current.Shutdown();
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
