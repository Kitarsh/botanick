using Discord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotANick.Discord.Modeles
{
    public class TopTenGame
    {
        /// <summary>
        /// L'identifiant Discord du message d'enregistrement.
        /// </summary>
        public ulong RegisterMsgId;

        /// <summary>
        /// L'index de Cap'TEN dans la liste des utilisateurs.
        /// </summary>
        public int? IndexCapten;

        /// <summary>
        ///  L'émoji utilisée pour s'enregister.
        /// </summary>
        public Emoji RegisterEmoteChar = new Emoji(char.ConvertFromUtf32(0x1F4AF));

        /// <summary>
        /// La couleur principale de la charte graphique du jeu.
        /// </summary>
        public Color ColorTopTen = new Color(20, 119, 134);

        /// <summary>
        /// La liste des nombres du jeu.
        /// </summary>
        public int[] NumberList = Enumerable.Range(1, 10).ToArray();

        /// <summary>
        /// La liste des utilisateurs enregistrés pour le jeu.
        /// </summary>
        public List<string> Users = new List<string> { };

        /// <summary>
        /// La liste des thèmes utilisés pour jouer.
        /// </summary>
        public List<string> Themes = new List<string> { };

        public TopTenGame()
        {
        }

        public int NbsUsers
        {
            get
            {
                return this.Users.Count();
            }
        }

        public bool HasCapten
        {
            get
            {
                return this.IndexCapten != null;
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
            if (players == null || players.Count() == 0)
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
            if (themes == null || themes.Count() == 0)
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
