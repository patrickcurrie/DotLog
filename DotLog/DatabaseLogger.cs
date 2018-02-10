using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DotLog
{
    class DatabaseLogger : DotLogger
    {
        public string ConnectionString { get; private set; }
        public string TableName { get; private set; }
        public List<string> ColumnList { get; private set; }

        public DatabaseLogger(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            // Get columns from a database and store them in a list of strings.
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string[] restrictions = new string[4] { null, null, "<" + tableName + ">", null };
                con.Open();
                ColumnList = con.GetSchema("Columns", restrictions).AsEnumerable().Select(s => s.Field<string>("Column_Name")).ToList();
            }
        }

        public override void Log(Dictionary<string, string> propertyDictionary)
        {
            lock (threadLock)
            {
                // Check if dictionary key matches any of the columns in the table.
                foreach (string column in ColumnList)
                {
                    if (propertyDictionary.ContainsKey(column) == false)
                    {
                        throw new ArgumentException("Dictionary specifies column that is not in table");
                    }
                }

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    try
                    {
                        using (SqlCommand com = new SqlCommand(SqlCommandString(propertyDictionary), con))
                        {
                            foreach (var pair in propertyDictionary)
                            {
                                com.Parameters.AddWithValue("@" + pair.Value, pair.Value);
                            }

                            com.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private string SqlCommandString(Dictionary<string, string> propertyDictionary)
        {
            StringBuilder com = new StringBuilder();
            com.Append("INSERT INTO" + TableName + " (");
            int k = 0;
            foreach (var pair in propertyDictionary)
            {
                if (k == propertyDictionary.Count - 1)
                    com.Append("@" + pair.Key + ") ");
                else
                    com.Append("@" + pair.Key + ", ");
                k++;
            }

            com.Append("Values (");
            k = 0;
            foreach(var pair in propertyDictionary)
            {
                if (k == propertyDictionary.Count - 1)
                    com.Append("@" + pair.Value + ")");
                else
                    com.Append("@" + pair.Value + ", ");
                k++;
            }

            return com.ToString();
        }
    }
}
