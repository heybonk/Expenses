namespace Expenses;

public partial class MainPage : ContentPage
{
	private MainViewModel _megistWindowViewModel = new MainViewModel();

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = _megistWindowViewModel;
	}
	protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _megistWindowViewModel.ResetButton();
    }
}

