using Lucet.CherryBranch.Utilities;
using Lucet.CherryBranch.DataModel;
using Api = Lucet.CherryBranch.API.DataModel;
using System.Reflection;

namespace Lucet.CherryBranch.App.Services
{
    public class RepoService
    {
        #region Private Fields

        private readonly HttpClient _httpClient;
        private readonly Uri? _baseAddress;

        #endregion Private Fields

        #region Constructors

        public RepoService(IConfiguration configurationManager, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RepoClient");
            _baseAddress = _httpClient.BaseAddress;
        }

        #endregion Constructors

        #region Public Methods

        public async Task<List<BranchEntity>> GetBranchList()
        {
            var response = await MakeRequest<Api.BranchGetListResponse>($"{_baseAddress}api/Branch/GetBranchList");

            return response.ToBranchList();
        }

        public async Task<List<CommitEntity>> GetCommitList(string branchName)
        {
            var response = await MakeRequest<Api.CommitGetListResponse>($"{_baseAddress}api/Branch/GetCommitList/{branchName.Replace(@"/", "%2F")}");

            return response.ToCommitList();
        }

        public async Task BranchAppend(string targetBranchId, string targetBranchName, string newBranchName, List<BranchEntity> commitList)
        {
            var request = new Api.BranchAppendRequest()
            {
                Name = newBranchName,
                Id = targetBranchId,
                TargetBranch = targetBranchName
            };

            foreach (var item in commitList)
            {
                request.CommitList.Add(new() { CommitId = item.Id });
            }

            await MakeRequest<Api.BranchGetListResponse, Api.BranchAppendRequest>(request, $"{_baseAddress}api/Branch/BranchAppend");

            Logger.StampTrace($"BranchAppend | {targetBranchId} | {targetBranchName} | {newBranchName}");
        }

        public async Task BranchAppend(string targetBranchId, string targetBranchName, string newBranchName, List<CommitEntity> commitList)
        {
            var request = new Api.BranchAppendRequest()
            {
                Name = newBranchName,
                Id = targetBranchId,
                TargetBranch = targetBranchName
            };

            foreach (var item in commitList)
            {
                request.CommitList.Add(new() { CommitId = item.CommitId });
            }

            await MakeRequest<Api.BranchGetListResponse, Api.BranchAppendRequest>(request, $"{_baseAddress}api/Branch/BranchAppend");

            Logger.StampTrace($"BranchAppend | {targetBranchId} | {targetBranchName} | {newBranchName}");
        }

        #endregion Public Methods

        #region Private Methods

        #region MakeRequest

        private async Task<T> MakeRequest<T>(string requestUri) where T : new()
        {
            T? entity = default;

            try
            {
                var result = await _httpClient.GetAsync(requestUri);

                result.EnsureSuccessStatusCode();

                entity = await result.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace} . {MethodBase.GetCurrentMethod()?.DeclaringType?.Name} . {MethodBase.GetCurrentMethod()?.Name} | {requestUri} | Exception", ex);
            }

            return entity ?? new();
        }

        private async Task MakeRequest<TRequest>(TRequest request, string requestUri)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(requestUri, request);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace} . {MethodBase.GetCurrentMethod()?.DeclaringType?.Name} . {MethodBase.GetCurrentMethod()?.Name} | {requestUri} | Exception", ex);
            }
        }

        /// <summary>
        /// Make Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private async Task<T> MakeRequest<T, TRequest>(TRequest request, string requestUri) where T : new()
        {
            T? entity = default;

            try
            {
                var result = await _httpClient.PostAsJsonAsync(requestUri, request);

                result.EnsureSuccessStatusCode();

                entity = await result.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name} | {requestUri} | Exception", ex);
            }

            return entity ?? new();
        }

        #endregion MakeRequest

        #endregion Private Methods
    }
}
