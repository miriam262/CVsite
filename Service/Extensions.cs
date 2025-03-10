using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class Extensions
    {
        public static void AddGitHubIntegration(this IServiceCollection services, Action<GitHubIntegrationsOptions>option)
        {
            services.Configure(option);
            services.AddScoped<IGitHubService,GithubService>();
        }
    }
}
