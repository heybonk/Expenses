namespace Expenses;

public partial class MonthlyWindow : ContentPage
{
	private MonthlyWindowViewModel _monthlyWindowViewModel = new();

	public MonthlyWindow()
	{
		InitializeComponent();
		this.BindingContext = _monthlyWindowViewModel;
	}
	protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MonthlyWindowViewModel)?.OnAppearing();
    }
}