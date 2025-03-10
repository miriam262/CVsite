using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service;
using Service.Entities;
using System.Collections.Generic;

namespace CVsite.CachedService
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private const string userPortFolioKey = "userPortFolioKey";
        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            if (_memoryCache.TryGetValue(userPortFolioKey, out List<RepositoryInfo> portfolio) && portfolio != null)
            {
                var lastUpdated = portfolio.Max(m => m.LastCommit);
                portfolio = await _gitHubService.GetPortfolio();
                bool isUpdated = portfolio.Any(r => r.LastCommit > lastUpdated);
                if (!isUpdated)
                {
                    return portfolio;
                }
            }
                portfolio = await _gitHubService.GetPortfolio();
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(250))
                .SetSlidingExpiration(TimeSpan.FromSeconds(90));
            return portfolio;
            }
        public Task<List<Repository>> SearchRepositories(string language = null, string name = null, string userName = null)
        {
            return _gitHubService.SearchRepositories(language, name, userName);
        }

        public Task<List<Activity>> GetEvents()
        {
            return _gitHubService.GetEvents();
        }
    }
}
