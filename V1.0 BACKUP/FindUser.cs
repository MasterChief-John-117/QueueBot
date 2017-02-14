using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;

namespace QueueBot
{
    public class FindUser
    {
        public static string onServer(string name, CommandEventArgs e)
        {
            IEnumerable<User> users = e.Message.Client.Servers.SelectMany(s => s.Users);
            int count = 0;
            string people = "";
            Regex rgx = new Regex(name);
            LinkedList<ulong> ids = new LinkedList<ulong>();
            foreach (User user in users)
            {
                if(rgx.IsMatch(user.Name.ToLower()) && !ids.Contains(user.Id) && user.Server == e.Message.Server)
                {
                    Console.WriteLine(user.Name);
                    count++;
                    people += user.Name + " : " + user.Id.ToString() + "\n";
                    ids.AddFirst(user.Id);
                }
            }
            if (count > 25)
            {
                return $"I found `{count}` users! Please use more strict parameters.. :sweat_smile: ";
            }
            else if (count >= 1 && count <= 25)
            {
                return people;
            }
            else
            {
                return "I couldn't find anyone :(";
            }
        }

        public static string global(string name, CommandEventArgs e)
        {
            IEnumerable<User> users = e.Message.Client.Servers.SelectMany(s => s.Users);
            int count = 0;
            string people = "";
            Regex rgx = new Regex(name);
            LinkedList<ulong> ids = new LinkedList<ulong>();
            foreach (User user in users)
            {
                if(rgx.IsMatch(user.Name.ToLower()) && !ids.Contains(user.Id))
                {
                    Console.WriteLine(user.Name);
                    count++;
                    people += user.Name + " : " + user.Id.ToString() + "\n";
                    ids.AddFirst(user.Id);
                }
            }
            if (count > 25)
            {
                return $"I found `{count}` users! Please use more strict parameters.. :sweat_smile: ";
            }
            else if (count >= 1 && count <= 25)
            {
                return people;
            }
            else
            {
                return "I couldn't find anyone :(";
            }
        }
    }
}