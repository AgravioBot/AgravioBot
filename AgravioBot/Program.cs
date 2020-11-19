using AgravioBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgravioBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            /* FIY
             * WARNING
             *  If your application throws any exceptions within an async context, they will be thrown all the way back up to the first non-async method; since our first non-async method is the program's Main method, this means that all unhandled exceptions will be thrown up there, which will crash your application.
             *  Discord.Net will prevent exceptions in event handlers from crashing your program, but any exceptions in your async main will cause the application to crash.
            */
            var services = ConfigureServices();
            new AgravioBot(services).MainAsync().GetAwaiter().GetResult();
        }

        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private static IServiceProvider ConfigureServices()
        {
            var config = BuildConfigFile();

            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            return new ServiceCollection()
                .AddSingleton(config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<AudioService>()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Create the configuration object from the config.json file
        /// </summary>
        /// <returns></returns>
        public static IConfiguration BuildConfigFile()
        {
            var _builder = new ConfigurationBuilder()
                //.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
             return _builder.Build();
        }
    }
}
