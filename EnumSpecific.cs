namespace Expenses;

public enum EditMode
{
    Tag,
    Category
}
public enum RegistMode
{
    Insert,
    Update,
    Delete
}
internal static class EnumExtentions
{
    internal static string ConvertToString(this RegistMode mode)
    {
        if (mode == RegistMode.Insert) return "新規登録";
        else if (mode == RegistMode.Update) return "修正登録";
        else return "削除";
    }
}
