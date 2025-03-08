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

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.SetTotalItemsInitialCountCommand.Execute(true);
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_viewModel.ClearPropertiesCommand.Execute(true);
	}

	private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
	{
		var viewModel = BindingContext as WordListViewModel;
		if (viewModel != null && viewModel.SearchWordsCommand.CanExecute(e.NewTextValue))
		{
			viewModel.SearchWordsCommand.Execute(e.NewTextValue);
		}
	}
}