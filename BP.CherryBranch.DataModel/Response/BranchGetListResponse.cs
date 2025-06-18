using Models = Lucet.CherryBranch.DataModel;

namespace Lucet.CherryBranch.API.DataModel
{
    public class BranchGetListResponse
    {
        public List<BranchInfo> Results { get; set; } = [];

        public List<Models.BranchEntity> ToBranchList()
        {
            var list = new List<Models.BranchEntity>();

            foreach (var item in Results)
            {
                list.Add(new Models.BranchEntity()
                {
                    Id = item.ObjectId,
                    Name = item.Name,
                    Url = item.Url,
                    DisplayName = item.Creator.Name,
                    UserName = item.Creator.UniqueName
                });
            }

            return list;
        }
    }

    public class BranchInfo
    {
        public string ObjectId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public BranchCreator Creator { get; set; } = new();

        public string Url { get; set; } = string.Empty;
    }

    public class BranchCreator
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string UniqueName { get; set; } = string.Empty;
    }
}
