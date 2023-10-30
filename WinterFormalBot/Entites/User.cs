namespace WinterFormalBot.Entites;

public class User
{
    public long Id { get; set; }
    public int TotalPoints { get; set; }
    public Quest? CurrentQuest { get; set; } = null;
}