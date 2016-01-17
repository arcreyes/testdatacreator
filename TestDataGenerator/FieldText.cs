using System;
using System.Collections.Generic;
using System.Text;

namespace TestDataGenerator
{
    public class FieldText :  FieldInformation
    {
        public FieldText(string fieldName) : base(fieldName, EFieldType.eFieldText)
        {
            Color = new TextColor();
            Bold = false;
            Italic = false;
            Underline = false;
        }

        #region Properties
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public TextColor Color { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        #endregion
    }
}
