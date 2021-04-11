using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BotANick.Core.Data;
using BotANick.Discord.Services;
using FluentAssertions;
using Xunit;
using Octokit;
using System.Linq;

namespace BotANick.Tests.Discord
{
    public class GitHubTests
    {
        private const string defaultIssueTitle = "Testing the GitHub Issue API.";

        [Fact, Trait("Category", "RequireAPIKeysCategory")]
        public async Task ShouldCreateIssueInGitHubFromIdee()
        {
            BotANick.Discord.Program.Main(new string[] { });
            var gitHubService = new GitHubService();

            const string expectedDescription = defaultIssueTitle;
            Idee idee = new Idee()
            {
                Description = expectedDescription,
            };

            var createdIssue = await gitHubService.AddIssueBasedOnIdee(idee);

            var issue = await gitHubService.GetLastIssue();

            issue.Title.Should().Be(expectedDescription);
            await ClearIssue(gitHubService, createdIssue);
        }

        [Fact, Trait("Category", "RequireAPIKeysCategory")]
        public async Task ShouldGitHubIdeeHaveLabel()
        {
            BotANick.Discord.Program.Main(new string[] { });
            var gitHubService = new GitHubService();
            var expectedLabel = await gitHubService.Client.Issue.Labels.Get("kitarsh", "botanick", "Idee");
            Idee idee = new Idee();

            var createdIssue = await gitHubService.AddIssueBasedOnIdee(idee);
            var issue = await gitHubService.GetLastIssue();

            issue.Labels.Select(l => l.Id).Should().Contain(expectedLabel.Id);
            await ClearIssue(gitHubService, createdIssue);
        }

        [Fact, Trait("Category", "RequireAPIKeysCategory")]
        public async Task ShouldDetectIfIdeeIsAlreadyGitHubIssue()
        {
            BotANick.Discord.Program.Main(new string[] { });
            var gitHubService = new GitHubService();
            var random = new Random();
            Idee idee = new Idee() { Description = random.NextDouble().ToString(), };

            var createdIssue = await gitHubService.AddIssueBasedOnIdee(idee);
            bool isAnIssue = await gitHubService.IsIdeeAnIssue(idee);

            isAnIssue.Should().BeTrue();
            await ClearIssue(gitHubService, createdIssue);
        }

        [Fact, Trait("Category", "RequireAPIKeysCategory")]
        public async Task ShouldSyncIdeeWithGitHubIssues()
        {
            BotANick.Discord.Program.Main(new string[] { });
            var gitHubService = new GitHubService();
            var random = new Random();
            var expectedDescription = random.NextDouble().ToString();
            Idee idee = new Idee() { Description = expectedDescription, };
            List<Idee> idees = new List<Idee> { idee };

            await gitHubService.CreateGitHubIssuesForMissingIdees(idees);

            var createdIssue = await gitHubService.GetLastIssue();
            createdIssue.Title.Should().Be(expectedDescription);
            await ClearIssue(gitHubService, createdIssue);
        }

        private static async Task ClearIssue(GitHubService gitHubService, Issue createdIssue)
        {
            var update = createdIssue.ToUpdate();
            update.State = ItemState.Closed;
            update.Title = defaultIssueTitle;
            update.RemoveLabel("Idee");
            update.AddLabel("TestingAPI");
            await gitHubService.Client.Issue.Update("kitarsh", "botanick", createdIssue.Number, update);
        }
    }
}
