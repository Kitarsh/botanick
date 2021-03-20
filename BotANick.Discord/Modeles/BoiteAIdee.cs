using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotANick.Discord.Modeles
{
    public class BoiteAIdee
    {
        public readonly Color ColorIdee = new Color(248, 100, 0);

        public readonly ulong IdBoiteChannel = 793819950355185694;

        public readonly Emoji EmoteUpVote = new Emoji("⬆️");

        public readonly Emoji EmoteEtatEnCours = new Emoji("🔧");

        public readonly Emoji EmoteEtatTermine = new Emoji("🏁");

        public readonly Emoji EmoteEtatRejete = new Emoji("❌");
    }
}
