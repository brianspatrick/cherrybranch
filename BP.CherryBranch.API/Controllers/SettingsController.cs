using Microsoft.AspNetCore.Mvc;
using Lucet.CherryBranch.DataModel;

namespace Lucet.CherryBranch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("GetSettings")]
        public async Task<SettingsEntity> GetSettings()
        {
            var entity = new SettingsEntity();

            await Task.Run(() =>
            {
                var section = _configuration.GetSection("RepoApi");
                var methods = section.GetSection("Methods");

                entity = new SettingsEntity()
                {
                    RepositoryType = section.GetValue<string>("RepoType") ?? string.Empty,
                    RepositoryId = section.GetValue<string>("RepositoryId") ?? string.Empty,
                    RepositoryPrefix = section.GetValue<string>("RepoPrefix") ?? string.Empty,
                    BaseAddress = section.GetValue<string>("BaseAddress") ?? string.Empty,
                    AuthorizationToken = section.GetValue<string>("AuthorizationToken") ?? string.Empty,
                    GetBranchListUri = methods.GetValue<string>("GetBranchList") ?? string.Empty,
                    GetCommitListUri = methods.GetValue<string>("GetCommitList") ?? string.Empty,
                    BranchDeleteUri = methods.GetValue<string>("BranchDelete") ?? string.Empty,
                    BranchCreateUri = methods.GetValue<string>("BranchCreate") ?? string.Empty
                };
            });

            return entity;
        }
    }
}
