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
                    if (Array.Exists(queue, element => element == e.Message.User.Name) == false)
                    {
                        queue = (queue ?? Enumerable.Empty<string>()).Concat(new[] { e.Message.User.Name }).ToArray();
                        await e.Channel.SendMessage(e.Message.User.Name + " has been added to the queue!");
                    }
                    else await e.Channel.SendMessage("You're already in the queue!");
                });
            commands.CreateCommand("queue")
                .Do(async (e) =>
                {
                    string message = "";
                    foreach (String value in queue)
                    {
                        message += (value + ", ");
                    }
                    await e.Message.Channel.SendMessage("Currently in the queue is: " + message);
                });
            commands.CreateCommand("next")
                .Do(async (e) =>
                {
                    string up = queue[0];
                    string next = "";
                    queue = queue.Where(w => w != queue[0]).ToArray();
                    IEnumerable<User> users = e.Message.Client.Servers.SelectMany(s => s.Users).Where(u => u.Name == up);
                    foreach (User user in users)
                    {
                        next = "<@" + user.Id + ">";
                    }

                    await e.Message.Channel.SendMessage(next + " is up!");
                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjcwNDIzMDM2MzIxMDcxMTA0.C16q5g.UsKTr1oY_alnmOBPTHlNsQkI3dM", TokenType.Bot);
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}