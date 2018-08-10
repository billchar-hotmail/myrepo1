using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Models
{
    public class Note
    {
        public Note()
        {
            Data = new NoteData();
        }

        public Note(Note note)
        {
            Id = note.Id;
            SyncStatus = note.SyncStatus;
            MergeStatus = note.MergeStatus;
            CurrentVersionNum = note.CurrentVersionNum;
            CreatedUTC = note.CreatedUTC;
            ModifiedUTC = note.ModifiedUTC;
            Data = new NoteData()
            {
                Title = note.Data.Title,
                Body = note.Data.Body
            };
        }

        public string Id { get; set; }
        public NoteSyncStatus SyncStatus { get; set; }
        public NoteMergeStatus MergeStatus { get; set; }
        public int CurrentVersionNum { get; set; }
        public NoteData Data { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime  ModifiedUTC { get; set; }
        public DateTime CreatedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(CreatedUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set { CreatedUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId);  }
        }
        public DateTime ModifiedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(ModifiedUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set { ModifiedUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId); }
        }

    }
}
