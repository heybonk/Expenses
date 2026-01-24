using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Converters;
using System.Collections.Specialized;
using CommunityToolkit.Maui.Core.Extensions;

namespace Expenses;

public class SettingViewModel : INotifyPropertyChanged
{
    private MainTag _mainTag;
    internal MainTag MainTag
    {
        get => this._mainTag;
        set
        {
            this._mainTag = value;
            OnPropertyChanged(nameof(MainTag));
        }
    }
    private TagCategory _tagCategory;
    internal TagCategory TagCategory
    {
        get => this._tagCategory;
        set
        {
            this._tagCategory = value;
            OnPropertyChanged(nameof(TagCategory));
        }
    }
    public IAsyncRelayCommand<MainTag> ExecDeleteMainTag { get; }
    public IAsyncRelayCommand<TagCategory> ExecDeleteTagCategory { get; }
    public IAsyncRelayCommand<MainTag> ExecUpdateMainTag { get; }
    public IAsyncRelayCommand<TagCategory> ExecUpdateTagCategory { get; }
    public IAsyncRelayCommand<TagCategory> ChangeOrder { get; }


    private string _name;
    public string Name
    {
        get => this._name;
        set
        {
            this._name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    private int _displayOrder;
    public int DisplayOrder
    {
        get => this._displayOrder;
        set
        {
            this._displayOrder = value;
            OnPropertyChanged(nameof(DisplayOrder));
        }
    }
    private EditMode _selectededitMode;
    public EditMode SelectedEditMode
    {
        get => this._selectededitMode;
        set
        {
            this._selectededitMode = value;
            OnPropertyChanged(nameof(SelectedEditMode));

        }
    }
    private bool _isEnabledGridButton;
    public bool IsEnabledGridButton
    {
        get => this._isEnabledGridButton;
        set
        {
            this._isEnabledGridButton = value;
            OnPropertyChanged(nameof(IsEnabledGridButton));
        }
    }
    private bool _isEnabledPicker;
    public bool IsEnabledPicker
    {
        get => this._isEnabledPicker;
        set
        {
            this._isEnabledPicker = value;
            OnPropertyChanged(nameof(IsEnabledPicker));
        }
    }
    private bool _isVisibleCancel;
    public bool IsVisibleCancel
    {
        get => this._isVisibleCancel;
        set
        {
            this._isVisibleCancel = value;
            OnPropertyChanged(nameof(IsVisibleCancel));
        }
    }
    private int _registButtonSpan;
    public int RegistButtonSpan
    {
        get => this._registButtonSpan;
        set
        {
            this._registButtonSpan = value;
            OnPropertyChanged(nameof(RegistButtonSpan));
        }
    }
    private bool _isEnabledIncome;
    public bool IsEnabledIncome
    {
        get => this._isEnabledIncome;
        set
        {
            this._isEnabledIncome = value;
            OnPropertyChanged(nameof(IsEnabledIncome));
        }
    }
    private bool _isIncome;
    public bool IsIncome
    {
        get => this._isIncome;
        set
        {
            this._isIncome = value;
            OnPropertyChanged(nameof(IsIncome));
        }
    }
    private TagCategory _selectedTagCategory;
    public TagCategory SelectedTagCategory
    {
        get => _selectedTagCategory;
        set
        {
            if (_selectedTagCategory != value)
            {
                _selectedTagCategory = value;
                OnPropertyChanged(nameof(SelectedTagCategory));
            }
        }
    }
    private RegistMode _registMode;
    public RegistMode RegistMode
    {
        get => this._registMode;
        set
        {
            this._registMode = value;
            this.OnPropertyChanged(nameof(RegistMode));
        }
    }
    private string _buttonText;
    public string ButtonText
    {
        get => this._buttonText;
        set
        {
            this._buttonText = value;
            this.OnPropertyChanged(nameof(ButtonText));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public Command Regist { get; }
    public Command Cancel { get; }

    public SettingViewModel()
    {
        this.MainTag = new MainTag();
        this.TagCategory = new TagCategory();
        this.PropertyChanged += this.Viewmodel_PropertyChanged;
        this.Regist = new Command(this.RegistData);
        this.Cancel = new Command(this.CancelUpdate);
        this.SetShowHeader();
        this.ExecUpdateMainTag = new AsyncRelayCommand<MainTag>(this.UpdateMainTag);
        this.ExecUpdateTagCategory = new AsyncRelayCommand<TagCategory>(this.UpdateTagCategory);
        this.ExecDeleteMainTag = new AsyncRelayCommand<MainTag>(this.DeleteMainTag);
        this.ExecDeleteTagCategory = new AsyncRelayCommand<TagCategory>(this.DeleteTagCategory);
        this.ChangeOrder = new AsyncRelayCommand<TagCategory>(this.ChangeOrder_Execute);

        this.SelectedEditMode = EditMode.Category;

        TableUtility.Instance.ReorderMainTag();
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void RegistData()
    {
        if (!this.CheckBeforeRegist()) return;
        this.InsertData();
        this.ShowCompleteMessage();
        this.ResetRegistValue();
    }
    private void CancelUpdate()
    {
        if (this.SelectedEditMode == EditMode.Category)
        {
            this.TagCategory = new();
        }
        else if (this.SelectedEditMode == EditMode.Tag)
        {
            this.MainTag = new();
        }
    }
    private void InsertData()
    {
        if (this.SelectedEditMode == EditMode.Tag)
        {
            if (this.RegistMode == RegistMode.Insert) this.MainTag.MainTagCD = Guid.NewGuid().ToString();
            this.MainTag.MainTagName = this.Name;
            this.MainTag.TagCategoryCD = this.SelectedTagCategory.TagCategoryCD;
            this.MainTag.TagCategoryName = this.SelectedTagCategory.TagCategoryName;
            this.MainTag.CatDisplayOrder = this.SelectedTagCategory.DisplayOrder;
            this.MainTag.DisplayOrder = 0;
            this.MainTag.Update();
            if (this.RegistMode == RegistMode.Insert) TableUtility.Instance.MainTags.Add(this.MainTag);
            this.SetShowHeader();
        }
        else if (this.SelectedEditMode == EditMode.Category)
        {
            if (this.RegistMode == RegistMode.Insert)
            {
                this.TagCategory.TagCategoryCD = Guid.NewGuid().ToString();
                this.TagCategory.IsVisible = 1;
                int displayorder = 0;
                if (TableUtility.Instance.TagCategories.Count > 0)
                {
                    displayorder = TableUtility.Instance.TagCategories.Max(x => x.DisplayOrder) + 1;
                }
                this.TagCategory.DisplayOrder = displayorder;
            }
            this.TagCategory.TagCategoryName = this.Name;
            this.TagCategory.IsIncome = this.IsIncome ? 1 : 0;
            this.TagCategory.Update();
            if (this.RegistMode == RegistMode.Insert) TableUtility.Instance.TagCategories.Add(this.TagCategory);
        }
    }
    private async void ShowCompleteMessage()
    {
        var RegistItemName = default(string);
        if (this.SelectedEditMode == EditMode.Tag)
        {
            RegistItemName = "タグ";
        }
        else if (this.SelectedEditMode == EditMode.Category)
        {
            RegistItemName = "カテゴリー";
        }
        Common.ShowMessage("完了", RegistItemName + "の登録が完了しました", "OK");
    }
    private async void ShowErrorMessage(string item)
    {
        Common.ShowMessage("エラー", $"{item}を入力してください", "OK");
    }
    private bool CheckBeforeRegist()
    {
        if (this.SelectedEditMode == EditMode.Category)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                this.ShowErrorMessage("タグカテゴリー名");
                return false;
            }
            if (this.RegistMode == RegistMode.Insert && TableUtility.Instance.TagCategories.Any(c => c.TagCategoryName == this.Name))
            {
                Common.ShowMessage("エラー", "同じ名前のタグカテゴリーが存在します", "OK");
                return false;
            }
        }
        else if (this.SelectedEditMode == EditMode.Tag)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                this.ShowErrorMessage("タグ名");
                return false;
            }
            else if (this.SelectedTagCategory == null || string.IsNullOrEmpty(this.SelectedTagCategory.TagCategoryCD))
            {
                this.ShowErrorMessage("タグカテゴリー名");
                return false;
            }
            if (this.RegistMode == RegistMode.Insert && TableUtility.Instance.MainTags.Any(c => c.MainTagName == this.Name))
            {
                Common.ShowMessage("エラー", "同じ名前のタグが存在します", "OK");
                return false;
            }
        }
        return true;
    }

    private void Viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedEditMode))
        {
            this.MainTag = new MainTag();
            this.TagCategory = new TagCategory();
            this.Name = string.Empty;
            this.IsEnabledPicker = this.SelectedEditMode == EditMode.Tag;
            this.IsEnabledIncome = this.SelectedEditMode == EditMode.Category;
        }
        else if (e.PropertyName == nameof(MainTag))
        {
            this.Name = this.MainTag.MainTagName;
            if (TableUtility.Instance.TagCategories != null)
            {
                this.SelectedTagCategory = TableUtility.Instance.TagCategories.Where(x => x.TagCategoryCD == this.MainTag.TagCategoryCD).FirstOrDefault();
            }
            this.DisplayOrder = this.MainTag.DisplayOrder;
            this.SetRegistMode();
        }
        else if (e.PropertyName == nameof(TagCategory))
        {
            this.Name = this.TagCategory.TagCategoryName;
            this.DisplayOrder = this.TagCategory.DisplayOrder;
            this.IsIncome = Convert.ToBoolean(this.TagCategory.IsIncome);
            this.SetRegistMode();
        }
        else if (e.PropertyName == nameof(RegistMode))
        {
            this.ButtonText = this.RegistMode.ConvertToString();
            this.IsEnabledGridButton = this.RegistMode == RegistMode.Insert;
            this.IsVisibleCancel = this.RegistMode == RegistMode.Update;
        }
        else if (e.PropertyName == nameof(IsVisibleCancel))
        {
            this.RegistButtonSpan = this.IsVisibleCancel ? 1 : 2;
        }
    }
    private async Task UpdateMainTag(MainTag mainTag)
    {
        this.MainTag = mainTag;
    }
    private async Task UpdateTagCategory(TagCategory tagCategory)
    {
        this.TagCategory = tagCategory;
    }
    private async Task DeleteMainTag(MainTag mainTag)
    {
        var result = await Application.Current.MainPage.DisplayAlert("確認", "すでに" + mainTag.MainTagName + "に紐づけられている費用は集計できなくなりますが削除しますか？", "Yes", "No");
        if (result == false) return;
        mainTag.DeleteData();
        TableUtility.Instance.MainTags.Remove(mainTag);
        this.SetShowHeader();
    }
    private async Task DeleteTagCategory(TagCategory tagCategory)
    {
        if (TableUtility.Instance.MainTags.Any(t => t.TagCategoryCD == tagCategory.TagCategoryCD))
        {
            Common.ShowMessage("エラー", "このタグカテゴリーが紐づけられているタグが存在するため削除できません", "OK");
            return;
        }
        var result = await Application.Current.MainPage.DisplayAlert("確認", tagCategory.TagCategoryName + "を削除しますか？", "Yes", "No");
        if (result == false) return;
        tagCategory.DeleteData();
        TableUtility.Instance.TagCategories.Remove(tagCategory);
        TableUtility.Instance.SetTagCategoryOrder(false);
    }
    private void ResetRegistValue()
    {
        this.MainTag = new();
        this.TagCategory = new();
        this.Name = null;
        this.IsIncome = false;
    }
    private void SetRegistMode()
    {
        if (this.SelectedEditMode == EditMode.Tag)
        {
            this.RegistMode = string.IsNullOrEmpty(this.MainTag.MainTagCD) ? RegistMode.Insert : RegistMode.Update;
        }
        else if (this.SelectedEditMode == EditMode.Category)
        {
            this.RegistMode = string.IsNullOrEmpty(this.TagCategory.TagCategoryCD) ? RegistMode.Insert : RegistMode.Update;
        }
    }
    private void SetShowHeader()
    {
        var list = TableUtility.Instance.MainTags
        .OrderBy(x => x.CatDisplayOrder)
        .ToList();

        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0 || list[i].TagCategoryCD != list[i - 1].TagCategoryCD)
            {
                list[i].IsShowHeader = true;
            }
            else
            {
                list[i].IsShowHeader = false;
            }
        }
        TableUtility.Instance.MainTags.Clear();

        foreach (var item in list)
        {
            TableUtility.Instance.MainTags.Add(item);
        }
    }

    private async Task ChangeOrder_Execute(TagCategory edittingtc)
    {
        var originorder = edittingtc.DisplayOrder;
        var neworder = originorder + 1;
        if (originorder == TableUtility.Instance.TagCategories.Max(x => x.DisplayOrder)) return;

        var tc = TableUtility.Instance.TagCategories.Where(x => x.DisplayOrder == neworder).FirstOrDefault();


        edittingtc.DisplayOrder = neworder;
        edittingtc.UpdateData();
        TableUtility.Instance.SetCatDisplayOrder(edittingtc);

        tc.DisplayOrder = originorder;
        tc.UpdateData();
        TableUtility.Instance.SetCatDisplayOrder(tc);
        this.SetShowHeader();

        TableUtility.Instance.TagCategories = TableUtility.Instance.TagCategories.OrderBy(x => x.DisplayOrder).ToObservableCollection();


        //22222
        // if (TableUtility.Instance.TagCategories.Last() == edittingtc) return;
        // var originorder = TableUtility.Instance.TagCategories.IndexOf(edittingtc);
        // var neworder = originorder + 1;
        // var tc = TableUtility.Instance.TagCategories[neworder];
        // edittingtc.DisplayOrder = neworder;
        // tc.DisplayOrder = originorder;
        // TableUtility.Instance.SetTagCategoryOrder(true);
        // this.SetShowHeader();




        //33333
        // if (TableUtility.Instance.TagCategories.Last() == edittingtc) return;

        // var originorder = TableUtility.Instance.TagCategories.IndexOf(edittingtc);
        // TableUtility.Instance.TagCategories.Move(originorder, originorder + 1);
        // TableUtility.Instance.SetTagCategoryOrder(false);

        // this.SetShowHeader();
    }
}
