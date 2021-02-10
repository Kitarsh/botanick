using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotANick.Discord.Services;
using System;

namespace BotANick.Discord.Modules
{
    [Name("Commandes du jeu Top Ten")]
    [Group("TopTen")]
    public class TopTenModule : ModuleBase<SocketCommandContext>
    {
        #region Variable pour la classe.

        /// <summary>
        ///  L'émoji utilisée pour s'enregister.
        /// </summary>
        private static Emoji _registerEmoteChar = new Emoji(char.ConvertFromUtf32(0x1F4AF));

        /// <summary>
        /// La couleur principale de la charte graphique du jeu.
        /// </summary>
        private static Color _colorTopTen = new Color(20, 119, 134);
        #endregion

        #region Variable le jeu. #TODO : Instancier.

        /// <summary>
        /// L'identifiant Discord du message d'enregistrement.
        /// </summary>
        private static ulong _registerMsgId = 0;

        /// <summary>
        /// La liste des utilisateurs enregistrés pour le jeu.
        /// </summary>
        private static List<string> _users = new List<string> { };

        /// <summary>
        /// La liste des thèmes utilisés pour jouer.
        /// </summary>
        private static List<string> _themes = new List<string> { };

        /// <summary>
        /// L'index de Cap'TEN dans la liste des utilisateurs.
        /// </summary>
        private static int? _indexCapten = null;
        #endregion

        [Command("regles")]
        [Summary("Donne les règles du jeu.")]
        public Task Rules()
            => ReplyAsync($"Les règles du jeu sont en ligne : https://www.cocktailgames.com/wp-content/uploads/2020/01/Top_ten_regles_BD.pdf");

        [Command("register")]
        [Summary("Permet aux joueurs de s'enregister")]
        public async Task RegisterByReactions()
        {
            var builder = new EmbedBuilder()
            {
                Color = _colorTopTen,
                Title = "TopTen",
                Description = "Réagissez avec l'emote 💯 pour vous inscrire !",
            };

            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());

            await msg.AddReactionAsync(_registerEmoteChar);
            _registerMsgId = msg.Id;
        }

        [Command("registerme")]
        [Summary("DEPRECATED : Ajoute le joueur")]
        public async Task RegisterUser()
        {
            var caller = Context.Message.Author.Username;
            _users.Add(caller);
            await ReplyAsync($"{_users.LastOrDefault()} est ajouté à la liste des joueurs !");
        }

        [Command("clear")]
        [Summary("Nettoie la liste des joueurs et les thèmes déjà utilisés.")]
        public async Task ClearRegister()
        {
            _users.Clear();
            _themes.Clear();
            _registerMsgId = 0;
            await ReplyAsync("La liste des joueurs a été vidée !");
        }

        [Command("players")]
        [Summary("Affiche la liste des joueurs")]
        public async Task ReadRegister()
        {
            await UpdatePlayers();
            if (_users.Count == 0)
            {
                await ReplyAsync("Personne ne joue !");
                return;
            }

            var fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = "Liste des joueurs :",
                Value = string.Join("\n    ", _users),
            });

            var builder = new EmbedBuilder()
            {
                Title = "TopTen",
                Color = _colorTopTen,
                Fields = fields
            };

            await ReplyAsync("", false, builder.Build());
        }

        [Command("play")]
        [Summary("Lance un round de jeu.")]
        public async Task GenerateNumbers()
        {
            await UpdatePlayers();
            if (_indexCapten == null)
            {
                await ReplyAsync("Personne ne joue !");
                return;
            }

            if (_themes.Count == 0)
            {
                _themes = TopTenService.GetRandomThemes();
            }

            var theme = _themes[0];
            _themes.RemoveAt(0);

            var builder = new EmbedBuilder()
            {
                Title = "TopTen",
                Color = _colorTopTen,
            };

            builder.AddField(f =>
            {
                f.IsInline = false;
                f.Name = "Le thème est le suivant :";
                f.Value = theme;
            });

            builder.AddField(f =>
            {
                f.IsInline = false;
                f.Name = "Le Cap'TEN est :";
                f.Value = _users[_indexCapten.Value];
            });

            builder.AddField(f =>
            {
                f.IsInline = false;
                f.Name = "Tirage des numéros :";
                f.Value = TopTenService.GenerateNumbers(_users);
            });

            await ReplyAsync("", false, builder.Build());
        }

        /// <summary>
        /// Met à jour la liste des joueurs et le CapTen.
        /// </summary>
        private async Task UpdatePlayers()
        {
            // Mise à jour de la liste des joueurs.
            if (_registerMsgId != 0)
            {
                var registerMsg = await Context.Channel.GetMessageAsync(_registerMsgId);
                var registeredUsers = await registerMsg.GetReactionUsersAsync(_registerEmoteChar, 11).FirstOrDefaultAsync();
                _users = registeredUsers.Where(u => !u.IsBot)
                                        .OrderBy(u => u.Username)
                                        .Select(u => u.Username)
                                        .ToList();
            }

            // Mise à jour du CapTen.
            if (_indexCapten == null && _users.Count > 0)
            {
                // Si le CapTen n'a jamais été défini, c'est le premier de la liste.
                _indexCapten = 0;
            }
            else
            {
                //Sinon, on passe au suivant dans la liste.
                _indexCapten += 1;
                if (_indexCapten >= _users.Count)
                {
                    // Si l'index de CapTen dépasse la taille de la liste, on revient au début de la liste.
                    _indexCapten = 0;
                }
            }
        }
    }
}
