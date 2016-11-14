using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Collections;

namespace JetWallet.Tools
{
    class FileSecurity
    {
        public static void EncryptWalletFile(IWallet jetWallet, string encryptionKey, string path)
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

                    AES.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
                    AES.IV = ASCIIEncoding.ASCII.GetBytes(encryptionKey.Substring(0, 16));

                    ICryptoTransform aesEncrypt = AES.CreateEncryptor();
                    using (CryptoStream cryptostream = new CryptoStream(fsEncrypted, aesEncrypt, CryptoStreamMode.Write))
                    {                        
                        IFormatter formatter = new BinaryFormatter();
                        var table = ConverterTools.Wallet2Hashtable(jetWallet);
                        formatter.Serialize(cryptostream, table);
                    }

                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void DecryptWalletFile(string path, string key)
        {
            try
            {
                AesCryptoServiceProvider AES = new AesCryptoServiceProvider();

                AES.KeySize = 256;
                AES.BlockSize = 128;

                AES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                AES.IV = ASCIIEncoding.ASCII.GetBytes(key.Substring(0, 16));

                using (FileStream fsread = new FileStream(path, FileMode.Open, FileAccess.Read))
                {

                    ICryptoTransform desdecrypt = AES.CreateDecryptor();

                    using (CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        Hashtable result = (Hashtable)formatter.Deserialize(cryptostreamDecr);
                        WalletGenerator.GenerateWalletFromFile(result);
                    }

                }

            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        public static void ApplyFileDefense(string path)
        {
            if (File.Exists(path))
            {
                bool recoveryFile = path.Contains("wallet.recover.jet");

                if (recoveryFile)
                {
                    File.SetAttributes(path, FileAttributes.Hidden | FileAttributes.ReadOnly);
                    return;
                }

                File.SetAttributes(path, FileAttributes.ReadOnly);
            }
            else
            {
                throw new Exception("Wallet File does not exist, " + path);
            }
           
        }

        public static void LiftFileDefense(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }

        }
    }
}
