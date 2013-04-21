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
            Sync.Synchronize(this);
                        
            SetContentView(Resource.Layout.MainMenu);
            RetailMobile.Fragments.ActionBar myActionBar = (RetailMobile.Fragments.ActionBar)SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
            myActionBar.SyncClicked += new Fragments.ActionBar.SyncCLickedDelegate(myActionBar_SyncClicked);
        }

        void myActionBar_SyncClicked()
        {
            //start sync
            Sync.Synchronize(this);
        }
     
    }
}