using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Extensions;
using SecureNotesWpfClient.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Services
{
    

    public class DatabaseDefinition
    {
        protected void BuildTable(SQLiteConnection conn, string tableName, string sql)
        {
            if (!conn.TableExists(tableName))
            {
                var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        
        protected void BuildDatabase(SQLiteConnection conn)
        {
            BuildTable(conn, "Config",
                @"CREATE TABLE Config (
                    name        VARCHAR(128) NULL,
                    value       VARCHAR(1028) NULL,
                    class       VARCHAR(128) NULL        
                 )"
            );

            BuildTable(conn, "ServerAccount",
                @"CREATE TABLE ServerAccount (
                    id          VARCHAR(38) NOT NULL, 
                    name        VARCHAR(80) NULL,
                    host        VARCHAR(128) NULL,
                    user_id     VARCHAR(80) NULL,
                    password    VARCHAR(80) NULL
                 )"
            );

            BuildTable(conn, "Notebook",
                @"CREATE TABLE Notebook (
                    id          VARCHAR(38) NOT NULL, 
                    name        VARCHAR(80) NULL,
                    server_id   VARCHAR(38) NULL,
                    data        BLOB NULL
                 )"
            );

            BuildTable(conn, "Note",
                @"CREATE TABLE Note (
                    id          VARCHAR(38) NOT NULL, 
                    notebook_id VARCHAR(38) NOT NULL,
                    current_version_num INT NOT NULL,
                    sync_status  INT NOT NULL,
                    merged       BIT NOT NULL,
                    deleted      BIT NOT NULL,
                    created_utc  DATETIME NULL,
                    deleted_utc  DATETIME NULL
                 )"
            );

            BuildTable(conn, "NoteVersion",
                @"CREATE TABLE NoteVersion (
                    note_id     VARCHAR(38) NOT NULL, 
                    version_num INT NOT NULL,
                    created_utc DATETIME NULL,
                    data        BLOB NULL
                 )"
            );

        }

        public static void CreateDatabase()
        {
            var dbFilename = AppConfig.DbFilename;
            var dbDir = Path.GetDirectoryName(dbFilename);
            Directory.CreateDirectory(dbDir);
            if (!System.IO.File.Exists(dbFilename))
                SQLiteConnection.CreateFile(dbFilename);
            SQLiteConnection conn = new SQLiteConnection(AppConfig.ConnectionString);
            conn.Open();
            try
            {
                var dbDef = new DatabaseDefinition();
                dbDef.BuildDatabase(conn);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void SeedDatabase()
        {
            var conn = new SQLiteConnection(AppConfig.ConnectionString);
            conn.Open();
            try
            {
                conn.SafeInsertIntoTable(t =>
                {
                    t.TableName = "ServerAccount";
                    t.Keys["id"] = "1";
                    t.Fields["name"] = "Account 1";
                    t.Fields["host"] = "http://localhost:58454";
                    t.Fields["user_id"] = "test";
                    t.Fields["password"] = "test";
                });

                conn.SafeInsertIntoTable(t =>
                {
                    t.TableName = "Notebook";
                    t.Keys["Id"] = "1";
                    t.Fields["name"] = "Notebook 1";
                    t.Fields["server_id"] = "1";
                    t.Fields["data"] = "";
                });

                var rand = new Random();
                var trans = conn.BeginTransaction();
                for(var i=1; i < 20; i++)
                {
                    trans.SafeInsertIntoTable(t =>
                    {
                        t.TableName = "Note";
                        t.Keys["Id"] = i.ToString();
                        t.Fields["notebook_id"] = "1";
                        t.Fields["current_version_num"] = 0;
                        t.Fields["sync_status"] = NoteSyncStatus.ReadyToSync;
                        t.Fields["merged"] = false;
                        t.Fields["deleted"] = false;
                        t.Fields["created_utc"] = DateTime.UtcNow;
                        t.Fields["deleted_utc"] = null;
                    });

                    var data1 = new NoteData()
                    {
                        Title = "Note " + rand.Next(10, 99).ToString() + " - " + i.ToString(),
                        Body = "Note Body " + i.ToString()
                    };

                    trans.SafeInsertIntoTable(t =>
                    {
                        t.TableName = "NoteVersion";
                        t.Keys["note_id"] = i.ToString();
                        t.Fields["version_num"] = 0;
                        t.Fields["created_utc"] = DateTime.UtcNow;
                        t.Fields["data"] = data1.AsString;

                    });
                }
                trans.Commit();
            }
            finally
            {
                conn.Close();
            }

        }





       
    }
}
