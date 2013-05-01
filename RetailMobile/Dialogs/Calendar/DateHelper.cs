
using System;
using Java.Util;
using Android.Text.Format;
using Android.Util;

namespace RetailMobile
{
    public class DateHelper
    {
        public static String GetDateString(Date date)
        {
            String dateUTCString = "";
            
            Calendar cal = new GregorianCalendar();
            cal.Set(date.Year + 1900, date.Month, date.Day, date.Hours, date.Minutes, date.Seconds);
            Date utcDate = cal.Time;
            
            dateUTCString = (utcDate.Year + 1900) + "-" + (utcDate.Month + 1) + "-" + (utcDate.Day) + " " + (utcDate.Hours) + ":" + (utcDate.Minutes);
            
            return dateUTCString;
        }
        
        public static String GetUTCDateString(Date date)
        {
            String dateUTCString = "";
            
            Calendar cal = new GregorianCalendar(Java.Util.TimeZone.GetTimeZone("GMT"));
            cal.Set(date.Year + 1900, date.Month, date.Day, date.Hours, date.Minutes, date.Seconds);
            Date utcDate = cal.Time;

            dateUTCString = (utcDate.Year + 1900) + "-" + (utcDate.Month + 1) + "-" + (utcDate.Day) + " " + (utcDate.Hours) + ":" + (utcDate.Minutes);
            
            return dateUTCString;
        }
        
        public static String GetUTCDateString(String date)
        {
            return GetUTCDateString(GetDate(date));
        }

		public static String GetLocalizedDateTimeString(DateTime date)
		{
			string dateS = date.ToShortDateString();
			return dateS;
		}
                        
        public static Date GetDate(String date)
        {
            Date d = null;
            
            try
            {
                // custom format for comunication -> 2012-10-24 14:37
                int year = 0;
                int month = 0;
                int day = 0;
                int hours = 0;
                int minutes = 0;
                
                string[] str = date.Split(' ');
                
                if (str.Length > 1)
                {
                    string[] time = str[1].Split(':');
                    if (time.Length > 1)
                    {
                        hours = int.Parse(time[0]);
                        minutes = int.Parse(time[1]);
                    }
                }
                
                string[] datePart = str[0].Split('-');
                if (datePart.Length == 3)
                {
                    year = int.Parse(datePart[0]) - 1900;
                    month = int.Parse(datePart[1]) - 1;
                    day = int.Parse(datePart[2]);
                }
                
                d = new Date(year, month, day, hours, minutes);
            } catch (Exception ex)
            {
                Log.Error("GetDate", ex.Message);
            }
            
            return d;
        }
    }
}

