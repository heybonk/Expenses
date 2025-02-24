using System;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Data;


namespace Expenses;

public class TableForDataGrid
{
    public ObservableCollection<TagAndCategory> TagAndCategorys { get; set; } = new ObservableCollection<TagAndCategory>();
    public ObservableCollection<MainTag> MainTags { get; set; } = new ObservableCollection<MainTag>();
    public ObservableCollection<TagCategory> TagCategories { get; set; } = new ObservableCollection<TagCategory>();
    public TableForDataGrid()
    {
        this.ResetTags();
        this.ResetTagCategories();
        this.ResetTagAndCategory();
    }
    private void ResetTags()
    {
        var r = new MainTag().SelectAll().Cast<MainTag>();
        this.MainTags = new ObservableCollection<MainTag>(r.OfType<MainTag>());
    }
    private void ResetTagCategories()
    {
        var r = new TagCategory().SelectAll().Cast<TagCategory>();
        this.TagCategories = new ObservableCollection<TagCategory>(r.OfType<TagCategory>());
    }
    private void ResetTagAndCategory()
    {
        var combinedData = from mainTag in MainTags
                           join tagCategory in TagCategories on mainTag.TagCategoryCD equals tagCategory.TagCategoryCD
                           select new TagAndCategory
                           {
                               MainTagName = mainTag.MainTagName,
                               TagCategoryName = tagCategory.TagCategoryName
                           };
        this.TagAndCategorys = new ObservableCollection<TagAndCategory>(combinedData.OfType<TagAndCategory>());
    }

    public class TagAndCategory
    {
        public string MainTagName { get; set; }
        public string TagCategoryName { get; set; }
    }
}
