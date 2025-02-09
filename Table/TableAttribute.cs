using System;

namespace Expenses;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TableAttribute: Attribute
{
    public ColumnType ColumnType{get{return this._columnType;}}
    public Category Category{get{return this._category;}}
    public bool Nullable{get{return this._nullable;}}


    private ColumnType _columnType;
    private Category _category;
    private bool _nullable;


    public TableAttribute(ColumnType columnType, Category category, bool nullable)
    {
        _columnType = columnType;
        _category = category;
        _nullable = nullable;
    }
}
