using System;
using System.Collections.Generic;
using BotANick.Twitch.Interfaces;
using BotANick.Twitch.Services;
using BotANick.Twitch.Commands;

namespace BotANick.Twitch.Commands
{
    public static class TextCommands
    {
        public static readonly List<string> HydrateResults = new List<string>
        {
            "Buvez de l'eau, c'est bon pour la santé !",
            "Allons-y tous ensemble ! Buvons un coup !",
            "Y'fait soif...",
            "*Bruit horrible de succion*",
            "Sapristi ! Où est ma Rosana ??",
            "Eh Marcel ! Un petit jaune ?!",
            "T'étouffe pas surtout Kappa",
        };

        public enum EnumTextCommand
        {
            Help = 1,
            Hydrate = 2,
            Toto = 3,
            Bonjour = 4,
            Rig = 5,
        };

        public static void Execute(string command, IWriteService writeSrv)
        {
            switch (GetTextCommands(command))
            {
                case EnumTextCommand.Help:
                    Help(writeSrv);
                    break;

                case EnumTextCommand.Hydrate:
                    Hydrate(writeSrv);
                    break;

                case EnumTextCommand.Toto:
                    Toto(writeSrv);
                    break;

                case EnumTextCommand.Bonjour:
                    Bonjour(writeSrv);
                    break;

                case EnumTextCommand.Rig:
                    Rig(writeSrv);
                    break;

                default:
                    break;
            }
        }

        public static EnumTextCommand GetTextCommands(string command)
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

        private static void Rig(IWriteService writeSrv)
        {
            writeSrv.WriteInChat("Il a 4 écrans et il ne parle que de ça...");
        }

        private static void Bonjour(IWriteService writeSrv)
        {
            writeSrv.WriteInChat("HeyGuys");
        }

        private static void Help(IWriteService writeSrv)
        {
            var msg = "Liste des commandes :";
            msg += AddCommandBasedOnEnum(typeof(EnumTextCommand));
            msg += AddCommandBasedOnEnum(typeof(DiscordCommands.EnumDiscordCommand), "Discord");

            writeSrv.WriteInChat(msg);
        }

        private static void Toto(IWriteService writeSrv)
        {
            writeSrv.WriteInChat("Votre langage est très évolué.");
        }

        private static void Hydrate(IWriteService writeSrv)
        {
            var rng = new Random();

            var pickedIndex = rng.Next(0, HydrateResults.Count - 1);
            writeSrv.WriteInChat(HydrateResults[pickedIndex]);
        }

        private static string AddCommandBasedOnEnum(Type enumType, string preCondition = "")
        {
            var msg = string.Empty;
            var enumArray = Enum.GetNames(enumType);
            foreach (var enumElement in enumArray)
            {
                if (!string.IsNullOrEmpty(preCondition))
                {
                    preCondition += " ";
                }
                msg += $" '!{preCondition}{enumElement}'";
            }

            return msg;
        }
    }
}
