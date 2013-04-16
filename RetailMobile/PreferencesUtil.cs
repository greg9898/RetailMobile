using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RetailMobile
{
    public class PreferencesUtil
    {
        public static string IP = "77.78.32.118";
        public static int Port = 2439;
        public static string SyncModel = "RetailMobile";

        public static string DecimalFormat = "######0.0##";

        /// <summary>
        /// dd.mm.yyyy
        /// </summary>
        public static string DateFormatDateOnly = "dd.MM.yyyy";
    }
}