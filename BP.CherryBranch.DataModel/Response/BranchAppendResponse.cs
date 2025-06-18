
namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchAppendResponse
    {
        public BranchAppendResponse()
        {
            // ...
        }

        public BranchAppendResponse(BranchCreateResponse response, string repoPrefix)
        {
            Name = response.Name;
            CherryPickId = response.CherryPickId;
            Url = response.Url;
            Status = response.Status;
            TargetBranch = response.TargetBranch.Replace(repoPrefix, string.Empty);
            GeneratedName = response.GeneratedName.Replace(repoPrefix, string.Empty);
        }

        public int? CherryPickId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string TargetBranch { get; set; } = string.Empty;

        public string GeneratedName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
