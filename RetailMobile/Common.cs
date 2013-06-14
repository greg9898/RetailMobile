using System;
using Android.Util;

namespace RetailMobile
{
    public static class Common
    {
        public enum Layouts
        {
            Land
,
            Port
,
            Sw600Land
,
            Sw600Port
        }

        public static int CurrentDealerID = 0;
        public static string DecimalFormat = "######0.0##";
        public static string CurrencyFormat = "######0.00";
        /// <summary>
        /// dd.MM.yyyy
        /// </summary>
        public static string DateFormatDateOnly = "dd.MM.yyyy";

        public static DateTime JavaDateToDatetime(Java.Util.Date date)
        {
            TimeSpan ss = TimeSpan.FromMilliseconds(date.Time);
          
            if (ss.Ticks < 0)
            {
                return DateTime.MinValue;
            }

            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0);//, DateTimeKind.Local);
            DateTime ddd = Jan1st1970.Add(ss);
            return ddd;
            //DateTime final = ddd.ToUniversalTime(); // Change to local-time
            //return new DateTime(621355968000000000L + date.Time * 10000);
        }

        public static void LogException(Java.Lang.Throwable ex)
        { 
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(Android.OS.Environment.ExternalStorageDirectory.ToString());
            if (!myDir.Exists)
                myDir.Create();
            string saveFile = System.IO.Path.Combine(myDir.FullName, "errorLog.txt");
            Java.IO.File logFile = new Java.IO.File(saveFile);
            if (!logFile.Exists())
            {
                try
                {
                    logFile.CreateNewFile();
                }
                catch (Java.IO.IOException e)
                {
                    Android.Util.Log.Error("LogException unable to create file", e.Message);
                }
            }

            Java.IO.PrintWriter pw;
            try
            {
                pw = new Java.IO.PrintWriter(new Java.IO.FileWriter(saveFile, true));
                ex.PrintStackTrace(pw);
                pw.Flush();
                pw.Close();
            }
            catch (Java.IO.IOException e)
            {
                Android.Util.Log.Error("LogException unable to save file", e.Message);
            }
        }

        public static void LogException(Exception ex)
        { 
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(Android.OS.Environment.ExternalStorageDirectory.ToString());
            if (!myDir.Exists)
                myDir.Create();
            string saveFile = System.IO.Path.Combine(myDir.FullName, "errorLog.txt");
            Java.IO.File logFile = new Java.IO.File(saveFile);
            if (!logFile.Exists())
            {
                try
                {
                    logFile.CreateNewFile();
                }
                catch (Java.IO.IOException e)
                {
                    Android.Util.Log.Error("LogException unable to create file", e.Message);
                }
            }

            Java.IO.PrintWriter pw;
            try
            {
                pw = new Java.IO.PrintWriter(new Java.IO.FileWriter(saveFile, true));
                pw.Write(ex.Message);
                pw.Write(ex.StackTrace);
                pw.Flush();
                pw.Close();
            }
            catch (Java.IO.IOException e)
            {
                Android.Util.Log.Error("LogException unable to save file", e.Message);
            }
        }
/// <summary>
        /// Checks if the device is a tablet or a phone
/// </summary>
/// <returns>The tablet device.</returns>
/// <param name="activityContext">Activity context.</param>
        public static bool isTabletDevice(Android.Content.Context activityContext)
        {
            bool isTablet = (int)activityContext.Resources.GetDimension(Resource.Dimension.isTablet) == 600 ? true : false;
        
            Log.Debug("isTabletDevice", "isTablet=" + isTablet);
            return isTablet;

//            Log.Debug("isTabletDevice", "ScreenLayout=" + Enum.GetName(typeof(Android.Content.Res.ScreenLayout), activityContext.Resources.Configuration.ScreenLayout));
//            // Verifies if the Generalized Size of the device is XLARGE to be considered a Tablet
//            bool xlarge = activityContext.Resources.Configuration.ScreenLayout == Android.Content.Res.ScreenLayout.SizeXlarge;
//            Log.Debug("isTabletDevice", "xlarge=" + xlarge);
//
//            // If XLarge, checks if the Generalized Density is at least MDPI (160dpi)
//            if (xlarge)
//            {
//                Android.Util.DisplayMetrics metrics = new  Android.Util.DisplayMetrics();
//                Android.App.Activity activity = (Android.App.Activity)activityContext;
//                activity.WindowManager.DefaultDisplay.GetMetrics(metrics);
//
//                // MDPI=160, DEFAULT=160, DENSITY_HIGH=240, DENSITY_MEDIUM=160,
//                // DENSITY_TV=213, DENSITY_XHIGH=320
//                if (metrics.DensityDpi == Android.Util.DisplayMetricsDensity.Default || metrics.DensityDpi == Android.Util.DisplayMetricsDensity.High
//                    || metrics.DensityDpi == Android.Util.DisplayMetricsDensity.Medium || metrics.DensityDpi == Android.Util.DisplayMetricsDensity.Xhigh
//                    || (int)metrics.DensityDpi == 213)
//                {
//                    Log.Debug("isTabletDevice", "=true");
//                    // Yes, this is a tablet!
//                    return true;
//                }
//            }
//
//            return false;
        }

        public static bool isPortrait(Android.Content.Context activityContext)
        {
            return  activityContext.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait;
        }
    }
}
