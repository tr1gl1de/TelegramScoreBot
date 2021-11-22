using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot
{
    public class Handlers
    {
        public static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
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

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!)
            };
            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }


        // private static ReplyKeyboardMarkup GetButtons()
        // {

        //     ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        //     {
        //         new KeyboardButton[] {"Добавить профиль ✅", "Удалить профиль ❎",
        //                                 "Показать профиль"},
        //     })
        //     {
        //         ResizeKeyboard = true
        //     };

        //     return replyKeyboardMarkup;
        // }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Received message type: {message.Type}");

            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                // "/addprofile" => AddProfile(botClient, message),
                // "/delprofile" => DelProfile(botClient, message),
                // "/showallprofiles" => ShowAllProfiles(botClient, message),
                "/keyboard" => SendReplyKeyboard(botClient, message),
                "/rkeyboard" => RemoveKeyboard(botClient, message),
                _ => Usage(botClient, message)
            };
            Message sentMessage = await action;
            Console.WriteLine($"The message wa sent with id: {sentMessage.MessageId}");
        }

        private static async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton[] {"Добавить профиль ✅", "Удалить профиль ❎"},
                    new KeyboardButton[] { "Показать все профили"}
                }
            )
            {
                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: replyKeyboardMarkup);
        }

        static async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message)
        {
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Removing keyboard",
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        private static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
        {
            const string usage = "Usage:\n"+
                                 "/addprofile - Добавить профиль\n" +
                                 "/delprofile - удалить профиль\n" +
                                 "/showallprofiles - показать все профили\n" +
                                 "/keyboard - Отправить свою клавиатуру\n" +
                                 "/rkeyboard - удалить клавиатуру\n";
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        // private static object ShowAllProfiles(ITelegramBotClient botClient, Message message)
        // {
        //     throw new NotImplementedException();
        // }

        // private static object DelProfile(ITelegramBotClient botClient, Message message)
        // {
        //     throw new NotImplementedException();
        // }

        // private static object AddProfile(ITelegramBotClient botClient, Message message)
        // {
        //     throw new NotImplementedException();
        // }
    }

}