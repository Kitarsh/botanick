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

    public static ICommand Noraj()
    {
        return new GlobalCommandModel
        {
            Name = "noraj",
            Description = "Add reaction 'NORAJ' on the last message.",
        };
    }

    public static ICommand GiveUp()
    {
        return new GlobalCommandModel
        {
            Name = "giveup",
            Description = "No ! Don't do that !",
        };
    }

    public static ICommand Indelivrables()
    {
        return new GlobalCommandModel
        {
            Name = "les-indelivrables",
            Description = "La meilleure chaîne Booktube !",
        };
    }
}
