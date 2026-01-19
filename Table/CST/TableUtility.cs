using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace Expenses;

public class TableUtility : INotifyPropertyChanged
{
    private static TableUtility _instance = new();
    public static TableUtility Instance => _instance;

    public event PropertyChangedEventHandler? PropertyChanged;

    private Color _color;
    public Color Color
    {
        get => this._color;
        set
        {
            this._color = value;
            this.OnPropertyChanged(nameof(Color));
        }
    }
    private Color _defaultColor;
    public Color DefaultColor
    {
        get => this._defaultColor;
        set
        {
            this._defaultColor = value;
            this.OnPropertyChanged(nameof(DefaultColor));
        }
    }
    private Color _defaultTextColor;
    public Color DefaultTextColor
    {
        get => this._defaultTextColor;
        set
        {
            this._defaultTextColor = value;
            this.OnPropertyChanged(nameof(DefaultTextColor));
        }
    }
    internal ObservableCollection<DTotal> dTotals = new();
    private ObservableCollection<MainTag> _mainTags = new();

    public ObservableCollection<MainTag> MainTags
    {
        get
        {
            return this._mainTags;
        }
        set
        {
            this._mainTags = value;
            OnPropertyChanged(nameof(MainTags));
        }
    }
    private ObservableCollection<TagCategory> _tagCategories = new();

    public ObservableCollection<TagCategory> TagCategories
    {
        get
        {
            return this._tagCategories;
        }
        set
        {
            this._tagCategories = value;
            OnPropertyChanged(nameof(TagCategories));
        }
    }
    private ObservableCollection<DTotal> _dTotalByMainTag = new();

    public ObservableCollection<DTotal> DTotalByMainTag
    {
        get
        {
            return this._dTotalByMainTag;
        }
        set
        {
            this._dTotalByMainTag = value;
            OnPropertyChanged(nameof(DTotalByMainTag));
        }
    }

    private int _totalAmount_ThisMonth;
    public int TotalAmount_ThisMonth
    {
        get
        {
            return this._totalAmount_ThisMonth;
        }
        set
        {
            this._totalAmount_ThisMonth = value;
            OnPropertyChanged(nameof(TotalAmount_ThisMonth));
        }
    }
    private int _totalIncome_ThisMonth;
    public int TotalIncome_ThisMonth
    {
        get
        {
            return this._totalIncome_ThisMonth;
        }
        set
        {
            this._totalIncome_ThisMonth = value;
            OnPropertyChanged(nameof(TotalIncome_ThisMonth));
        }
    }
    private int _totalBalance_ThisMonth;
    public int TotalBalance_ThisMonth
    {
        get
        {
            return this._totalBalance_ThisMonth;
        }
        set
        {
            this._totalBalance_ThisMonth = value;
            OnPropertyChanged(nameof(TotalBalance_ThisMonth));
        }
    }
    protected void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void SetDTotals()
    {
        var t = new DTotal().SelectAll().OfType<DTotal>();
        Instance.dTotals = new ObservableCollection<DTotal>(t);
    }
    internal void SetAmountByMonth(DateTime d)
    {
        this.SetDTotals();
        this.SetAmountByMainTag(d);
    }
    internal void SetAmountByMainTag(DateTime d)
    {
        this.DTotalByMainTag = Instance.dTotals.Where(x => x.Year == d.Year
                                                        && x.Month == d.Month
                                                        && x.Amount != 0).ToObservableCollection();

        foreach (var temp in this.DTotalByMainTag)
        {
            var c = TableUtility.Instance.MainTags.FirstOrDefault(t => t.MainTagCD == temp.MainTagCD);
            if (c != null)
            {
                temp.MainTagName = c.MainTagName;
                temp.TagCategoryCD = c.TagCategoryCD;
                temp.TagCategoryName = c.TagCategoryName;
            }
            else
            {
                temp.MainTagCD = "hoge";
                temp.MainTagName = "";
                temp.TagCategoryCD = "hoge";
                temp.TagCategoryName = "※タグ削除済";
            }
        }

        var groups = this.DTotalByMainTag
    .GroupBy(x => x.TagCategoryCD)
    .ToDictionary(
        g => g.Key,
        g => g.Sum(x => x.Amount)
    );


        foreach (var item in this.DTotalByMainTag)
        {
            item.Amount_Category = groups[item.TagCategoryCD];
            var a = TableUtility.Instance.TagCategories.FirstOrDefault(t => t.TagCategoryCD == item.TagCategoryCD);
            if (a != null) item.IsIncome = a.IsIncome;
        }

        var hiyou = this.DTotalByMainTag.Where(x => x.IsIncome == 0);
        var income = this.DTotalByMainTag.Where(x => x.IsIncome == 1);

        if (hiyou != null)
        {
            this.TotalAmount_ThisMonth = hiyou.Sum(x => x.Amount);
        }
        if (income != null)
        {
            this.TotalIncome_ThisMonth = income.Sum(x => x.Amount);
        }
        this.TotalBalance_ThisMonth = this.TotalIncome_ThisMonth - this.TotalAmount_ThisMonth;
        if (this.TotalBalance_ThisMonth < 0)
        {
            this.Color = Colors.Red;
        }
        else if (this.TotalBalance_ThisMonth > 0)
        {
            this.Color = TableUtility.Instance.DefaultColor;
        }
        else
        {
            this.Color = Colors.Transparent;
        }
    }
    public void SetOrder()
    {
        var l = TableUtility.Instance.TagCategories.OrderBy(x => x.DisplayOrder).ToList();
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].DisplayOrder != i) ;
            {
                l[i].DisplayOrder = i;
                l[i].UpdateData();
            }
        }
        TableUtility.Instance.TagCategories = l.ToObservableCollection();
    }
}
