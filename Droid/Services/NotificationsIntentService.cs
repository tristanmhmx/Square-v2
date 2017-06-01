using System;
using Android.App;
using Android.Content;
using Android.Gms.Gcm;
using Android.Gms.Iid;

namespace Square.Droid.Services
{
    [Service(Exported = false)]
    public class NotificationsIntentService : IntentService
    {
        private static object locker = new object();
        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                lock(locker)
                {
                    var instanceId = InstanceID.GetInstance(this);
                    var token = instanceId.GetToken("792341316392", GoogleCloudMessaging.InstanceIdScope, null);
                    //Send token to service (azure, amazon)
                    Subscribe(token);
                }
                
            }
            catch(Exception e)
            {
                
            }
        }
        void Subscribe(string token)
        {
            var pubSub = GcmPubSub.GetInstance(this);
            pubSub.Subscribe(token, "/topics/global", null);
        }
    }
}
