using System;
using System.Collections.Generic;
using System.Text;

namespace BotANick.Data
{
    public class TopTenTheme
    {
        /// <summary>
        /// L'identifiant du thème pour le jeu Top Ten.
        /// </summary>
        public int TopTenThemeId { get; set; }

        /// <summary>
        /// Le thème.
        /// </summary>
        public string Theme { get; set; }
    }
}
