namespace BotANick.Discord.SlashCommands.Shared;

public class SubCommand : ICommand
{
    public string Name { get; set; }

    public string Description { get; set; }

    public ApplicationCommandOptionType CommandOptionType
    {
        get
        {
            return ApplicationCommandOptionType.SubCommand;
        }
    }
}
