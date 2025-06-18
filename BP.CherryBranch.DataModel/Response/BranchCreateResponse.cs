
namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchCreateResponse
    {
        public int? CherryPickId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string TargetBranch { get; set; } = string.Empty;

        public string GeneratedName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
 
        public string Url { get; set; } = string.Empty;
    }
}
