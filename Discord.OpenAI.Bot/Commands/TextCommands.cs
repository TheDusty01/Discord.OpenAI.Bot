using Discord.OpenAI.Bot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.OpenAI.Bot.Commands
{
    
    public class TextCommands : BaseCommandModule
    {
        private readonly TextService textService;

        public TextCommands(TextService textService)
        {
            this.textService = textService;
        }

        [Command("ask")]
        [Description("Ask a question")]
        [RequireGuild]
        [Cooldown(4, 1.5, CooldownBucketType.Channel)]
        public async Task AskCommand(CommandContext ctx,
            [Description("Your input text/question"), RemainingText] string inputText)
        {
            await ctx.TriggerTypingAsync();

            IEnumerable<string> answers;
            try
            {
                // Api request
                answers = await textService.AskQuestion(inputText);
            }
            catch (TextService.OpenAIException ex)
            {
                await ctx.RespondAsync(ex.Message);
                return;
            }

            string formattedAnswer = string.Join("\n", answers);
            if (string.IsNullOrEmpty(formattedAnswer))
            {
                formattedAnswer = "*No answer found*";
            }

            // Send answer
            await ctx.RespondAsync(formattedAnswer);
        }
    }
}
