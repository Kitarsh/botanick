namespace BotANick.Discord.SlashCommands.TopTen;

public class TopTenGeneralCommandModel : ICommand
{
    public TopTenGeneralCommandModel()
    {
        SubCommands = new List<SubCommand>();

        //SubCommandGroups = new List<SubCommandGroup>();
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<SubCommand> SubCommands { get; }

    public static TopTenGeneralCommandModel GenerateTopTenCommands()
    {
        var mdl = new TopTenGeneralCommandModel();

        mdl.Name = "topten";
        mdl.Description = "Les commandes liées au jeu TopTen.";

        mdl.SubCommands.Add(GenerateGameStartSubCmd());
        mdl.SubCommands.Add(GenerateRegisterSubCmd());
        mdl.SubCommands.Add(GeneratePlaySubCmd());
        mdl.SubCommands.Add(GenerateResultsSubCmd());

        return mdl;
    }

    public static SubCommand GenerateResultsSubCmd()
    {
        return new SubCommand
        {
            Name = "results",
            Description = "Donne les résultats de la manche."
        };
    }

    public static SubCommand GeneratePlaySubCmd()
    {
        return new SubCommand
        {
            Name = "play",
            Description = "Lance une manche de jeu.",
        };
    }

    public static SubCommand GenerateRegisterSubCmd()
    {
        return new SubCommand
        {
            Name = "register",
            Description = "C'est pour t'enregistrer dans la partie !",
        };
    }

    public static SubCommand GenerateGameStartSubCmd()
    {
        return new SubCommand
        {
            Name = "gamestart",
            Description = "Démarre une nouvelle partie : Affiche un message pour enregistrer les joueurs.",
        };
    }
}
