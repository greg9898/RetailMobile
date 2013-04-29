using Android.Content;

namespace RetailMobile
{
    public class PreferencesUtil
    {
		private static string APP_SHARED_PREFS = "com.alphamobile.RetailPreferences";

		//87.203.80.42
		//2439
		//public static string IP = "";
        public static string IP = "77.78.32.118";
		//public static string IP = "87.203.80.42";
        public static int Port = 2439;
        public static string SyncModel = "RetailMobile2";
		//public static string SyncModel = "RetailMobile";
		public static string Username = "";
		public static string Password = "";

        public static string DecimalFormat = "######0.0##";

        /// <summary>
        /// dd.mm.yyyy
        /// </summary>
        public static string DateFormatDateOnly = "dd.MM.yyyy";

		public static void SavePreferences(Context context)
		{
			ISharedPreferences appSharedPrefs = context.GetSharedPreferences(
				APP_SHARED_PREFS,FileCreationMode.Private);
			ISharedPreferencesEditor editor = appSharedPrefs.Edit();
			editor.PutString("IP",IP);
			editor.PutInt("Port",Port);
			editor.PutString("SyncModel",SyncModel);
			editor.PutString("Username",Username);
			editor.PutString("Password",Password);
			editor.Commit();
		}

		public static void LoadSettings(Context context) 
		{
			ISharedPreferences appSharedPrefs = context.GetSharedPreferences(
				APP_SHARED_PREFS,FileCreationMode.Private);

			IP = appSharedPrefs.GetString("IP","77.78.32.118");
			//IP = appSharedPrefs.GetString("IP","");
			Port = appSharedPrefs.GetInt("Port",2439);
			//SyncModel = appSharedPrefs.GetString("SyncModel","RetailMobile");
			SyncModel = appSharedPrefs.GetString("SyncModel","RetailMobile2");
			Username = appSharedPrefs.GetString("Username","");
			Password = appSharedPrefs.GetString("Password","");
		}
    }
}