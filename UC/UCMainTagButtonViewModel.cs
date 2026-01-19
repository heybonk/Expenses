using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Expenses;

public class UCMainTagButtonViewModel : ObservableObject
{
    private MainTag _mainTag;
    public MainTag MainTag
    {
        get => _mainTag;
        set
        {
            this._mainTag = value;
        }
    }
    public string MainTagCD
    {
        get => this._mainTag.MainTagCD;
    }
    public string MainTagName
    {
        get => _mainTag.MainTagName;
        set
        {
            this._mainTag.MainTagName = value;
        }
    }
    public IAsyncRelayCommand<MainTag> OpenRegistWindow { get; set;}
    public UCMainTagButtonViewModel()
    {
    }
}
