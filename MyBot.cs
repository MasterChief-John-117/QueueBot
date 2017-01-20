using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.Modules;



namespace QueueBot
{

    class MyBot
    {
        Dictionary<string, LinkedList<string>> queues = new Dictionary<string, LinkedList<string>>();
        DiscordClient discord;
        CommandService commands;
        ModuleManager manager;
        public CommandEventArgs c;
        public LinkedList<string> usingq;
        public List<string> blacklist = new List<string>();


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
                    if (!blacklist.Contains(e.Message.User.Id.ToString()))
                    {
                        setOrGetQueue(e);
                        if (!usingq.Contains(e.Message.User.Name))
                        {
                            usingq.AddLast(e.Message.User.Name);
                            await e.Channel.SendMessage(e.Message.User.Name + " has been added to the queue!");
                        }
                        else await e.Channel.SendMessage("You're already in the queue!");
                    }
                    else
                    {
                        await e.Message.Delete();
                    }
                });
            discord.GetService<CommandService>().CreateCommand("queue")
                .Alias(new String[] {"q"})
                .Do(async (e) => {
                    if (!blacklist.Contains(e.Message.User.Id.ToString()))
                    {
                        string message = "";
                        setOrGetQueue(e);
                        foreach (String value in usingq)
                        {
                            message += (value);
                        }
                        if (message == "") message = "No one! Add yourself with `=qme` if you want to go.";
                        await e.Message.Channel.SendMessage("Currently in the queue is: " + message);
                    }
                    else
                    {
                        await e.Message.Delete();
                    }
            });
            discord.GetService<CommandService>().CreateCommand("next")
                .Alias(new String[] {"up"})
                .Do(async (e) =>
                {
                    setOrGetQueue(e);
                    if (!blacklist.Contains(e.Message.User.Id.ToString()))
                    {
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

                        }}
                    else
                    {
                        await e.Message.Delete();
                    }

                });
            discord.GetService<CommandService>().CreateCommand("leave")
                .Alias(new String[] {"dqme"})
                .Do(async (e) =>
                {
                    setOrGetQueue(e);
                    if (!blacklist.Contains(e.Message.User.Id.ToString()))
                    {
                        if (usingq.Contains(e.Message.User.Name))
                        {
                            usingq.Remove(e.Message.User.Name);
                            await e.Message.Channel.SendMessage(e.Message.User.Name + " has left the queue");
                        }
                        else await e.Message.Channel.SendMessage("You weren't in the queue!");
                    }
                    else
                    {
                        await e.Message.Delete();
                    }

                });


            //ADMIN COMMANDS
            discord.GetService<CommandService>().CreateCommand("blacklistUser")
                .Parameter("userId", ParameterType.Required)
                .Do(async (e) =>
                {

                    if (e.Message.User.ServerPermissions.BanMembers)
                    {
                        blacklist.Add(e.Message.Text.Substring(15));
                        await e.Message.Channel.SendMessage("Requested user has been added to *The Blacklist* ó_ò \nIf you want them un-blacklisted, please contact @MasterChief_John-117#1911");
                    }
                    else
                    {
                        await e.Message.Delete();
                        await e.Message.Channel.SendMessage("You don't have permssions to use that command!");
                    }

                });
                



           discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("", TokenType.Bot); //token outdated
            });
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ": " + e.Message);
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