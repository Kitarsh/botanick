using Discord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotANick.Discord.Modeles
{
    public class TopTenGame
    {
        public TopTenGame()
        {
        }

        /// <summary>
        /// La liste des nombres du jeu.
        /// </summary>
        public int[] NumberList { get; } = Enumerable.Range(1, 10).ToArray();

        /// <summary>
        /// La liste des utilisateurs enregistrés pour le jeu.
        /// </summary>
        public List<string> Users { get; } = new List<string> { };

        /// <summary>
        /// La liste des thèmes utilisés pour jouer.
        /// </summary>
        public List<string> Themes { get; set; } = new List<string> { };

        /// <summary>
        /// L'identifiant Discord du message d'enregistrement.
        /// </summary>
        public ulong RegisterMsgId { get; set; }

        /// <summary>
        /// L'index de Cap'TEN dans la liste des utilisateurs.
        /// </summary>
        public int? IndexCapten { get; set; }

        /// <summary>
        ///  L'émoji utilisée pour s'enregister.
        /// </summary>
        public Emoji RegisterEmoteChar { get; } = new Emoji(char.ConvertFromUtf32(0x1F4AF));

        /// <summary>
        /// La couleur principale de la charte graphique du jeu.
        /// </summary>
        public Color ColorTopTen { get; } = new Color(20, 119, 134);

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

        private bool IsUserRegistered(string player)
        {
            return this.Users.Contains(player);
        }
    }
}
