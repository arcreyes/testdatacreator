﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace TestDataGenerator
{
    public static class Excel
    {
        public enum EVersion
        {
            eXLS = 0,
            eXLSX
        };

        public static int Write(DataSet dataSet, string filePath, EVersion eVersion)
        {
            string connectionString = GetConnectionString(filePath, eVersion);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
            
                foreach (DataTable table in dataSet.Tables)
                {
                    string colIdCollection = string.Empty;
                    string colValueCollection = string.Empty;
                    char[] trimChar = {'$'};
                    string tableName = table.TableName.Trim(trimChar);

                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        colIdCollection += table.Columns[col].ColumnName;
                        colValueCollection += table.Columns[col].ColumnName + " VARCHAR";

                        if (col + 1 < table.Columns.Count)
                        { 
                            colIdCollection += ",";
                            colValueCollection += ",";
                        }
                    }

                    //cmd.CommandText = "CREATE TABLE [table1] (id INT, name VARCHAR, datecol DATE );";
                    cmd.CommandText = "CREATE TABLE [" + tableName + "] (" + colValueCollection + ");";
                    cmd.ExecuteNonQuery();



                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        string rowValueCollection = string.Empty;


                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            rowValueCollection += "'" + table.Rows[row][col].ToString() + "'";

                            if (col + 1 < table.Columns.Count)
                            {
                                rowValueCollection += ",";
                            }

                        }

                        //cmd.CommandText = "INSERT INTO [table1](id,name,datecol) VALUES(1,'AAAA','2014-01-01');";
                        cmd.CommandText = "INSERT INTO [" + tableName + "](" + colIdCollection + ") VALUES(" + rowValueCollection + ");";
                        cmd.ExecuteNonQuery();

                    }
                }


                conn.Close();
            }


            return 0;
        }

        public static DataSet Read(string filename)
        {
            DataSet ds = new DataSet();

            string connectionString = GetConnectionString(filename, GetVersion(filename));

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    DataTable dt = new DataTable();
                    dt.TableName = sheetName;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                }

                cmd = null;
                conn.Close();
            }

            return ds;
        }

        private static string GetConnectionString(string filePath, EVersion eVersion)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            if (eVersion == EVersion.eXLS)
            {
                // XLS - Excel 2003 and Older
                props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                props["Extended Properties"] = "Excel 8.0";
                props["Data Source"] = filePath;
            }
            else
            {
                // XLSX - Excel 2007, 2010, 2012, 2013
                props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                props["Extended Properties"] = "Excel 12.0 XML";
                props["Data Source"] = filePath;
            }

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }

        private static EVersion GetVersion(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext.ToLower() == ".xlsx")
            {
                return EVersion.eXLSX;
            }
            else if (ext.ToLower() == ".xls")
            {
                return EVersion.eXLS;
            }
            else
            {
                throw new System.ArgumentException("invalid file extension", "filename");
            }

        }
    }
}
