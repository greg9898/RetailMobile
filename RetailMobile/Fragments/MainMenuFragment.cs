using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using RetailMobile.Fragments;

namespace RetailMobile
{
    public class MainMenuFragment : Android.Support.V4.App.Fragment
    {
        Button btnListInvoices;
        Button btnListItems;
        Button btnListCustomers;
        bool isPopupMenu;
        bool isTablet;

        public bool IsPopupMenu
        {
            get{ return isPopupMenu;}
            set{ isPopupMenu = value;}
        }

        public MainMenuFragment()//required
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

            isTablet = Common.isTabletDevice(this.Activity);
           
            if (isTablet)
            {
                if (!Common.isPortrait(this.Activity))
                {
                    RetailMobile.Fragments.ActionBar bar = (RetailMobile.Fragments.ActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
                    bar.SettingsClicked += new RetailMobile.Fragments.ActionBar.SettingsCLickedDelegate(SettingsClicked);
                }
            }
            else
            {  
                RetailMobile.Fragments.ActionBar bar = (RetailMobile.Fragments.ActionBar)this.FragmentManager.FindFragmentByTag("ActionBarMain");
                if (bar != null)
                {
                    bar.SettingsClicked += new RetailMobile.Fragments.ActionBar.SettingsCLickedDelegate(SettingsClicked);
                }
            }
      
            btnListInvoices = v.FindViewById<Button>(Resource.Id.btnAddInvoice);
            btnListItems = v.FindViewById<Button>(Resource.Id.btnAddItem);
            btnListCustomers = v.FindViewById<Button>(Resource.Id.btnAddCustomer);

            btnListInvoices.Click += new EventHandler(btnListInvoices_Click);
            btnListItems.Click += new EventHandler(btnListItems_Click);
            btnListCustomers.Click += new EventHandler(btnListCustomers_Click);

            return v;
        }

        void SettingsClicked()
        {
            if (isTablet)
            {                
                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new SettingsFragment());

                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
            else
            {
//                var intent = new Android.Content.Intent();
//                intent.SetClass(this.Activity, typeof(SettingsFragmentActivity));
//                StartActivity(intent);
                var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.actionbar_phone_fragment, new ItemActionBar());
                ft.Replace(Resource.Id.content_phone_fragment, new SettingsFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnListInvoices_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
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
                    this.Activity.FindViewById<RelativeLayout>(Resource.Id.popup_mainmenu_inner).Visibility = ViewStates.Gone;
                }
            }
            else
            {
                DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.actionbar_phone_fragment, new RetailMobile.Fragments.ItemActionBar(), "ItemActionBar");
                ft.Replace(Resource.Id.content_phone_fragment, details);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnListItems_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
                f.Visibility = ViewStates.Visible;

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.details_fragment, DetailsFragment.NewInstance((int)MainMenu.MenuItems.Items));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, ItemFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
                this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
                this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;

                if (isPopupMenu)
                {
                    this.Activity.FindViewById<RelativeLayout>(Resource.Id.popup_mainmenu_inner).Visibility = ViewStates.Gone;
                }
            }
            else
            {
                DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Items);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_phone_fragment, details);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnListCustomers_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
                f.Visibility = ViewStates.Visible;

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.details_fragment, DetailsFragment.NewInstance((int)MainMenu.MenuItems.Customers));
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
                    this.Activity.FindViewById<RelativeLayout>(Resource.Id.popup_mainmenu_inner).Visibility = ViewStates.Gone;
                }
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_phone_fragment, DetailsFragment.NewInstance((int)MainMenu.MenuItems.Customers));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnAddInvoice_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_phone_fragment, InvoiceInfoFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnAddItem_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, ItemFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;  
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_phone_fragment, ItemFragment.NewInstance(0));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }

        void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (isTablet)
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, CustomerFragment.NewInstance(0, new string[] { }));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_phone_fragment, CustomerFragment.NewInstance(0, new string[] { }));
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
        }
    }
}
