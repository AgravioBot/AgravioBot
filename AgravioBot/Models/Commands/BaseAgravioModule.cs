using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AgravioBot.Models.Commands
{
    public class BaseAgravioModule : ModuleBase
    {

        [Command("shaerox")]
        [Alias("wolin")]
        public Task ShaeroxAsync()
        {
            return ReplyAsync("furro");
        }

        [Command("greymane")]
        [Alias("cringris", "gilneas", "glenn")]
        public Task GreymaneAsync()
        {
            const string emoji = "🐺";
            return ReplyAsync($"SYLVAAAAAAAAAANNNAAAAAAAAAAAAAAAAAAAS {emoji}");
        }

    }
}
