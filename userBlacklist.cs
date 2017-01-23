using System;
using System.Timers;
using Discord;
using Discord.Commands;

namespace QueueBot

{
    public class userBlacklist
    {
        public static string[] bringIn()
        {
            string[] list =  System.IO.File.ReadAllLines(@"C:\Users\maste\Documents\GitHub\Queuebot\blacklist.txt");
            foreach (string str in list)
            {
                MyBot.blackused.Clear();
                MyBot.blackused.Add(str, 0);
            }
            Console.WriteLine(DateTime.Now + " Blacklist introduced with " + list.Length + " users");

            return System.IO.File.ReadAllLines(@"C:\Users\maste\Documents\GitHub\Queuebot\blacklist.txt");
        }

        public static void sendOut(Object source, ElapsedEventArgs e)
        {
            string[] list = MyBot.blacklist.ToArray();
            foreach (string str in list)
            {
                if (!MyBot.blacklist.Contains(str)) MyBot.blackused.Add(str, 0);
            }
            System.IO.File.WriteAllLines(@"C:\Users\maste\Documents\GitHub\Queuebot\blacklist.txt", list);
            Console.WriteLine(DateTime.Now + " Blacklist updated with " + MyBot.blacklist.Count + " users");
        }


        public static async void commandUsed(CommandEventArgs e)
        {
            if (!MyBot.blackused.ContainsKey(e.Message.User.Id.ToString())) MyBot.blackused.Add(e.Message.User.Id.ToString(), 0);
            MyBot.blackused[e.Message.User.Id.ToString()]++;
            e.Message.Delete();
            if (MyBot.blackused[e.Message.User.Id.ToString()] < 3)
            {
                await e.Message.User.SendMessage("You've been blacklisted! You've tried `" +
                                                 MyBot.blackused[e.Message.User.Id.ToString()] +
                                                 "` times. If you try `3` times, mods will be alterted");
            }
            if (MyBot.blackused[e.Message.User.Id.ToString()] == 3)
            {
                await e.Message.User.SendMessage(
                    "You have attemted to use a command `3` times. As such, moderators on this server will be alterted to bot abuse");
                Console.WriteLine(e.Message.User.Name +
                                  " has used a command `3` times post blacklist on server " +
                                  e.Message.Server.Name);
            }
        }



    }
}