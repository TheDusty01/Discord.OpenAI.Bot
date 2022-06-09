using Discord.OpenAI.Bot.Services;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Discord.OpenAI.Bot.Commands
{

    public class TextSlashCommands : ApplicationCommandModule
    {
        private readonly TextService textService;

        public TextSlashCommands(TextService textService)
        {
            this.textService = textService;
        }

        [SlashCommand("ask", "Ask a question")]
        [SlashRequireGuild]
        public async Task AskCommand(InteractionContext ctx,
            [Option("question", "Your input text/question")] string inputText)
        {
            await ctx.DeferAsync(false);

            IEnumerable<string> answers;
            try
            {
                // Api request
                answers = await textService.AskQuestion(inputText);
            }
            catch (TextService.OpenAIException ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(ex.Message));
                return;
            }

            string formattedAnswer = string.Join("\n", answers);
            if (string.IsNullOrEmpty(formattedAnswer))
            {
                formattedAnswer = "*No answer found*";
            }

            // Send answer
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(formattedAnswer));
        }
    }
}
