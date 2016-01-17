using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TestDataGenerator
{
    public partial class CustomPanel : Panel
    {
        public CustomPanel()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
        }
    }
}
