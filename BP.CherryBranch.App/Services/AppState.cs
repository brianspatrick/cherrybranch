using Lucet.CherryBranch.DataModel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace Lucet.CherryBranch.App.Services
{
    public class AppState
    {
        public List<BranchEntity> BranchList { get; set; } = [];

        public APIService APIService { get; set; }

        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public NavigationManager NavigationManager { get; set; }

        public ConfigurationManager ConfigurationManager { get; set; }

        public AppState(APIService apiService, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager, ConfigurationManager configurationManager)
        {
            APIService = apiService;
            AuthenticationStateProvider = authenticationStateProvider;
            NavigationManager = navigationManager;
            ConfigurationManager = configurationManager;
        }

        public async Task<SettingsEntity> GetSettings()
        {
            return await APIService.SettingsService.GetSettings();
        }

        public async Task<List<BranchEntity>> GetBranchList()
        {
            return await APIService.RepoService.GetBranchList();
        }

        public async Task<List<CommitEntity>> GetCommitList(string branchName)
        {
            return await APIService.RepoService.GetCommitList(branchName);
        }
    }
}
