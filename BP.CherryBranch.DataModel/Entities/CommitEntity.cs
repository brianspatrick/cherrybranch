namespace Lucet.CherryBranch.DataModel
{
    public class CommitEntity
    {
        public string CommitId { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public bool Selected { get; set; } = false;
    }
}
