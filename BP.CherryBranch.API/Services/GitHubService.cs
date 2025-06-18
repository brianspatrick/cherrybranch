namespace Lucet.CherryBranch.API
{
    using System.Net;
    using Lucet.CherryBranch.Utilities;
    using Lucet.CherryBranch.API.DataModel;
    using ADO = DataModel.ADO;
    using System.Text;
    using Lucet.CherryBranch.API.Contracts;

    internal class GitHubService : IRepository
    {
        #region Private Members

        private static HttpClient? _httpClient;

        private static IConfigurationSection? ConfigSection { get; set; }

        private static string RepositoryId { get; set; } = string.Empty;

        private static string RepoPrefix { get; set; } = string.Empty;

        private static HttpClient HttpClient
        {
            get
            {
                return _httpClient ??= CreateHttpClient();
            }
        }

        #endregion Private Members

        #region Public Methods

        public async Task<BranchGetListResponse> GetBranchList()
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task<CommitGetListResponse> GetCommitList(string branchName)
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task<BranchDeleteResponse> BranchDelete(BranchDeleteRequest request)
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task<BranchCreateResponse> BranchCreate(BranchCreateRequest request)
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task<BranchAppendResponse> BranchAppend(BranchAppendRequest request)
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Constructor

        public GitHubService(IConfigurationSection section)
        {
            ConfigSection ??= section;

            Methods.ConfigSection ??= section.GetSection("Methods");

            RepositoryId = section.GetValue<string>("RepositoryId") ?? string.Empty;
            RepoPrefix = section.GetValue<string>("RepoPrefix") ?? string.Empty;

            if (HttpClient != null)
            {
                HttpClient.BaseAddress ??= new Uri(section.GetValue<string>("BaseAddress") ?? string.Empty);

                if (HttpClient.DefaultRequestHeaders.Authorization == null)
                {
                    string encodedToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"user:{section.GetValue<string>("AuthorizationToken") ?? string.Empty}"));
                    HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedToken);
                }
            }
        }

        #endregion Constructor

        #region Private Methods

        private static HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new();

            httpClient.DefaultRequestHeaders.Add("acccept", "text/plain");
            httpClient.DefaultRequestHeaders.Add("Content_Type", "application/json");

            return httpClient;
        }

        private async static Task<T> MakeRequest<T, TRequest>(TRequest request, string methodUri) where T : new()
        {
            string requestUri = $@"{HttpClient.BaseAddress}{methodUri}";

            T? entity = default;

            try
            {
                var result = await HttpClient.PostAsJsonAsync(requestUri, request);

                if (result?.StatusCode == HttpStatusCode.OK || result?.StatusCode == HttpStatusCode.Created)
                {
                    entity = await result.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"LucetService.MakeRequest (POST) | {requestUri} | Exception", ex);
            }

            return entity ?? new();
        }

        private async static Task<T> MakeRequest<T>(string methodUri) where T : new()
        {
            string requestUri = $@"{HttpClient.BaseAddress}{methodUri}";

            T? entity = default;

            try
            {
                var result = await HttpClient.GetAsync(requestUri);

                if (result?.StatusCode == HttpStatusCode.OK || result?.StatusCode == HttpStatusCode.Created)
                {
                    entity = await result.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"LucetService.MakeRequest (GET) | {requestUri} | Exception", ex);
            }

            return entity ?? new();
        }

        #endregion Private Methods

        #region Private Classes

        internal readonly struct Methods
        {
            #region Lucet Method Names

            private readonly struct ADOMethods
            {
                public const string GetBranchList = "GetBranchList";
                public const string GetCommitList = "GetCommitList";
                public const string BranchDelete = "BranchDelete";
                public const string BranchCreate = "BranchCreate";
            }

            #endregion Lucet Method Names

            #region Private Methods

            private static string GetConfigValue(string key)
            {
                return ConfigSection?.GetValue<string>(key) ?? string.Empty;
            }

            #endregion Private Methods

            public static IConfigurationSection? ConfigSection { get; set; }

            public static string GetBranchList => GetConfigValue(ADOMethods.GetBranchList);

            public static string GetCommitList => GetConfigValue(ADOMethods.GetCommitList);

            public static string BranchDelete => GetConfigValue(ADOMethods.BranchDelete);

            public static string BranchCreate => GetConfigValue(ADOMethods.BranchCreate);
        }

        #endregion Private Classes
    }
}
