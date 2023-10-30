namespace WinterFormalBot.Entites;

public class Quest
{
    public long QuestId { get; set; }
    public List<User> CurrentUsers { get; set; } = new();
    public int QuestPoints { get; set; }
}