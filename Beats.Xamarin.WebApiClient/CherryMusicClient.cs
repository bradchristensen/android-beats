using Beats.Xamarin.WebApiClient.Contracts.Request;
using Beats.Xamarin.WebApiClient.Contracts.Response;
using Beats.Xamarin.WebApiClient.Exceptions;
using Beats.Xamarin.WebApiClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Beats.Xamarin.WebApiClient
{
    public class CherryMusicClient : IDisposable
    {
        private readonly HttpServiceHelper _httpClient;
        private string _sessionId;
        private string _server;

        public CherryMusicClient()
        {
            _httpClient = new HttpServiceHelper();
        }

        public CherryMusicClient(string sessionId, string server)
        {
            _sessionId = sessionId;
            _server = server;
            _httpClient = new HttpServiceHelper(sessionId, server);
        }

        public async Task<Cookie> Authenticate(string server, string username, string password)
        {
            var response = await _httpClient.PostAsync(server, new Dictionary<string, string>
            {
                ["username"] = username,
                ["password"] = password,
                ["login"] = "login",
            });

            var content = await response.Content.ReadAsStringAsync();

            if (content.Contains("<title>CherryMusic | Login</title>") || response.StatusCode != HttpStatusCode.OK)
            {
                throw new LoginFailedException();
            }

            _sessionId = response.Cookies["session_id"].Value;
            _server = server.TrimEnd('/');

            return response.Cookies["session_id"];
        }

        public async Task Heartbeat()
        {
            var response = await _httpClient.PostAsync($"{_server}/api/heartbeat", new { });
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new SessionTimeoutException();
            }
        }

        public async Task<DirectoryListing> GetDirectoryListing()
        {
            return await _httpClient.PostAsync<DirectoryListing>(
                $"{_server}/api/compactlistdir",
                new CompactListDirectory { Directory = ".", FilterString = "" }
            );
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
