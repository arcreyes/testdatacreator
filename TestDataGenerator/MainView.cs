using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TestDataGenerator
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            fieldInformationList = new BindingList<FieldInformation>();

            listBoxFieldInformation.DisplayMember = "FieldName";
            listBoxFieldInformation.ValueMember = "FieldName";
            listBoxFieldInformation.DataSource = fieldInformationList;


            //customPanel1.Location

            Size clientSize = panel1.ClientSize;
            clientSize.Width -= 2; // remove 1 pixel on each side
            clientSize.Height -= 2; // remove 1 pixel on each side


            int width = (int)((double)clientSize.Height * paperSize.Width / paperSize.Height + 0.5);
            int height = panel1.ClientSize.Height-2;

            customPanel1.Left = clientSize.Width / 2 - width / 2;
            customPanel1.Top = 0;
            customPanel1.ClientSize = new Size(width, height);

                

            //textBoxFieldName.DataBindings.Add("FieldName", selectedFieldInfo, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        #region Private Members
        private BindingList<FieldInformation> fieldInformationList;
        private Point startPoint;
        private Point endPoint;
        private bool mousePressed = false;
        private Rectangle previewRect = Rectangle.Empty;
        private int selectedFieldInfoIndex = -1;
        private EMouseActions eAction = EMouseActions.eNone;
        private RectangleF originalRect;
        private Size paperSize = PaperSizes.A4;
        #endregion

        #region Enumarations
        private enum EMouseActions
        {
            eNone = 0,
            eCreate,
            eMove,
            eResize
        };

        private enum ERectanglePosition
        {
            eNone = 0,
            eTopLeft,
            eTopRight,
            eBottomLeft,
            eBottomRight,
            eCenter
        }
        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            var red = new Pen(Color.Red, 1);

            foreach (FieldInformation fieldInfo in this.fieldInformationList)
            {
                g.DrawRectangle(red, fieldInfo.Position.X, fieldInfo.Position.Y,
                    fieldInfo.Position.Width, fieldInfo.Position.Height);
            }

            if (previewRect.IsEmpty == false)
            {
                var green = new Pen(Color.Green, 1);
                g.DrawRectangle(green, previewRect.X, previewRect.Y, previewRect.Width, previewRect.Height);
            }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //base.OnMouseDown(e);

            mousePressed = true;

            startPoint = new Point(e.X, e.Y);
            endPoint = new Point(e.X, e.Y);

            selectedFieldInfoIndex = GetSelectedFieldInformation(startPoint);
            if (selectedFieldInfoIndex < 0)
            {
                eAction = EMouseActions.eCreate;

                Rectangle screenRect = customPanel1.RectangleToScreen(new Rectangle(Point.Empty, customPanel1.ClientSize));
                Cursor.Clip = screenRect;
            }
            else
            {
                eAction = EMouseActions.eMove;

                originalRect = fieldInformationList[selectedFieldInfoIndex].Position;

                int left = Math.Abs(startPoint.X - (int)originalRect.Left);
                int top = Math.Abs(startPoint.Y - (int)originalRect.Top);

                Rectangle controlRect = new Rectangle(Point.Empty, customPanel1.ClientSize - new Size((int)originalRect.Size.Width, (int)originalRect.Size.Height));
                controlRect.Offset(left, top);

                Rectangle screenRect = customPanel1.RectangleToScreen(controlRect);
                Cursor.Clip = screenRect;
            }

            
            
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed == true)
            {
                endPoint = new Point(e.X, e.Y);

                if (eAction == EMouseActions.eCreate)
                {
                    previewRect = GetRectangle(startPoint, endPoint);
                }
                else if (eAction == EMouseActions.eMove) 
                {
                    int x = endPoint.X - startPoint.X;
                    int y = endPoint.Y - startPoint.Y;

                    RectangleF rect = fieldInformationList[selectedFieldInfoIndex].Position;
                    rect.Offset(x, y);
                    fieldInformationList[selectedFieldInfoIndex].Position = rect;

                    startPoint = endPoint;
                }

                customPanel1.Invalidate();

                
            }
            else
            {
                Point position = new Point(e.X, e.Y);


                if (GetSelectedFieldInformation(position) != -1)
                {
                    customPanel1.Cursor = Cursors.SizeAll;
                }
                else
                {
                    customPanel1.Cursor = Cursors.Default;
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;

            if (eAction == EMouseActions.eCreate)
            {
                if (startPoint != endPoint)
                {
                    Rectangle rect = GetRectangle(startPoint, endPoint);

                    if (IntersectsWithFieldInformation(rect) == false)
                    {
                        FieldText fieldText = new FieldText("fieldtext" + (fieldInformationList.Count + 1).ToString());

                        fieldText.Position = rect;

                        this.fieldInformationList.Add(fieldText);
                        listBoxFieldInformation.Invalidate();

                        listBoxFieldInformation.SelectedItem = fieldText;
                    }
                }
            }
            else if (eAction == EMouseActions.eMove)
            {
                if (IntersectsWithFieldInformation(fieldInformationList[selectedFieldInfoIndex].Position, selectedFieldInfoIndex) == true)
                {
                    fieldInformationList[selectedFieldInfoIndex].Position = originalRect;
                }
            }

            previewRect = Rectangle.Empty;
            customPanel1.Invalidate();


            Cursor.Clip = new Rectangle(0, 0, 0, 0);
        }

        private void listBoxFieldInformation_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            int index = listBox.SelectedIndex;

            if (index < 0)
                return;

            FieldInformation fieldInfo = fieldInformationList[index];

            textBoxFieldName.Text = fieldInfo.FieldName;
            textBoxPosition.Text = fieldInfo.Position.ToString();
            textBoxAlignment.Text = fieldInfo.Alignment.ToString();

            if (fieldInfo.FieldType == EFieldType.eFieldText)
            {
                radioButtonText.Checked = true;
            }
            else
            {
                radioButtonImage.Checked = true;
            }


            textBoxFieldName.Refresh();
        }

        public static Rectangle GetRectangle(Point p1, Point p2)
        {
            int top = Math.Min(p1.Y, p2.Y);
            int bottom = Math.Max(p1.Y, p2.Y);
            int left = Math.Min(p1.X, p2.X);
            int right = Math.Max(p1.X, p2.X);

            Rectangle rect = Rectangle.FromLTRB(left, top, right, bottom);

            return rect;
        }

        private int GetSelectedFieldInformation(Point point)
        {
            int i;
            for (i = 0; i < fieldInformationList.Count; i++ )
            {
                if (fieldInformationList[i].Position.Contains(point) == true)
                {
                    break;
                }
            }

            if (i >= fieldInformationList.Count)
            {
                return -1;
            }
            else
            {
                return i;
            }
        }

        private bool IntersectsWithFieldInformation(RectangleF rect, int ignoreIndex = -1)
        {
            bool bIntersects = false;

            int i;
            for (i = 0; i < fieldInformationList.Count; i++ )
            {
                if (i == ignoreIndex)
                    continue;

                bIntersects = fieldInformationList[i].Position.IntersectsWith(rect);
                if (bIntersects == true)
                {
                    break;
                }

            }

            return bIntersects;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Excel 2013 (.xlsx)|*.xlsx|Excel 2003 (.xls)|*.xls";
            saveFileDialog.DefaultExt = "xlsx";

            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                DataSet ds = ConvertFieldInformationListToDataSet();

                Excel.Write(ds, saveFileDialog.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "test.xlsx");

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                DataSet ds = Excel.Read(openFileDialog1.FileName);
            }
        }

        private DataSet ConvertFieldInformationListToDataSet()
        {
            DataSet ds = new DataSet();

            DataTable table = new DataTable("fieldinfo");

            table.Columns.Add("No#", typeof(int));
            table.Columns.Add("Field Name", typeof(string));
            table.Columns.Add("Left", typeof(float));
            table.Columns.Add("Top", typeof(float));
            table.Columns.Add("Right", typeof(float));
            table.Columns.Add("Bottom", typeof(float));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("Alignment", typeof(string));
            table.Columns.Add("Field Type", typeof(string));
            table.Columns.Add("Font Name", typeof(string));
            table.Columns.Add("Font Size", typeof(int));
            table.Columns.Add("Color", typeof(string));
            table.Columns.Add("Red", typeof(double));
            table.Columns.Add("Green", typeof(double));
            table.Columns.Add("Blue", typeof(double));
            table.Columns.Add("Cyan", typeof(double));
            table.Columns.Add("Magenta", typeof(double));
            table.Columns.Add("Yellow", typeof(double));
            table.Columns.Add("Black", typeof(double));
            table.Columns.Add("Gray", typeof(double));
            table.Columns.Add("Bold", typeof(string));
            table.Columns.Add("Italic", typeof(string));
            table.Columns.Add("Underline", typeof(string));
            table.Columns.Add("Auto Fit", typeof(string));



            int index = 1;
            foreach (FieldInformation fieldInfo in fieldInformationList)
            {
                string fontName = string.Empty;
                double fontSize = 0.0;
                double red = 0.0;
                double green = 0.0;
                double blue = 0.0;
                double cyan = 0.0;
                double magenta = 0.0;
                double yellow = 0.0;
                double black = 0.0;
                double gray = 0.0;
                string bold = "No";
                string italic = "No";
                string underline = "No";
                string autofit = "No";

                if (fieldInfo.FieldType == EFieldType.eFieldText)
                {
                    FieldText fieldText = fieldInfo as FieldText;

                    fontName = fieldText.FontName;
                    fontSize = fieldText.FontSize;

                    if (fieldText.Color.ColorNamespace == EColorNamespace.eRGB)
                    {
                        red = fieldText.Color.Red;
                        green = fieldText.Color.Green;
                        blue = fieldText.Color.Blue;
                    }
                    else if (fieldText.Color.ColorNamespace == EColorNamespace.eCMYK)
                    {
                        cyan = fieldText.Color.Cyan;
                        magenta = fieldText.Color.Magenta;
                        yellow = fieldText.Color.Yellow;
                        black = fieldText.Color.Black;
                    }
                    else if (fieldText.Color.ColorNamespace == EColorNamespace.eGray)
                    {
                        gray = fieldText.Color.Gray;
                    }

                    bold = Converter.GetYesNo(fieldText.Bold);
                    italic = Converter.GetYesNo(fieldText.Italic);
                    underline = Converter.GetYesNo(fieldText.Underline);
                }
                else if (fieldInfo.FieldType == EFieldType.eFieldImage)
                {
                    FieldImage fieldImage = fieldInfo as FieldImage;

                    autofit = Converter.GetYesNo(fieldImage.AutoFit);
                }

                red = 0.25;

                table.Rows.Add(index,
                    fieldInfo.FieldName,
                    fieldInfo.Position.Left,
                    fieldInfo.Position.Top,
                    fieldInfo.Position.Right,
                    fieldInfo.Position.Bottom,
                    "ps",
                    Converter.EAlignmentToString(fieldInfo.Alignment),
                    Converter.EFieldTypeToString(fieldInfo.FieldType),
                    fontName,
                    fontSize,
                    "RGB",
                    red,
                    green,
                    blue,
                    cyan,
                    magenta,
                    yellow,
                    black,
                    gray,
                    bold,
                    italic,
                    underline,
                    autofit);

                index++;
            }

            ds.Tables.Add(table);

            return ds;
        }

        private void ConvertDataSetToFieldInformationList(DataSet ds)
        {
            BindingList<FieldInformation> newFieldInformationList = new BindingList<FieldInformation>();

            foreach (DataTable table in ds.Tables)
            {
                string colName = string.Empty;

                int no = -1;
                string fieldName = string.Empty;
                float left = -1;
                float top = -1;
                float right = -1;
                float bottom = -1;
                EAlignment eAlignment = EAlignment.eLeft;
                EFieldType eFieldType = EFieldType.eFieldText;

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    for (int col = 0; col < table.Rows.Count; col++)
                    {
                        switch (table.Columns[col].ColumnName)
                        {
                            case "No#":
                                no = int.Parse(table.Rows[row][col].ToString());
                                break;
                            case "Field Name":
                                fieldName = table.Rows[row][col].ToString();
                                break;
                            case "Left":
                                left = float.Parse(table.Rows[row][col].ToString());
                                break;
                            case "Top":
                                top = float.Parse(table.Rows[row][col].ToString());
                                break;
                            case "Right":
                                right = float.Parse(table.Rows[row][col].ToString());
                                break;
                            case "Bottom":
                                bottom = float.Parse(table.Rows[row][col].ToString());
                                break;
                            case "Alignment":
                                eAlignment = Converter.StringToEAlignment(table.Rows[row][col].ToString());
                                break;
                            case "Field Type":
                                eFieldType = Converter.StringToEFieldType(table.Rows[row][col].ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    if (eFieldType == EFieldType.eFieldText)
                    {
                        FieldText fieldText = new FieldText(fieldName);

                        fieldText.Position = RectangleF.FromLTRB(left, top, right, bottom);
                        fieldText.Alignment = eAlignment;
                    }
                    else if (eFieldType == EFieldType.eFieldImage)
                    {
                        FieldImage fieldImage = new FieldImage(fieldName);

                        fieldImage.Position = RectangleF.FromLTRB(left, top, right, bottom);
                        fieldImage.Alignment = eAlignment;
                    }
                }
            }
        }

        private void MainView_Resize(object sender, EventArgs e)
        {
            //customPanel1.Location

            Size clientSize = panel1.ClientSize;
            clientSize.Width -= 2; // remove 1 pixel on each side
            clientSize.Height -= 2; // remove 1 pixel on each side


            int width = (int)((double)clientSize.Height * paperSize.Width / paperSize.Height + 0.5);
            int height = panel1.ClientSize.Height - 2;

            customPanel1.Left = clientSize.Width / 2 - width / 2;
            customPanel1.Top = 0;
            customPanel1.ClientSize = new Size(width, height);
        }
    }
}
