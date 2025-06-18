using System.Text.Json.Serialization;
using Models = Lucet.CherryBranch.API.DataModel;

namespace Lucet.CherryBranch.API.DataModel.ADO
{
    public class BranchDeleteRequest
    {
        public static List<BranchDeleteRequest> ToBranchDeleteRequest(Models.BranchDeleteRequest request, string repoPrefix)
        {
            var list = new List<BranchDeleteRequest>();

            foreach (var item in request.BranchList)
            {
                var entity = new BranchDeleteRequest()
                {
                    OldObjectId = item.Id,
                    Name = $"{repoPrefix}{item.Name}",
                    NewObjectId = "0000000000000000000000000000000000000000"
                };

                list.Add(entity);
            }

            return list;
        }

        public string Name { get; set; } = string.Empty;

        public string OldObjectId { get; set; } = string.Empty;

        public string NewObjectId { get; set; } = string.Empty;
    }

    public class BranchDeleteResponse
    {
        [JsonPropertyName("value")]
        public List<BranchDeleteInfo> Results { get; set; } = [];

        public Models.BranchDeleteResponse ToBranchDeleteResponse(string repoPrefix)
        {
            var response = new Models.BranchDeleteResponse();

            foreach (var item in Results)
            {
                response.Results.Add(new Models.BranchDeleteInfo()
                {
                    Name = item.Name.Replace(repoPrefix, string.Empty),
                    Success = item.Success,
                    UpdateStatus = item.UpdateStatus,
                });
            }

            return response;
        }
    }

    public class BranchDeleteInfo
    {
        public string RepositoryId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string UpdateStatus { get; set; } = string.Empty;

        public bool Success { get; set; }
    }

    public class BranchCreateRequest
    {
        public BranchCreateRequest(Models.BranchCreateRequest request, string repoPrefix, string repositoryId) 
        {
            OnToRefName = $"{repoPrefix}{request.TargetBranch}";
            GeneratedRefName = $"{repoPrefix}{request.Name}";
            Repository.Name = repositoryId;

            foreach (var item in request.CommitList)
            {
                Source.CommitList.Add(new BranchCreateCommitEntity() { CommitId = item.CommitId });
            }
        }

        public string OnToRefName { get; set; } = string.Empty;

        public string GeneratedRefName { get; set; } = string.Empty;

        public BranchCreateSourceEntity Source { get; set; } = new();

        public RepositoryInfo Repository { get; set; } = new();
    }

    public class BranchCreateResponse
    {
        public int? CherryPickId { get; set; }

        public BranchCreateParameter Parameters { get; set; } = new();

        public string Status { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public Models.BranchCreateResponse ToBranchCreateResponse(string repoPrefix)
        {
            return new Models.BranchCreateResponse()
            {
                CherryPickId = CherryPickId,
                Url = Url,
                Status = Status,
                TargetBranch = Parameters.OnToRefName.Replace(repoPrefix, string.Empty),
                GeneratedName = Parameters.GeneratedRefName.Replace(repoPrefix, string.Empty),
                Name = Parameters.Repository.Name,
            };
        }
    }

    public class BranchCreateParameter
    {
        public BranchCreateRepository Repository { get; set; } = new();

        public string OnToRefName { get; set; } = string.Empty;

        public string GeneratedRefName { get; set; } = string.Empty;
    }

    public class BranchCreateRepository
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }

    public class BranchCreateSourceEntity
    {
        public List<BranchCreateCommitEntity> CommitList { get; set; } = [];
    }

    public class BranchCreateCommitEntity
    {
        public string CommitId { get; set; } = string.Empty;
    }

    public class RepositoryInfo
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetBranchListResponse
    {
        [JsonPropertyName("value")]
        public List<BranchInfo> Results { get; set; } = [];

        public Models.BranchGetListResponse ToBranchListResponse(string repoPrefix)
        {
            var response = new Models.BranchGetListResponse();

            foreach (var item in Results)
            {
                var result = new Models.BranchInfo()
                {
                    ObjectId = item.ObjectId,
                    Name = item.Name.Replace(repoPrefix, string.Empty),
                    Url = item.Url
                };

                var creator = new Models.BranchCreator()
                {
                    Id = item.Creator.Id,
                    Name = item.Creator.DisplayName,
                    UniqueName = item.Creator.UniqueName
                };
                result.Creator = creator;

                response.Results.Add(result);
            }

            return response;
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

        public string DisplayName { get; set; } = string.Empty;

        public string UniqueName { get; set; } = string.Empty;
    }

    public class GetCommitListResponse
    {
        [JsonPropertyName("value")]
        public List<CommitInfo> Results { get; set; } = [];

        public Models.CommitGetListResponse ToCommitListResponse()
        {
            var response = new Models.CommitGetListResponse();

            foreach (var item in Results)
            {
                var result = new Models.CommitInfo()
                {
                    CommitId = item.CommitId,
                    Name = item.Author.Name,
                    Email = item.Author.Email,
                    Date = item.Author.Date,
                    Comment = item.Comment,
                    Url = item.Url
                };

                response.Results.Add(result);
            }

            return response;
        }
    }

    public class CommitInfo
    {
        public string CommitId { get; set; } = string.Empty;

        public CommitAuthor Author { get; set; } = new();

        public string Comment { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;
    }

    public class CommitAuthor
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;
    }
}
