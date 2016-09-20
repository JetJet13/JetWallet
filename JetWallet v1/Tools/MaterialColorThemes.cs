using System;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JetWallet_v1.Model;


namespace JetWallet_v1.Tools
{
    public class MaterialColorThemes:INotifyPropertyChanged
    {

        public Color ColorPick
        {
            get;
            set;
        }

        private static Color _red = Color.FromArgb(255, 183, 28, 28);
        public static Color Red { get { return _red; } }

        private static Color _pink = Color.FromArgb(255, 245, 0, 87);
        public static Color Pink { get { return _pink; } }

        private static Color _purple = Color.FromArgb(255, 74, 20, 140);
        public static Color Purple { get { return _purple; } }

        private static Color _deeppurple = Color.FromArgb(255, 98, 0, 234);
        public static Color DeepPurple { get { return _deeppurple; } }

        private static Color _indigo = Color.FromArgb(255, 26, 35, 126);
        public static Color Indigo { get { return _indigo; } }

        private static Color _blue = Color.FromArgb(255, 41, 98, 255);
        public static Color Blue { get { return _blue; } }

        private static Color _lightblue = Color.FromArgb(255, 0, 145, 234);
        public static Color LightBlue { get { return _lightblue; } }

        private static Color _cyan = Color.FromArgb(255, 0, 188, 212);
        public static Color Cyan { get { return _cyan; } }

        private static Color _teal = Color.FromArgb(255, 0, 150, 136);
        public static Color Teal { get { return _teal; } }

        private static Color _green = Color.FromArgb(255, 0, 200, 83);
        public static Color Green { get { return _green; } }

        private static Color _lightgreen = Color.FromArgb(255, 100, 221, 23);
        public static Color LightGreen { get { return _lightgreen; } }

        private static Color _limegreen = Color.FromArgb(255, 174, 234, 0);
        public static Color LimeGreen { get { return _limegreen; } }

        private static Color _yellow = Color.FromArgb(255, 255, 214, 0);
        public static Color Yellow { get { return _yellow; } }

        private static Color _amber = Color.FromArgb(255, 255, 171, 0);
        public static Color Amber { get { return _amber; } }

        private static Color _orange = Color.FromArgb(255, 255, 109, 0);
        public static Color Orange { get { return _orange; } }

        private static Color _deeporange = Color.FromArgb(255, 255, 61, 0);
        public static Color DeepOrange { get { return _deeporange; } }

        private static Color _brown = Color.FromArgb(255, 62, 39, 35);
        public static Color Brown { get { return _brown; } }

        private static Color _grey = Color.FromArgb(255, 97, 97, 97);
        public static Color Grey { get { return _grey; } }

        private static Color _bluegrey = Color.FromArgb(255, 38, 50, 56);
        public static Color BlueGrey { get { return _bluegrey; } }

        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
