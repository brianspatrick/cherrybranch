namespace Lucet.CherryBranch.DataModel
{
    public class BranchEntity
    {
        public string Id { get; set; } = string.Empty;

        public int? CherryPickId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string TargetBranch { get; set; } = string.Empty;

        public string GeneratedName { get; set; } = string.Empty;

        public bool Selected { get; set; } = false;
    }
}
