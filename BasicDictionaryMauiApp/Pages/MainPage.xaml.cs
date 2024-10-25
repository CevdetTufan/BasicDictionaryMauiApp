using BasicDictionaryMauiApp.ViewModels;

namespace BasicDictionaryMauiApp.Pages;

public partial class MainPage : ContentPage
{
	private readonly MainPageViewModel _viewModel;
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;

	}

	private async Task SlideFrame()
	{
		double offScreenPositionRight = this.Width;
		double offScreenPositionLeft = -SlidingFrame.Width;

		//right
		await SlidingFrame.TranslateTo(offScreenPositionRight, 0, 500, Easing.SpringOut);

		await Task.Delay(50);

		//dequee item
		ExecuteDequeeCommand();

		//left
		SlidingFrame.TranslationX = offScreenPositionLeft;
		await SlidingFrame.TranslateTo(0, 0, 500, Easing.CubicInOut);
	}

	private async void btnSlide_Clicked(object sender, EventArgs e)
	{
		await SlideFrame();
	}

	private void ExecuteDequeeCommand()
	{
		_viewModel.DequeeCommand.Execute(true);
	}
}