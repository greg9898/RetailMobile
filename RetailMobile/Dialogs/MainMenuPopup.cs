using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Util;

namespace RetailMobile
{
    public  class MainMenuPopup
    {
        public static void InitPopupMenu(FragmentActivity ctx, int idBelow)
        {
            int layoutWidth = (ctx.Resources.DisplayMetrics.WidthPixels * 31) / 100;
            int layoutHeight = 4 * ((int)ctx.Resources.GetDimension(Resource.Dimension.main_menu_icon_size) + 2);
            RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(layoutWidth, layoutHeight);
            lp.AddRule(LayoutRules.Below, idBelow);
            lp.TopMargin = (int)ctx.Resources.GetDimension(Resource.Dimension.action_bar_height);
            
            Log.Debug("InitPopupMenu", " before popupMenu=");
            RelativeLayout popupMenu = ctx.FindViewById<RelativeLayout>(Resource.Id.popupMenu);
            Log.Debug("InitPopupMenu", "popupMenu=" + popupMenu);
            popupMenu.LayoutParameters = lp;
            popupMenu.SetBackgroundResource(Resource.Drawable.actionbar_background);

//            MainMenuFragment mainmenupopup_fragment = (MainMenuFragment)ctx.SupportFragmentManager.FindFragmentById(Resource.Id.mainmenupopup_fragment);  
//            mainmenupopup_fragment.IsPopupMenu = true;

            Button btnSettings = ctx.FindViewById<Button>(Resource.Id.btnSettingsMain);
            btnSettings.Touch += (object sender, View.TouchEventArgs e) => { 
                switch (e.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Up:
                        popupMenu.Visibility = ViewStates.Gone;
                        SettingsClicked(ctx);
                        break;
                }
            };
        }

        static void SettingsClicked(FragmentActivity ctx)
        {
            if (Common.isTabletDevice(ctx))
            {                
                ctx.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
                var ft = ctx.SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new SettingsFragment());

                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit(); 
            }
            else
            {
                var intent = new Android.Content.Intent();
                intent.SetClass(ctx, typeof(SettingsFragmentActivity));
                ctx.StartActivity(intent);
            }
        }
    }
}

