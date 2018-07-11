using SecureNotesWpfClient.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Extensions
{
    public static class SQLiteConnectionExtensions
    {
        public static bool TableExists(this SQLiteConnection db, string tableName)
        {
            string sql1 = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            var cmd1 = new SQLiteCommand(sql1, db);
            bool result = (cmd1.ExecuteScalar() != null);
            return result;
        }

        public static int ExecuteNonQuery(this SQLiteConnection conn, string sql)
        {
            var cmd = new SQLiteCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        private static object InternalExecuteScalar(SQLiteConnection conn, string sql, object defaultValue)
        {
            object result;
            var cmd = new SQLiteCommand(sql, conn);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
                result = reader[0];
            else
                result = defaultValue;
            return result;
        }

        public static string ExecuteScalar(this SQLiteConnection conn, string sql, string defaultValue)
        {
            return (string)InternalExecuteScalar(conn, sql, defaultValue);
        }

        public static int ExecuteScalar(this SQLiteConnection conn, string sql, int defaultValue)
        {
            return (int)InternalExecuteScalar(conn, sql, defaultValue);
        }

        public static bool ExecuteScalar(this SQLiteConnection conn, string sql, bool defaultValue)
        {
            return (bool)InternalExecuteScalar(conn, sql, defaultValue);
        }

        public static DateTime ExecuteScalar(this SQLiteConnection conn, string sql, DateTime defaultValue)
        {
            return (DateTime)InternalExecuteScalar(conn, sql, defaultValue);
        }

        //private static SQLiteConnection CreateConnection(string filename)
        //{
        //    if (!System.IO.File.Exists(filename))
        //        SQLiteConnection.CreateFile(filename);
        //    SQLiteConnection db = new SQLiteConnection("Data Source=" + filename);
        //    db.Open();
        //    return db;
        //}


        private static int InternalInsertIntoTable(SQLiteConnection conn, SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            var builder = new SqlTableUpdateBuilder();
            code.Invoke(builder);
            var cmd = builder.BuildInsert(conn, trans);
            return cmd.ExecuteNonQuery();
        }

        public static int InsertIntoTable(this SQLiteConnection conn, Action<SqlTableUpdateBuilder> code)
        {
            return InternalInsertIntoTable(conn, null, code);
        }

        public static int InsertIntoTable(this SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            return InternalInsertIntoTable(trans.Connection, trans, code);
        }

        private static int InternalUpdateTable(SQLiteConnection conn, SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            var builder = new SqlTableUpdateBuilder();
            code.Invoke(builder);
            var cmd = builder.BuildUpdate(conn, trans);
            return cmd.ExecuteNonQuery();
        }

        public static int UpdateTable(this SQLiteConnection conn, Action<SqlTableUpdateBuilder> code)
        {
            return InternalUpdateTable(conn, null, code);
        }

        public static int UpdateTable(this SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            return InternalUpdateTable(trans.Connection, trans, code);
        }

        private static int InternalSaveToTable(SQLiteConnection conn, SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            var builder = new SqlTableUpdateBuilder();
            code.Invoke(builder);

            var countCmd = builder.BuildCount(conn, trans);
            var col1 = countCmd.ExecuteScalar();
            if (col1 == null)
                throw new Exception($"Unable to select from table [{builder.TableName}].");
            var count = Convert.ToInt32(col1);
            if (count == 0)
            {
                var insertCmd = builder.BuildInsert(conn, trans);
                return insertCmd.ExecuteNonQuery();
            }
            else if (count == 1)
            {
                var updateCmd = builder.BuildUpdate(conn, trans);
                return updateCmd.ExecuteNonQuery();
            }
            else
                throw new Exception($"Unable to save to table {builder.TableName}: Duplicate Keys Found.");
        }

        public static int SaveToTable(this SQLiteConnection conn, Action<SqlTableUpdateBuilder> code)
        {
            return InternalSaveToTable(conn, null, code);
        }

        public static int SaveToTable(this SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            return InternalSaveToTable(trans.Connection, trans, code);
        }

        private static int InternalSafeInsertIntoTable(SQLiteConnection conn, SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            int result = 0;

            var builder = new SqlTableUpdateBuilder();
            code.Invoke(builder);

            var countCmd = builder.BuildCount(conn, trans);
            var col1 = countCmd.ExecuteScalar();
            if (col1 == null)
                throw new Exception($"Unable to select from table [{builder.TableName}].");
            var count = Convert.ToInt32(col1);
            if (count == 0)
            {
                var insertCmd = builder.BuildInsert(conn, trans);
                result = insertCmd.ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// Inserts the record into the table only if that record does not already exist in the table.
        /// </summary>
        public static int SafeInsertIntoTable(this SQLiteConnection conn, Action<SqlTableUpdateBuilder> code)
        {
            return InternalSafeInsertIntoTable(conn, null, code);
        }

        /// <summary>
        /// Inserts the record into the table only if that record does not already exist in the table.
        /// </summary>
        public static int SafeInsertIntoTable(this SQLiteTransaction trans, Action<SqlTableUpdateBuilder> code)
        {
            return InternalSafeInsertIntoTable(trans.Connection, trans, code);
        }




        public static string GetConfigValue(this SQLiteConnection conn, string name, string defaultValue = null)
        {
            var sql = "SELECT value FROM CONFIG WHERE name = @name";
            var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", name.ToUpper());
            var col1 = cmd.ExecuteScalar();
            if (col1 == null)
                return defaultValue;
            else
                return col1.ToString();
        }

        public static void SetConfigValue(this SQLiteConnection conn, string name, string value)
        {
            conn.SaveToTable(t =>
            {
                t.TableName = "CONFIG";
                t.Keys["name"] = name.ToUpper();
                t.Fields["value"] = value;
            });
        }


        //    if (DoesTableExist("test", db) == false)
        //    {
        //        string sql1 = "CREATE TABLE test(name VARCHAR(40), value VARCHAR(1028))";
        //        var cmd = new SQLiteCommand(sql1, db);
        //        cmd.ExecuteNonQuery();
        //    }
        //    db.Close();
        //}

        //private void PopulateDatabase()
        //{
        //    SQLiteConnection db = new SQLiteConnection("Data Source=" + dbName);
        //    db.Open();
        //    var cmd1 = new SQLiteCommand("insert into test(name, value) values('test1', 'This is a test.')", db);
        //    cmd1.ExecuteNonQuery();
        //    var cmd2 = new SQLiteCommand("insert into test(name, value) values('test2', 'This is another test.')", db);
        //    cmd2.ExecuteNonQuery();
        //    db.Close();
        //}

        //private void QueryDatabase()
        //{
        //    SQLiteConnection db = new SQLiteConnection("Data Source=" + dbName);
        //    db.Open();
        //    var cmd1 = new SQLiteCommand("select * from test", db);
        //    var reader = cmd1.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        textBox.Text += "name=" + reader["name"] + " value=" + reader["value"] + "\n";
        //    }
        //    db.Close();

        //}

    }
}
