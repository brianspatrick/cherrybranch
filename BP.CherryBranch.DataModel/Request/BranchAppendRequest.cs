namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchAppendRequest
    {
        public string Id { get; set; } = string.Empty;

        public string TargetBranch { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public List<BranchCreateCommitEntity> CommitList { get; set; } = [];

        public BranchCreateRequest ToCreateRequest()
        {
            var request = new BranchCreateRequest()
            {
                TargetBranch = TargetBranch,
                Name = Name
            };

            foreach (var item in CommitList)
            {
                request.CommitList.Add(new BranchCreateCommitEntity() { CommitId = item.CommitId });
            }

            return request;
        }
    }

    public class BranchAppendCommitEntity
    {
        public string CommitId { get; set; } = string.Empty;
    }
}
