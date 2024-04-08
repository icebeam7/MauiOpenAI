namespace MauiOpenAI.Models.Images
{
    public class GenerationRequest
    {
        public string caption { get; set; }

        public string resolution { get; set; } = "512x512";
    }
}
