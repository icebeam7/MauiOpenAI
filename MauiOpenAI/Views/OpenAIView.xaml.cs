using MauiOpenAI.ViewModels;

namespace MauiOpenAI.Views;

public partial class OpenAIView : ContentPage
{
	OpenAIViewModel vm;
	public OpenAIView(OpenAIViewModel vm)
	{
		InitializeComponent();

		this.vm = vm;
		this.BindingContext = vm;
	}
}