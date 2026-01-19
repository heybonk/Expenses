namespace Expenses;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("MainPage/RegistWindow", typeof(RegistWindow));
		Routing.RegisterRoute("MainPage/Setting", typeof(Setting));
		Routing.RegisterRoute("MainPage/MonthlyWindow", typeof(MonthlyWindow));
	}
}
