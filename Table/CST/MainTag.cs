using System;

namespace Expenses;

public class MainTag:Table
{
    [TableAttribute(ColumnType.PrimaryKey, Category.TEXT, false)]
    public string MainTagCD { get; set; }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string MainTagName
    {
        get => this._mainTagName;
        set
        {
            this._mainTagName = value;
            this.OnPropertyChanged(nameof(MainTagName));
        }
    }

    [TableAttribute(ColumnType.JustColumn, Category.TEXT, true)]
    public string TagCategoryCD { get; set; }
    
    [TableAttribute(ColumnType.JustColumn, Category.INTEGER, false)]
    public int DisplayOrder { get; set; }

    public string TagCategoryName
    {
        get => this._tagCategoryName;
        set
        {
            this._tagCategoryName = value;
            this.OnPropertyChanged(nameof(TagCategoryName));
        }
    }
    public bool IsShowHeader { get; set; }
    public int CatDisplayOrder { get; set; }
    private string _tagCategoryName;
    private string _mainTagName;



    public MainTag() : base()
    {
    }

}
