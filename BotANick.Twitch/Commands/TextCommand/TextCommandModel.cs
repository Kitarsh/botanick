namespace BotANick.Twitch.Commands.TextCommand;

public class TextCommandModel
{
    private readonly CommandService commandSrv;

    private readonly List<string> HydrateResults = new List<string>
        {
            "Buvez de l'eau, c'est bon pour la santé !",
            "Allons-y tous ensemble ! Buvons un coup !",
            "Y'fait soif...",
            "*Bruit horrible de succion*",
            "Sapristi ! Où est ma Rosana ??",
            "Eh Marcel ! Un petit jaune ?!",
            "T'étouffe pas surtout Kappa",
        };

    public TextCommandModel(CommandService commandService)
    {
        this.commandSrv = commandService;
    }

    private enum EnumTextCommand
    {
        Help = 1,
        Hydrate = 2,
        Toto = 3,
        Bonjour = 4,
        Rig = 5,
        Indelivrables = 6,
        GiveUp = 7,
        TimeHydrate = 8,
    };

    public void Execute(string command)
    {
        switch (GetTextCommands(command))
        {
            case EnumTextCommand.Help: Help(); break;
            case EnumTextCommand.Hydrate: Hydrate(); break;
            case EnumTextCommand.Toto: Toto(); break;
            case EnumTextCommand.Bonjour: Bonjour(); break;
            case EnumTextCommand.Rig: Rig(); break;
            case EnumTextCommand.Indelivrables: Indelivrables(); break;
            case EnumTextCommand.GiveUp: GiveUp(); break;
            case EnumTextCommand.TimeHydrate: HydrateTime(); break;
            default: break;
        }
    }

    private EnumTextCommand GetTextCommands(string command)
    {
        var enumArray = Enum.GetNames(typeof(EnumTextCommand));
        foreach (var enumElement in enumArray)
        {
            if (command.ToLower().StartsWith(enumElement.ToLower()))
            {
                return (EnumTextCommand)Enum.Parse(typeof(EnumTextCommand), enumElement);
            }
        }

        return 0;
    }

    private void Rig()
    {
        commandSrv.WriteInChat("Il a 4 écrans et il ne parle que de ça...");
    }

    private void Bonjour()
    {
        commandSrv.WriteInChat("HeyGuys");
    }

    private void Help()
    {
        var msg = "Liste des commandes :";
        msg += AddCommandBasedOnEnum(typeof(EnumTextCommand));

        //msg += AddCommandBasedOnEnum(typeof(DiscordCommands.EnumDiscordCommand), "Discord");

        commandSrv.WriteInChat(msg);
    }

    private void Toto()
    {
        commandSrv.WriteInChat("Votre langage est très évolué.");
    }

    private void Hydrate()
    {
        var rng = new Random();

        var pickedIndex = rng.Next(0, HydrateResults.Count - 1);
        commandSrv.LastHydrate = DateTime.Now;
        commandSrv.WriteInChat(HydrateResults[pickedIndex]);
    }

    private void Indelivrables()
    {
        commandSrv.WriteInChat("Allez tous regarder la chaîne YouTube des Indélivrables : https://www.youtube.com/channel/UCl7djHZZcnOt-t05QMYx90g");
    }

    private void GiveUp()
    {
        commandSrv.WriteInChat("https://www.youtube.com/watch?v=dQw4w9WgXcQ LUL");
    }

    private void HydrateTime()
    {
        if (commandSrv.LastHydrate == null)
        {
            commandSrv.WriteInChat("Il n'a jamais bu Kappa");
            return;
        }

        commandSrv.WriteInChat($"Il a bu pour la dernière fois, il y a {GetDiffMinutesLastTimeHydrate().ToString(new CultureInfo("en-US"))} minute(s).");
    }

    private decimal GetDiffMinutesLastTimeHydrate()
    {
        var now = DateTime.Now;
        var diffDates = now - commandSrv.LastHydrate.Value;
        var diffMinutes = Math.Round(diffDates.TotalMinutes, 3);
        return Convert.ToDecimal(diffMinutes);
    }

    private string AddCommandBasedOnEnum(Type enumType, string preCondition = "")
    {
        StringBuilder bld = new StringBuilder();
        var enumArray = Enum.GetNames(enumType);
        foreach (var enumElement in enumArray)
        {
            bld.Append(" '!");
            if (!string.IsNullOrEmpty(preCondition))
            {
                bld.Append($"{preCondition} ");
            }
            bld.Append(enumElement);
            bld.Append("'");
        }

        return bld.ToString();
    }
}
