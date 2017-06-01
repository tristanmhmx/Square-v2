using System;
using Android.App;
using Android.Content;
using Android.Gms.Gcm;

namespace Square.Droid.Services
{
    [Service(Exported = false), IntentFilter(new [] {"com.google.android.c2dm.intent.RECEIVE"})]
    public class NotificationsListenerService : GcmListenerService
    {
        public override void OnMessageReceived(string from, Android.OS.Bundle data)
        {
            var message = data.GetString("message");
            SendNotification(message);
        }


        void SendNotification(string message)
        {
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(this)
                                                      .SetSmallIcon(Resource.Drawable.icon)
                                                      .SetContentTitle("Square")
                                                      .SetContentText(message)
                                                      .SetAutoCancel(true)
                                                      .SetContentIntent(pendingIntent);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}
