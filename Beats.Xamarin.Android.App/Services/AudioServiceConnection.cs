using Android.Content;
using Android.OS;

namespace Beats.Xamarin.Droid.App.Services
{
    public class AudioServiceConnection : Java.Lang.Object, IServiceConnection
    {
        AudioServiceConnectedActivity _activity;

        public AudioServiceConnection(AudioServiceConnectedActivity activity)
        {
            _activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var binder = (AudioServiceBinder)service;
            _activity.BoundAudioService = binder.GetAudioService();
            _activity.IsBound = true;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _activity.IsBound = false;
        }
    }
}