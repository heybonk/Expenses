using System;

namespace Expenses;

public class TagCategory:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
    public string TagCategoryCD { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string TagCategoryName
    {
        get => this._tagCategoryName;
        set
        {
            this._tagCategoryName = value;
            this.OnPropertyChanged(nameof(TagCategoryName));
        }
    }

    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int DisplayOrder { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int IsIncome { get; set; }
    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public int IsVisible { get; set; }
    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string Color { get; set; }
    


    private string _tagCategoryName;

    public TagCategory() : base()
    {
    }
    internal static void AddColumn()
    {
        try
        {
            var q = @"
ALTER TABLE ""TagCategory"" ADD COLUMN  ""IsVisible"" INTEGER NOT NULL Default 1;
";
            Execute(q);
        }
        catch (Exception ex)
        {
            
        }
    }
}