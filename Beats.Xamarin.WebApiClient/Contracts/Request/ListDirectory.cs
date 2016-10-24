using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Request
{
    public class ListDirectory
    {
        [JsonProperty("directory")]
        public string Directory { get; set; }
    }
}