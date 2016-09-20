using GalaSoft.MvvmLight;
using System;
using System.Windows.Media;
using JetWallet.Tools;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.ViewModel
{

    public class ColorSchemeViewModel : ViewModelBase
    {
        
        public const string ColorPickPropertyName = "ColorPick";

        private Color _colorpick = MaterialColorThemes.BlueGrey;

        
        public Color ColorPick
        {
            get
            {
                return _colorpick;
            }

            set
            {
                if (_colorpick == value)
                {
                    return;
                }

                _colorpick = value;
                RaisePropertyChanged(ColorPickPropertyName);
                Messenger.Default.Send<string>(_colorpick.ToString(), "UpdateColorScheme");
            }
        }
        
        public ColorSchemeViewModel()
        {
        }

        public void SetColorPick(string s)
        {
            ColorPick = (Color)ColorConverter.ConvertFromString(s);
        }
    }
}