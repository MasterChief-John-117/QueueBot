using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;


namespace QueueBot
{

    class MyBot
    {
        Dictionary<string, LinkedList<string>> queues = new Dictionary<string, LinkedList<string>>();
        DiscordClient discord;
        CommandService commands;
        public CommandEventArgs c;
        public LinkedList<string> usingq;


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
                x.HelpMode = HelpMode.Public;

            });

            commands = discord.GetService<CommandService>();




            //queue commands

            //Dictionary <  Guid, LinkedList < User >> queues = new Dictionary < Guid, LinkedList< User >>();

            LinkedList<string> queue = new LinkedList<string>();

            discord.GetService<CommandService>().CreateCommand("qme")

                .Alias(new String[] {"add"})
                .Do(async (e) =>
                {
                    setOrGetQueue(e);
                    if (!usingq.Contains(e.Message.User.Name))
                    {
                        usingq.AddLast(e.Message.User.Name);
                        await e.Channel.SendMessage(e.Message.User.Name + " has been added to the queue!");
                    }
                    else await e.Channel.SendMessage("You're already in the queue!");
                });
            discord.GetService<CommandService>().CreateCommand("queue")
                .Alias(new String[] {"q"})
                .Do(async (e) =>
                {
                    string message = "";
                    setOrGetQueue(e);
                    foreach (String value in usingq)
                    {
                        message += (value);
                    }
                    if (message == "") message = "No one! Add yourself with `=qme` if you want to go.";
                    await e.Message.Channel.SendMessage("Currently in the queue is: " + message);
                });
            discord.GetService<CommandService>().CreateCommand("next")
                .Alias(new String[] {"up"})
                .Do(async (e) =>
                {
                    setOrGetQueue(e);

                    if (usingq.Count() == 0)
                    {
                        await e.Message.Channel.SendMessage(
                            "No one is in the queue! Use `=qme` to get placed in the queue.");
                    }
                    else if (usingq.Count() != 0)
                    {
                        string up = usingq.First();
                        string next = "";
                        IEnumerable<User> users = e.Message.Client.Servers.SelectMany(s => s.Users).Where(u => u.Name == up);
                        foreach (User user in users)
                        {
                            next = "<@" + user.Id + ">";
                        }
                        await e.Message.Channel.SendMessage(next + " is up!");
                        usingq.RemoveFirst();

                    }
                });
            discord.GetService<CommandService>().CreateCommand("leave")
                .Alias(new String[] {"dqme", "leave"})
                .Do(async (e) =>
                {
                    setOrGetQueue(e);
                    if (usingq.Contains(e.Message.User.Name))
                    {
                        usingq.Remove(e.Message.User.Name);
                        await e.Message.Channel.SendMessage(e.Message.User.Name + " has left the queue");
                    }
                    else await e.Message.Channel.SendMessage("You weren't in the queue!");

                });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("", TokenType.Bot); //token outdated
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void setOrGetQueue(CommandEventArgs e)
        {
            if (queues.ContainsKey(e.Message.Server.ToString()))
            {
                usingq = queues[e.Message.Server.ToString()];
            }
            else
            {
                LinkedList<string> queue = new LinkedList<string>();
                queues.Add(e.Message.Server.ToString(), queue);
                usingq = queues[e.Message.Server.ToString()];
            }
        }

    }
}