using Discord.OpenAI.Bot.Commands;
using Discord.OpenAI.Bot.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.OpenAI.Bot
{
    public class BotService : IHostedService
    {
        private readonly ILogger<BotService> logger;
        private readonly IServiceProvider services;
        private readonly DiscordClient discordClient;
        private readonly SlashCommandsExtension slash;
        private readonly CommandsNextExtension cnext;

        public static bool IsReady { get; private set; } = false;

        #region Init
        public BotService(ILogger<BotService> logger, IServiceProvider services, DiscordClient discordClient, SlashCommandsExtension slash, CommandsNextExtension cnext)
        {
            this.logger = logger;
            this.services = services;
            this.discordClient = discordClient;
            this.slash = slash;
            this.cnext = cnext;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            ConfigureOpenAI();
            ConfigureCommands();
            ConfigureSlashCommands();
            discordClient.GuildDownloadCompleted += DiscordClient_GuildDownloadCompleted;
            await discordClient.ConnectAsync(new DiscordActivity("Replying to questions", ActivityType.Playing), UserStatus.Online).ConfigureAwait(false);
            await discordClient.InitializeAsync().ConfigureAwait(false);
        }

        private Task DiscordClient_GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            IsReady = true;
            logger.LogInformation("Bot started.");

            _ = Task.Run(InitServices);

            return Task.CompletedTask;
        }

        private void ConfigureOpenAI()
        {
            IOpenAIService openAiService = services.GetRequiredService<IOpenAIService>();
            openAiService.SetDefaultEngineId(Engines.Ada);
        }

        private void ConfigureCommands()
        {
            cnext.RegisterCommands<TextCommands>();

            cnext.CommandErrored += (s, e) =>
            {
                logger.LogError(e.Exception, "Command has thrown an exception: {command}", e.Context.Command?.QualifiedName);
                return Task.CompletedTask;
            };
        }

        private void ConfigureSlashCommands()
        {
            slash.RegisterCommands<TextSlashCommands>();   // Global

            slash.SlashCommandErrored += (s, e) =>
            {
                logger.LogError(e.Exception, "SlashCommand has thrown an exception: {command}", e.Context.CommandName);
                return Task.CompletedTask;
            };
        }

        private void InitServices()
        {
            services.GetRequiredService<TextService>();
        }
        #endregion

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            IsReady = false;
            await discordClient.DisconnectAsync().ConfigureAwait(false);
        }

        public virtual void Dispose()
        {
            discordClient.Dispose();
        }
    }
}
