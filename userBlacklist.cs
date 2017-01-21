using System;
using System.Timers;
using Discord;

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


    }
}