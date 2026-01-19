using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Expenses
{
    public class DMain : Table
    {
        [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
        public string TourokuNO { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.DATE, true)]
        public DateTime RecordingDate
        {
            get
            {
                return this._recordingDate;
            }
            set
            {
                this._recordingDate = value;
                this.OnPropertyChanged(nameof(RecordingDate));
            }
        }
        private DateTime _recordingDate;

        [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
        public int Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
                this.OnPropertyChanged(nameof(Amount));
            }
        }
        private int _amount;

        [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
        public string Bikou
        {
            get
            {
                return this._bikou;
            }
            set
            {
                this._bikou = value;
                this.OnPropertyChanged(nameof(Bikou));
            }
        }
        private string _bikou;

        [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
        public string? MainTagCD { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
        public string UpdateDatetime { get; set; }

        public string? MainTagName { get; set; }
        private bool _isShowHeader;
        public bool IsShowHeader
        {
            get
            {
                return this._isShowHeader;
            }
            set
            {
                this._isShowHeader = value;
                this.OnPropertyChanged(nameof(IsShowHeader));
            }
        }


        public DMain() : base()
        {
        }
        internal IEnumerable<DMain> SelectByMonth(DateTime datetime)
        {
            var start = new DateTime(datetime.Year, datetime.Month, 1);
            var end = start.AddMonths(1);
            var query =
            $"SELECT * FROM DMain WHERE RecordingDate >= '{start}' AND RecordingDate < '{end}' ORDER BY RecordingDate";
            var l = this.Select(query);
            if (l == null || l.Count() == 0) return null;
            else return l.OfType<DMain>();
        }
        internal IEnumerable<DMain> SelectForTotal()
        {
            var sb = new StringBuilder();
            sb.Append("SELECT strftime('%Y', RecordingDate) AS Year");
            sb.Append("      ,strftime('%m', RecordingDate) AS Month");
            sb.Append("      ,SUM(Amount) AS Amount");
            sb.Append("      ,MainTagCD   AS Amount");
            sb.Append("  FROM DMain");
            sb.Append("GROUP BY strftime('%Y', RecordingDate), strftime('%m', RecordingDate),MainTagCD");
            var query = sb.ToString();
            var l = this.Select(query);
            if (l == null || l.Count() == 0) return null;
            else return l.OfType<DMain>();
        }
    }
}
