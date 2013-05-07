using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		const int BACK_BUTTON = 72224;
		EditText tbIP;
		EditText tbPort;
		EditText tbSyncModel;
		Button btnLogout;
		Button btnSync;
		RetailMobile.Fragments.ItemActionBar actionBar;

		public override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.SettingsFragment, container, false);

			tbIP = v.FindViewById<EditText> (Resource.Id.tbIP);
			tbPort = v.FindViewById<EditText> (Resource.Id.tbPort);
			tbSyncModel = v.FindViewById<EditText> (Resource.Id.tbSyncModel);
			btnLogout = v.FindViewById<Button> (Resource.Id.btnLogout);
			btnSync = v.FindViewById<Button> (Resource.Id.btnMainSync);

			if (Common.CurrentDealerID == 0) {
				btnLogout.Visibility = ViewStates.Gone;
				btnSync.Visibility = ViewStates.Gone;
			} else {
				btnLogout.Visibility = ViewStates.Visible;
				btnSync.Visibility = ViewStates.Visible;
			}

			tbIP.Text = PreferencesUtil.IP;
			tbPort.Text = PreferencesUtil.Port.ToString ();
			tbSyncModel.Text = PreferencesUtil.SyncModel;

			btnSync.Click += new EventHandler (btnSync_Click);
			btnLogout.Click += new EventHandler (btnLogout_Click);

			this.Activity.FindViewById<LinearLayout> (Resource.Id.layoutList).Visibility = ViewStates.Gone;

			actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.FindFragmentById (Resource.Id.ActionBar);
			actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate (ActionBarButtonClicked);
			actionBar.ClearButtons ();
			//string save = this.Activity.GetString(Resource.String.btnSave);
			actionBar.AddButtonRight (SAVE_BUTTON, "", Resource.Drawable.save_48);
			actionBar.AddButtonLeft (BACK_BUTTON, "", Resource.Drawable.back_48);

			string title = this.Activity.GetString (Resource.String.Settings);
			actionBar.SetTitle (title);

			return v;
		}

		void btnSync_Click (object sender, EventArgs e)
		{
			//btnSync.Text = this.Activity.GetString(Resource.String.SynchronizationInProgress);
			((MainMenu)this.Activity).ShowProgressBar ();
			Task.Factory.StartNew (() => Sync.Synchronize (this.Activity)
			).ContinueWith (task => this.Activity.RunOnUiThread (() => { 
				((MainMenu)this.Activity).HideProgressBar ();
				Toast.MakeText (this.Activity.ApplicationContext, this.Activity.GetString (Resource.String.SynchronizationComplete), ToastLength.Short).Show ();
			})
			);//.ContinueWith(task => this.Activity.RunOnUiThread(() => this.btnSync.Text = this.Activity.GetString(Resource.String.SynchronizationComplete)));

			//Sync.Synchronize(this.Activity);
		}

		void btnLogout_Click (object sender, EventArgs e)
		{
			Common.CurrentDealerID = 0;
			this.Activity.FindViewById<LinearLayout> (Resource.Id.LayoutMenu).Visibility = ViewStates.Invisible;
			var ft = ((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.BeginTransaction ();
			ft.Replace (Resource.Id.detailInfo_fragment, new LoginFragment ());
			ft.SetTransition (Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
			ft.Commit ();
		}

		void ActionBarButtonClicked (int id)
		{
			switch (id) {
			case BACK_BUTTON:            
				if (Common.CurrentDealerID == 0) {
					if (this.Activity == null) {
						return; //?
					}

					var ft = this.Activity.SupportFragmentManager.BeginTransaction ();
					ft.Replace (Resource.Id.detailInfo_fragment, new LoginFragment ());
					ft.SetTransition (Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
					ft.Commit ();
				} 
				break;
			case SAVE_BUTTON:
				try {
					PreferencesUtil.IP = tbIP.Text;
					PreferencesUtil.Port = int.Parse (tbPort.Text);
					PreferencesUtil.SyncModel = tbSyncModel.Text;
					PreferencesUtil.SavePreferences (this.Activity);

					Toast.MakeText (this.Activity.ApplicationContext, "Saved", ToastLength.Short).Show ();

					if (Common.CurrentDealerID == 0) {
						var ft = this.Activity.SupportFragmentManager.BeginTransaction ();
						ft.Replace (Resource.Id.detailInfo_fragment, new LoginFragment ());
						ft.SetTransition (Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
						ft.Commit ();
					}
				} catch (Exception ex) {
					Android.Util.Log.Error ("exception", ex.Message);
				}
				break;
			}
		}
	}
}
