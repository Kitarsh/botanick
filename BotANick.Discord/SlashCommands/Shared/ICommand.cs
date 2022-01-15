namespace BotANick.Discord.SlashCommands.Shared;

public interface ICommand
{
    string Name { get; set; }

    string Description { get; set; }
}
