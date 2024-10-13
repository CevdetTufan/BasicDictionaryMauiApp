using BasicDictionaryMauiApp.Pages;

namespace BasicDictionaryMauiApp
{
	public partial class App : Application
	{
		public App(WordAddPage page)
		{
			InitializeComponent();

			MainPage = page;
		}
	}
}
