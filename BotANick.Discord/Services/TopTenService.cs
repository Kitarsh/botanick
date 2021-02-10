using System;
using System.Collections.Generic;
using System.Linq;
using BotANick.Core.Data;

namespace BotANick.Discord.Services
{
    static class TopTenService
    {
        static private int[] InitialNumbers = new int[10] {
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

        /// <summary>
        /// Copy to clipboard the generated numbers along with players names
        /// </summary>
        /// <param name="Players">List of players.</param>
        static public string GenerateNumbers(List<string> Players)
        {
            string textToClipboard = "";
            var numbers = GetRandomList();

            foreach (var player in Players)
            {
                var number = PopFromList(numbers);
                var tmpText = $"\r\n{player} ||`{number:00}`||";
                textToClipboard += tmpText;
            }
            return textToClipboard;
        }

        /// <summary>
        /// Récupère un thème aléatoire depuis la base de données.
        /// </summary>
        /// <returns>Le thème obtenu.</returns>
        static public List<string> GetRandomThemes()
        {
            using(var dbContext = new DataContext())
            {
                Random rand = new Random();
                int nbThemes = dbContext.TopTenTheme.Count();

                return dbContext.TopTenTheme.AsEnumerable()
                                            .Select(ttt => ttt.Theme)
                                            .OrderBy(t => rand.Next())
                                            .ToList();
            }
        }

        static private List<int> GetRandomList()
        {
            var rng = new Random();
            return InitialNumbers.OrderBy(n => rng.Next())
                                 .ToList();
        }

        static private int PopFromList(List<int> numbers)
        {
            var value = numbers.FirstOrDefault();
            numbers.RemoveAt(0);
            return value;
        }
    }
}
