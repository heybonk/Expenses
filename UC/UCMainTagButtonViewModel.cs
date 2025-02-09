using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Expenses;

public class UCMainTagButtonViewModel : ObservableObject
{
    private MainTag _mainTag;
    internal MainTag MainTag
    {
        get => _mainTag;
        set
        {
            this._mainTag = value;
        }
    }
    internal string MainTagCD
    {
        get => this._mainTag.MainTagName;
    }
    public string MainTagName
    {
        get => _mainTag.MainTagName;
        set
        {
            this._mainTag.MainTagName = value;
        }
    }
    public IAsyncRelayCommand<string> OpenRegistWindow { get; set;}
    public UCMainTagButtonViewModel()
    {
    }
}
