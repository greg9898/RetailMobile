using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Util;

namespace RetailMobile
{
    [Activity(Label = "Settings Fragment")]
	public class SettingsFragment : BaseFragment
    {
        EditText tbIP;
        EditText tbPort;
        EditText tbSyncModel;
        EditText tbSyncUser;
        EditText tbSyncPass;
        Button btnLogout;
        Button btnSync;
        RetailMobile.Fragments.ItemActionBar actionBar;

        private void InitActionBar()
        {
            bool showMenuButton = false;
            actionBar = (RetailMobile.Fragments.ItemActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBar3);
            if(actionBar == null)
            {
                actionBar = (RetailMobile.Fragments.ItemActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBar1);
                showMenuButton = true;
            }
            actionBar.ClearButtons();
            actionBar.AddButtonRight(ControlIds.SETTINGS_SAVE_BUTTON, "", Resource.Drawable.save_48);

            if(showMenuButton)
                actionBar.AddButtonLeft(ControlIds.INVOICE_MAINMENU_BUTTON, "", Resource.Drawable.menu_32);
            else
                actionBar.AddButtonLeft(ControlIds.SETTINGS_BACK_BUTTON, "", Resource.Drawable.back_48);

            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            string title = this.Activity.GetString(Resource.String.Settings);
            actionBar.SetTitle(title);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug("SettingsFragment ", "OnCreateView");
            View v = inflater.Inflate(Resource.Layout.SettingsFragment, container, false);

            tbIP = v.FindViewById<EditText>(Resource.Id.tbIP);
            tbPort = v.FindViewById<EditText>(Resource.Id.tbPort);
            tbSyncModel = v.FindViewById<EditText>(Resource.Id.tbSyncModel);
            tbSyncUser = v.FindViewById<EditText>(Resource.Id.tbSyncUser);
            tbSyncPass = v.FindViewById<EditText>(Resource.Id.tbSyncPass);
            btnLogout = v.FindViewById<Button>(Resource.Id.btnLogout);
            btnSync = v.FindViewById<Button>(Resource.Id.btnMainSync);

            if (Common.CurrentDealerID == 0)
            {
                btnLogout.Visibility = ViewStates.Gone;
                btnSync.Visibility = ViewStates.Visible;
            }
            else
            {
                btnLogout.Visibility = ViewStates.Visible;
                btnSync.Visibility = ViewStates.Visible;
            }

            tbIP.Text = PreferencesUtil.IP;
            tbPort.Text = PreferencesUtil.Port.ToString();
            tbSyncModel.Text = PreferencesUtil.SyncModel;
            tbSyncUser.Text = PreferencesUtil.SyncUser;
            tbSyncPass.Text = PreferencesUtil.SyncPass;

            btnSync.Click += new EventHandler(btnSync_Click);
            btnLogout.Click += new EventHandler(btnLogout_Click);

            InitActionBar();
            return v;
        }

        void btnSync_Click(object sender, EventArgs e)
        {
            ((Main)this.Activity).ShowProgressBar();
            if (Common.CurrentDealerID == 0)
            {
                Task.Factory.StartNew(() => Sync.SyncUsers (this.Activity)
                ).ContinueWith(task => this.Activity.RunOnUiThread (() => { 
                    ((Main)this.Activity).HideProgressBar ();
                    Toast.MakeText (this.Activity.ApplicationContext, this.Activity.GetString (Resource.String.SynchronizationComplete), ToastLength.Short).Show ();
                }));
            }
            else
            {
                Task.Factory.StartNew(() => Sync.Synchronize (this.Activity)
                ).ContinueWith(task => this.Activity.RunOnUiThread (() => { 
				    ((Main)this.Activity).HideProgressBar ();
				    Toast.MakeText (this.Activity.ApplicationContext, this.Activity.GetString (Resource.String.SynchronizationComplete), ToastLength.Short).Show ();
			    }));
            }
        }

        void btnLogout_Click(object sender, EventArgs e)
        {
            Common.CurrentDealerID = 0;
            /*this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Invisible;
            var ft = this.Activity.SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, new LoginFragment());
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();*/
            var ft = this.Activity.SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment1, new LoginFragment());
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();
        }

        void ActionBarButtonClicked(int id)
        {
            switch (id)
            {
                case ControlIds.SETTINGS_MENU_BUTTON:     
                    {
                    ((Main)this.Activity).ToggleMenu();
                    }
                    break;
                case ControlIds.SETTINGS_BACK_BUTTON:     
                    if (Common.CurrentDealerID == 0)
                    {
                        if (this.Activity == null)
                        {
                            return; //?
                        }

                        this.Activity.SupportFragmentManager.PopBackStack();


                        /*if (Common.isTabletDevice(this.Activity))
                        {                
                            var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.detailInfo_fragment, new LoginFragment());
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit(); 
                        }
                        else
                        {
                            var intent = new Android.Content.Intent();
                            intent.SetClass(this.Activity, typeof(LoginFragmentActivity));
                            StartActivity(intent);
                        }*/
                    } 
                    break;
                case ControlIds.SETTINGS_SAVE_BUTTON:
                    try
                    {
                        PreferencesUtil.IP = tbIP.Text;
                        PreferencesUtil.Port = int.Parse(tbPort.Text);
                        PreferencesUtil.SyncModel = tbSyncModel.Text;
                        PreferencesUtil.SyncUser = tbSyncUser.Text;
                        PreferencesUtil.SyncPass = tbSyncPass.Text;
                        PreferencesUtil.SavePreferences(this.Activity);

                        Toast.MakeText(this.Activity.ApplicationContext, "Saved", ToastLength.Short).Show();

                        if (Common.CurrentDealerID == 0)
                        {
                            /*var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.fragment1, new LoginFragment());
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit(); */

                            this.Activity.SupportFragmentManager.PopBackStack();

                            /*if (Common.isTabletDevice(this.Activity))
                            {
                                var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                                ft.Replace(Resource.Id.detailInfo_fragment, new LoginFragment());
                                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                                ft.Commit();
                            }
                            else
                            {
                                var intent = new Android.Content.Intent();
                                intent.SetClass(this.Activity, typeof(LoginFragmentActivity));
                                StartActivity(intent);
                            }*/
                        }
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("ActionBarButtonClicked SAVE_BUTTON", ex.Message);
                    }
                    break;
            }
        }
    }
}
