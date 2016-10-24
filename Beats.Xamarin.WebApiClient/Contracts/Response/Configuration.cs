using Newtonsoft.Json;
using System.Collections.Generic;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class Configuration
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("fetchalbumart")]
        public bool FetchAlbumArt { get; set; }

        [JsonProperty("auto_login")]
        public bool AutoLogin { get; set; }

        [JsonProperty("getencoders")]
        public List<string> Encoders { get; set; }

        [JsonProperty("getdecoders")]
        public List<string> Decoders { get; set; }

        [JsonProperty("transcodepath")]
        public string TranscodePath { get; set; }

        [JsonProperty("isadmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("servepath")]
        public string ServePath { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("transcodingenabled")]
        public bool TranscodingEnabled { get; set; }
    }
}