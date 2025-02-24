using System;

namespace Expenses;

public static class Common
{
    public static async void ShowMessage(string title,string message,string cancel)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }

}
