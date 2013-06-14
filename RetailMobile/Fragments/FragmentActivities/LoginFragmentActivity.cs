using Android.App;
using Android.OS;

namespace RetailMobile
{
    [Activity(Label = "LoginFragmentActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen" )]
    public class LoginFragmentActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Add(Android.Resource.Id.Content, new LoginFragment());
            ft.Commit();
        }
    }
}
