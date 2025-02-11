using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Expenses;

public class SettingViewModel : INotifyPropertyChanged
{
    internal MainTag MainTag { get; set; }
    internal TagCategory TagCategory { get; set; }

    private string _cd;
    public string CD
    {
        get => this._cd;
        set
        {
            this._cd = value;
            OnPropertyChanged(nameof(CD));
        }
    }
    private string _name;
    public string Name
    {
        get => this._name;
        set
        {
            this._name = value;
            OnPropertyChanged(nameof(Name));
            if (this.SelectedEditMode == EditMode.Tag)
            {
                this.MainTag.MainTagName = value;
            }
            else if (this.SelectedEditMode == EditMode.Category)
            {
                this.TagCategory.TagCategoryName = value;
            }
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
            this.IsEnabledPicker = value == EditMode.Tag;
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
    public ObservableCollection<TagCategory> TagCategories { get; } = new ObservableCollection<TagCategory>();
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
                this.MainTag.TagCategoryCD = value.TagCategoryCD;
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    public Command Regist { get; }
    public SettingViewModel()
    {
        this.MainTag = new MainTag();
        this.TagCategory = new TagCategory();
        this.PropertyChanged += this.Viewmodel_PropertyChanged;
        this.Regist = new Command(this.RegistData);
        var a = new TagCategory().SelectAll().Cast<TagCategory>();
        this.TagCategories = new ObservableCollection<TagCategory>(a.OfType<TagCategory>());
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void RegistData()
    {
        this.InsertData();
        this.ShowCompleteMessage();
    }
    private void InsertData()
    {
        if (this.SelectedEditMode == EditMode.Tag)
        {
            this.MainTag.MainTagCD = Guid.NewGuid().ToString();
            this.MainTag.DisplayOrder = 0;
            this.MainTag.InsertData();
        }
        else if (this.SelectedEditMode == EditMode.Category)
        {
            this.TagCategory.TagCategoryCD = Guid.NewGuid().ToString();
            this.TagCategory.DisplayOrder = 0;
            this.TagCategory.InsertData();
            this.TagCategories.Add(this.TagCategory);
        }
    }
    private async void ShowCompleteMessage()
    {
        await Application.Current.MainPage.DisplayAlert("完了", "操作が完了しました", "OK");
    }
    private void Viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedEditMode))
        {
            this.MainTag = new MainTag();
            this.TagCategory = new TagCategory();
        }
    }
}
