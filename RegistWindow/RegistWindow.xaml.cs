namespace Expenses;

[QueryProperty(nameof(MainTag), "maintag")]
[QueryProperty(nameof(DMain), "dmain")]


public partial class RegistWindow : ContentPage
{
	private RegistWindowViewModel _registWindowViewModel = new RegistWindowViewModel();
	private DMain _dMain;
	public DMain DMain
	{
		get { return _dMain; }
		set
		{
			_dMain = value;  // プロパティに値を設定
			_registWindowViewModel.DMain = value;  // ViewModel に値を渡す
		}
	}
	private MainTag _mainTag;
	public MainTag MainTag
	{
		get { return _mainTag; }
		set
		{
			_mainTag = value;  // プロパティに値を設定
			_registWindowViewModel.MainTag = _mainTag;  // ViewModel に値を渡す
			_registWindowViewModel.Title = _mainTag.MainTagName;  // ViewModel に値を渡す
		}
	}
	public RegistWindow()
	{
		InitializeComponent();
		this.BindingContext = this._registWindowViewModel;
	}

}