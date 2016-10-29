using Android.OS;

namespace Beats.Xamarin.Droid.App.Services
{
    public class AudioServiceBinder : Binder
    {
        private AudioService _service;

        public AudioServiceBinder(AudioService service)
        {
            _service = service;
        }

        public AudioService GetAudioService()
        {
            return _service;
        }
    }
}