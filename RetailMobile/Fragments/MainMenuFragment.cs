using System;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace RetailMobile
{
    public class MainMenuFragment : Android.Support.V4.App.Fragment
    {
        Button btnListInvoices;
        Button btnListItems;
        Button btnListCustomers;
        DetailsFragment details;
        bool isPopupMenu;
        Common.Layouts layout ;

        public bool IsPopupMenu
        {
            get{ return isPopupMenu;}
            set{ isPopupMenu = value;}
        }

        public MainMenuFragment(bool isPopup)
        {
            isPopupMenu = isPopup;
        }

        public MainMenuFragment()
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.FragmentMainMenu, container, false);
      
            RetailMobile.Fragments.ActionBar bar = (RetailMobile.Fragments.ActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
            bar.SettingsClicked += new RetailMobile.Fragments.ActionBar.SettingsCLickedDelegate(SettingsClicked);

            btnListInvoices = v.FindViewById<Button>(Resource.Id.btnAddInvoice);
            btnListItems = v.FindViewById<Button>(Resource.Id.btnAddItem);
            btnListCustomers = v.FindViewById<Button>(Resource.Id.btnAddCustomer);

            btnListInvoices.Click += new EventHandler(btnListInvoices_Click);
            btnListItems.Click += new EventHandler(btnListItems_Click);
            btnListCustomers.Click += new EventHandler(btnListCustomers_Click);

            if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                layout = this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu) != null ? Common.Layouts.Sw600Land : Common.Layouts.Land;
            }
            else
            {
                layout = this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.menuLoginFragment) == null ? Common.Layouts.Sw600Port : Common.Layouts.Port;
            }   

            return v;
        }

        void SettingsClicked()
        {
            if (layout == Common.Layouts.Land || layout == Common.Layouts.Port)
            {                
                var intent = new Android.Content.Intent();
                intent.SetClass(this.Activity, typeof(SettingsFragmentActivity));
                StartActivity(intent);
            }
            else
            {
                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new SettingsFragment());

                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnListInvoices_Click(object sender, EventArgs e)
        {
            //FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            //f.Visibility = ViewStates.Visible;
            details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            InvoiceInfoFragment detailsInfo = InvoiceInfoFragment.NewInstance(0);
            ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, detailsInfo);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();
            detailsInfo.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(details.InvoiceSaved);

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;

            if (isPopupMenu)
            {
                this.Activity.FindViewById<RelativeLayout>(Resource.Id.popupMenu).Visibility = ViewStates.Gone;
            }
        }

        void btnListItems_Click(object sender, EventArgs e)
        {
            FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            f.Visibility = ViewStates.Visible;
            details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Items);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            ItemFragment detailsInfo = ItemFragment.NewInstance(0);
            ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, detailsInfo);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;

            if (isPopupMenu)
            {
                this.Activity.FindViewById<RelativeLayout>(Resource.Id.popupMenu).Visibility = ViewStates.Gone;
            }
        }

        void btnListCustomers_Click(object sender, EventArgs e)
        {
            FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            f.Visibility = ViewStates.Visible;

            details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Customers);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            CustomerFragment detailsInfo = CustomerFragment.NewInstance(0, new string[] { });
            ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, detailsInfo);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
            this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;

            if (isPopupMenu)
            {
                this.Activity.FindViewById<RelativeLayout>(Resource.Id.popupMenu).Visibility = ViewStates.Gone;
            }
        }

        void btnAddInvoice_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
        }

        void btnAddItem_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, ItemFragment.NewInstance(0));
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
        }

        void btnAddCustomer_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.detailInfo_fragment, CustomerFragment.NewInstance(0, new string[] { }));
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
        }
    }
}
