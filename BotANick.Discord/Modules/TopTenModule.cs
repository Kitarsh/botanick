using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotANick.Discord.Services;
using System;
using BotANick.Discord.Modeles;
using BotANick.Core.Data;
using Discord.WebSocket;

namespace BotANick.Discord.Modules
{
    [Name("Commandes du jeu Top Ten")]
    [Group("TopTen")]
    public class TopTenModule : ModuleBase<SocketCommandContext>
    {
        private static TopTenGame _topten = new TopTenGame();

        [Command("regles")]
        [Summary("Donne les règles du jeu.")]
        public Task Rules()
            => ReplyAsync($"Les règles du jeu sont en ligne : https://www.cocktailgames.com/wp-content/uploads/2020/01/Top_ten_regles_BD.pdf");

        [Command("register")]
        [Summary("Permet aux joueurs de s'enregister")]
        public async Task RegisterByReactions()
        {
            EmbedBuilder builder = EmbedBuilderService.InitBuilder(new List<EmbedFieldBuilder>(),
                                                                   _topten.ColorTopTen,
                                                                   "Réagissez avec l'emote 💯 pour vous inscrire !");

            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());
            await msg.AddReactionAsync(_topten.RegisterEmoteChar);

            _topten.StoreRegisterMsg(msg.Id);
        }

        [Command("registerme")]
        [Summary("Ajoute le joueur")]
        public async Task RegisterUser()
        {
            var user = Context.Message.Author.Username;
            _topten.RegisterUser(user);
            await ReplyAsync($"{_topten.Users.LastOrDefault()} est ajouté à la liste des joueurs !");
        }

        [Command("clear")]
        [Summary("Nettoie la liste des joueurs et les thèmes déjà utilisés.")]
        public async Task ClearRegister()
        {
            _topten.Clear();
            await ReplyAsync("La liste des joueurs a été vidée !");
        }

        [Command("players")]
        [Summary("Affiche la liste des joueurs")]
        public async Task ReadRegister()
        {
            await UpdatePlayers();
            if (_topten.Users.Count == 0)
            {
                await ReplyAsync("Personne ne joue !");
                return;
            }

            EmbedBuilder builder = EmbedBuilderService.GenerateBuilderReadRegister(_topten.Users, _topten.ColorTopTen);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("play")]
        [Summary("Lance un round de jeu.")]
        public async Task GenerateNumbers()
        {
            await UpdatePlayers();
            if (_topten.IndexCapten == null)
            {
                await ReplyAsync("Personne ne joue !");
                return;
            }

            if (_topten.Themes.Count == 0)
            {
                using (var dbContext = new SqlLiteContext())
                {
                    _topten.Themes = TopTenService.GetRandomThemes(dbContext);
                }
            }

            var theme = _topten.GetNextTheme();
            EmbedBuilder builder = EmbedBuilderService.GenerateBuilderForNumberDisplay(_topten, theme);

            await ReplyAsync("", false, builder.Build());
        }

        private async Task<List<string>> GetNicknamesFromUser(IReadOnlyCollection<IUser> registeredUsers)
        {
            var allPlayers = registeredUsers.Where(ui => !ui.IsBot).ToList();
            var allNicknamesTask = allPlayers.Select(iu => GetNickname(iu));
            return (await Task.WhenAll(allNicknamesTask)).ToList();
        }

        private async Task<string> GetNickname(IUser iu)
        {
            var user = await Context.Channel.GetUserAsync(iu.Id, CacheMode.AllowDownload);

            if (user is SocketGuildUser userSocket && !string.IsNullOrEmpty(userSocket.Nickname))
            {
                return userSocket.Nickname;
            }
            else
            {
                return iu.Username;
            }
        }

        /// <summary>
        /// Met à jour la liste des joueurs et le CapTen.
        /// </summary>
        private async Task UpdatePlayers()
        {
            // Mise à jour de la liste des joueurs.
            if (_topten.RegisterMsgId != 0)
            {
                var registerMsg = await Context.Channel.GetMessageAsync(_topten.RegisterMsgId);
                var registeredUsers = await registerMsg.GetReactionUsersAsync(_topten.RegisterEmoteChar, 11).FirstOrDefaultAsync();
                List<string> nicknames = await GetNicknamesFromUser(registeredUsers);
                _topten.RegisterUser(nicknames);
            }

            _topten.NextCapten();
        }
    }
}
