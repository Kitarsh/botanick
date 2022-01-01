namespace BotANick.Discord.SlashCommands.TopTen;

public class TopTenGameModel
{
    private static int[] InitialNumbers = new int[10] {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
        };

    public TopTenGameModel(SocketSlashCommand cmd)
    {
        this.RegisterSlhCmd = cmd;
    }

    public static Emoji RegisterEmoteChar { get; } = new Emoji(char.ConvertFromUtf32(0x1F4AF));

    public static Color ColorTopTen { get; } = new Color(20, 119, 134);

    public static string GameStartMsg { get; } = "Inscrivez-vous avec la commande **/topten register** ! \n \n Les règles du jeu sont en ligne : https://www.cocktailgames.com/wp-content/uploads/2020/01/Top_ten_regles_BD.pdf'";

    public int NbsUsers
    {
        get
        {
            return this.Players.Count;
        }
    }

    public bool HasCapten
    {
        get
        {
            return this.IndexCapten != null;
        }
    }

    public string Capten
    {
        get
        {
            if (IndexCapten.HasValue && this.Players[IndexCapten.Value].Name != null)
            {
                return this.Players[IndexCapten.Value].Name;
            }
            else
            {
                return "No Capten was selected";
            }
        }
    }

    public int[] NumberList { get; } = Enumerable.Range(1, 10).ToArray();

    public List<TopTenPlayerModel> Players { get; } = new List<TopTenPlayerModel> { };

    public List<string> Themes { get; set; } = new List<string> { };

    public SocketSlashCommand? RegisterSlhCmd { get; set; }

    public int? IndexCapten { get; set; }

    /// <summary>
    /// Récupère des thèmes aléatoires depuis la base de données.
    /// </summary>
    /// <returns>Le thème obtenu.</returns>
    public static List<string> GetRandomThemes(BotANickContext dbContext)
    {
        Random rand = new Random();

        return dbContext.TopTenTheme.AsEnumerable()
                                    .Select(ttt => ttt.Theme)
                                    .OrderBy(t => rand.Next())
                                    .ToList();
    }

    public static List<int> GetRandomList()
    {
        var rng = new Random();
        return InitialNumbers.OrderBy(n => rng.Next())
                             .ToList();
    }

    public void GenerateNumbers()
    {
        var numbers = GetRandomList();
        foreach (var player in Players)
        {
            var number = PopFromList(numbers);

            player.Number = number;
            _ = player.RespondWithNumber();
        }
    }

    public string GetResults()
    {
        StringBuilder bld = new StringBuilder();
        var orderedByNumberPlayers = Players.OrderBy(p => p.Name)
                                            .ToList();
        foreach (var player in orderedByNumberPlayers)
        {
            if (player.Name == null)
            {
                throw new Exception("Un joueur n'a pas de nom !");
            }

            bld.Append($"\r\n||`{player.Number:00}`|| : {player.Name} ");
        }
        return bld.ToString();
    }

    public void RegisterUser(string playerName, SocketSlashCommand cmd)
    {
        if (!IsUserRegistered(playerName))
        {
            this.Players.Add(new TopTenPlayerModel
            {
                Name = playerName,
                RegisterSlashCommand = cmd,
            });
        }
    }

    public void RegisterTheme(string theme)
    {
        this.Themes.Add(theme);
    }

    public void RegisterTheme(string[] themes)
    {
        if (themes == null || !themes.Any())
        {
            return;
        }

        foreach (var theme in themes)
        {
            this.RegisterTheme(theme);
        }
    }

    public void NextCapten()
    {
        // Mise à jour du CapTen.
        if (!this.HasCapten && this.NbsUsers > 0)
        {
            // Si le CapTen n'a jamais été défini, c'est le premier de la liste.
            this.IndexCapten = 0;
        }
        else
        {
            //Sinon, on passe au suivant dans la liste.
            this.IndexCapten++;
            if (this.IndexCapten >= this.NbsUsers)
            {
                // Si l'index de CapTen dépasse la taille de la liste, on revient au début de la liste.
                this.IndexCapten = 0;
            }
        }
    }

    public string GetNextTheme()
    {
        string themeToReturn = this.Themes[0];
        this.Themes.RemoveAt(0);
        return themeToReturn;
    }

    public async Task ChangeStartMsgWithRegistered()
    {
        if (this.RegisterSlhCmd == null)
        {
            return;
        }

        StringBuilder stgBld = new();

        stgBld.AppendLine(GameStartMsg);
        stgBld.AppendLine($"Liste des joueurs enregistrés :");

        foreach (var player in this.Players)
        {
            stgBld.AppendLine(player.Name);
        }
        await this.RegisterSlhCmd.ModifyOriginalResponseAsync(msg => msg.Content = stgBld.ToString());
    }

    private static int PopFromList(List<int> numbers)
    {
        var value = numbers.FirstOrDefault();
        numbers.RemoveAt(0);
        return value;
    }

    private bool IsUserRegistered(string player)
    {
        return this.Players.Select(p => p.Name).Contains(player);
    }
}
