using Newtonsoft.Json;
using System.Collections.Generic;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class RestorePlaylist : List<Playlist>
    {
    }

    public class Playlist
    {
        [JsonProperty("closable")]
        public bool Closable { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("reason_open")]
        public string ReasonOpen { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("playlist")]
        public List<PlaylistTrack> Tracks { get; set; }
    }

    public class PlaylistTrack
    {
        [JsonProperty("duration")]
        public decimal Duration { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("meta")]
        public TrackMetadata Metadata { get; set; }
    }
}