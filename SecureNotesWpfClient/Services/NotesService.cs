using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Services
{
    public class NotesService
    {


        protected List<Note> GetSynchedNotes(string notebookId, SQLiteConnection conn)
        {
            var list = new List<Note>();
            var sql = "SELECT n.id, n.sync_status, n.merge_status, n.current_version_num, n.created_utc, v.created_utc, v.data " +
                      "FROM Note n " +
                      "LEFT OUTER JOIN NoteVersion v ON ((n.id = v.note_id) AND (n.current_version_num = v.version_num)) " +
                      "WHERE n.notebook_id = @notebook_id " +
                      "AND n.sync_status == @sync_status";
            var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@notebook_id", notebookId);
            cmd.Parameters.AddWithValue("@sync_status", NoteSyncStatus.Synched);
            var reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var note = new Note();
                    note.Id = reader.GetString(0);
                    note.SyncStatus = (NoteSyncStatus)reader.GetInt32(1);
                    note.MergeStatus = (NoteMergeStatus)reader.GetInt32(2);
                    note.CurrentVersionNum = reader.GetInt32(3);
                    note.CreatedUTC = reader.GetDateTime(4);
                    note.ModifiedUTC = reader.GetDateTime(5);
                    note.Data.AsString = reader.GetString(6);
                    list.Add(note);
                }
            }
            finally
            {
                reader.Close();
            }
            return list;
        }

        protected List<Note> GetModifiedNotes(string notebookId, SQLiteConnection conn)
        {
            var list = new List<Note>();
            var sql = "SELECT n.id, n.sync_status, n.merge_status, n.current_version_num, n.created_utc, v.created_utc, v.data " +
                      "FROM Note n " +
                      "LEFT OUTER JOIN NoteVersion v ON ((n.id = v.note_id) AND (v.version_num = 0)) " +
                      "WHERE n.notebook_id = @notebook_id";
            var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@notebook_id", notebookId);
            cmd.Parameters.AddWithValue("@sync_status", NoteSyncStatus.Synched);
            var reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var note = new Note();
                    note.Id = reader.GetString(0);
                    note.SyncStatus = (NoteSyncStatus)reader.GetInt32(1);
                    note.MergeStatus = (NoteMergeStatus)reader.GetInt32(2);
                    note.CurrentVersionNum = reader.GetInt32(3);
                    note.CreatedUTC = reader.GetDateTime(4);
                    note.ModifiedUTC = reader.GetDateTime(5);
                    note.Data.AsString = reader.GetString(6);
                    list.Add(note);
                }
            }
            finally
            {
                reader.Close();
            }
            return list;
        }

        protected void LoadNotebook(string notebookId, Notebook notebook, SQLiteConnection conn)
        {
            var sql = "SELECT id, name, server_id, data " +
                      "FROM Notebook " +
                      "WHERE id = @notebook_id";
            var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@notebook_id", notebookId);
            var reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    notebook.Id = reader.GetString(0);
                    notebook.Name = reader.GetString(1);
                    notebook.ServerId = reader.GetString(2);
                    notebook.Data.AsString = reader.GetString(3);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        public Notebook GetNotebook(string notebookId)
        {
            var notebook = new Notebook();
            var conn = new SQLiteConnection(AppConfig.ConnectionString);
            conn.Open();
            try
            {
                LoadNotebook(notebookId, notebook, conn);
                var notes = new List<Note>();
                notes.AddRange(GetSynchedNotes(notebookId, conn));
                notes.AddRange(GetModifiedNotes(notebookId, conn));
                notebook.Notes.Clear();
                foreach (var note in notes)
                    notebook.Notes.Add(note);
            }
            finally
            {
                conn.Close();
            }
            return notebook;
        }







    }

}
