using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotANick.Discord.SlashCommands.Shared
{
    public static class ICommandExtension
    {
        public static SlashCommandBuilder GenerateSlashCommandBuilder(this ICommand cmd)
        {
            var command = new SlashCommandBuilder();
            command.WithName(cmd.Name);
            command.WithDescription(cmd.Description);

            return command;
        }
    }
}
