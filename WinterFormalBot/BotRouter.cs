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

    public BotRouter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _db = serviceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task HandleUpdate(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken ct)
    {
        await _db.Database.EnsureCreatedAsync(ct);

        await botClient.SendTextMessageAsync(
            update.Message!.Chat.Id,
            "Text message",
            cancellationToken: ct);
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