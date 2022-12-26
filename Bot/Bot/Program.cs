// See https://aka.ms/new-console-template for more information
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;



var botClient = new TelegramBotClient("5671813375:AAGnn_pVIclL3qI_dsuxd2hJZ7VVZpRSD7k");
int GameNumberInArray = 0;
string[] games = new string[1000];
string allGamesText = "";
using CancellationTokenSource cts = new();
string[,] playersId = new string[1000, 8];
long[] chatsId = new long[1000];
string[] memesLinks = new string[4] { "https://github.com/SEMI-SAINT-GAMES/MemologBotFiles/blob/main/kot-kashlyaet-mem.png?raw=true", "https://github.com/SEMI-SAINT-GAMES/MemologBotFiles/blob/main/f82f161d4f4190d531ec746cb999c089.jpg?raw=true", "https://github.com/SEMI-SAINT-GAMES/MemologBotFiles/blob/main/d1149a8df4940c6661236abda17395d8.jpg?raw=true", "https://github.com/SEMI-SAINT-GAMES/MemologBotFiles/blob/main/chmonya-andrij-ryazanczev-dnr_1.jpg?raw=true" };
string[] groupUsedMemes = new string[1000];
string[] quaestions = new string[4] { "Коли прийшов воювати у складі 2-ї арміі світу, а виявилось шо ти чмоня", "Коли ти котик", "Коли ти скала", "Прийде баке та зробить буккаке"};

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (message.Text == "Ты пидор")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: chatId.ToString(),
        cancellationToken: cancellationToken);
    }
    else if (message.Text == "г")
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Я пидор", "Ты пидор" },
})
        {
            ResizeKeyboard = true
        };

        Message sendMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose a response",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
    else if (message.Text == "Начать")
    {
        
                
                
                games[GameNumberInArray] = GameNumberInArray.ToString() + "-" + RndStr(5);
                string code = games[GameNumberInArray];
                allGamesText += "/" + code;

                Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Перейди за посиланням @MemologFunBot та напиши мені цей код",
                cancellationToken: cancellationToken);

                Message codeMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: code,
                cancellationToken: cancellationToken);
                return;
                
            
        
        
    }
   
    
    else if (allGamesText.Contains(message.Text))
    {
        Console.WriteLine("Contains");
        for (int i = 0; i < games.Length; i++)
        {
            if (games[i] == message.Text)
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,

            text: "Все добре друже, зараз пришлю тобі мемчіки",
            cancellationToken: cancellationToken);
                Message singUpMessage = await botClient.SendTextMessageAsync(
            chatId: chatsId[i],

            text: "+1 у грі",
            cancellationToken: cancellationToken);
                for (int k = 0; k < 8; k++)
                { 
                    if (playersId[i, k] == null)
                    {
                        playersId[i, k] = message.Chat.Id.ToString();
                        Console.WriteLine(playersId[i, k].ToString());
                        return;
                    }
                }
            }

        }
    }
    else if (message.Text == "Готовий")
    {
        
            Random rnd = new Random();
            int rand = rnd.Next(0, 4);
            Message sentMessage = await botClient.SendTextMessageAsync(
           chatId: chatId,

           text: quaestions[rand],
           cancellationToken: cancellationToken);
            for (int i = 0; i < GameNumberInArray; i++)
            {
                if (chatsId[i] == message.Chat.Id)
                {
                Console.Write("i =" + i.ToString());
                    for (int k = 0; k < 8; k++)
                    {
                        if (playersId[i, k] != null)
                        {
                            for (int j = 0; j < memesLinks.Length; j++)
                            {
                                Message meme = await botClient.SendPhotoAsync(
                               chatId: playersId[i, k],
                               photo: memesLinks[j],
                               parseMode: ParseMode.Html,
                               cancellationToken: cancellationToken);
                            }
                        }
                        else
                        { }
                    }
                }
            }
        
    }
    
    /*else
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "все хуйня, давай по новой",

            cancellationToken: cancellationToken);
    }*/

}
static string RndStr(int len)
{
    string s = "", symb = "abcdefghijklmnopqrstuvwxyz0123456789";
    Random rnd = new Random();
    for (int i = 0; i < len; i++)
      s += symb[rnd.Next(0, symb.Length)];
    return s;
    

}
/*static string GamesArrayToText();
{ 

    for (int i = 0; i < games.Length; i++)
        allGamesText += games[i];
    return allGamesText;

}*/

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

