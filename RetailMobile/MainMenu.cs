using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace RetailMobile
{
    [Activity(Label = "Ασυρματη Παραγγελιοληψια", MainLauncher = true, Icon = "@drawable/retail", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen"
              ,ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainMenu : Android.Support.V4.App.FragmentActivity
    {
        public enum MenuItems
        {
            Items = 0
,
            Customers = 1
,
            Invoices = 2
,
            CheckableItemsTest = 3
        }

        RetailMobile.Fragments.ActionBar myActionBar;

        protected override void OnCreate(Bundle bundle)
        {   
            base.OnCreate(bundle);

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => {
                Common.LogException(e.Exception);// proper solution not yet found :/
            };

            PreferencesUtil.LoadSettings(this);
            Sync.GenerateDatabase(this);

            Crashlytics.Android.Crashlytics1.Start(this);

            SetContentView(Resource.Layout.MainMenu);
            myActionBar = (RetailMobile.Fragments.ActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
            myActionBar.SyncClicked += new Fragments.ActionBar.SyncCLickedDelegate(MyActionBar_SyncClicked);

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
                
                DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
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

        void MyActionBar_SyncClicked()
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