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

    public static Emoji RegisterEmoteChar { get; } = new Emoji(char.ConvertFromUtf32(0x1F4AF));

    public static Color ColorTopTen { get; } = new Color(20, 119, 134);

    public int NbsUsers
    {
        get
        {
            return this.Users.Count;
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
            if (IndexCapten.HasValue)
            {
                return this.Users[IndexCapten.Value];
            }
            else
            {
                return "No Capten was selected";
            }
        }
    }

    public int[] NumberList { get; } = Enumerable.Range(1, 10).ToArray();

    public List<string> Users { get; } = new List<string> { };

    public List<string> Themes { get; set; } = new List<string> { };

    public ulong RegisterMsgId { get; set; }

    public int? IndexCapten { get; set; }

    /// <summary>
    /// Copy to clipboard the generated numbers along with players names
    /// </summary>
    /// <param name="Players">List of players.</param>
    public static string GenerateNumbers(List<string> Players)
    {
        StringBuilder bld = new StringBuilder();
        var numbers = GetRandomList();
        foreach (var player in Players)
        {
            var number = PopFromList(numbers);
            bld.Append($"\r\n{player} ||`{number:00}`||");
        }
        return bld.ToString();
    }

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

    public void RegisterUser(string player)
    {
        if (!IsUserRegistered(player))
        {
            this.Users.Add(player);
        }
    }

    public void RegisterUser(IEnumerable<string> players)
    {
        if (players == null || !players.Any())
        {
            return;
        }

        foreach (var player in players)
        {
            this.RegisterUser(player);
        }
    }

    public void ClearUser()
    {
        this.Users.Clear();
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

    public void ClearTheme()
    {
        this.Themes.Clear();
    }

    public void StoreRegisterMsg(ulong idMsg)
    {
        this.RegisterMsgId = idMsg;
    }

    public void ResetRegisterMsg()
    {
        this.RegisterMsgId = 0;
    }

    public void Clear()
    {
        this.ResetRegisterMsg();
        this.ClearUser();
        this.ClearTheme();
        this.ClearCapten();
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

    public void ClearCapten()
    {
        this.IndexCapten = null;
    }

    public string GetNextTheme()
    {
        string themeToReturn = this.Themes[0];
        this.Themes.RemoveAt(0);
        return themeToReturn;
    }

    private static int PopFromList(List<int> numbers)
    {
        var value = numbers.FirstOrDefault();
        numbers.RemoveAt(0);
        return value;
    }

    private bool IsUserRegistered(string player)
    {
        return this.Users.Contains(player);
    }
}
