using System;
using System.Timers;
using Discord;

namespace QueueBot

{
    public class userBlacklist
    {
        public static string[] bringIn()
        {
            return System.IO.File.ReadAllLines(@"C:\Users\maste\Documents\GitHub\Queuebot\blacklist.txt");
        }

        public static void sendOut(Object source, ElapsedEventArgs e)
        {
            string[] list = MyBot.blacklist.ToArray();
            System.IO.File.WriteAllLines(@"C:\Users\maste\Documents\GitHub\Queuebot\blacklist.txt", list);
            Console.WriteLine(DateTime.Now + " Blacklist updated with " + MyBot.blacklist.Count + " users");
        }


    }
}