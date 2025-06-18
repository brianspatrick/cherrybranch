namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchDeleteRequest
    {
        public List<BranchEntity> BranchList { get; set; } = [];
    }

    public class BranchEntity
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
