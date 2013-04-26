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
    [Activity(Label = "Main menu", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class MainMenu : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {   
            base.OnCreate(bundle);
			PreferencesUtil.LoadSettings(this);
			Sync.GenerateDatabase(this);
			Sync.SyncUsers(this);
                                    
            SetContentView(Resource.Layout.MainMenu);
            RetailMobile.Fragments.ActionBar myActionBar = (RetailMobile.Fragments.ActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
            myActionBar.SyncClicked += new Fragments.ActionBar.SyncCLickedDelegate(myActionBar_SyncClicked);

			//RetailMobile.LoginFragment loginFragment = (RetailMobile.LoginFragment)SupportFragmentManager.FindFragmentById(Resource.Id.detail);

			if(string.IsNullOrEmpty(PreferencesUtil.Username) == false &&
			   string.IsNullOrEmpty(PreferencesUtil.Password) == false &&
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

        void myActionBar_SyncClicked()
        {
            //start sync
			if(Common.CurrentDealerID == 0)
			{
				Sync.SyncUsers(this);
			}
			else
			{
            	Sync.Synchronize(this);
			}
        }
     
    }
}