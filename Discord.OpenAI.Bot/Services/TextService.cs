using Discord.OpenAI.Bot.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models;
using OpenAI.GPT3.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.OpenAI.Bot.Services
{
    public class TextService
    {
        private const string InputTextWildcard = "%inputText%";

        private readonly ILogger<TextService> logger;
        private readonly IConfiguration configuration;
        private readonly IOptions<OpenAIAskParams> askParams;
        private readonly IOpenAIService openAIService;

        public TextService(ILogger<TextService> logger, IConfiguration configuration, IOptions<OpenAIAskParams> askParams, IOpenAIService openAIService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.askParams = askParams;
            this.openAIService = openAIService;
        }

        private CompletionCreateRequest AskParamsToRequest(OpenAIAskParams askParams, string inputText)
        {
            return new CompletionCreateRequest
            {
                MaxTokens = askParams.MaxTokens,
                Temperature = askParams.Temperature,
                TopP = askParams.TopP,
                N = askParams.N,
                Logprobs = askParams.Logprobs,
                Echo = askParams.Echo,
                Stop = askParams.Stop,
                FrequencyPenalty = askParams.FrequencyPenalty,
                PresencePenalty = askParams.PresencePenalty,
                BestOf = askParams.BestOf,
                LogitBias = askParams.LogitBias,
                Prompt = askParams.PromptTextPlaceholder.Replace(InputTextWildcard, inputText)
            };
        }

        public async Task<IEnumerable<string>> AskQuestion(string inputText)
        {
            var result = await openAIService.Completions.Create(AskParamsToRequest(askParams.Value, inputText), configuration["OpenAI:AskEngine"]);

            if (result.Successful)
            {
                return result.Choices.Select(x => x.Text);
            }
            else
            {
                if (result.Error is null)
                {
                    logger.LogWarning("OpenAI Answer error: Unknown");
                    throw new OpenAIException();
                }
                else
                {
                    logger.LogWarning("OpenAI Answer error {code}: {message}", result.Error.Code, result.Error.Message);
                    throw new OpenAIException(result.Error.Code!, result.Error.Message!);
                }

            }
        }

        public class OpenAIException : Exception
        {
            public OpenAIException() : base("OpenAI error: Unknown")
            {

            }

            public OpenAIException(string code, string message) : base($"OpenAI error {code}: {message}")
            {

            }
        }

    }
}
