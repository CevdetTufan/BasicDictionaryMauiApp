using BasicDictionaryMauiApp.ViewModels;

namespace BasicDictionaryMauiApp.Pages;

public partial class WordAddPage : FlyoutPage
{
	private readonly WordAddPageViewModel _viewModel;
	public WordAddPage(WordAddPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}
