namespace Lucet.CherryBranch.DataModel
{
    public class SettingsEntity
    {
        public string RepositoryType { get; set; } = string.Empty;

        public string BaseAddress { get; set; } = string.Empty;

        public string RepositoryId { get; set; } = string.Empty;

        public string RepositoryPrefix { get; set; } = string.Empty;

        public string AuthorizationToken { get; set; } = string.Empty;

        public string GetBranchListUri { get; set; } = string.Empty;

        public string GetCommitListUri { get; set; } = string.Empty;

        public string BranchDeleteUri { get; set; } = string.Empty;

        public string BranchCreateUri { get; set; } = string.Empty;
    }
}
