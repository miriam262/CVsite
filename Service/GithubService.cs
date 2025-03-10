using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GithubService: IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationsOptions _options;
        //public GithubService(IOptions<GitHubIntegrationsOptions> options)
        //{
        //    var gitHubOptions = options.Value;

        //    if (string.IsNullOrEmpty(gitHubOptions.Token))
        //    {
        //        throw new ArgumentException("GitHub Token is missing in configuration.");
        //    }

        //    _client = new GitHubClient(new ProductHeaderValue("my-CV-site"))
        //    {
        //        Credentials = new Credentials(gitHubOptions.Token) // הכנסת ה-Token ל-GitHubClient
        //    };
        //}

        public GithubService(IOptions<GitHubIntegrationsOptions> options)
        {
            _options = options.Value;
            if (string.IsNullOrEmpty(_options.Token))
            {
                throw new ArgumentException("GitHub Token is missing in configuration.");
            }
            _client = new GitHubClient(new ProductHeaderValue("my-CV-site"))
            {
                Credentials = new Credentials(_options.Token) // הכנסת ה-Token ל-GitHubClient
            };
        }
        public async Task<List<Activity>> GetEvents()
        {
            var acts = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
            return acts.ToList();
        }
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            var acts = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
            var repositories = await _client.Repository.GetAllForCurrent();
            var portFolio = new List<RepositoryInfo>();
            foreach (var repo in repositories)
            {
                var pullRequest = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                portFolio.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Language = repo.Language,
                    Stars = repo.StargazersCount,
                    PullRequests = pullRequest.Count(),
                    LastCommit = repo.PushedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "No commits",
                    RepoUrl = repo.HtmlUrl
                });
            }
            return portFolio;
        }
        public async Task<List<Repository>> SearchRepositories(string language = null, string name = null, string userName = null)
        {
            var request = new SearchRepositoriesRequest(name)
            {
                Language = language != null ? Enum.Parse<Octokit.Language>(language, true) : Octokit.Language.Unknown,
                User = userName
            };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }

    }
}
