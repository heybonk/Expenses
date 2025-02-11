using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityToolkit;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;



namespace Expenses;

public class RegistWindowViewModel : INotifyPropertyChanged
{
    internal DMain DMain { get; set; }
    private DateTime _recordingDate;
    public DateTime RecordingDate
    {
        get => this._recordingDate;
        set
        {
            this._recordingDate = value;
            this.DMain.RecordingDate = value;
        }
    }
    private int _amount;
    public int Amount
    {
        get => this._amount;
        set
        {
            this._amount = value;
            this.DMain.Amount = value;
        }
    }
    private string? _mainTagCD;
    public string? MainTagCD
    {
        get => this._mainTagCD;
        set
        {
            this._mainTagCD = value;
            this.DMain.MainTagCD = value;
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public Command Regist { get; }
    public RegistWindowViewModel()
    {
        this.DMain = new DMain();
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
        this.DMain.TourokuNO = Guid.NewGuid().ToString();
        this.DMain.UpdateDatetime = DateTime.Now.ToString();
        this.DMain.InsertData();
    }
    private async void ShowCompleteMessage()
    {
        await Application.Current.MainPage.DisplayAlert("完了", "操作が完了しました", "OK");
    }
}
