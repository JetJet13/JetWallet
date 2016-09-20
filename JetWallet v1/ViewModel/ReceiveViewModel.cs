using System;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using JetWallet_v1.Tools;
using JetWallet_v1.Model;
using JetWallet_v1.View;
using NBitcoin;
using QRCoder;
using NBitcoin.SPV;

namespace JetWallet_v1.ViewModel
{
    
    public class ReceiveViewModel : ViewModelBase, IDisposable
    {
        private QRCodeGenerator _qrcoder;
        private ReceiveView _rview;

        public System.Windows.Media.Brush ColorScheme
        {
            get { return new System.Windows.Media.SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("Receive_Title"); }
        }

        public string TextAddress
        {
            get { return TextTools.RetrieveStringFromResource("Receive_Address"); }
        }

        public string TextCopy
        {
            get { return TextTools.RetrieveStringFromResource("Copy"); }
        }

        public string TextNotice
        {
            get { return TextTools.RetrieveStringFromResource("Receive_Notice"); }
        }

        public string TextCancel
        {
            get { return TextTools.RetrieveStringFromResource("Cancel"); }
        }


        public const string AddressPropertyName = "Address";
        private string _address = string.Empty;        
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                if (_address == value)
                {
                    return;
                }

                _address = value;
                RaisePropertyChanged(AddressPropertyName);
            }
        }

       
        public const string QRCodeImagePropertyName = "QRCodeImage";        
        private BitmapImage _qrcodeimage;
        public BitmapImage QRCodeImage
        {
            get
            {
                return _qrcodeimage;
            }

            set
            {
                if (_qrcodeimage == value)
                {
                    return;
                }

                _qrcodeimage = value;
                RaisePropertyChanged(QRCodeImagePropertyName);
            }
        }
                
        public ReceiveViewModel()
        {
            CopyCmd = new RelayCommand(() => { this.ExecuteCopy(); });
            CancelCmd = new RelayCommand(() => { this.ExecuteCancel(); });
            Messenger.Default.Register<string>(this, "OpenReceiveView", (string s) => { this.OpenView(s); });
            Messenger.Default.Register<string>(this, "NewReceivedTransactionFound", (string s) => { this.ShowSuccessDialog(s); });
        }

        public RelayCommand CopyCmd
        {
            get;
            private set;
        }
        public RelayCommand CancelCmd
        {
            get;
            private set;
        }
        

        private void OpenView(string s)
        {
            _qrcoder = new QRCodeGenerator();
            Address = s;
            QRCodeData qrCodeData = _qrcoder.CreateQrCode(Address, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            QRCodeImage = Converters.Bitmap2BitmapImage(qrCode.GetGraphic(20));
            _rview = new ReceiveView();
            _rview.ShowDialog();
        }

        private void ExecuteCopy()
        {
            Clipboard.SetText(Address);
        }

        private async void ExecuteCancel()
        {
            string title = TextTools.RetrieveStringFromResource("Receive_Dialog_Cancel_Title");
            string message = TextTools.RetrieveStringFromResource("Receive_Dialog_Cancel_Message");
            var result = await _rview.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                this.CloseView();
            }
        }

       
        private async void ShowSuccessDialog(string s)
        {
            if (_rview != null)
            {
                string title = TextTools.RetrieveStringFromResource("Receive_Dialog_Success_Title");
                string message = TextTools.RetrieveStringFromResource("Receive_Dialog_Success_Message").Replace("*amount*",s);
                await _rview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                this.CloseView();
            }
            return;
        }
        
        private void CloseView()
        {
            this.Dispose();
            _rview.Close();
        }

        public void Dispose()
        {
            ((IDisposable)_qrcoder).Dispose();
        }
    }
}