using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZpNewsBot
{
    internal class Bot
    {
        TelegramBotClient botClient;
        CancellationTokenSource cancellationToken;
        public Bot()
        {
            botClient = new TelegramBotClient("5282483226:AAE_uS-e-qH21RroO3cFRH0nILggVkS2lb0");
            cancellationToken = new CancellationTokenSource();
        }

        async void sendMsg(string photourl, string title, string telegraphLink)
        {
            Message message = await botClient.SendPhotoAsync(
            chatId: -1001549217373,
            photo: $"{photourl}",
            caption: $"<a href=\"{telegraphLink}\">{title}</a>",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken.Token);
        }

        async void sendMsg(string title, string telegraphLink)
        {
            Message message = await botClient.SendPhotoAsync(
            chatId: -1001549217373,
            photo: null,
            caption: $"<a href=\"{telegraphLink}\">{title}</a>",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken.Token);
        }
    }
}
