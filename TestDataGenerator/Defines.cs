using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TestDataGenerator
{
    public enum EFieldType
    {
        eFieldText = 0,
        eFieldImage,
        eFieldPath
    };

    public enum EColorNamespace
    {
        eRGB = 0,
        eCMYK,
        eGray
    };

    public enum EAlignment
    {
        eLeft = 0,
        eCenter,
        eRight
    }

    public static class PaperSizes
    {
        public static Size A4 = new Size(595, 892);
        public static Size Letter = new Size(612, 792);
    }

    public static class Converter
    {

        public static string EFieldTypeToString(EFieldType eFieldType)
        {
            string str = string.Empty;

            switch(eFieldType)
            {
                case EFieldType.eFieldText:
                    str = "Text";
                    break;
                case EFieldType.eFieldImage:
                    str = "Image";
                    break;
                case EFieldType.eFieldPath:
                    str = "Path";
                    break;
                default:
                    break;
            }

            return str;
        }

        public static string EAlignmentToString(EAlignment eAlignment)
        {
            string str = string.Empty;

            switch (eAlignment)
            {
                case EAlignment.eLeft:
                    str = "Left";
                    break;
                case EAlignment.eCenter:
                    str = "Center";
                    break;
                case EAlignment.eRight:
                    str = "Right";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

}
