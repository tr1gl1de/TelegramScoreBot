using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot
{
    class Program
    {
        private static string token = "2117402229:AAEqvYd5QkSy4No75Te5Z1Snr7HXIlgmE00";
        private static TelegramBotClient client;

        static async Task Main(string[] args)
        {
            client = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            client.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            switch (messageText)
            {
                case "Добавить профиль ✅":
                    Message addProfile = await client.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Вы добавили профиль",
                        replyMarkup: GetButtons(),
                        cancellationToken: cancellationToken);
                    break;
                case "Добавить профиль":
                    Message addProfile2 = await client.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Вы добавили профиль",
                        replyMarkup: GetButtons(),
                        cancellationToken: cancellationToken);
                    break;
                case "Удалить профиль ❎":
                    Message delProfile = await client.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Вы удалили профиль",
                        replyMarkup: GetButtons(),
                        cancellationToken: cancellationToken);
                    break;
                case "Удалить профиль":
                    Message delProfile2 = await client.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Вы удалили профиль",
                        replyMarkup: GetButtons(),
                        cancellationToken: cancellationToken);
                    break;
            }
        }

        private static ReplyKeyboardMarkup GetButtons()
        {

            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Добавить профиль ✅", "Удалить профиль ❎"},
            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

        private static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
