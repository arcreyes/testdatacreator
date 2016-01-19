using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace TestDataGenerator
{
    public class FieldInformation : INotifyPropertyChanged
    {
        public FieldInformation(string fieldName, EFieldType eFieldType)
        {
            FieldName = fieldName;
            FieldType = eFieldType;
            Alignment = EAlignment.eLeft;
        }

        public override string ToString()
        { 
            return FieldName;
        }

        #region Properties
        private string fieldName;
        public string FieldName 
        {
            get 
            { 
                return fieldName; 
            }
            set
            {
                fieldName = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("FieldName"));
            }
        }

        public RectangleF Position { get; set; }
        public EUnit Unit { get; set; }
        public EAlignment Alignment { get; set; }
        public EFieldType FieldType { get; set; }
        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        #endregion


    }
}
