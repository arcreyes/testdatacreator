using System;
using System.Collections.Generic;
using System.Text;

namespace TestDataGenerator
{
    public class FieldImage : FieldInformation
    {
        public FieldImage(string fieldName) : base(fieldName, EFieldType.eFieldImage)
        {
            AutoFit = false;
        }

        #region Properties
        public bool AutoFit { get; set; }
        #endregion
    }
}
