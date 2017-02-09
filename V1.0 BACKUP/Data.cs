using System;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Visibility;
using WebSocket4Net;

namespace QueueBot
{
    public class UserData
    {
        public static ulong userId;
        public static string userName;
        public static bool isBlacklisted;

        public UserData(MessageEventArgs m)
        {
            userId = m.Message.User.Id;
            userName = String.IsNullOrEmpty(m.Message.User.Nickname) ? m.Message.User.Name : m.Message.User.Nickname;

            isBlacklisted = MyBot.blacklist.Contains(userId.ToString());
        }
        public UserData(UserUpdatedEventArgs m)
        {
            userId = m.After.Id;
            userName = String.IsNullOrEmpty(m.After.Nickname) ? m.After.Name : m.After.Nickname;

            isBlacklisted = MyBot.blacklist.Contains(userId.ToString());
        }

        public ulong uId()
        {
            return userId;
        }

        public string uName()
        {
            return userName;
        }

        public bool uisBlackListed()
        {
            return isBlacklisted;
        }

    }
}