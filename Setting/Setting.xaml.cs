using System.Collections.ObjectModel;

namespace Expenses;

[QueryProperty(nameof(MainTags), "MainTags")]
public partial class Setting : ContentPage
{
	private SettingViewModel _settingViewModel = new SettingViewModel();
	private ObservableCollection<MainTag> _mainTags;
	public ObservableCollection<MainTag> MainTags
	{
		get { return _mainTags; }
		set
		{
			_mainTags = value;  // プロパティに値を設定
			_settingViewModel.MainTags = value;  // ViewModel に値を渡す
		}
	}

	public Setting()
	{
		InitializeComponent();
		this.BindingContext = _settingViewModel;
	}
}