using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AgravioBot.Services
{
    public class LogService
    {
        //Discord's log is agnostic, setting up their log with the console TODO: text file?
        public Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
