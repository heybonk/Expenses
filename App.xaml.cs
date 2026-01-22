namespace Expenses;

public partial class App : Application
{
	private VersionAdditional _version;

	public App()
	{
		InitializeComponent();

		DataBaseHelper.Init();
		TableHelper.Init();
		_version = new VersionAdditional();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var loadingPage = new ContentPage
		{
			Content = new ActivityIndicator
			{
				IsRunning = true,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			}
		};

		var window = new Window(loadingPage);
		_ = InitializeAsync(window, loadingPage);
		return window;
	}

	private async Task InitializeAsync(Window window, Page page)
	{
		await _version.SetConfig();
		_version.SetNowVersion();

		if (!_version.CheckVersion())
		{
			await page.DisplayAlert(
				"アップデートのお知らせ",
				"新しいバージョンがあります。アップデート後にアプリを再起動してください。",
				"App Storeへ");

			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				await Launcher.OpenAsync(
					new Uri("itms-apps://apps.apple.com/jp/app/id6757752428")
				);
			});

			return;
		}
	    _version.Execute();
		window.Page = new AppShell();
	}
}