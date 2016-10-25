using Beats.Xamarin.WebApiClient.Contracts.Response;
using Beats.Xamarin.WebApiClient.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Android.Net;

namespace Beats.Xamarin.WebApiClient.Helpers
{
    internal class HttpServiceHelperResponse
    {
        public Dictionary<string, Cookie> Cookies { get; set; }

        public HttpContent Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    internal class HttpServiceHelper : IDisposable
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

        public HttpServiceHelper(string sessionId, string server) : this()
        {
            var cookies = _cookieContainer
                .GetCookies(new Uri(server))
                .Cast<Cookie>()
                .ToDictionary(cookie => cookie.Name);
            if (cookies.Count == 0)
            {
                _cookieContainer.Add(new Cookie("session_id", sessionId, "/", new Uri(server).Host));
            }
        }

        private async Task<HttpServiceHelperResponse> SendAsync(HttpMethod method, string uri, HttpContent postBody)
        {
            var msg = new HttpRequestMessage(method, uri);
            msg.Content = postBody;

            var response = await _httpClient.SendAsync(msg);

            return new HttpServiceHelperResponse
            {
                Content = response.Content,
                Cookies = _cookieContainer
                    .GetCookies(new Uri(uri))
                    .Cast<Cookie>()
                    .GroupBy(cookie => cookie.Name)
                    .Select(cookie => cookie.First())
                    .ToDictionary(cookie => cookie.Name),
                StatusCode = response.StatusCode,
            };
        }

        public async Task<HttpServiceHelperResponse> PostAsync(string uri, Dictionary<string, string> postValues)
        {
            var formEncodedValues = new FormUrlEncodedContent(postValues);

            return await SendAsync(HttpMethod.Post, uri, formEncodedValues);
        }

        public async Task<HttpServiceHelperResponse> PostAsync(string uri, object serializableObject)
        {
            var postValues = new Dictionary<string, string>()
            {
                { "data", JsonConvert.SerializeObject(serializableObject) }
            };

            return await PostAsync(uri, postValues);
        }

        public async Task<T> PostAsync<T>(string uri, object deserializableObject)
        {
            var response = await PostAsync(uri, deserializableObject);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var serializableResult = await response.Content.ReadAsStringAsync();
                var deserializedResult = JsonConvert.DeserializeObject<DataResponse<T>>(serializableResult);
                return deserializedResult.Data;
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default(T);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException($"Server returned a negative status code ({response.StatusCode})");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SessionTimeoutException();
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// A compatibility method for response objects that are returned like:
        /// data: "jsonObject" instead of data: jsonObject
        /// therefore the "jsonObject" string needs to be deserialized again into the actual object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="deserializableObject"></param>
        /// <returns></returns>
        public async Task<T> PostAsyncAndDeserializeTwice<T>(string uri, object deserializableObject)
        {
            var response = await PostAsync<string>(uri, deserializableObject);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
