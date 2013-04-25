
using System;

namespace RetailMobile
{
    public static class Common
    {
		public static int CurrentDealerID = 0;

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
    }
}

