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
        private Task<List<Activity>> oldActs;
        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
            oldActs = _gitHubService.GetEvents();
        }
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            var oldActsResult = await oldActs; 
            var newActs = await _gitHubService.GetEvents();
            int oldCount = oldActsResult.Count(a => a.Type == "PushEvent" || a.Type == "CreateEvent");

            int newCount = newActs.Count(a => a.Type == "PushEvent" || a.Type == "CreateEvent");

            if (newCount == oldCount && 
                _memoryCache.TryGetValue(userPortFolioKey, out List<RepositoryInfo> portfolio) && portfolio != null)
                return portfolio;
            else
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(250))
                .SetSlidingExpiration(TimeSpan.FromSeconds(90));
                portfolio = await _gitHubService.GetPortfolio();
                _memoryCache.Set(userPortFolioKey, portfolio, cacheOptions);
                oldActs = Task.FromResult(newActs);
                return portfolio;
            } 
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
