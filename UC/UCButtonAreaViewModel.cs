using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace Expenses;

public class UCButtonAreaViewModel : INotifyPropertyChanged
{
    public ObservableCollection<UCMainTagButtonViewModel> MainTagButtons { get; }
        = new ObservableCollection<UCMainTagButtonViewModel>();
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _tagCategoryName;
    public string TagCategoryName
    {
        get => this._tagCategoryName;
        set
        {
            this._tagCategoryName = value;
            this.OnPropertyChanged(nameof(TagCategoryName));
        }
    }
    private TagCategory _tagCategory;
    public TagCategory TagCategory
    {
        get => this._tagCategory;
        set
        {
            this._tagCategory = value;
            this.OnPropertyChanged(nameof(TagCategory));
        }
    }
    private bool _isVisibleButtons;
    public bool IsVisibleButtons
    {
        get => this._isVisibleButtons;
        set
        {
            this._isVisibleButtons = value;
            OnPropertyChanged(nameof(IsVisibleButtons));
            if (value)
            {
                this.Text = "ー";
            }
            else
            {
                this.Text = "∨";
            }
        }
    }
    private string _text;
    public string Text
    {
        get => this._text;
        set
        {
            this._text = value;
            OnPropertyChanged(nameof(Text));
        }
    }
    public IAsyncRelayCommand ChangeVisible { get; }

    public UCButtonAreaViewModel(List<MainTag> ms,TagCategory tagCategory)
    {
        this.TagCategory = tagCategory;
        this.TagCategoryName = tagCategory.TagCategoryName;
        this.SetButton(ms);
        this.ChangeVisible = new AsyncRelayCommand(this.Change);
        this.IsVisibleButtons = Convert.ToBoolean(tagCategory.IsVisible);
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void SetButton(List<MainTag> ms)
    {
        this.MainTagButtons.Clear();

        foreach (var m in ms)
        {
            var model = new UCMainTagButtonViewModel
            {
                MainTag = m,
                OpenRegistWindow = new AsyncRelayCommand<MainTag>(this.OpenSub)
            };

            MainTagButtons.Add(model);
        }
    }
    private async Task OpenSub(MainTag maintag)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "maintag", maintag }
        };
        await Shell.Current.GoToAsync("RegistWindow", navigationParameter);
    }
    private async Task Change()
    {
        this.IsVisibleButtons = !this.IsVisibleButtons;
        this.TagCategory.IsVisible = this.IsVisibleButtons ? 1 : 0;
        this.TagCategory.UpdateData();
    }
}
