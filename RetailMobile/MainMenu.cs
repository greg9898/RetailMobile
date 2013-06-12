using System;
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
        Common.Layouts layout ;

        protected override void OnCreate(Bundle bundle)
        {   
            base.OnCreate(bundle);

            /*AppDomain.CurrentDomain.UnhandledException +=  new UnhandledExceptionEventHandler((o,e)=>
            {
                Android.Util.Log.Debug("e : ",e.ToString() );
                Android.Util.Log.Debug("e.ExceptionObject: ",e.ExceptionObject.ToString() );
                Crashlytics.Android.Crashlytics1.Log(e.ToString());
                Crashlytics.Android.Crashlytics1.Log(e.ExceptionObject.ToString());
            });*/

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => {
                RetailMobile.Error.LogError(this, e.Exception.Message, e.Exception.StackTrace);
            };

            int ver = (int)Resources.GetDimension(Resource.Dimension.ver);
            Log.Debug("[[Dimens]]", "ver=" + ver);

            PreferencesUtil.LoadSettings(this);
            Sync.GenerateDatabase(this);

            Crashlytics.Android.Crashlytics1.Start(this);

            SetContentView(Resource.Layout.MainMenu);
            
            myActionBar = (RetailMobile.Fragments.ActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);       
            myActionBar.SyncClicked += new Fragments.ActionBar.SyncCLickedDelegate(MyActionBar_SyncClicked);
            myActionBar.MenuClicked += new RetailMobile.Fragments.ActionBar.MenuClickedDelegate(MenuClicked);
            myActionBar.SettingsClicked += new RetailMobile.Fragments.ActionBar.SettingsCLickedDelegate(SettingsClicked);

            if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                layout = this.FindViewById<LinearLayout>(Resource.Id.LayoutMenu) != null ? Common.Layouts.Sw600Land : Common.Layouts.Land;
            }
            else
            {
                layout = this.SupportFragmentManager.FindFragmentById(Resource.Id.menuLoginFragment) == null ? Common.Layouts.Sw600Port : Common.Layouts.Port;

                InitPopupMenu();
            }     

            ShowProgressBar();

            System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncUsers(this)).ContinueWith(task => this.RunOnUiThread(() => HideProgressBar()));

            if (!string.IsNullOrEmpty(PreferencesUtil.Username) && !string.IsNullOrEmpty(PreferencesUtil.Password) &&
                LoginFragment.Login(this, PreferencesUtil.Username, PreferencesUtil.Password))
            {
                this.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
                this.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
                this.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;

                var ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.details_fragment, DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
                
                ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
                {
                    this.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Visible;
                }
                else
                {
                    myActionBar.ButtonMenuVisibility = ViewStates.Visible;
                    myActionBar.ButtonSettingsVisibility = ViewStates.Gone;
                }
            }
            else
            {
                if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                {
                    myActionBar.ButtonMenuVisibility = ViewStates.Gone;
                }

                if (layout == Common.Layouts.Port)
                { 
//                    var intent = new Intent();
//                    intent.SetClass(this, typeof(LoginFragmentActivity));
//                    StartActivity(intent);
                }
                else
                {
                    var ft = SupportFragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.detailInfo_fragment, new LoginFragment());
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }
            }
        }

        void InitPopupMenu()
        {
            int layoutWidth = (Resources.DisplayMetrics.WidthPixels * 31) / 100;
            int layoutHeight = 4 * ((int)Resources.GetDimension(Resource.Dimension.main_menu_icon_size) + 2);
            RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(layoutWidth, layoutHeight);
            lp.AddRule(LayoutRules.Below, myActionBar.Id);
            lp.TopMargin = (int)Resources.GetDimension(Resource.Dimension.action_bar_height);

            RelativeLayout popupMenu = this.FindViewById<RelativeLayout>(Resource.Id.popupMenu);
            popupMenu.LayoutParameters = lp;
            popupMenu.SetBackgroundResource(Resource.Drawable.actionbar_background);

            MainMenuFragment mainmenupopup_fragment = (MainMenuFragment)SupportFragmentManager.FindFragmentById(Resource.Id.mainmenupopup_fragment);  
            mainmenupopup_fragment.IsPopupMenu = true;

            Button btnSettings = this.FindViewById<Button>(Resource.Id.btnSettingsMain);
            btnSettings.Touch += (object sender, View.TouchEventArgs e) => { 
                switch (e.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Up:
                        popupMenu.Visibility = ViewStates.Gone;
                        SettingsClicked();
                        break;
                }
            };
        }

        void SettingsClicked()
        {
            if (layout == Common.Layouts.Land || layout == Common.Layouts.Port)
            {                
                var intent = new Android.Content.Intent();
                intent.SetClass(this, typeof(SettingsFragmentActivity));
                StartActivity(intent);
            }
            else
            {
                this.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
                var ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new SettingsFragment());

                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void MenuClicked()
        {
            RelativeLayout popupMenu = this.FindViewById<RelativeLayout>(Resource.Id.popupMenu);
            if (popupMenu.Visibility == ViewStates.Visible)
            {
                popupMenu.Visibility = ViewStates.Gone;
                return;
            }

            popupMenu.Visibility = ViewStates.Visible;
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
