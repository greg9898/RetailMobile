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
using Android.Support.V4.App;

namespace RetailMobile
{
    [Activity(Label = "Settings Fragment")]
	public class SettingsFragment : BaseFragment
    {
		const int SAVE_BUTTON = 423;
		EditText tbIP;
		EditText tbPort;
		EditText tbSyncModel;
		RetailMobile.Fragments.ItemActionBar actionBar;

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

        }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate(Resource.Layout.SettingsFragment, container, false);

			tbIP = v.FindViewById<EditText>(Resource.Id.tbIP);
			tbPort = v.FindViewById<EditText>(Resource.Id.tbPort);
			tbSyncModel = v.FindViewById<EditText>(Resource.Id.tbSyncModel);

			tbIP.Text = PreferencesUtil.IP;
			tbPort.Text = PreferencesUtil.Port.ToString();
			tbSyncModel.Text = PreferencesUtil.SyncModel;

			actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
			actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
			string save = this.Activity.GetString(Resource.String.btnSave);
			actionBar.AddButtonRight(SAVE_BUTTON,save,0);

			string title = this.Activity.GetString(Resource.String.Settings);
			actionBar.SetTitle(title);

			return v;
		}

		void ActionBarButtonClicked(int id)
		{
			if(id == SAVE_BUTTON)
			{
				try
				{
					PreferencesUtil.IP = tbIP.Text;
					PreferencesUtil.Port = int.Parse(tbPort.Text);
					PreferencesUtil.SyncModel = tbSyncModel.Text;
					PreferencesUtil.SavePreferences(this.Activity);
				}
				catch(Exception ex)
				{
				}
			}
		}
    }
}