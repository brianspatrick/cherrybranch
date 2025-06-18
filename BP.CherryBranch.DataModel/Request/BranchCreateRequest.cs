namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchCreateRequest
    {
        public string TargetBranch { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public List<BranchCreateCommitEntity> CommitList { get; set; } = [];
    }

    public class BranchCreateCommitEntity
    {
        public string CommitId { get; set; } = string.Empty;
    }
}
