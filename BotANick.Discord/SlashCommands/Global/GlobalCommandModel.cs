namespace BotANick.Discord.SlashCommands.Global;

public class GlobalCommandModel : ICommand
{
    public string Name { get; set; }

    public string Description { get; set; }

    public static ICommand Hug()
    {
        return new GlobalCommandModel
        {
            Name = "hug",
            Description = "BotANick will give you a hug! If he is in the mood ...",
        };
    }
}
