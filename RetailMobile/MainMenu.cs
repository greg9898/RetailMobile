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
using Android.Util;

namespace RetailMobile
{
    [Activity(Label = "Ασυρματη Παραγγελιοληψια", MainLauncher = true, Icon = "@drawable/retail", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class MainMenu : Android.Support.V4.App.FragmentActivity
    {
        RetailMobile.Fragments.ActionBar myActionBar;

        protected override void OnCreate(Bundle bundle)
        {   
            base.OnCreate(bundle);
            PreferencesUtil.LoadSettings(this);
            Sync.GenerateDatabase(this);

            SetContentView(Resource.Layout.MainMenu);
            myActionBar = (RetailMobile.Fragments.ActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
            myActionBar.SyncClicked += new Fragments.ActionBar.SyncCLickedDelegate(myActionBar_SyncClicked);

            ShowProgressBar();
            System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncUsers(this)).ContinueWith(task => this.RunOnUiThread(() => HideProgressBar()));
            //Sync.SyncUsers(this);

            //RetailMobile.LoginFragment loginFragment = (RetailMobile.LoginFragment)SupportFragmentManager.FindFragmentById(Resource.Id.detail);

            if (!string.IsNullOrEmpty(PreferencesUtil.Username) && !string.IsNullOrEmpty(PreferencesUtil.Password) &&
                LoginFragment.Login(this, PreferencesUtil.Username, PreferencesUtil.Password))
            {
                this.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Visible;
                this.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
                this.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
                this.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
                
                DetailsFragment details = DetailsFragment.NewInstance((int)Base.MenuItems.Invoices);
                var ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.details_fragment, details);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
                
                ft = SupportFragmentManager.BeginTransaction();
                //ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
                ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
            else
            {
                var ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new LoginFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        public void ShowProgressBar()
        {
            myActionBar.ShowProgress();
        }

        public void HideProgressBar()
        {
            myActionBar.HideProgress();
        }

        void myActionBar_SyncClicked()
        {
            ShowProgressBar();

            //start sync
            if (Common.CurrentDealerID == 0)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncUsers(this)).ContinueWith(task => this.RunOnUiThread(() => HideProgressBar()));
            }
            else
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncTrans(this)).ContinueWith(task => this.RunOnUiThread(() => HideProgressBar()));
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(Resource.String.ExitPrompt)
                    .SetCancelable(false)
                        .SetPositiveButton(Resource.String.Yes, new EventHandler<DialogClickEventArgs>((o,ea) => {
                    Finish();
                }))
                        .SetNegativeButton(Resource.String.No, new EventHandler<DialogClickEventArgs>((o,ea) => {

                }));
                AlertDialog alert = builder.Create();
                alert.Show();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}