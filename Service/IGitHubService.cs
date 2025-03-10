using Octokit;
using Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        public Task<List<RepositoryInfo>> GetPortfolio();
        public Task<List<Activity>> GetEvents();
        public Task<List<Repository>> SearchRepositories(string language = null, string name = null, string userName = null);
    }
}
