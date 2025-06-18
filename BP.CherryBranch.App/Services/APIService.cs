using Lucet.CherryBranch.App.Services.Contracts;

namespace Lucet.CherryBranch.App.Services
{
    public class APIService(RepoService repoService, SettingsService settingsService) : IAPIService
    {
        #region Public Properties

        public RepoService RepoService { get; } = repoService;

        public SettingsService SettingsService { get; } = settingsService;

        #endregion Public Properties
    }
}
