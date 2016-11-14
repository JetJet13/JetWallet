using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Tools
{
    class HashGenerator
    {
        public static string GeneratePasswordHash(SecureString password)
        {
            HashAlgorithm SHA = new SHA256CryptoServiceProvider();
            IntPtr ss = Marshal.SecureStringToGlobalAllocUnicode(password);
            byte[] passBytes = Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(ss));

            // Clear IntPtr from Memory after converting to byte array
            Marshal.ZeroFreeGlobalAllocUnicode(ss);

            byte[] hash = SHA.ComputeHash(passBytes);
            return BitConverter.ToString(hash).Replace("-", "").Substring(0, 32);

        }

        public static string GenerateRecoveryPhraseHash(string recoveryPhrase)
        {
            HashAlgorithm SHA = new SHA256CryptoServiceProvider();
            var testPassBytes = Encoding.UTF8.GetBytes(recoveryPhrase);
            byte[] hash = SHA.ComputeHash(testPassBytes);
            int passHashLength = 32;
            return BitConverter.ToString(hash).Replace("-", "").Substring(0, passHashLength);
        }
    }
}
