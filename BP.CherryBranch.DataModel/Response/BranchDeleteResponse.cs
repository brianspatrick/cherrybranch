
namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchDeleteResponse
    {
        public List<BranchDeleteInfo> Results { get; set; } = [];
    }

    public class BranchDeleteInfo
    {
        public string Name { get; set; } = string.Empty;

        public string UpdateStatus { get; set; } = string.Empty;

        public bool Success { get; set; }
    }
}
