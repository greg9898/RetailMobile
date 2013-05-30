using System;

namespace RetailMobile
{
    public static class Common
    {
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
    }
}

