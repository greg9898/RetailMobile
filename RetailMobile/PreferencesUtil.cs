using Android.Content;

namespace RetailMobile
{
    public class PreferencesUtil
    {
<<<<<<< Upstream, based on origin/master
        private static bool IsDebug = true;
        private static string APP_SHARED_PREFS = "com.alphamobile.RetailPreferences";
        //87.203.80.42
        //2439
=======
        static bool IsDebug = true;
        static string APP_SHARED_PREFS = "com.alphamobile.RetailPreferences";
		//87.203.80.42
		//2439
>>>>>>> da3b64e  - show customer names in statistics tab2 as first column  - search by customer name with suggestive drop down
        public static string IP = "";
        //public static string IP = "77.78.32.118";
        //public static string IP = "87.203.80.42";
        public static int Port = 2489;
        public static string SyncModel = "RetailMobile2";
        public static string SyncUser = "sa";
        public static string SyncPass = "";
        //public static string SyncModel = "RetailMobile";
        public static string Username = "";
        public static string Password = "";

        public static void SavePreferences(Context context)
        {
            ISharedPreferences appSharedPrefs = context.GetSharedPreferences(
                APP_SHARED_PREFS, FileCreationMode.Private);
            ISharedPreferencesEditor editor = appSharedPrefs.Edit();
            editor.PutString("IP", IP);
            editor.PutInt("Port", Port);
            editor.PutString("SyncModel", SyncModel);
            editor.PutString("SyncUser", SyncUser);
            editor.PutString("SyncPass", SyncPass);
            editor.PutString("Username", Username);
            editor.PutString("Password", Password);
            editor.Commit();
        }

        public static void LoadSettings(Context context)
        {
            ISharedPreferences appSharedPrefs = context.GetSharedPreferences(
                APP_SHARED_PREFS, FileCreationMode.Private);
            if (IsDebug)
            {
                IP = appSharedPrefs.GetString("IP", "77.78.32.118");
                Port = appSharedPrefs.GetInt("Port", 2489);
                SyncModel = appSharedPrefs.GetString("SyncModel", "RetailMobile2");
            }
            else
            {
                IP = appSharedPrefs.GetString("IP", "85.73.254.138");
                Port = appSharedPrefs.GetInt("Port", 2439);
                SyncModel = appSharedPrefs.GetString("SyncModel", "RetailMobile3");
            }
            //IP = appSharedPrefs.GetString ("IP", "77.78.32.118");
            //IP = appSharedPrefs.GetString("IP","");
            //Port = appSharedPrefs.GetInt ("Port", 2489);
            //Port = appSharedPrefs.GetInt ("Port", 2639);
            //SyncModel = appSharedPrefs.GetString("SyncModel","RetailMobile");
            // SyncModel = appSharedPrefs.GetString("SyncModel", "RetailMobile3");
            Username = appSharedPrefs.GetString("Username", "");
            Password = appSharedPrefs.GetString("Password", "");
        }
    }
}
