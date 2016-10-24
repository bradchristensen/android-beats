using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Request
{
    public class CompactListDirectory
    {
        [JsonProperty("directory")]
        public string Directory { get; set; }

        [JsonProperty("filterstr")]
        public string FilterString { get; set; }
    }
}