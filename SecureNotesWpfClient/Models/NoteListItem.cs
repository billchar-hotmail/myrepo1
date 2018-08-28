using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Models
{
    public class NoteListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _id;
        private NoteSyncStatus _syncStatus;
        private bool _merged;
        private bool _deleted;
        private int _currentVersionNum;
        private NoteData _data;
        private DateTime _createdUTC;
        private DateTime _modifiedUTC;
        private DateTime? _deletedUTC;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public NoteSyncStatus SyncStatus
        {
            get { return _syncStatus; }
            set
            {
                _syncStatus = value;
                NotifyPropertyChanged();
            }
        }

        public bool Merged
        {
            get { return _merged; }
            set
            {
                _merged = value;
                NotifyPropertyChanged();
            }
        }

        public bool Deleted
        {
            get { return _deleted; }
            set
            {
                _deleted = value;
                NotifyPropertyChanged();
            }
        }

        public int CurrentVersionNum
        {
            get { return _currentVersionNum; }
            set
            {
                _currentVersionNum = value;
                NotifyPropertyChanged();
            }
        }

        public NoteData Data
        {
            get { return _data; }
            private set { _data = value; }
         }

        public DateTime CreatedUTC
        {
            get { return _createdUTC; }
            set
            {
                _createdUTC = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime ModifiedUTC
        {
            get { return _modifiedUTC; }
            set
            {
                _modifiedUTC = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime? DeletedUTC
        {
            get { return _deletedUTC; }
            set
            {
                _deletedUTC = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime CreatedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(_createdUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set
            {
                _createdUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId);
                NotifyPropertyChanged();
            }
        }

        public DateTime ModifiedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(ModifiedUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set
            {
                _modifiedUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId);
                NotifyPropertyChanged();
            }
        }

        public DateTime? DeletedDT
        {
            get {
                if (_deletedUTC == null)
                    return null;
                else
                    return TimeZoneHelper.ConvertFromUTC(DeletedUTC??DateTime.MinValue, TimeZoneHelper.DefaultTimeZoneId);
            }
            set
            {
                if (value == null)
                    _deletedUTC = null;
                else
                    _deletedUTC = TimeZoneHelper.ConvertToUTC(value??DateTime.MinValue, TimeZoneHelper.DefaultTimeZoneId);
                NotifyPropertyChanged();
            }
        }

        public string ItemState
        {
            get { return GetItemState(); }
        }

        public NoteListItem()
        {
            _data = new NoteData();
            _data.PropertyChanged += ((sender, eventArgs) => NotifyPropertyChanged("Data"));
        }

        private string GetItemState()
        {
            if ((SyncStatus == NoteSyncStatus.Synched) && Deleted)
                return "DEL";
            else if ((SyncStatus == NoteSyncStatus.Synched) && !Deleted)
                return "SYC";
            else if ((SyncStatus == NoteSyncStatus.ReadyToSync) && Deleted)
                return "PD";
            else if (SyncStatus == NoteSyncStatus.Pending)
                return "PM";
            else if ((SyncStatus == NoteSyncStatus.ReadyToSync) && Merged)
                return "MER";
            else
                return "MOD";
        }

     }
}
