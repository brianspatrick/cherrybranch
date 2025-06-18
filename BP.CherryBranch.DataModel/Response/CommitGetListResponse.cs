using Models = Lucet.CherryBranch.DataModel;

namespace Lucet.CherryBranch.API.DataModel
{
    public class CommitGetListResponse
    {
        public List<CommitInfo> Results { get; set; } = [];

        public List<Models.CommitEntity> ToCommitList()
        {
            var list = new List<Models.CommitEntity>();

            foreach (var item in Results)
            {
                list.Add(new Models.CommitEntity()
                {
                    CommitId = item.CommitId,
                    Name = item.Name,
                    Url = item.Url,
                    Email = item.Email,
                    Date = item.Date,
                    Comment = item.Comment
                });
            }

            return list;
        }
    }

    public class CommitInfo
    {
        public string CommitId { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
