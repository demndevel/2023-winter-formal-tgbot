using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using WinterFormalBot;

const string token = "6634114187:AAGSz8h1pA8jssmgGMl4h54xY5VLYQRd-04";

using var cancellationTokenSource = new CancellationTokenSource();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString: "Data Source=WinterFormalBot.db"));
builder.Services.AddHangfire(configuration => configuration.UseSQLiteStorage("Data Source=WinterFormalBotHangfire.db"));

using var host = builder.Build();

var botClient = new TelegramBotClient(token);
var botRouter = new BotRouter(host.Services);
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: botRouter.HandleUpdate,
    pollingErrorHandler: botRouter.HandlePollingError,
    receiverOptions: receiverOptions,
    cancellationToken: cancellationTokenSource.Token);

await host.RunAsync();

Console.ReadLine();

cancellationTokenSource.Cancel();
