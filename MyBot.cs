using System;
using Discord;
using Discord.Commands;


namespace QueueBot
{

    class MyBot
    {

        DiscordClient discord;
        CommandService commands;
        private static CommandEventArgs e;

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


            LinkedList<string> queue = new LinkedList<string>();



            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(Token.token, TokenType.Bot); //takes token from Settings.cs
            });
        }


        //Conole logging
        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ": " + e.Message);
        }
    }
}