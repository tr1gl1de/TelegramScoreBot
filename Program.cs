using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TgBot
{
    class Program
    {
        private static TelegramBotClient client;

        // public static Dictionary<int, Profile> profiles = new Dictionary<int, Profile>();
        
        static async Task Main(string[] args)
        {
            client = new TelegramBotClient(Configuration.BotToken);
            using var cts = new CancellationTokenSource();



            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            client.StartReceiving(
                Handlers.HandleUpdateAsync,
                Handlers.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

    }
}
