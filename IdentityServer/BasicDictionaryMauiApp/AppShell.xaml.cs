using BasicDictionaryMauiApp.Pages;

namespace BasicDictionaryMauiApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	private static void RegisterRoutes()
	{
		Routing.RegisterRoute("WordAddPage", typeof(WordAddPage));
		Routing.RegisterRoute("WordListPage", typeof(WordListPage));
		Routing.RegisterRoute("MainPage", typeof(MainPage));
	}
}