using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace RetailMobile
{
    public class TitlesFragment1 : Android.Support.V4.App.ListFragment
    {
        public enum MenuItems
        {
            Items = 0
,
            Customers = 1
,
            Invoices = 2
,
            CheckableItemsTest = 3
        }

        bool _isDualPane ;
        int _currentObjId = -1;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var adapter = new MainMenuAdapter(Activity);
            ListAdapter = adapter;
            ListView.DividerHeight = 4;

            if (savedInstanceState != null)
            {
                _currentObjId = savedInstanceState.GetInt("idLvl1", -1);
            }
            var details = Activity.FindViewById<View>(Resource.Id.details_fragment);
            _isDualPane = details != null && details.Visibility == ViewStates.Visible;

            if (_isDualPane)
            {
                ListView.ChoiceMode = ChoiceMode.Single;
                ShowDetails(_currentObjId);
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("idLvl1", _currentObjId);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowDetails(position);
        }

        void ShowDetails(int objId)
        {
            if (objId == -1)
            {
                return;
            }

            if (_isDualPane)
            {
                var ft = FragmentManager.BeginTransaction();
                var detailFragment = FragmentManager.FindFragmentById(Resource.Id.detailInfo_fragment);
                if (detailFragment != null)
                {
                    ft.Remove(detailFragment);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }

                // We can display everything in place with fragments.
                // Have the list highlight this item and show the data.
                ListView.SetItemChecked(objId, true);
                // Check what fragment is shown, replace if needed.
                var details = FragmentManager.FindFragmentById(Resource.Id.details_fragment) as DetailsFragment;
                if (details == null || details.ParentObjId != objId)
                {
                    // Make new fragment to show this selection.
                    details = DetailsFragment.NewInstance(objId);
                    // Execute a transaction, replacing any existing
                    // fragment with this one inside the frame.
                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.details_fragment, details);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }
            }
            else
            {
                var intent = new Intent();
                intent.SetClass(Activity, typeof(DetailsActivity));
                intent.PutExtra("idLvl1", objId);
                StartActivity(intent);
            }
        }
    }
}