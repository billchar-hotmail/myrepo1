using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Helpers
{
    public class SqlTableUpdateBuilder
    {
        public string TableName { get; set; }
        public Dictionary<string, object> Keys { get; set; }
        public Dictionary<string, object> Fields { get; set; }

        public SqlTableUpdateBuilder()
        {
            Keys = new Dictionary<string, object>();
            Fields = new Dictionary<string, object>();
        }

        public SQLiteCommand BuildInsert(SQLiteConnection conn, SQLiteTransaction trans)
        {
            if (String.IsNullOrEmpty(TableName))
                throw new Exception("Unable to build SQL INSERT: TableName is not defined.");

            if ((Keys.Count + Fields.Count) == 0)
                throw new Exception("Unable to build SQL INSERT: No Fields or Keys have been defined.");

            var sb = new StringBuilder();
            sb.Append($"INSERT INTO {TableName} (");
            var fieldNames = new List<string>();
            for (var index = 0; index < Keys.Count; index++)
                fieldNames.Add(Keys.Keys.ElementAt(index));
            for (var index = 0; index < Fields.Count; index++)
                fieldNames.Add(Fields.Keys.ElementAt(index));
            sb.Append(String.Join(",", fieldNames));

            sb.Append(") VALUES (");
            var paramNames = new List<string>();
            for (var index = 0; index < Keys.Count; index++)
                paramNames.Add("@K" + index.ToString());
            for (var index = 0; index < Fields.Count; index++)
                paramNames.Add("@F" + index.ToString());
            sb.Append(String.Join(",", paramNames));
            sb.Append(");");

            var cmd = new SQLiteCommand(sb.ToString(), conn, trans);
            cmd.CommandType = System.Data.CommandType.Text;
            for (var index = 0; index < Keys.Count; index++)
                cmd.Parameters.AddWithValue("@K" + index.ToString(), Keys.Values.ElementAt(index));
            for (var index = 0; index < Fields.Count; index++)
                cmd.Parameters.AddWithValue("@F" + index.ToString(), Fields.Values.ElementAt(index));
            return cmd;
        }

        public SQLiteCommand BuildUpdate(SQLiteConnection conn, SQLiteTransaction trans)
        {
            if (String.IsNullOrEmpty(TableName))
                throw new Exception("Unable to build SQL UPDATE: TableName is not defined.");

            if (Keys.Count == 0)
                throw new Exception("Unable to build SQL UPDATE: No Keys have been defined.");

            if (Fields.Count == 0)
                throw new Exception("Unable to build SQL UPDATE: No Fields have been defined.");

            var sb = new StringBuilder();
            sb.Append($"UPDATE {TableName} SET ");
            var setList = new List<string>();
            for (var index = 0; index < Fields.Count; index++)
                setList.Add(Fields.Keys.ElementAt(index) + "=@F" + index.ToString());
            sb.Append(String.Join(",", setList));

            sb.Append(" WHERE ");
            var whereList = new List<string>();
            for (var index = 0; index < Keys.Count; index++)
                whereList.Add(Keys.Keys.ElementAt(index) + "=@K" + index.ToString());
            sb.Append(String.Join(" AND ", whereList));
            sb.Append(";");

            var cmd = new SQLiteCommand(sb.ToString(), conn, trans);
            cmd.CommandType = System.Data.CommandType.Text;
            for (var index = 0; index < Keys.Count; index++)
                cmd.Parameters.AddWithValue("@K" + index.ToString(), Keys.Values.ElementAt(index));
            for (var index = 0; index < Fields.Count; index++)
                cmd.Parameters.AddWithValue("@F" + index.ToString(), Fields.Values.ElementAt(index));
            return cmd;
        }

        public SQLiteCommand BuildCount(SQLiteConnection conn, SQLiteTransaction trans)
        {
            if (String.IsNullOrEmpty(TableName))
                throw new Exception("Unable to build SQL COUNT: TableName is not defined.");

            if (Keys.Count == 0)
                throw new Exception("Unable to build SQL COUNT: No Keys have been defined.");

            var sb = new StringBuilder();
            sb.Append($"SELECT COUNT(*) FROM {TableName} WHERE ");
            var whereList = new List<string>();
            for (var index = 0; index < Keys.Count; index++)
                whereList.Add(Keys.Keys.ElementAt(index) + "=@K" + index.ToString());
            sb.Append(String.Join(" AND ", whereList));
            sb.Append(";");

            var cmd = new SQLiteCommand(sb.ToString(), conn, trans);
            cmd.CommandType = System.Data.CommandType.Text;
            for (var index = 0; index < Keys.Count; index++)
                cmd.Parameters.AddWithValue("@K" + index.ToString(), Keys.Values.ElementAt(index));
            return cmd;
        }

    }
}
