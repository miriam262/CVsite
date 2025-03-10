using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Entities;

namespace CVsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;
        public RepoController(IGitHubService gitHubService) 
        { 
            _gitHubService = gitHubService;
        }
        [HttpGet]
        public async Task<List<RepositoryInfo>> getPortfolio()
        {
            return await _gitHubService.GetPortfolio();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories([FromQuery] string language = null, [FromQuery] string name = null, [FromQuery] string userName = null)
        {
            try
            {
                var repositories = await _gitHubService.SearchRepositories(language, name, userName);

                if (repositories == null || repositories.Count == 0)
                {
                    return NotFound("No repositories found matching the criteria.");
                }

                return Ok(repositories);
            }
            catch (System.ArgumentException ex) // טיפול בשגיאות אם יש בעיה בהמרת השפה
            {
                return BadRequest($"Invalid language parameter: {ex.Message}");
            }
        }
    }
}
