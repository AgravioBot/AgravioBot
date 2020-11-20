using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace AgravioBot.Services
{
    public class CommandHandler
    {
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly LogService _logService;

        public CommandHandler(IServiceProvider services)
        {
            // Populating fields...
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logService = services.GetRequiredService<LogService>();
            _services = services;

            // Hooks CommandExecuted && MessageReceived events
            _commands.CommandExecuted += CommandExecutedAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ensures we don't process system/other bot messages
            // AgravioBot only responds to true dwarfs
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = 0;

            // get prefix from the configuration file
            char prefix = Char.Parse(_config["prefix"]);

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                    return;
            }

            var context = new SocketCommandContext(_client, message);

            // execute command if one is found that matches
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services
                );

            // If the command fails, sends a message to the user
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync($"Failed to execute: {result.ErrorReason}");
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                _logService.Log(new LogMessage(LogSeverity.Error, "CommandExecutedAsync",$"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!"));
                return;
            }

            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                _logService.Log(new LogMessage(LogSeverity.Error, "CommandExecutedAsync", $"Command [{command.Value.Name}] executed for -> [{context.User.Username}]"));
                return;
            }
        }
    }
}