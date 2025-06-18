using Microsoft.AspNetCore.Mvc;
using Lucet.CherryBranch.API.DataModel;
using Lucet.CherryBranch.API.Contracts;

namespace Lucet.CherryBranch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController(IRepository service) : ControllerBase
    {
        private readonly IRepository _service = service;

        [HttpGet("GetBranchList")]
        public async Task<BranchGetListResponse> GetBranchList()
        {
            return await _service.GetBranchList();
        }

        [HttpGet("GetCommitList/{branchName}")]
        public async Task<CommitGetListResponse> GetCommitList(string branchName)
        {
            return await _service.GetCommitList(branchName);
        }

        [HttpPost("BranchDelete")]
        public async Task<BranchDeleteResponse> BranchDelete(BranchDeleteRequest request)
        {
            return await _service.BranchDelete(request);
        }

        [HttpPost("BranchCreate")]
        public async Task<BranchCreateResponse> BranchCreate(BranchCreateRequest request)
        {
            return await _service.BranchCreate(request);
        }

        [HttpPost("BranchAppend")]
        public async Task<BranchAppendResponse> BranchAppend(BranchAppendRequest request)
        {
            return await _service.BranchAppend(request);
        }
    }
}
