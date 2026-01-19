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

    public UCButtonAreaViewModel(List<MainTag> ms)
    {
        this.TagCategoryName=ms.FirstOrDefault()?.TagCategoryName;
        this.SetButton(ms);
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
}
