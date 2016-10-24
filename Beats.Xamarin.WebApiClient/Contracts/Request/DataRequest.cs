using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Request
{
    public class DataRequest<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}