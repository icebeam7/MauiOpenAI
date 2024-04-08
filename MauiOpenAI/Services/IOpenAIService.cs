using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiOpenAI.Services
{
    public interface IOpenAIService
    {
        Task<string> AskQuestion(string query);
        Task<string> CreateImage(string query);
    }
}
