using System;
using System.Collections.Generic;
using System.Text;

namespace TestDataGenerator
{
    public class TextColor
    {
        public TextColor()
        {
            Red = 0.0;
            Green = 0.0;
            Blue = 0.0;
            ColorNamespace = EColorNamespace.eRGB;
        }
        public TextColor(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            ColorNamespace = EColorNamespace.eRGB;
        }

        public TextColor(double cyan, double magenta, double yellow, double black)
        {
            Cyan = cyan;
            Magenta = magenta;
            Yellow = yellow;
            Black = black;
            ColorNamespace = EColorNamespace.eCMYK;
        }

        public TextColor(double gray)
        {
            Gray = gray;
            ColorNamespace = EColorNamespace.eGray;
        }

        //public static bool operator ==(TextColor a, TextColor b)
        //{
        //    if (a.ColorNamespace != b.ColorNamespace)
        //        return false;

        //    if (a.ColorNamespace == EColorNamespace.eRGB)
        //    {
        //        return a.Red == b.Red &&
        //            a.Green == b.Green &&
        //            a.Blue == b.Blue;
        //    }
        //    else if (a.ColorNamespace == EColorNamespace.eCMYK)
        //    {
        //        return a.Cyan == b.Cyan &&
        //            a.Magenta == b.Magenta &&
        //            a.Yellow == b.Yellow &&
        //            a.Black == b.Black;
        //    }
        //    else if (a.ColorNamespace == EColorNamespace.eGray)
        //    {
        //        return a.Gray == b.Gray;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool operator !=(TextColor a, TextColor b)
        //{
        //    return !(a==b);
        //}

        #region Properties
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Cyan { get; set; }
        public double Magenta { get; set; }
        public double Yellow { get; set; }
        public double Black { get; set; }
        public double Gray { get; set; }
        public EColorNamespace ColorNamespace { get; set; }
        #endregion
    }
}
