//MyBot.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading;

namespace DiscoBot
{

    class MyBot
    {
        DiscordClient discord;
        CommandService commands;

        public MyBot()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = false;
            });

            commands = discord.GetService<CommandService>();

            commands.CreateCommand("hello")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Hello.");
                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjcwNDIzMDM2MzIxMDcxMTA0.C13qlw.r6EucqYbLrN6KpkI7ulK3KoUoQo", TokenType.Bot);
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}