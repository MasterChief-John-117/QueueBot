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
using Nito.AsyncEx;

namespace QueueBot
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

            //Dictionary <  Guid, LinkedList < User >> queues = new Dictionary < Guid, LinkedList< User >>();

            LinkedList<string> queue = new LinkedList<string>();

            discord.GetService<CommandService>().CreateCommand("qme")
                .Alias(new String[] {"add"})
                .Do(async (e) =>
                {
                    if (!queue.Contains(e.Message.User.Name))
                    {
                        queue.AddLast(e.Message.User.Name);
                        await e.Channel.SendMessage(e.Message.User.Name + " has been added to the queue!");
                    }
                    else await e.Channel.SendMessage("You're already in the queue!");
                });
            discord.GetService<CommandService>().CreateCommand("queue")
                .Alias(new String[] {"q"})
                .Do(async (e) =>
                {
                    string message = "";
                    foreach (String value in queue)
                    {
                        message += (value + ", ");
                    }
                    if (message == "") message = "No one! Add yourself with `=qme` if you want to go.";
                    await e.Message.Channel.SendMessage("Currently in the queue is: " + message);
                });
            discord.GetService<CommandService>().CreateCommand("next")
                .Alias(new String[] {"up"})
                .Do(async (e) =>
                {
                    string up = queue.First();
                    string next = "";
                    queue.RemoveFirst();
                    IEnumerable<User> users = e.Message.Client.Servers.SelectMany(s => s.Users).Where(u => u.Name == up);
                    foreach (User user in users)
                    {
                        next = "<@" + user.Id + ">";
                    }
                    if (queue.Any() == true)
                    {
                        await e.Message.Channel.SendMessage(
                            "No one is in the queue! Use `=qme` to get placed in the queue.");}
                    else await e.Message.Channel.SendMessage(next + " is up!");
                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjcwNDIzMDM2MzIxMDcxMTA0.C1_5uw.5syPhpLx6tELGT21EawMIzlyKe8", TokenType.Bot); //token outdated
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}