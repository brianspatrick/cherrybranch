using Lucet.CherryBranch.API.DataModel;

namespace Lucet.CherryBranch.API.Contracts
{
    public interface IRepository
    {
        Task<BranchGetListResponse> GetBranchList();

        Task<CommitGetListResponse> GetCommitList(string branchName);

        Task<BranchDeleteResponse> BranchDelete(BranchDeleteRequest request);

        Task<BranchCreateResponse> BranchCreate(BranchCreateRequest request);

        Task<BranchAppendResponse> BranchAppend(BranchAppendRequest request);
    }
}
