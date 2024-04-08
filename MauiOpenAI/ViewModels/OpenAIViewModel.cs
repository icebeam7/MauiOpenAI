using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using MauiOpenAI.Services;

namespace MauiOpenAI.ViewModels
{
    public partial class OpenAIViewModel : BaseViewModel
    {
        IOpenAIService openAIService;

        [ObservableProperty]
        string prompt;

        [ObservableProperty]
        string answer;

        [ObservableProperty]
        string imageUrl;

        [ObservableProperty]
        [NotifyPropertyChangedFor("IsImage")]
        bool isText;

        public bool IsImage => !IsText;

        public OpenAIViewModel(IOpenAIService openAIService)
        {
            Title = "Generative AI";
            this.openAIService = openAIService;
        }

        [RelayCommand]
        async Task AskQuestionAsync()
        {
            if (IsBusy)
                return;
            try
            {
                Answer = string.Empty;
                ImageUrl = string.Empty;

                IsBusy = true;
                Answer = await openAIService.AskQuestion(Prompt);
                IsText = true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GenerateImageAsync()
        {
            if (IsBusy)
                return;
            try
            {
                Answer = string.Empty;
                ImageUrl = string.Empty;

                IsBusy = true;
                ImageUrl = await openAIService.CreateImage(Prompt);
                IsText = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
