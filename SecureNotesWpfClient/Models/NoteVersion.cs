using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Models
{
    public class NoteVersion
    {
        public NoteVersion()
        {
            Data = new NoteData();
        }

        public string NoteId { get; set; }
        public int VersionNum { get; set; }
        public DateTime CreatedUTC { get; set; }
        public NoteData Data { get; set; }

        public DateTime CreatedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(CreatedUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set { CreatedUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId); }
        }
    }
}
