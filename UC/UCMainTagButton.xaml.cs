namespace Expenses;

public partial class UCMainTagButton : ContentView
{
	public UCMainTagButtonViewModel _uCMainTagButtonViewModel = new UCMainTagButtonViewModel();
	// public string MainTagCD{get; set;}

	public UCMainTagButton()
	{
		InitializeComponent();
		// this.BindingContext = this._uCMainTagButtonViewModel;
	}
}