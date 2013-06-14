using Android.App;
using Android.OS;

namespace RetailMobile
{
    [Activity(Label = "TransactionFragmentActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen" )]
    public class TransactionFragmentActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            SetContentView(Resource.Layout.transaction_frmt_activity);
        }
    }
}
