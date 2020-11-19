using AgravioBot.Services;
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
    public class AudioModule : ModuleBase
    {
        private readonly AudioService _service;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannelAsync()
        {
            //// Get the audio channel
            //channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            ////if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            //var audioClient = channel.ConnectAsync().Result;

            //SendAsync(audioClient, "sylvanas.mp3");

            //return channel.DisconnectAsync();
            var voiceChannel = (Context.User as IGuildUser)?.VoiceChannel;
            await _service.JoinAudio(Context.Guild, voiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }

        [Command("sylvanas", RunMode = RunMode.Async)]
        public Task SylvanasAsync()
        {
            try
            {
                JoinChannelAsync().Wait();
                var cat = EmojiOne.EmojiOne.ShortnameToUnicode(":pouting_cat:");  // TODO TEST ON LINUX, DEPENDENCIES ARE .NET4.7 AND NOT CORE

                ReplyAsync($"SYLVAAAAAAAAAANAAAAAAAAAAAAAAAAAAAS {cat}");
                PlayCmd("./Resources/sylvanas.mp3").Wait();
            }
            finally
            {
                LeaveCmd();
            }
            return Task.CompletedTask;
        }


        #region testing audio service

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try
                {
                    await output.CopyToAsync(discord);
                }
                finally
                {
                    await discord.FlushAsync();
                }
            }
        }

        #endregion
    }
}