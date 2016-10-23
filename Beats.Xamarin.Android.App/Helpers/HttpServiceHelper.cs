using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace Beats.Xamarin.Android.App.Helpers
{
    public class HttpServiceHelperResponse
    {
        public Dictionary<string, Cookie> Cookies { get; set; }

        public HttpContent Content { get; set; }
    }

    public class HttpServiceHelper : IDisposable
    {
        private HttpClient _httpClient;
        private CookieContainer _cookieContainer;

        public HttpServiceHelper()
        {
            _cookieContainer = new CookieContainer();

            HttpClientHandler handler = new AndroidClientHandler()
            {
                CookieContainer = _cookieContainer,
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task<HttpServiceHelperResponse> PostAsync(string uri, Dictionary<string, string> postValues)
        {
            var formEncodedValues = new FormUrlEncodedContent(postValues);

            var msg = new HttpRequestMessage(HttpMethod.Post, uri);
            msg.Content = formEncodedValues;

            var response = await _httpClient.SendAsync(msg);

            return new HttpServiceHelperResponse
            {
                Content = response.Content,
                Cookies = _cookieContainer
                    .GetCookies(new Uri(uri))
                    .Cast<Cookie>()
                    .ToDictionary(cookie => cookie.Name),
            };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
