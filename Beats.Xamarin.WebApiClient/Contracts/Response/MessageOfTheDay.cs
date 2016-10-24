using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class MessageOfTheDay
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public string Message { get; set; }
    }
}