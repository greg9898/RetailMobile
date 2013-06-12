using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace RetailMobile
{
    [Activity(Label = "SettingsFragmentActivity")]
    public class SettingsFragmentActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FrameLayout frame = new FrameLayout(this);
            frame.Id = Buttons.SETTINGS_FRAGMENT_ACTIVITY_ID;
            SetContentView(frame, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            if (bundle == null)
            {
                Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
//                ft.Add(frame.Id, new RetailMobile.Fragments.ItemActionBar());
//                ft.Replace(Resource.Id.menuLoginFragment, new RetailMobile.Fragments.ItemActionBar());
                ft.Replace(Resource.Id.menuLoginFragment, new SettingsFragment());
                ft.Commit();
            }
         
//            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
//            ft.Add(Android.Resource.Id.Content, new SettingsFragment());
           
        }
    }
}
