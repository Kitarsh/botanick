using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

namespace BotANick.Discord.Services
{
    public static class ExtensionsEmbedFieldBuilder
    {
        public static string ToString(this EmbedFieldBuilder efb)
        {
            return $"{efb.Name}, {efb.Value}";
        }

        public static EmbedFieldBuilder Create(string Name, string Value)
        {
            var fieldBuilder = new EmbedFieldBuilder
            {
                Name = Name,
                Value = Value
            };
            return fieldBuilder;
        }
    }

    public static class ExtensionsEmbedBuilder
    {
        public static string ToStringCustom(this EmbedBuilder eb)
        {
            var fieldsStringList = new List<string>();

            foreach (var field in eb.Fields)
            {
                fieldsStringList.Add(ExtensionsEmbedFieldBuilder.ToString(field));
            }

            return $"{eb.Title}, {eb.Description}, {eb.Color}, {eb.Author}, {string.Join(", ", fieldsStringList)}, {eb.Url}";
        }
    }

    public class EmbedBuilderService
    {
        public static EmbedBuilder GenerateBuilderForNumberDisplay(Modeles.TopTenGame topTen, string theme)
        {
            string numbers = TopTenService.GenerateNumbers(topTen.Users);

            var fields = new List<EmbedFieldBuilder>
            {
                ExtensionsEmbedFieldBuilder.Create("Le thème est le suivant :", theme),
                ExtensionsEmbedFieldBuilder.Create("Le Cap'TEN est :", topTen.Capten),
                ExtensionsEmbedFieldBuilder.Create("Tirage des numéros :", string.IsNullOrEmpty(numbers) ? "No players" : numbers),
            };

            return InitBuilder(fields, topTen.ColorTopTen);
        }

        public static EmbedBuilder InitBuilder(List<EmbedFieldBuilder> fields, Color colorTopTen, string Description = null)
        {
            return new EmbedBuilder()
            {
                Title = "TopTen",
                Color = colorTopTen,
                Description = Description,
                Fields = fields
            };
        }

        public static EmbedBuilder GenerateBuilderReadRegister(List<string> users, Color colorTopTen)
        {
            string playersString = string.Join("\n    ", users);
            var fields = new List<EmbedFieldBuilder>
            {
                ExtensionsEmbedFieldBuilder.Create("Liste des joueurs :", string.IsNullOrEmpty(playersString) ? "No Players" : playersString),
            };

            EmbedBuilder builder = InitBuilder(fields, colorTopTen);
            return builder;
        }
    }
}
