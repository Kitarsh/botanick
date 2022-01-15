using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotANick.Data.Models
{
    public class TwitchCommand
    {
        public int TwitchCommandId { get; set; }

        public string Command { get; set; }

        public string Reponse { get; set; }
    }
}
