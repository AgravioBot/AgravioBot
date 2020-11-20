using AgravioBot.Services;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AgravioBot.Models.Commands
{
    public class AudioModule : ModuleBase
    {
        private readonly AudioService _service;
        private readonly IConfiguration _configuration;
        private readonly FileInfo _audioPath;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service, IConfiguration config)
        {
            _service = service;
            _configuration = config;

            _audioPath = new FileInfo(_configuration["audio_resources_path"]);
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannelAsync()
        {
            // Gets the voice channel where user is connected
            var voiceChannel = (Context.User as IGuildUser)?.VoiceChannel;

            if (voiceChannel == null)
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel");
                return;
            }

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
            if (!IsProperChannel())
                return Task.CompletedTask;

            try
            {
                JoinChannelAsync().Wait();

                const string emoji = "🐺";
                ReplyAsync($"SYLVAAAAAAAAAANNNAAAAAAAAAAAAAAAAAAAS {emoji}");

                PlayCmd($"{_audioPath}/sylvanas.mp3").Wait();
                //Id: 779413613517471754 = Concilio
            }
            finally
            {
                LeaveCmd();
            }
            return Task.CompletedTask;
        }

        [Command("leeroy", RunMode = RunMode.Async)]
        [Alias("jenkins")]
        public Task LeeroyAsync()
        {
            if (!IsProperChannel())
                return Task.CompletedTask;

            try
            {
                JoinChannelAsync().Wait();

                const string emoji = "🍗";
                ReplyAsync($"LEEEEEEEEEEEEEERRROOOOOOOOOOOY {emoji}");

                PlayCmd($"{_audioPath}/leeroy_jenkins.mp3").Wait();
            }
            finally
            {
                LeaveCmd();
            }
            return Task.CompletedTask;
        }

        private bool IsProperChannel()
        {
            const long SylvanasChannelId = 779413613517471754;
            if (Context.Channel.Id != SylvanasChannelId)
                return false;

            return true;
        }

        #region testing audio service

        //private Process CreateStream(string path)
        //{
        //    return Process.Start(new ProcessStartInfo
        //    {
        //        FileName = "ffmpeg",
        //        Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
        //        UseShellExecute = false,
        //        RedirectStandardOutput = true,
        //    });
        //}

        //private async Task SendAsync(IAudioClient client, string path)
        //{
        //    // Create FFmpeg using the previous example
        //    using (var ffmpeg = CreateStream(path))
        //    using (var output = ffmpeg.StandardOutput.BaseStream)
        //    using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
        //    {
        //        try
        //        {
        //            await output.CopyToAsync(discord);
        //        }
        //        finally
        //        {
        //            await discord.FlushAsync();
        //        }
        //    }
        //}

        #endregion
    }
}