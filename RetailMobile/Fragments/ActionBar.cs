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

namespace RetailMobile.Fragments
{
    public class ActionBar : Android.Support.V4.App.Fragment
    {
		public delegate void SettingsCLickedDelegate();
		public event SettingsCLickedDelegate SettingsClicked;

        public delegate void SyncCLickedDelegate();
        public event SyncCLickedDelegate SyncClicked;

        Button btnSync;
		Button btnSettings;
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.main_action_bar, container, true);
            btnSync = v.FindViewById<Button>(Resource.Id.btnSync);
            btnSync.Click += new EventHandler(btnSync_Click);

			btnSettings = v.FindViewById<Button>(Resource.Id.btnSettings);
			btnSettings.Click += new EventHandler(btnSettings_Click);

            return v;
        }

        void btnSync_Click(object sender, EventArgs e)
        {
            if (SyncClicked != null)
                SyncClicked();
        }

		void btnSettings_Click(object sender, EventArgs e)
		{
			if (SettingsClicked != null)
				SettingsClicked();
		}

    }
}