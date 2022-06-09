using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.OpenAI.Bot.Settings
{
    public class OpenAIAskParams
    {
        public string PromptTextPlaceholder
        {
            get;
            set;
        } = "";

        public int? MaxTokens
        {
            get;
            set;
        }

        public float? Temperature
        {
            get;
            set;
        }

        public float? TopP
        {
            get;
            set;
        }

        public int? N
        {
            get;
            set;
        }

        public int? Logprobs
        {
            get;
            set;
        }

        public bool? Echo
        {
            get;
            set;
        }

        public string? Stop
        {
            get;
            set;
        }

        public float? PresencePenalty
        {
            get;
            set;
        }

        public float? FrequencyPenalty
        {
            get;
            set;
        }

        public int? BestOf
        {
            get;
            set;
        }

        public object? LogitBias
        {
            get;
            set;
        } = null;



    }
}
