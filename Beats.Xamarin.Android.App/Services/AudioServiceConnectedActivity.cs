using Android.App;

namespace Beats.Xamarin.Droid.App.Services
{
    public abstract class AudioServiceConnectedActivity : Activity
    {
        public bool IsBound { get; set; }

        public AudioService BoundAudioService { get; set; }
    }
}