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

    public enum EUnit
    {
        ePSPoint,
        eMm,
        eInch
    };

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

        public static EFieldType StringToEFieldType(string fieldType)
        {
            EFieldType e = EFieldType.eFieldText;

            switch (fieldType)
            {
                case "Text":
                    e = EFieldType.eFieldText;
                    break;
                case "Image":
                    e = EFieldType.eFieldImage;
                    break;
                case "Path":
                    e = EFieldType.eFieldPath;
                    break;
                default:
                    break;
            }

            return e;
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

        public static EAlignment StringToEAlignment(string alignment)
        {
            EAlignment e = EAlignment.eLeft;

            switch (alignment)
            {
                case "Left":
                    e = EAlignment.eLeft;
                    break;
                case "Center":
                    e = EAlignment.eCenter;
                    break;
                case "Right":
                    e = EAlignment.eRight;
                    break;
                default:
                    break;
            }

            return e;
        }

        public static string GetYesNo(bool bBool)
        {
            if (bBool == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }
    }

}
