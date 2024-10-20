using BasicDictionaryMauiApp.ViewModels;

namespace BasicDictionaryMauiApp.Pages;

public partial class WordListPage : ContentPage
{
	private readonly WordListViewModel _viewModel;
	public WordListPage(WordListViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}