using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetWallet_v1.Model;
using GalaSoft.MvvmLight.Messaging;
using NBitcoin;
using NBitcoin.Protocol;
using NBitcoin.SPV;
using JetWallet_v1.ViewModel;

namespace JetWallet_v1.Tools
{
    class FileTools
    {        
                
        private static void FileDefense(string path, bool hidden = false)
        {
            if (hidden)
            {
                File.SetAttributes(path, FileAttributes.Encrypted | FileAttributes.Hidden | FileAttributes.ReadOnly);
                return;
            }

            File.SetAttributes(path, FileAttributes.Encrypted | FileAttributes.ReadOnly);
        }

        public static bool SaveNewWallet(WalletModel newWallet)
        {
            string path = newWallet.FileLocation;
            string recoveryPath = newWallet.FileRecover;

            if (File.Exists(path) && File.Exists(recoveryPath))
            {
                string message = TextTools.RetrieveStringFromResource("Error_A200")
                    .Replace("*path*",path)
                    .Replace("*recoverypath*", recoveryPath);
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                return false;
            }
            else if (File.Exists(path))
            {
                string existingWalletName = TextTools.DecodeWalletName(path);
                string message = TextTools.RetrieveStringFromResource("Error_A201")
                    .Replace("*name*", existingWalletName)
                    .Replace("*path*", path);
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                return false;
            }
            else if (File.Exists(recoveryPath))
            {
                string existingWalletName = TextTools.DecodeWalletName(recoveryPath);
                string message = TextTools.RetrieveStringFromResource("Error_A202")
                    .Replace("*name*", existingWalletName)
                    .Replace("*recoverypath*", recoveryPath);
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                return false;
            }


            FileTools.EncryptWallet(newWallet, newWallet.PassHash, path);
            FileTools.EncryptWallet(newWallet, newWallet.RecoveryPhraseHash, recoveryPath);

            string addmanPath = Path.Combine(App.WalletsDir, newWallet.Name, "addrman.dat");
            new AddressManager().SavePeerFile(addmanPath, newWallet.NetworkChoice);
            // Make Wallet File Encrypted and ReadOnly
            // Make Recovery File Hidden and Readonly
            FileDefense(path);
            FileDefense(recoveryPath, true);
            return true;

        }

        
        public static void UpdateWalletFile(WalletModel newWallet)
        {
            
            string path = newWallet.FileLocation;
            string recoveryPath = newWallet.FileRecover;

            if (File.Exists(path) && File.Exists(recoveryPath))
            {

                // set wallet files to normal for editing/updating purposes
                File.SetAttributes(recoveryPath, FileAttributes.Normal);
                File.SetAttributes(path, FileAttributes.Normal);

                FileTools.EncryptWallet(newWallet, newWallet.PassHash, path);
                FileTools.EncryptWallet(newWallet, newWallet.RecoveryPhraseHash, recoveryPath);
                
                // reapply defensive settings
                FileDefense(path);
                FileDefense(recoveryPath, true);

            }
            else
            {
                // Wallet Files did not update...Wallet Files do not exist at specified location
                string message = TextTools.RetrieveStringFromResource("Error_A300");
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");

            }



        }

        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);


        public static bool EncryptWallet(WalletModel wallet, string sKey, string path)
        {
            try
            {
                using (FileStream fsEncrypted = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    AesCryptoServiceProvider AES = new AesCryptoServiceProvider();

                    // AES key size uses 256 bit (32 byte) encryption
                    // AES iv size uses 128 (16 byte) encryption
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    AES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    AES.IV = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 16));

                    ICryptoTransform desencrypt = AES.CreateEncryptor();
                    using (CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write))
                    {
                        
                        Hashtable data = Generators.Wallet2HashTable(wallet);

                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(cryptostream, data);
                    }
                    
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
        public static WalletModel DecryptWallet(string path, string sKey)
        {
            WalletModel newWallet;
            AesCryptoServiceProvider AES = new AesCryptoServiceProvider();

            AES.KeySize = 256;
            AES.BlockSize = 128;

            AES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            AES.IV = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 16));

            using (FileStream fsread = new FileStream(path, FileMode.Open, FileAccess.Read))
            {

                ICryptoTransform desdecrypt = AES.CreateDecryptor();

                using (CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                {
                    IFormatter formatter = new BinaryFormatter();

                    Hashtable rData = (Hashtable)formatter.Deserialize(cryptostreamDecr);

                    newWallet = Generators.HashTable2Wallet(rData);                    
                }
                    
            }
            return newWallet;
            
        }

        public static bool CheckPasswordAttempt(string path, string pass)
        {
            bool attemptResult = false;
            AesCryptoServiceProvider AES = new AesCryptoServiceProvider();

            AES.KeySize = 256;
            AES.BlockSize = 128;

            AES.Key = ASCIIEncoding.ASCII.GetBytes(pass);

            AES.IV = ASCIIEncoding.ASCII.GetBytes(pass.Substring(0, 16));

            using (FileStream fsread = new FileStream(path, FileMode.Open, FileAccess.Read))
            {


                ICryptoTransform desdecrypt = AES.CreateDecryptor();

                try
                {
                    using (CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        Hashtable rData = (Hashtable)formatter.Deserialize(cryptostreamDecr);
                    }
                    attemptResult = true;
                }
                catch
                {
                    attemptResult = false;
                }
                               
            }
            return attemptResult;
            
        }

        private static string GetConfPath()
        {
            var path = Path.Combine(App.AppDir, @"..\jet.conf");
            var fullPath = Path.GetFullPath(path);
            return fullPath;
        }

        public static void FetchConf()
        {
            string path = GetConfPath();
            if (File.Exists(path))
            {                  
                StreamReader reader = new StreamReader(path);
                string walletpath = reader.ReadLine();
                string colorScheme = reader.ReadLine();
                string language = reader.ReadLine();
                string currency = reader.ReadLine();

                Global.VML.ColorScheme.SetColorPick(colorScheme);
                Global.VML.Language.SetLanguage(language);
                Global.VML.Currency.UpdateActiveProps(currency);

                reader.Close();
                if (!walletpath.Equals("None") && File.Exists(walletpath))
                {
                    CheckPassword.InvokePasswordPrompt(walletpath);
                }                   

            }
            else
            {
                CreateConf();
            }

        }

        public static void SetConfCurrency(string curr)
        {
            string path = GetConfPath();
            if (File.Exists(path))
            {                
                string[] arrLine = File.ReadAllLines(path);
                {
                    arrLine[3] = curr;
                    File.WriteAllLines(path, arrLine);                    
                    Global.VML.Currency.UpdateActiveProps(curr);
                }

            }
        }
        public static void SetConfColorScheme(string color)
        {
            string path = GetConfPath();
            if (File.Exists(path))
            {
                string[] arrLine = File.ReadAllLines(path);
                {
                    arrLine[1] = color;
                    File.WriteAllLines(path, arrLine);                   
                }

            }
        }
        public static void SetConfLanguage(string lang)
        {
            string path = GetConfPath();
            if (File.Exists(path))
            {                
                string[] arrLine = File.ReadAllLines(path);
                {
                    arrLine[2] = lang;
                    File.WriteAllLines(path, arrLine);                    
                    Global.VML.Language.SetLanguage(lang);
                }
            }           
            
        }

        public static void SetWalletPath(string walletPath)
        {
           string confPath = GetConfPath();
            if (File.Exists(confPath))
            {                
                string[] arrLine = File.ReadAllLines(confPath);
                {
                    arrLine[0] = walletPath;
                    File.WriteAllLines(confPath, arrLine);                   
                }

            }      
        }

        public static void ClearConfWalletSettings()
        {
            string defaultPath = "None";

            string path = GetConfPath();
            if (File.Exists(path))
            {
                string[] arrLine = File.ReadAllLines(path);
                {
                    arrLine[0] = defaultPath;
                    File.WriteAllLines(path, arrLine);
                }

            }

        }

        private static void CreateConf()
        {
            string path = GetConfPath();
            StreamWriter writer = new StreamWriter(path);

            // Default Settings
            string defaultPath = "None";
            string defaultColorScheme = MaterialColorThemes.BlueGrey.ToString();
            string defaultLanguage = "English";
            string defaultCurrency = "USD";

            writer.WriteLine(defaultPath);
            writer.WriteLine(defaultColorScheme);
            writer.WriteLine(defaultLanguage);
            writer.WriteLine(defaultCurrency);

            writer.Close();
            Global.VML.Currency.UpdateActiveProps(defaultCurrency);
            Messenger.Default.Send<string>("", "OpenSetLanguageView");
        }
    }
}
