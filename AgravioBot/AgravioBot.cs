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
        private static IServiceProvider _services;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services"></param>
        public AgravioBot(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();
            _commandHandler = services.GetRequiredService<CommandHandler>();
            //_client = new DiscordSocketClient();

            //Hook into log event and write it out to the console
            _client.Log += Log;

            //Hook into the client ready event
            _client.Ready += ReadyAsync;

            //Hook into the message received event, this is how we handle the hello world example
            //_client.MessageReceived += MessageReceivedAsync;


        }

        public async Task MainAsync()
        {
            //This is where we get the Token value from the configuration file
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
            await _services.GetRequiredService<CommandHandler>().InitializeAsync();

            // Block this task until it is closed, this way the bot stays online.
            await Task.Delay(-1);
        }

        //Discord's log is agnostic, setting up their log with the console TODO: text file?
        private Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        //When bot is ready
        private Task ReadyAsync()
        {
            //Console.WriteLine($"Connected as -> [] :)");
            Log(new LogMessage(LogSeverity.Verbose, "ReadyAsync", "Hold your grudges"));
            return Task.CompletedTask;
        }

        ////Called when a user sends a message on the discord server
        //private async Task MessageReceivedAsync(SocketMessage message)
        //{
        //    //AgravioBot only responds to true dwarfs
        //    if(message.Author.IsBot)
        //        return;

        //    if (message.Content == "$hello") // TODO USE CommandService https://discord.foxbot.me/docs/guides/commands/intro.html
        //    {
        //        await message.Channel.SendMessageAsync("world!");
        //    }
        //}
    }
}


/*
 * Your client was successfully created. The client ID is 9209b6aa-4156-418f-a3b8-7acee5f7feee and the client secret is LtiNPsn6xStXD9zWmERFIQqa8919jlyUmDm3N0FB. This secret will never be shown again, so make sure to copy it now. If you lose the secret, you will have to delete this client and make a new one.
 * */