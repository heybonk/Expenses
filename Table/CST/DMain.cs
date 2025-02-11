using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;



namespace Expenses
{
    public class  DMain:Table
    {
        [TableAttribute(ColumnType.PrimaryKey,Category.TEXT,false)]
        public string TourokuNO { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.DATE, true)]
        public DateTime RecordingDate { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
        public int Amount { get; set; }

        [TableAttribute(ColumnType.JustColumn,Category.TEXT,true)]
        public string? MainTagCD { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
        public string UpdateDatetime { get; set; }
        public DMain() : base()
        {
        }
    }
}
