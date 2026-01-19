namespace Expenses;

public partial class MainPage : ContentPage
{
	private MainViewModel _megistWindowViewModel = new MainViewModel();

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = _megistWindowViewModel;

		TableUtility.Instance.DefaultColor = this.SettingBtn.BackgroundColor;
		TableUtility.Instance.DefaultTextColor = this.SettingBtn.TextColor;
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
	}
	protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainViewModel)?.OnAppearing();
    }
}

