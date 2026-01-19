using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using System.Text.Json;
using CommunityToolkit.Maui.Core.Extensions;

namespace Expenses;

public class MainViewModel : ObservableObject
{
    public ObservableCollection<UCButtonAreaViewModel> ButtonAreas { get; }
    = new ObservableCollection<UCButtonAreaViewModel>();

    public IAsyncRelayCommand OpenSettingWindow { get; }
    public IAsyncRelayCommand OpenMonthlyWindow { get; }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public MainViewModel()
    {
        DataBaseHelper.Init();
        TableHelper.Init();
        DTotal.UpdatebyMonth();
        this.OpenSettingWindow = new AsyncRelayCommand(this.OpenSetting);
        this.OpenMonthlyWindow = new AsyncRelayCommand(this.OpenMonthly);

        this.ResetTagCategories();
        this.ResetTag();
    }
    internal void ResetButtons()
    {
        this.ButtonAreas.Clear();

        var result = TableUtility.Instance.MainTags
    .OrderBy(x => x.DisplayOrder)
    .GroupBy(x => x.TagCategoryCD)
    .Select(g => g.ToList())
    .ToList();

        foreach (var item in result)
        {
            var model = new UCButtonAreaViewModel(item);
            this.ButtonAreas.Add(model);
        }
    }
    private void ResetMainTags()
    {
        var maintags = new MainTag().SelectAll().OfType<MainTag>();
        TableUtility.Instance.MainTags = new ObservableCollection<MainTag>(maintags);
    }
    private void ResetTagCategories()
    {
        var tagCategories = new TagCategory().SelectAll().OfType<TagCategory>();
        TableUtility.Instance.TagCategories = new ObservableCollection<TagCategory>(tagCategories);
    }
    private async Task OpenSub(MainTag maintag)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "maintag", maintag }
        };
        await Shell.Current.GoToAsync("RegistWindow", navigationParameter);
    }
    private async Task OpenSetting()
    {
        var navigationParameter = new Dictionary<string, object> { };
        await Shell.Current.GoToAsync("Setting", navigationParameter);
    }
    private async Task OpenMonthly()
    {
        var navigationParameter = new Dictionary<string, object> { };
        await Shell.Current.GoToAsync("MonthlyWindow", navigationParameter);
    }
    private void ResetTag()
    {
        this.ResetMainTags();
        this.ResetButtons();
        foreach (var maintag in TableUtility.Instance.MainTags)
        {
            var c = TableUtility.Instance.TagCategories.FirstOrDefault(t => t.TagCategoryCD == maintag.TagCategoryCD);
            if (c != null)
            {
                maintag.TagCategoryName = c.TagCategoryName;
            }
        }
    }
    public void OnAppearing()
    {
        TableUtility.Instance.SetAmountByMonth(DateTime.Now);
        this.ResetButtons();
    }
}
