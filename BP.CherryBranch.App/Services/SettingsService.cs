using Lucet.CherryBranch.Utilities;
using Lucet.CherryBranch.DataModel;
using System.Reflection;

namespace Lucet.CherryBranch.App.Services
{
    public class SettingsService
    {
        #region Private Fields

        private readonly HttpClient _httpClient;
        private readonly Uri? _baseAddress;

        #endregion Private Fields

        #region Constructors

        public SettingsService(IConfiguration configurationManager, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RepoClient");
            _baseAddress = _httpClient.BaseAddress;
        }

        #endregion Constructors

        #region Public Methods

        public async Task<SettingsEntity> GetSettings()
        {
            return await MakeRequest<SettingsEntity>($"{_baseAddress}api/Settings/GetSettings");
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
