using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Beats.Xamarin.Datastore;
using System.Collections.Generic;

namespace Beats.Xamarin.Droid.App.Services
{
    [Service]
    public class AudioService : Service, MediaPlayer.IOnPreparedListener
    {
        public static readonly string ACTION_PLAY = "beats.xamarin.droid.app.PLAY";

        private MediaPlayer _player = null;
        private IBinder _binder = null;
        private Repository _repository = new Repository();
        private int? _currentNotificationId = null;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Start();
        }

        public override IBinder OnBind(Intent intent)
        {
            if (_binder == null)
            {
                _binder = new AudioServiceBinder(this);
            }

            return _binder;
        }

        public void PlayTrack(string path)
        {
            var deets = _repository.LoginDetails;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Cookie", $"session_id={deets.SessionId}\r\n");

            var uri = Android.Net.Uri.Parse($"{deets.ServerUrl}/serve/{path}");
            _player = new MediaPlayer();
            _player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);
            _player.SetOnPreparedListener(this);
            _player.SetAudioStreamType(Stream.Music);
            _player.SetDataSource(ApplicationContext, uri, headers);
            _player.PrepareAsync();

            var pendingIntent = PendingIntent.GetActivity(
                ApplicationContext,
                0,
                new Intent(ApplicationContext, typeof(MainActivity)),
                PendingIntentFlags.UpdateCurrent
            );

            var builder = new NotificationCompat.Builder(this);
            var notification = builder
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetTicker(new Java.Lang.String(path))
                .SetOngoing(true)
                .SetContentTitle(new Java.Lang.String(path))
                .SetContentText(new Java.Lang.String(path))
                .Build();

            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            _currentNotificationId = (_currentNotificationId ?? 0);
            notificationManager.Notify(_currentNotificationId.Value, notification);
        }
    }
}