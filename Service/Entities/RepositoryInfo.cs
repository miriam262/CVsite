using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entities
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public string LastCommit { get; set; }
        public string RepoUrl { get; set; }
    }
}
