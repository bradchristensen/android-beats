using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class TrackMetadata
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("track")]
        public string TrackNumber { get; set; }

        [JsonProperty("length")]
        public decimal Length { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}