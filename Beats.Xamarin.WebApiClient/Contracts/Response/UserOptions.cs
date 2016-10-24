using Newtonsoft.Json;

namespace Beats.Xamarin.WebApiClient.Contracts.Response
{
    public class UserOptions
    {
        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("misc")]
        public Miscellaneous Miscellaneous { get; set; }

        [JsonProperty("custom_theme")]
        public CustomTheme CustomTheme { get; set; }

        [JsonProperty("keyboard_shortcuts")]
        public KeyboardShortcuts KeyboardShortcuts { get; set; }

        [JsonProperty("ui")]
        public UserInterface UserInterface { get; set; }
    }

    public class Media
    {
        [JsonProperty("may_download")]
        public bool MayDownload { get; set; }

        [JsonProperty("force_transcode_to_bitrate")]
        public int ForceTranscodeToBitrate { get; set; }
    }

    public class Miscellaneous
    {
        [JsonProperty("show_playlist_download_buttons")]
        public bool ShowPlaylistDownloadButtons { get; set; }

        [JsonProperty("autoplay_on_add")]
        public bool AutoplayOnAdd { get; set; }
    }

    public class CustomTheme
    {
        [JsonProperty("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty("white_on_black")]
        public bool WhiteOnBlack { get; set; }
    }

    public class KeyboardShortcuts
    {
        [JsonProperty("search")]
        public short Search { get; set; }

        [JsonProperty("stop")]
        public short Stop { get; set; }

        [JsonProperty("next")]
        public short Next { get; set; }

        [JsonProperty("prev")]
        public short Previous { get; set; }

        [JsonProperty("play")]
        public short Play { get; set; }

        [JsonProperty("pause")]
        public short Pause { get; set; }
    }

    public class UserInterface
    {
        [JsonProperty("display_album_art")]
        public bool DisplayAlbumArt { get; set; }

        [JsonProperty("confirm_quit_dialog")]
        public bool ConfirmQuitDialog { get; set; }
    }
}