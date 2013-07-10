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
using Com.Jeremyfeinstein.Slidingmenu.Lib;

namespace RetailMobile
{
    //ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation
    [Activity(Label = "Ασυρματη Παραγγελιοληψια", MainLauncher = true, Icon = "@drawable/retail", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen",
              ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation)]
    public class Main : Android.Support.V4.App.FragmentActivity
    {
        MainMenuFragment fragmentMainMenu;
        DetailsFragment fragmentDetails;
        InvoiceInfoFragment fragmentInvoice;
        RetailMobile.Fragments.ItemActionBar mainActionBar;
        RelativeLayout pbLoadingLayout;
        Com.Jeremyfeinstein.Slidingmenu.Lib.SlidingMenu menu;

        protected override void OnCreate(Bundle bundle)
        {   
            base.OnCreate(bundle);

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => {
                RetailMobile.Error.LogError(this, e.Exception.Message, e.Exception.StackTrace);
            };



            PreferencesUtil.LoadSettings(this);
            Sync.GenerateDatabase(this);

            SetContentView(Resource.Layout.main);



            pbLoadingLayout = FindViewById<RelativeLayout>(Resource.Id.pbLoadingLayout);

            RelativeLayout layoutFragment1 = FindViewById<RelativeLayout>(Resource.Id.fragment1);
            RelativeLayout layoutFragment2 = FindViewById<RelativeLayout>(Resource.Id.fragment2);
            RelativeLayout layoutFragment3 = FindViewById<RelativeLayout>(Resource.Id.fragment3);
            LinearLayout layout2 = FindViewById<LinearLayout>(Resource.Id.layout2);

            mainActionBar = (RetailMobile.Fragments.ItemActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar1);

            bool isLoggedIn = false;
            if (layoutFragment1 != null)
            {
                if (!string.IsNullOrEmpty(PreferencesUtil.Username) && !string.IsNullOrEmpty(PreferencesUtil.Password) &&
                    LoginFragment.Login(this, PreferencesUtil.Username, PreferencesUtil.Password))
                {
                    isLoggedIn = true;
                    if (layoutFragment2 == null && layoutFragment3 == null)
                    {
                        fragmentInvoice = new InvoiceInfoFragment();
                        var ft = SupportFragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.fragment1, fragmentInvoice);
                        ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                        ft.AddToBackStack("Invoice");
                        ft.Commit();
                    }
                    else
                    {
                    fragmentMainMenu = new MainMenuFragment();
                    var ft = SupportFragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment1, fragmentMainMenu);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.AddToBackStack("MainMenu");
                    ft.Commit();
                    }
                }
                else
                {
                    isLoggedIn = false;
                    LoginFragment fragmentLogin = new LoginFragment();
                    var ft = SupportFragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment1, fragmentLogin);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.AddToBackStack("Login");
                    ft.Commit();
                }
            }
            if(layoutFragment2 != null && layoutFragment3 != null)
            {
                if (isLoggedIn)
                {
                    layout2.Visibility = ViewStates.Visible;

                        fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                        var ft = SupportFragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.fragment2, fragmentDetails);
                        ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                        ft.Commit();

                        fragmentInvoice = InvoiceInfoFragment.NewInstance(0);
                        ft = SupportFragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.fragment3, fragmentInvoice);
                        ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                        ft.Commit();
                }
                else
                {
                    layoutFragment3.Visibility = ViewStates.Gone;
                    layoutFragment2.Visibility = ViewStates.Gone;
                }
            }

            if(layoutFragment2 == null && layoutFragment3 == null)
            {
            menu = new Com.Jeremyfeinstein.Slidingmenu.Lib.SlidingMenu(this);
            menu.Mode = 0;
            menu.TouchModeAbove = SlidingMenu.TouchmodeNone;
            menu.SetShadowWidthRes(Resource.Dimension.shadow_width);
            menu.SetShadowDrawable(Resource.Drawable.shadow);
            menu.SetBehindOffsetRes(Resource.Dimension.slidingmenu_offset);
            menu.SetFadeDegree(0.35f);
            menu.AttachToActivity(this, Com.Jeremyfeinstein.Slidingmenu.Lib.SlidingMenu.SlidingContent);
            menu.SetMenu(Resource.Layout.FragmentMainMenu);
            }

            SupportFragmentManager.ExecutePendingTransactions();

            System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncUsers(this)).ContinueWith(task => this.RunOnUiThread(() => HideProgressBar()));
        }

        public void ToggleMenu()
        {
            if(menu != null)
                menu.Toggle();
        }

        public void ShowProgressBar()
        {
            if(pbLoadingLayout != null)
                pbLoadingLayout.Visibility = ViewStates.Visible;
            //mainActionBar.ShowProgress();
        }

        public void HideProgressBar()
        {
            if(pbLoadingLayout != null)
                pbLoadingLayout.Visibility = ViewStates.Gone;
            //mainActionBar.HideProgress();
        }
    }
}

