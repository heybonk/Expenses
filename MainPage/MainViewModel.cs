using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Expenses;

public class MainViewModel : ObservableObject
{
    public StackLayout StackLayout { get; set; }
    public IAsyncRelayCommand OpenSettingWindow { get; }

    public MainViewModel()
    {
        this.StackLayout = new StackLayout();
		DataBaseHelper.Init();
		TableHelper.Init();
        this.OpenSettingWindow = new AsyncRelayCommand(this.OpenSetting);
        ResetButton();
    }
    internal void ResetButton()
    {
        this.StackLayout.Children.Clear();
        var maintags = new MainTag().SelectAll();
        foreach (var m in maintags)
        {
            var maintag = m as MainTag;
            var button = new UCMainTagButton();
            var model = new UCMainTagButtonViewModel();
            model.MainTag = maintag;
            model.OpenRegistWindow = new AsyncRelayCommand<string>(this.OpenSub);
            button.BindingContext = model;
            this.StackLayout.Children.Add(button);
        }
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
        await Shell.Current.GoToAsync("Setting");
    }
}
