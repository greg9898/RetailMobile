using Android.App;
using Android.OS;

namespace RetailMobile
{
    [Activity(Label = "SettingsFragmentActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class SettingsFragmentActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.settings_fragment_activity);

//            FrameLayout frameContent = new FrameLayout(this);
//            FrameLayout frameFrgmtActionbar = new FrameLayout(this);
//            frameFrgmtActionbar.Id = ControlIds.SETTINGS_FRAGMENT_ACTIVITY_ITEMACTIONBAR_ID;
//            FrameLayout frameFrgmtSettings = new FrameLayout(this);
//            frameFrgmtSettings.Id = ControlIds.SETTINGS_FRAGMENT_ACTIVITY_SETTINGS_ID;
//
//            SetContentView(frameContent, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
//
//            frameContent.AddView(frameFrgmtActionbar);
//            frameContent.AddView(frameFrgmtSettings);
//
//            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
//            ft.Replace(frameFrgmtActionbar.Id, new RetailMobile.Fragments.ItemActionBar());
//            ft.Commit();
//            
//            ft = SupportFragmentManager.BeginTransaction();
//            ft.Replace(frameFrgmtSettings.Id, new SettingsFragment());
//            ft.Commit();
           
        }
    }
}
