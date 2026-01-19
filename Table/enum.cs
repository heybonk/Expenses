namespace Expenses;

public enum ColumnType
{
    None,
    JustColumn,
    PrimaryKey,
}

public enum Category
{
    //文字
    TEXT,
    //整数
    INTEGER,
    //少数
    REAL,
    //日付
    DATE,
    //日付時間
    DATETIME,
}