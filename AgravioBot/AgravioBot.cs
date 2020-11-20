using System;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using AgravioBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AgravioBot
{
    // DOCS: https://discord.foxbot.me/docs/guides/getting_started/first-bot.html
    // https://discord.foxbot.me/docs/api/index.html
    // https://discord.com/developers/docs/intro
    // DOCS WARCRAFTLOGS: https://www.warcraftlogs.com/api/docs
    //
    // ICON: https://www.flaticon.com/
    // ALTERNATIVE ICON: https://www.flaticon.com/free-icon/dwarf_3408545?term=dwarf&page=1&position=16

    public class AgravioBot
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private CommandHandler _commandHandler;
        private LogService _logService;
        private static IServiceProvider _services;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services"></param>
        public AgravioBot(IServiceProvider services)
        {
            // Getting services from DI
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();
            _commandHandler = services.GetRequiredService<CommandHandler>();
            _logService = services.GetRequiredService<LogService>();

            //Hooks log & ready events
            _client.Log += _logService.Log;
            _client.Ready += ReadyAsync;
        }

        public async Task MainAsync()
        {
            using (_client)
            {
                //This is where we get the Token value from the configuration file
                await _client.LoginAsync(TokenType.Bot, _config["token"]);
                await _client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await _services.GetRequiredService<CommandHandler>().InitializeAsync();

                // Block this task until it is closed, this way the bot stays online.
                await Task.Delay(-1);
            }
        }

        //When bot is ready
        private Task ReadyAsync()
        {
            _logService.Log(new LogMessage(LogSeverity.Verbose, "ReadyAsync", "Hold your grudges"));
            return Task.CompletedTask;
        }
    }
}
