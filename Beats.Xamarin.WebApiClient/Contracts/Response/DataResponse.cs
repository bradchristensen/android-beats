using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class DataResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}