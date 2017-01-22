using System;
using Discord;
using Discord.Commands;
using System.Timers;
using System.Collections.Generic;


namespace QueueBot
{
    public class Antispam
    {
        public static Dictionary<string, int> useruse = new Dictionary<string, int>();


        public static void increment(CommandEventArgs e)
        {
            if (useruse.ContainsKey(e.Message.User.Id.ToString()))
            {
                useruse[e.Message.User.Id.ToString()]++;
            }
            else
            {
                useruse.Add(e.Message.User.Id.ToString(), 1);
            }
        }

        public static void decrement(Object source, ElapsedEventArgs e)
        {
            foreach (string used in useruse.Keys)
            {
                useruse[used]--;
                Console.WriteLine(used + " has " + useruse[used].ToString());
            }
        }

    }

}