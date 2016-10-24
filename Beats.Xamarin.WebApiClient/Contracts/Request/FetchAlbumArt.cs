using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Request
{
    public class FetchAlbumArt
    {
        [JsonProperty("directory")]
        public string Directory { get; set; }
    }
}