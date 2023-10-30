using Microsoft.EntityFrameworkCore;
using WinterFormalBot.Entites;
using User = Telegram.Bot.Types.User;

namespace WinterFormalBot;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Quest> Quests { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}