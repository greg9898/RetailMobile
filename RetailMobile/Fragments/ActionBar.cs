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
        ProgressBar pbSync;

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

            pbSync = v.FindViewById<ProgressBar>(Resource.Id.pbSync1);

            return v;
        }

        public void ShowProgress()
        {
            if(btnSync != null && pbSync != null)
            {
                btnSync.Visibility = ViewStates.Invisible;
                pbSync.Visibility = ViewStates.Visible;
            }
        }

        public void HideProgress()
        {
            if(btnSync != null && pbSync != null)
            {
                btnSync.Visibility = ViewStates.Visible;
                pbSync.Visibility = ViewStates.Invisible;
            }
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

        public static   RelativeLayout.LayoutParams ButtonLayoutParams(Android.Content.Res.Resources r)
        {
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            layoutParams.AddRule(LayoutRules.CenterVertical);
            layoutParams.LeftMargin = (int)r.GetDimension(Resource.Dimension.action_bar_button_left_margin);
            layoutParams.RightMargin = (int)r.GetDimension(Resource.Dimension.action_bar_button_right_margin);
            layoutParams.Height = (int)r.GetDimension(Resource.Dimension.action_bar_button_height);
            layoutParams.Width = (int)r.GetDimension(Resource.Dimension.action_bar_button_width);
            return layoutParams;
        }
    }
}