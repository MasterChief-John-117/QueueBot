using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (User user in users)
            {
                if (user.Name.ToLower().Contains(name.ToLower()) && user.Server == e.Message.Server)
                {
                    Console.WriteLine(user.Name);
                    count++;
                    people += user.Name + " : " + user.Id.ToString() + "\n";
                }
            }
            if (count >= 1)
            {
                return people;
            }
            else
            {
                return "I couldn't find anyone :(";
            }
        }

