using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Request
{
    public class Search
    {
        [JsonProperty("searchstring")]
        public string SearchString { get; set; }
    }
}