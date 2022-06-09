using Discord.OpenAI.Bot;
using Discord.OpenAI.Bot.Services;
using Discord.OpenAI.Bot.Settings;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.GPT3.Extensions;

Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()

    // Configure services
    .ConfigureServices((hostContext, services) =>
    {
        // Configure settings
        services.Configure<OpenAIAskParams>(hostContext.Configuration.GetSection("OpenAI:AskParams"));

        services.AddOpenAIService(options =>
        {
            options.ApiKey = hostContext.Configuration["OpenAI:ApiKey"]!;
            options.Organization = hostContext.Configuration["OpenAI:Organization"]!;
        })

        .AddSingleton(s =>
        {
            return new DiscordClient(new DiscordConfiguration
            {
                LoggerFactory = s.GetRequiredService<ILoggerFactory>(),
                Token = hostContext.Configuration["Discord:Token"],
                Intents = DiscordIntents.All
            });
        })
        .AddSingleton(s => s.GetRequiredService<DiscordClient>().UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = s
        }))
        .AddSingleton(s => s.GetRequiredService<DiscordClient>().UseCommandsNext(new CommandsNextConfiguration
        {
            Services = s,
            EnableDefaultHelp = false,
            StringPrefixes = new string[] { hostContext.Configuration.GetValue<string>("Discord:CommandPrefix") }
        }))

        // Add services
        .AddSingleton<TextService>()

        // Add hosted service
        .AddHostedService<BotService>();
    })
    .Build()
    .Run();