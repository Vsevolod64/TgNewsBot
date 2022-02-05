using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZpNewsBot
{
    internal class Bot
    {
        public async void init()
        {
            
        }
        public async void SendNews(string news, string image, string title, bool init)
        {
            if (init)
            {
                var botClient = new TelegramBotClient("5282483226:AAE_uS-e-qH21RroO3cFRH0nILggVkS2lb0");

                using var cts = new CancellationTokenSource();

                // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { } // receive all update types
                };
                botClient.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken: cts.Token);

                var me = await botClient.GetMeAsync();

                Console.WriteLine($"Start listening for @{me.Username}");
                Console.ReadLine();

                // Send cancellation request to stop bot
                cts.Cancel();
            }
           

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                Message message = await botClient.SendPhotoAsync(
                    chatId: -1001549217373,
                    photo: image,
                    caption: "<a href=\""+news+"\">"+title+"</a>",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
             }

            Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
}
