namespace Expenses;

[QueryProperty(nameof(MainTagCD), "maintagCD")]
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
	private string _mainTagCD;
	public string MainTagCD
	{
		get { return _mainTagCD; }
		set
		{
			_mainTagCD = value;  // プロパティに値を設定
			_registWindowViewModel.MainTagCD = value;  // ViewModel に値を渡す
		}
	}
	public RegistWindow()
	{
		InitializeComponent();
		this.BindingContext = this._registWindowViewModel;
	}
}