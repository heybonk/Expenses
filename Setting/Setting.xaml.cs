namespace Expenses;

public partial class Setting : ContentPage
{
	private SettingViewModel _settingViewModel = new SettingViewModel();

	public Setting()
	{
		InitializeComponent();
		this.BindingContext = _settingViewModel;
	}
}