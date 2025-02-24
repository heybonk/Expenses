using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json;
using CommunityToolkit.Maui.Core.Extensions;

namespace Expenses;

public class MainViewModel : ObservableObject
{
    public StackLayout StackLayout { get; set; }
    public IAsyncRelayCommand OpenSettingWindow { get; }

    internal ObservableCollection<MainTag> MainTags { get; set; }

    public MainViewModel()
    {
        this.StackLayout = new StackLayout();
		DataBaseHelper.Init();
		TableHelper.Init();
        this.OpenSettingWindow = new AsyncRelayCommand(this.OpenSetting);

        this.ResetMainTags();

        this.ResetButtons();


    }
    internal void ResetButtons()
    {
        this.StackLayout.Children.Clear();

        foreach (var m in this.MainTags)
        {
            this.SetButton(m);
        }
    }
    private void ResetMainTags()
    {
        var maintags = new MainTag().SelectAll().OfType<MainTag>();
        if (maintags != null)
        {
            this.MainTags = new ObservableCollection<MainTag>(maintags);
        }
        else
        {
            this.MainTags = new ObservableCollection<MainTag>();
        }
        this.MainTags.CollectionChanged += this.MainTags_CollectionChanged;
    }
    private void SetButton(MainTag m)
    {
        var button = new UCMainTagButton();
        var model = new UCMainTagButtonViewModel();
        model.MainTag = m;
        model.OpenRegistWindow = new AsyncRelayCommand<string>(this.OpenSub);
        button.BindingContext = model;
        this.StackLayout.Children.Add(button);
    }
    private async Task OpenSub(string maintagCD)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "maintagCD", maintagCD }
        };
        await Shell.Current.GoToAsync("RegistWindow", navigationParameter);
    }
    private async Task OpenSetting()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "MainTags", this.MainTags }
        };
        await Shell.Current.GoToAsync("Setting", navigationParameter);
    }
    public void MainTags_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var newItem in e.NewItems)
            {
                var item = newItem as MainTag;
                if (newItem != null)
                {
                    this.SetButton(newItem as MainTag);
                }
            }
        }
    }
}
