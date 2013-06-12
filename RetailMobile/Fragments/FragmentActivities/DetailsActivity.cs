using System;
using Android.App;
using Android.OS;

namespace RetailMobile
{
    [Activity(Label = "Details Activity")]
	public class DetailsActivity : Android.Support.V4.App.FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var index = Intent.Extras.GetInt("current_obj_id", -1);

            var details = DetailsFragment.NewInstance(index); // DetailsFragment.NewInstance is a factory method to create a Details Fragment
            Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Add(Android.Resource.Id.Content, details);
            ft.Commit();
        }
    }
}