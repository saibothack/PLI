using Android.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(PLI.Droid.Tracking))]
namespace PLI.Droid
{
    public class Tracking : Utilities.ITracking
    {
        Intent startServiceIntent;
        Intent stopServiceIntent;
        public Context objContext { get; set; }

        public Tracking() {
            objContext = Forms.Context;
        }

        public void Initial()
        {

            startServiceIntent = new Intent(objContext, typeof(TimestampService));
            startServiceIntent.SetAction(Utilities.Constants.ACTION_START_SERVICE);

            stopServiceIntent = new Intent(objContext, typeof(TimestampService));
            stopServiceIntent.SetAction(Utilities.Constants.ACTION_STOP_SERVICE);


            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                objContext.StartForegroundService(startServiceIntent);
            }
            else
            {
                objContext.StartService(startServiceIntent);
            }
        }

        public void Stop()
        {
            objContext.StopService(stopServiceIntent);
        }
    }
}