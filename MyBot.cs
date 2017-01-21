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
        public static Dictionary<string, int> blackused = new Dictionary<string, int>();
        DiscordClient discord;
        CommandService commands;
        ModuleManager manager;
        public CommandEventArgs c;
        public LinkedList<string> usingq;
        public static List<string> blacklist = userBlacklist.bringIn().ToList<string>();
        public System.Timers.Timer blacktimer;


        public MyBot()
        {

            blacktimer = new System.Timers.Timer(60 * 1000);
            blacktimer.Elapsed += userBlacklist.sendOut;
            blacktimer.AutoReset = true;
            blacktimer.Enabled = true;




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
                        blackused[e.Message.User.Id.ToString()]++;
                        await e.Message.Delete();
                        await e.Message.User.SendMessage("You've been blacklisted! This is your `" +
                                                   blackused[e.Message.User.Id.ToString()] +
                                                   "` time. If you try `3` times, mods will be alterted");
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
                            message += (value + ", ");
                        }
                        if (message == "") message = "No one! Add yourself with `=qme` if you want to go.";
                        await e.Message.Channel.SendMessage("Currently in the queue is: " + message);
                    }
                    else
                    {
                        blackused[e.Message.User.Id.ToString()]++;
                        await e.Message.Delete();
                        await e.Message.User.SendMessage("You've been blacklisted! This is your `" +
                                                         blackused[e.Message.User.Id.ToString()] +
                                                         "` time. If you try `3` times, mods will be alterted");                    }
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
                        blackused[e.Message.User.Id.ToString()]++;
                        await e.Message.Delete();
                        await e.Message.User.SendMessage("You've been blacklisted! This is your `" +
                                                         blackused[e.Message.User.Id.ToString()] +
                                                         "` time. If you try `3` times, mods will be alterted");                    }

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
                        blackused[e.Message.User.Id.ToString()]++;
                        await e.Message.Delete();
                        await e.Message.User.SendMessage("You've been blacklisted! This is your `" +
                                                         blackused[e.Message.User.Id.ToString()] +
                                                         "` time. If you try `3` times, mods will be alterted");                    }

                });


            //ADMIN COMMANDS
            discord.GetService<CommandService>().CreateCommand("blacklistUser")
                .Parameter("userId", ParameterType.Required)
                .Description("Moderator only")
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
                        await e.Message.User.SendMessage("You don't have permssions to use that command! (`blacklistUser`)");
                    }

                });


            //MY COMMANDS

            discord.GetService<CommandService>()
                .CreateCommand("listall")
                .Do(async (e) =>
                {
                    if (e.Message.User.Id.ToString() == "169918990313848832")
                    {
                        string message = "";
                        foreach (KeyValuePair<string, LinkedList<string>> kvp in queues)
                        {
                            message = message + kvp.Key + ": " + kvp.Value.Count() + " member(s) in queue \n";
                        }
                        await e.Message.Channel.SendMessage(message);
                    }
                    else
                    {
                        await e.Message.Delete();
                        await e.Message.User.SendMessage("You don't have permission to use the command `allqs`");
                    }
                });
                



           discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjcwNDIzMDM2MzIxMDcxMTA0.C2PmRQ.GFYDBrR3DG1nY56hPvO9pBcNaao", TokenType.Bot); //token outdated
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