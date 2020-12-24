﻿using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BotANick.Services;
using Microsoft.Extensions.Configuration;
using Discord.WebSocket;

namespace BotANick.Modules
{
    [Name("Commandes basiques")]
    public class BasicsModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Alias("s")]
        [Summary("Fait dire quelque chose au bot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task Say([Remainder] string text)
            => ReplyAsync(text);

        [Command("hug")]
        [Summary("Une preuve d'amour du bot.")]
        public async Task Hug()
        {
            var sckUserMessage = Context.Message;
            int teddy_bear = 0x1F9F8; // U+1F9F8.
            var emojis = new List<string>
            {
                "\u2764",
                "\uD83D\uDD25",
                Char.ConvertFromUtf32(teddy_bear),
            };

            var rng = new Random();
            var emoji = emojis.OrderBy(e => rng.Next())
                              .FirstOrDefault();

            await MessageService.ReactWithEmoteAsync(sckUserMessage, emoji);
        }

        [Command("noraj")]
        public async Task NorajReactions()
        {
            var msgsEnumerable = await Context.Channel.GetMessagesAsync(2, CacheMode.AllowDownload)
                                            .FlattenAsync();
            var msgs = msgsEnumerable.OrderByDescending(msg => msg.CreatedAt)
                                     .ToList();

            var messageToReact = msgs[1]; // On réagit au message juste avant le message de commande.
            await Context.Channel.DeleteMessageAsync(msgs[0]); // On supprime le message de commande.

            await messageToReact.AddReactionAsync(new Emoji("🇳"));
            await messageToReact.AddReactionAsync(new Emoji("🇴"));
            await messageToReact.AddReactionAsync(new Emoji("🇷"));
            await messageToReact.AddReactionAsync(new Emoji("🇦"));
            await messageToReact.AddReactionAsync(new Emoji("🇯"));
        }

        [Command("test")]
        public async Task TestTwitchBot()
        {
        }
    }
}
