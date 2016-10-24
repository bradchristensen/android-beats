using Newtonsoft.Json;
using System.Collections.Generic;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class DirectoryListing : List<DirectoryListingItem>
    {
    }

    public class DirectoryListingItem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("urlpath")]
        public string UrlPath { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("filescount")]
        public int FilesCount { get; set; }

        [JsonProperty("filescountestimate")]
        public bool FilesCountEstimate { get; set; }

        [JsonProperty("foldercount")]
        public int FolderCount { get; set; }
    }
}