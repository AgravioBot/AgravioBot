using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AgravioBot.Models.Commands
{
    public class BaseAgravioModule : ModuleBase
    {

        [Command("wolin")]
        public Task WolinAsync()
        {
            return ReplyAsync("furro");
        }

        [Command("gilneas")]
        public Task GilneasAsync()
        {
            return GreymaneAsync();
        }

        [Command("cringris")]
        public Task CringrisAsync()
        {
            return GreymaneAsync();
        }

        [Command("greymane")]
        public Task GreymaneAsync()
        {
            var cat = EmojiOne.EmojiOne.ShortnameToUnicode(":pouting_cat:");

            return ReplyAsync($"SYLVANAAAAAAAAAAAAAAAAAAAAAAAS {cat}");
        }
    }
}
