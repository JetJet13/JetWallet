using System;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using NBitcoin;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace JetWallet.Tools
{
    public class Converters
    {
        public static decimal Btc2Currency(Money val)
        {
            decimal valBtc = val.ToDecimal(MoneyUnit.BTC);
            decimal calc = valBtc * Global.VML.Currency.ActivePrice;
            decimal newAmount = Math.Round(calc, 2);
            return newAmount;
        }

        public static Money Currency2Btc(decimal val)
        {
            decimal roundVal = Math.Round(val, 2);
            decimal calc = (roundVal / Global.VML.Currency.ActivePrice);
            Money newAmountBtc = new Money(calc, MoneyUnit.BTC);
            return newAmountBtc;
        }

        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static string ByteArrayToHexString(byte[] value)
        {
            SoapHexBinary shb = new SoapHexBinary(value);
            return shb.ToString();
        }

    }
}
