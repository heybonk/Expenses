using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityToolkit;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using ObjCRuntime;
using System.Threading.Tasks;


namespace Expenses;

public class RegistWindowViewModel : INotifyPropertyChanged
{
    private string _title;
    public string Title
    {
        get => this._title;
        set
        {
            this._title = value;
            this.OnPropertyChanged(nameof(Title));
        }
    }
    private DMain _dmain;
    internal DMain DMain
    {
        get => this._dmain;
        set
        {
            this._dmain = value;
            this.OnPropertyChanged(nameof(DMain));
        }
    }
    private string _tourokuNO;
    public string TourokuNO
    {
        get => this._tourokuNO;
        set
        {
            this._tourokuNO = value;
            this.OnPropertyChanged(nameof(TourokuNO));
        }
    }
    private DateTime _recordingDate;
    public DateTime RecordingDate
    {
        get => this._recordingDate;
        set
        {
            this._recordingDate = value;
            this.OnPropertyChanged(nameof(RecordingDate));
        }
    }
    private decimal? _amount;
    public decimal? Amount
    {
        get => this._amount;
        set
        {
            this._amount = value;
            this.OnPropertyChanged(nameof(Amount));
        }
    }
    private string _bikou;
    public string Bikou
    {
        get => this._bikou;
        set
        {
            this._bikou = value;
            this.OnPropertyChanged(nameof(Bikou));
        }
    }
    private MainTag _mainTag;
    public MainTag MainTag
    {
        get => this._mainTag;
        set
        {
            this._mainTag = value;
        }
    }
    private RegistMode _registMode;
    public RegistMode RegistMode
    {
        get => this._registMode;
        set
        {
            this._registMode = value;
            this.OnPropertyChanged(nameof(RegistMode));
        }
    }
    private string _buttonText;
    public string ButtonText
    {
        get => this._buttonText;
        set
        {
            this._buttonText = value;
            this.OnPropertyChanged(nameof(ButtonText));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public AsyncRelayCommand Regist { get; }
    public RegistWindowViewModel()
    {
        this.PropertyChanged += this.Viewmodel_PropertyChanged;
        this.Regist = new AsyncRelayCommand(this.RegistData);
        this.ResetRegistValue();
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void Viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DMain))
        {
            this.TourokuNO = this.DMain.TourokuNO;
            this.RecordingDate = this.DMain.RecordingDate;
            this.Amount = this.DMain.Amount;
            this.Bikou = this.DMain.Bikou;
            this.MainTag = new MainTag()
            {
                MainTagCD = this.DMain.MainTagCD,
            };
            this.MainTag.SetByPrimaryKey();

            this.RegistMode = string.IsNullOrEmpty(this.TourokuNO) ? RegistMode.Insert : RegistMode.Update;
        }
        else if (e.PropertyName == nameof(RegistMode))
        {
            this.ButtonText = this.RegistMode.ConvertToString();
        }
    }
    private async Task RegistData()
    {
        if (!this.CheckBeforeRegist()) return;
        this.UpdateData();
        await this.ShowCompleteMessage();
        this.BackWindow();
    }
    private void UpdateData()
    {
        if (this.RegistMode == RegistMode.Update)
        {
            var rTotal = new DTotal();
            rTotal.Month = this.DMain.RecordingDate.Month;
            rTotal.Year = this.DMain.RecordingDate.Year;
            rTotal.MainTagCD = this.DMain.MainTagCD;
            rTotal.SetByPrimaryKey();
            rTotal.Amount -= this.DMain.Amount;
            rTotal.Update();
        }

        var total = new DTotal();
        total.Month = this.RecordingDate.Month;
        total.Year = this.RecordingDate.Year;
        total.MainTagCD = this.MainTag.MainTagCD;
        total.SetByPrimaryKey();
        total.Amount += (int)this.Amount;
        total.Update();

        if (this.RegistMode == RegistMode.Insert)
        {
            this.DMain.TourokuNO = Guid.NewGuid().ToString();
        }
        this.DMain.MainTagCD = this.MainTag.MainTagCD;
        this.DMain.Amount = (int)this.Amount;
        this.DMain.Bikou = this.Bikou;
        this.DMain.RecordingDate = this.RecordingDate;
        this.DMain.UpdateDatetime = DateTime.Now.ToString();
        this.DMain.Update();

        TableUtility.Instance.SetAmountByMonth(this.RecordingDate);
    }
    private async Task ShowCompleteMessage()
    {
        await Application.Current.MainPage.DisplayAlert("完了", "記録の登録が完了しました", "OK");
    }
    private async void ShowErrorMessage()
    {
        await Application.Current.MainPage.DisplayAlert("エラー", "金額を入力してください", "OK");
    }
    private async void ShowShousuuErrorMessage()
    {
        await Application.Current.MainPage.DisplayAlert("エラー", "整数を入力してください", "OK");
    }
    private async void ShowChangedMonthMessage()
    {
        await Application.Current.MainPage.DisplayAlert("エラー", "修正モードでは異なる月へ変更はできません。新規からやり直してください。", "OK");
    }
    private bool CheckBeforeRegist()
    {
        if (this.Amount % 1 != 0)
        {
            this.ShowShousuuErrorMessage();
            return false;
        }
        // if (this.Amount.Equals(0))
        // {
        //     this.ShowErrorMessage();
        //     return false;
        // }
        if (this.RegistMode == RegistMode.Update
        && (this.RecordingDate.Month != this.DMain.RecordingDate.Month || this.RecordingDate.Year != this.DMain.RecordingDate.Year))
        {
            this.ShowChangedMonthMessage();
            return false;
        }
        return true;
    }
    private void ResetRegistValue()
    {
        this.DMain = new DMain();
        this.Amount = null;
        this.Bikou = null;
        this.RecordingDate = DateTime.Now;
    }
    private async Task BackWindow()
    {
        await Shell.Current.GoToAsync("..");
    }
}
