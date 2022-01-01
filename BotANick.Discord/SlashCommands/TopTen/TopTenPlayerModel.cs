namespace BotANick.Discord.SlashCommands.TopTen;

public class TopTenPlayerModel
{
    public string? Name { get; set; }

    public SocketSlashCommand? RegisterSlashCommand { get; set; }

    public int? Number { get; set; }

    public async Task RespondWithNumber()
    {
        if (RegisterSlashCommand == null)
        {
            return;
        }

        await this.RegisterSlashCommand.FollowupAsync($"Ton nombre est le : {Number}", ephemeral: true);
    }
}
