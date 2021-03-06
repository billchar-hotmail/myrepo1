﻿using SecureNotesWpfClient.Data;
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


        protected List<NoteListItem> GetSynchedNotes(string notebookId, SQLiteConnection conn)
        {
            var list = new List<NoteListItem>();
            var sql = "SELECT n.id, n.sync_status, n.merged, n.deleted, n.current_version_num, n.created_utc, n.deleted_utc, v.created_utc, v.data " +
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
                    var note = new NoteListItem();
                    note.Id = reader.GetString(0);
                    note.SyncStatus = (NoteSyncStatus)reader.GetInt32(1);
                    note.Merged = reader.GetBoolean(2);
                    note.Deleted = reader.GetBoolean(3);
                    note.CurrentVersionNum = reader.GetInt32(4);
                    note.CreatedUTC = reader.GetDateTime(5);
                    note.DeletedUTC = reader.IsDBNull(6) ? null : new Nullable<DateTime>(reader.GetDateTime(6));
                    note.ModifiedUTC = reader.GetDateTime(7);
                    note.Data.AsString = reader.GetString(8);
                    list.Add(note);
                }
            }
            finally
            {
                reader.Close();
            }
            return list;
        }

        protected List<NoteListItem> GetModifiedNotes(string notebookId, SQLiteConnection conn)
        {
            var list = new List<NoteListItem>();
            var sql = "SELECT n.id, n.sync_status, n.merged, n.deleted, n.current_version_num, n.created_utc, n.deleted_utc, v.created_utc, v.data " +
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
                    var note = new NoteListItem();
                    note.Id = reader.GetString(0);
                    note.SyncStatus = (NoteSyncStatus)reader.GetInt32(1);
                    note.Merged = reader.GetBoolean(2);
                    note.Deleted = reader.GetBoolean(3);
                    note.CurrentVersionNum = reader.GetInt32(4);
                    note.CreatedUTC = reader.GetDateTime(5);
                    note.DeletedUTC = reader.IsDBNull(6) ? null : new Nullable<DateTime>(reader.GetDateTime(6));
                    note.ModifiedUTC = reader.GetDateTime(7);
                    note.Data.AsString = reader.GetString(8);
                    list.Add(note);
                }
            }
            finally
            {
                reader.Close();
            }
            return list;
        }

        protected List<NoteVersion> GetNoteVersions(string noteId, SQLiteConnection conn)
        {
            var list = new List<NoteVersion>();
            var sql = "SELECT v.note_id, v.version_num, v.created_utc, v.data " +
                      "FROM NoteVersion v " + 
                      "WHERE v.note_id == @note_id " +
                      "ORDER BY created_utc";
            var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@note_id", noteId);
            var reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var note = new NoteVersion
                    {
                        NoteId = reader.GetString(0),
                        VersionNum = reader.GetInt32(1),
                        CreatedUTC = reader.GetDateTime(2)
                    };
                    note.Data.AsString = reader.GetString(3);
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
                var notes = new List<NoteListItem>();
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

        public List<NoteVersion> GetNoteHistory(string noteId)
        {
            var history = new List<NoteVersion>();
            var conn = new SQLiteConnection(AppConfig.ConnectionString);
            conn.Open();
            try
            {
                history.AddRange(GetNoteVersions(noteId, conn));
            }
            finally
            {
                conn.Close();
            }
            return history;

        }







    }

}
