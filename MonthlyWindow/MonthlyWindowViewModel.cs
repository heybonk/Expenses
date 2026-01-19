using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;

namespace Expenses;

public class MonthlyWindowViewModel : INotifyPropertyChanged
{
    public int PrevMonth => -1;
    public int NextMonth => 1;
    private DateTime _month;
    public DateTime Month
    {
        get => this._month;
        set
        {
            this._month = value;
            this.OnPropertyChanged(nameof(Month));
            var temp = new DMain().SelectByMonth(this.Month);
            if (temp != null)
            {
                this.lDmain = temp.ToObservableCollection();
            }
            else this.lDmain = new();
        }
    }
    private ObservableCollection<DMain> _lDmain = new();
    public ObservableCollection<DMain> lDmain
    {
        get => this._lDmain;
        set
        {
            this._lDmain = value;
            foreach (var d in value)
            {
                var mainTag = TableUtility.Instance.MainTags.FirstOrDefault(t => t.MainTagCD == d.MainTagCD);
                if (mainTag != null) d.MainTagName = mainTag.MainTagName;
                else d.MainTagName = "※タグ削除済";
            }
            this.OnPropertyChanged(nameof(lDmain));
        }
    }
    private bool _isShowMeisai = true;
    public bool IsShowMeisai
    {
        get => this._isShowMeisai;
        set
        {
            this._isShowMeisai = value;
            OnPropertyChanged(nameof(IsShowMeisai));
        }
    }
    private bool _isShowSummary = false;
    public bool IsShowSummary
    {
        get => this._isShowSummary;
        set
        {
            this._isShowSummary = value;
            OnPropertyChanged(nameof(IsShowSummary));
        }
    }
    public IAsyncRelayCommand<DMain> OpenRegistWindow { get; }
    public IAsyncRelayCommand<DMain> Delete { get; }
    public IAsyncRelayCommand<int> Move { get; }
    public IAsyncRelayCommand ChangeDisplay { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public MonthlyWindowViewModel()
    {
        this.Month = DateTime.Now;
        this.OpenRegistWindow = new AsyncRelayCommand<DMain>(this.OpenRegist);
        this.Delete = new AsyncRelayCommand<DMain>(this.DeleteData);
        this.Move = new AsyncRelayCommand<int>(this.MoveMonth);
        this.ChangeDisplay = new AsyncRelayCommand(this.Change);
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private async Task OpenRegist(DMain dmain)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "dmain", dmain }
        };
        await Shell.Current.GoToAsync("RegistWindow", navigationParameter);
    }
    private async Task DeleteData(DMain dmain)
    {
        var result = await Application.Current.MainPage.DisplayAlert("確認", dmain.MainTagName + "：" + dmain.Amount + Environment.NewLine + "削除しますか？", "Yes", "No");
        if (result == false) return;
        dmain.DeleteData();
        var total = new DTotal();
        this.lDmain.Remove(dmain);

        total.Month = dmain.RecordingDate.Month;
        total.Year = dmain.RecordingDate.Year;
        total.MainTagCD = dmain.MainTagCD;
        total.SetByPrimaryKey();
        total.Amount -= dmain.Amount;
        total.Update();

        TableUtility.Instance.SetAmountByMonth(this.Month);
        this.SetShowHeader();
    }
    private async Task MoveMonth(int num)
    {
        this.Month = this.Month.AddMonths(num);
        TableUtility.Instance.SetAmountByMonth(this.Month);
        this.SetShowHeader();
    }
    private async Task Change()
    {
        this.IsShowMeisai = !this.IsShowMeisai;
        this.IsShowSummary = !this.IsShowSummary;
    }
    public void OnAppearing()
    {
        var sorted = this.lDmain.OrderBy(x => x.RecordingDate).ToList();

        this.lDmain.Clear();
        foreach (var item in sorted)
        {
            this.lDmain.Add(item);
        }
        this.SetShowHeader();
    }
    private void SetShowHeader()
    {
        var list = this.lDmain
    .OrderBy(x => x.RecordingDate)
    .ToList();

        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0 || list[i].RecordingDate != list[i - 1].RecordingDate)
            {
                list[i].IsShowHeader = true;
            }
            else
            {
                list[i].IsShowHeader = false;
            }
        }
        this.lDmain = list.ToObservableCollection();


        var list2 = TableUtility.Instance.DTotalByMainTag
    .OrderBy(x => x.TagCategoryCD)
    .ToList();
        for (int i = 0; i < list2.Count; i++)
        {
            if (i == 0 || list2[i].TagCategoryCD != list2[i - 1].TagCategoryCD)
            {
                list2[i].IsShowHeader = true;
            }
            else
            {
                list2[i].IsShowHeader = false;
            }
        }
        TableUtility.Instance.DTotalByMainTag = list2.ToObservableCollection();

    }
}
