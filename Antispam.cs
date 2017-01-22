using System;
using Discord;
using Discord.Commands;
using System.Timers;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace QueueBot
{
    public class Antispam
    {
        public static Dictionary<string, int> useruse = new Dictionary<string, int>();
        public static Dictionary<string, int> muchspam = new Dictionary<string, int>();


        public static void increment(CommandEventArgs e)
        {
            if (useruse.ContainsKey(e.Message.User.Id.ToString()))
            {
                useruse[e.Message.User.Id.ToString()]++;
                if (useruse[e.Message.User.Id.ToString()] > 3) hasspammed(e);
            }
            else
            {
                useruse.Add(e.Message.User.Id.ToString(), 1);
            }
        }

        public static void decrement(Object source, ElapsedEventArgs e)
        {
            foreach (var used in useruse.Keys)
            {
                if (useruse[used] > 0)
                {
                    useruse[used]--;
                }
            }
        }

        public static async void hasspammed(CommandEventArgs e)
        {
            if (!Ids.whitelist.Contains(e.Message.User.Id.ToString()))
            {


                if (muchspam.ContainsKey(e.Message.User.Id.ToString()))
                {
                    muchspam[e.Message.User.Id.ToString()]++;
                    if (muchspam[e.Message.User.Id.ToString()] == 2)
                        await e.Message.User.SendMessage(
                            "This message has been sent to alert you that our spam detection has picked you up. This is warning number `2`. \nNext time our system flags you, you will be automatically blacklisted as spam protection.");
                    if (muchspam[e.Message.User.Id.ToString()] == 3)
                    {
                        MyBot.blacklist.Add(e.Message.User.Id.ToString());
                        Console.WriteLine(e.Message.User.Name + " on server " + e.Message.Server.Name + " (owner ID: " + e.Message.Server.Owner.Id.ToString() + ") has been blacklisted for spam");
                        await e.Message.User.SendMessage(
                            "We regret to inform you that you have been blacklisted as spam protection. If you feel this was in error, please contact your server moderators and @MasterChief_John-117#1911");
                    }
                }

                else
                {
                    muchspam.Add(e.Message.User.Id.ToString(), 1);
                    await e.Message.User.SendMessage(
                        "This message has been sent to alert you that our spam detection has picked you up. This is warning number `1`. \nYou will recieve one more warning, then be automatically blacklisted as spam protection.");
                }
            }
        }
    }
}