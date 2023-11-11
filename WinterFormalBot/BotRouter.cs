using System.Text;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WinterFormalBot;

public class BotRouter
{
    private readonly AppDbContext _db;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITelegramBotClient _botClient;

    public BotRouter(IServiceProvider serviceProvider, ITelegramBotClient botClient)
    {
        _serviceProvider = serviceProvider;
        _botClient = botClient;
        _db = serviceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task HandleUpdate(
        ITelegramBotClient _,
        Update update,
        CancellationToken ct)
    {
        await _db.Database.EnsureCreatedAsync(ct);

        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleTextMessages(update, ct);
                break;
        }
    }

    private async Task HandleTextMessages(Update update, CancellationToken ct)
    {
        switch (update.Message!.Text)
        {
            case "/help":
                await HandleHelpCommand(update, ct);
                break;
        }
    }
    
    private async Task HandleHelpCommand(Update update, CancellationToken ct)
    {
        await _botClient.SendTextMessageAsync(update.Message!.Chat.Id, "Помощь по боту", cancellationToken: ct);
    }

    public Task HandlePollingError(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}