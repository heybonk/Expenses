using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Layouts;



namespace Expenses
{
    public class DTotal : Table
    {
        [TableAttribute(ColumnType.PrimaryKey, Category.INTEGER, false)]
        public int Year { get; set; }

        [TableAttribute(ColumnType.PrimaryKey, Category.INTEGER, false)]
        public int Month { get; set; }

        [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
        public string? MainTagCD { get; set; }

        [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
        public int Amount { get; set; }

        public DTotal() : base()
        {
        }
        public string? MainTagName { get; set; }
        public string? TagCategoryCD { get; set; }
        public string? TagCategoryName { get; set; }
        public int IsIncome { get; set; }
        public bool IsShowHeader { get; set; }

        public int Amount_Category { get; set; }

        internal static void UpdatebyMonth()
        {
            var q = @"
INSERT INTO DTotal (Year, Month,MainTagCD, Amount)
SELECT 
    substr(RecordingDate, 1, 4) AS Year,
    substr(RecordingDate, 6, 2) AS Month,
    MainTagCD AS MainTagCD,
    SUM(Amount) AS TotalAmount
FROM DMain
GROUP BY Year, Month, MainTagCD
ON CONFLICT(Year, Month,MainTagCD) DO UPDATE SET
    Amount = excluded.Amount;
;
";
            Execute(q);
        }
    }
}
