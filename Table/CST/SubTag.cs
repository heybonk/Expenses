using System;

namespace Expenses;

internal class SubTag:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
    public string SubTagCD { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string SubTagName { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int DisplayOrder { get; set; }

    public SubTag() : base()
    {
    }

}
