using BotANick.Core.Data;
using Microsoft.Extensions.Configuration;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotANick.Discord.Services
{
    public class GitHubService
    {
        private static IConfigurationRoot _config;

        public GitHubService()
        {
            Client = new GitHubClient(new ProductHeaderValue("BotANick"))
            {
                Credentials = new Credentials(_config["tokens:github"]),
            };
        }

        public GitHubClient Client { get; }

        public static void InitConfig(IConfigurationRoot config)
        {
            _config = config;
        }

        public async Task<Issue> GetLastIssue()
        {
            var issues = await Client.Issue.GetAllForRepository("kitarsh", "botanick");
            var lastIssue = issues.OrderByDescending(i => i.UpdatedAt)
                                  .FirstOrDefault();
            return lastIssue;
        }

        public async Task<Issue> AddIssueBasedOnIdee(Idee idee)
        {
            var newIssue = new NewIssue(idee.Description ?? "NoDescriptionProvided.");
            newIssue.Labels.Add("Idee");
            return await Client.Issue.Create("kitarsh", "botanick", newIssue);
        }

        public async Task<bool> IsIdeeAnIssue(Idee idee)
        {
            var issues = await Client.Issue.GetAllForRepository("kitarsh", "botanick");
            return issues.Any(i => i.Title == idee.Description);
        }

        public async Task CreateGitHubIssuesForMissingIdees(IEnumerable<Idee> idees)
        {
            foreach (var idee in idees)
            {
                if (!await IsIdeeAnIssue(idee))
                {
                    await AddIssueBasedOnIdee(idee);
                }
            }
        }
    }
}
