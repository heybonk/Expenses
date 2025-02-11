using System;

namespace Expenses;

public class MainTag:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
    public string MainTagCD { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string MainTagName { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string TagCategoryCD { get; set; }
    
    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int DisplayOrder { get; set; }
    public MainTag() : base()
    {
    }

}
