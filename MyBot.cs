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
                x.PrefixChar = '=';
                x.AllowMentionPrefix = false;
            });

            commands = discord.GetService<CommandService>();

            //queue commands

            string[] queue = { };
            commands.CreateCommand("qme")
                .Do(async (e) =>
                {
                    if (e.Message.User.Roles.Select(user => user.Name).Intersect(queue).Any() == false)
                    {
                        queue.Concat(Enumerable.Repeat(e.Message.User.Name,1)).ToArray();
                    }
                    await e.Channel.SendMessage(e.Message.User.Name + " has been added to the queue");
                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjcwNDIzMDM2MzIxMDcxMTA0.C151Zg.xFJ3sZLXgAa8pC6PPuHH2WBH4yk", TokenType.Bot);
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}