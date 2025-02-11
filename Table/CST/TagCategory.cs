using System;

namespace Expenses;

public class TagCategory:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
    public string TagCategoryCD { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string TagCategoryName { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int DisplayOrder { get; set; }

    public TagCategory() : base()
    {
    }

}
