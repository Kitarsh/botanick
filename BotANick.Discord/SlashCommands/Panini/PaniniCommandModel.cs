namespace BotANick.Discord.SlashCommands.Panini;

public class PaniniCommandModel : ICommand
{
    public string Name { get; set; }

    public string Description { get; set; }

    public static ICommand PaniniServerPub()
    {
        return new PaniniCommandModel
        {
            Name = "panini",
            Description = "Link to Panini server.",
        };
    }
}
