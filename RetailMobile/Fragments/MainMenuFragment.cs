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
        Button btnAddInvoice;
        Button btnListInvoices;
        Button btnAddItem;
        Button btnListItems;
        Button btnAddCustomer;
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

            btnAddInvoice = v.FindViewById<Button>(Resource.Id.btnAddInvoice);
            btnListInvoices = v.FindViewById<Button>(Resource.Id.btnListInvoice); ;
            btnAddItem = v.FindViewById<Button>(Resource.Id.btnAddItem); ;
            btnListItems = v.FindViewById<Button>(Resource.Id.btnListItems); ;
            btnAddCustomer = v.FindViewById<Button>(Resource.Id.btnAddCustomer); ;
            btnListCustomers = v.FindViewById<Button>(Resource.Id.btnListCustomers);

            btnAddInvoice.Click += new EventHandler(btnAddInvoice_Click);
            btnListInvoices.Click += new EventHandler(btnListInvoices_Click);
            btnAddItem.Click += new EventHandler(btnAddItem_Click);
            btnListItems.Click += new EventHandler(btnListItems_Click);
            btnAddCustomer.Click += new EventHandler(btnAddCustomer_Click);
            btnListCustomers.Click += new EventHandler(btnListCustomers_Click);


            return v;
        }

        void btnListInvoices_Click(object sender, EventArgs e)
        {
            FrameLayout f = this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment);
            f.Visibility = ViewStates.Visible;
            details = DetailsFragment.NewInstance((int)Base.MenuItems.Invoices);
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.details_fragment, details);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();
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
