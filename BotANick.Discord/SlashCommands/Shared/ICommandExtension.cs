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

        public static SlashCommandBuilder GenerateSlashCommandBuilder(this TopTenGeneralCommandModel cmd)
        {
            var command = new SlashCommandBuilder();
            command.WithName(cmd.Name);
            command.WithDescription(cmd.Description);

            if (cmd.SubCommands.Any())
            {
                foreach (var subCommand in cmd.SubCommands)
                {
                    command.AddOption(
                        new SlashCommandOptionBuilder()
                            .WithName(subCommand.Name)
                            .WithDescription(subCommand.Description)
                            .WithType(subCommand.CommandOptionType)
                        );
                }
            }

            return command;
        }
    }
}
