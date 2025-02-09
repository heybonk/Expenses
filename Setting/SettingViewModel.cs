using System;
using System.ComponentModel;

namespace Expenses;

public class SettingViewModel : INotifyPropertyChanged
{
    internal MainTag MainTag { get; set; }
    private string _mainTagCD;
    public string MainTagCD
    {
        get => this._mainTagCD;
        set
        {
            this._mainTagCD = value;
        }
    }
    private string _mainTagName;
    public string MainTagName
    {
        get => this._mainTagName;
        set
        {
            this._mainTagName = value;
            this.MainTag.MainTagName = value;
        }
    }
    private string _subTagCD;
    public string SubTagCD
    {
        get => this._subTagCD;
        set
        {
            this._subTagCD = value;
        }
    }
    private int _displayOrder;
    public int DisplayOrder
    {
        get => this._displayOrder;
        set
        {
            this._displayOrder = value;
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public Command Regist { get; }
    public SettingViewModel()
    {
        this.MainTag = new MainTag();
        this.Regist = new Command(this.RegistData);
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
        this.MainTag.MainTagCD = Guid.NewGuid().ToString();
        this.MainTag.DisplayOrder = 0;
        this.MainTag.InsertData();

    }
    private async void ShowCompleteMessage()
    {
        await Application.Current.MainPage.DisplayAlert("完了", "操作が完了しました", "OK");
    }

}
