using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotANick.Core.Data;

namespace BotANick.Discord.Services
{
    public static class TopTenService
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
            StringBuilder bld = new StringBuilder();
            var numbers = GetRandomList();
            foreach (var player in Players)
            {
                var number = PopFromList(numbers);
                bld.Append($"\r\n{player} ||`{number:00}`||");
            }
            return bld.ToString();
        }

        /// <summary>
        /// Récupère des thèmes aléatoires depuis la base de données.
        /// </summary>
        /// <returns>Le thème obtenu.</returns>
        static public List<string> GetRandomThemes(IDataContext dbContext)
        {
            Random rand = new Random();

            return dbContext.TopTenTheme.AsEnumerable()
                                        .Select(ttt => ttt.Theme)
                                        .OrderBy(t => rand.Next())
                                        .ToList();
        }

        static public List<int> GetRandomList()
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
