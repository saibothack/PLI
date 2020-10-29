using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Java.Lang;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PLI.Droid
{
    [Service]
    public class TimestampService : Service
    {
        public string Tag = "TimestampService";
        const string LocationPackageName = "com.mx.sysware.pli";
        const int NotificationId = 12345678;
        string ChannelId = "channel_01";
        const string ExtraStartedFromNotification = LocationPackageName + ".started_from_notification";
        NotificationManager NotificationManager;
        Handler ServiceHandler;
        Handler ServiceHandlerTracking;
        long TimeSeconds = 0;

        const long TrackingIntervalInMilliseconds = 900000;

        /**
	     * The desired interval for location updates. Inexact. Updates may be more or less frequent.
	     */
        const long UpdateIntervalInMilliseconds = 1000;

        /**
		 * The fastest rate for active location updates. Updates will never be more frequent
		 * than this value.
		 */
        const long FastestUpdateIntervalInMilliseconds = UpdateIntervalInMilliseconds / 2;

        public override void OnCreate()
		{
			base.OnCreate();
            
            NotificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                string name = GetString(Resource.String.app_name);
                NotificationChannel mChannel = new NotificationChannel(ChannelId, name, NotificationImportance.High);
                NotificationManager.CreateNotificationChannel(mChannel);
            }

            RegisterForegroundService();

            StartTimer();
            StartTracking();
            Tracking();
        }

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.Sticky;
		}

		public override IBinder OnBind(Intent intent)
        {
			return null;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

        Notification GetNotification()
        {
            Intent intent = new Intent(this, typeof(TimestampService));

            var text = "Su rastreo esta encendido";

            // Extra to help us figure out if we arrived in onStartCommand via the notification or not.
            intent.PutExtra(ExtraStartedFromNotification, true);

            // The PendingIntent that leads to a call to onStartCommand() in this service.
            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            // The PendingIntent to launch activity.
            var activityPendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .AddAction(Resource.Drawable.ic_launch_custom, GetString(Resource.String.launch_activity),
                    activityPendingIntent)
                .AddAction(Resource.Drawable.ic_cancel, GetString(Resource.String.remove_location_updates),
                    servicePendingIntent)
                .SetContentText(text)
                .SetContentTitle("Rastreo")
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Drawable.ic_launch_custom)
                .SetTicker(text)
                .SetWhen(JavaSystem.CurrentTimeMillis());

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                builder.SetChannelId(ChannelId);
            }

            return builder.Build();
        }

        public void RegisterForegroundService()
        {
            var notification = GetNotification();

            NotificationManager.Notify(NotificationId, notification);
            StartForeground(NotificationId, notification);

        }

        public void StartTimer()
        {
            ServiceHandler = new Handler(Looper.MainLooper);
            ServiceHandler.PostDelayed(() =>
            {
                TimeSeconds++;
                Log.Info(Tag, TimeSeconds.ToString());
                StartTimer();

                ServiceHandler.Dispose();
                ServiceHandler = null;
            }, UpdateIntervalInMilliseconds);
        }

        public void StartTracking()
        {
            ServiceHandlerTracking = new Handler(Looper.MainLooper);
            ServiceHandlerTracking.PostDelayed(async () =>
            {

                await Tracking();
                StartTracking();

                ServiceHandlerTracking.Dispose();
                ServiceHandlerTracking = null;
            }, TrackingIntervalInMilliseconds);

        }

        public async Task Tracking()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Models.Post.Location postLocation = new Models.Post.Location
                    {
                        latitude = location.Latitude,
                        longitude = location.Longitude
                    };

                    await App.oServiceManager.SetLocation(postLocation);

                    Log.Info(Tag, $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Log.Info(Tag, "FeatureNotSupportedException - " + fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Log.Info(Tag, "FeatureNotEnabledException - " + fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                Log.Info(Tag, "PermissionException - " + pEx.Message);
            }
            catch (System.Exception ex)
            {
                Log.Info(Tag, "Exception - " + ex.Message);
            }
        }
    }
}