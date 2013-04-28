using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.FragmentMainMenu, container, false);
			//Android.Support.V4.App.FragmentManager
			RetailMobile.Fragments.ActionBar bar = (RetailMobile.Fragments.ActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBarMain);
			//RetailMobile.Fragments.ActionBar bar = v.FindViewById<RetailMobile.Fragments.ActionBar>(Resource.Id.ActionBarMain);
			bar.SettingsClicked += new RetailMobile.Fragments.ActionBar.SettingsCLickedDelegate(SettingsClicked);

            
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
			this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;

			SettingsFragment settings = new SettingsFragment();
			var ft = FragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.detailInfo_fragment, settings);
			ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
			ft.Commit();

			//RetailMobile.Fragments.ItemActionBar bar = (RetailMobile.Fragments.ItemActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBar);
			//bar.AddButtonRight(1,"save",0);

		}

        void btnListInvoices_Click(object sender, EventArgs e)
        {
            //FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            //f.Visibility = ViewStates.Visible;
            details = DetailsFragment.NewInstance((int)Base.MenuItems.Invoices);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

			InvoiceInfoFragment detailsInfo = InvoiceInfoFragment.NewInstance(0);
			ft = FragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.detailInfo_fragment, detailsInfo);
			ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
			ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
			this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
			this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
        }

        void btnListItems_Click(object sender, EventArgs e)
        {
            FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            f.Visibility = ViewStates.Visible;
            details = DetailsFragment.NewInstance((int)Base.MenuItems.Items);
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
        }

        void btnListCustomers_Click(object sender, EventArgs e)
        {
            FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            f.Visibility = ViewStates.Visible;

            details = DetailsFragment.NewInstance((int)Base.MenuItems.Customers);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

			CustomerFragment detailsInfo = CustomerFragment.NewInstance(0);
			ft = FragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.detailInfo_fragment, detailsInfo);
			ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
			ft.Commit();

			this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
			this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
			this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
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
            ft.Replace(Resource.Id.detailInfo_fragment, CustomerFragment.NewInstance(0));
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
        }
    }
}
