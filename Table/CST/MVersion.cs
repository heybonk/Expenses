using System;

namespace Expenses;

public class MVersion:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.INTEGER, false)]
    public int CD { get; set; }
    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int AppVersion { get; set; }
    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int TableVersion { get; set; }

    public MVersion() : base()
    {
    }

}
