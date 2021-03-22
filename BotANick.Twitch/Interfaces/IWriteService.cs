using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace BotANick.Twitch.Interfaces
{
    public interface IWriteService
    {
        public void WriteInChat(string msg);
    }
}
