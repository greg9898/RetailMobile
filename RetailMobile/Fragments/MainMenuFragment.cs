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
        Button btnSync;
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
        RetailMobile.Fragments.ItemActionBar actionBar;
        private void InitActionBar()
        {
            actionBar = (RetailMobile.Fragments.ItemActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBar1);
            actionBar.ClearButtons();
            actionBar.AddButtonLeft(1,"",Resource.Drawable.settings_48);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
        }

        private void ActionBarButtonClicked(int buttonID)
        {
            if (buttonID == 1)
            {
                SettingsClicked();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.FragmentMainMenu, container, false);

            isTablet = Common.isTabletDevice(this.Activity);
           
            InitActionBar();

            /*if (isTablet)
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
            }*/
      
            btnListInvoices = v.FindViewById<Button>(Resource.Id.btnAddInvoice);
            btnListItems = v.FindViewById<Button>(Resource.Id.btnAddItem);
            btnListCustomers = v.FindViewById<Button>(Resource.Id.btnAddCustomer);
            btnSync = v.FindViewById<Button>(Resource.Id.btnSync);

            btnListInvoices.Click += new EventHandler(btnListInvoices_Click);
            btnListItems.Click += new EventHandler(btnListItems_Click);
            btnListCustomers.Click += new EventHandler(btnListCustomers_Click);
            btnSync.Click += new EventHandler(btnSync_Click);

            return v;
        }

        void SettingsClicked()
        {
            RelativeLayout f3 = this.Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);
            if(f3 != null)
            {
                LinearLayout f2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
                f2.Visibility = ViewStates.Gone;

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment3, new SettingsFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.AddToBackStack("Settings");
                ft.Commit();
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment1, new SettingsFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.AddToBackStack("Settings");
                ft.Commit();
            }

            /*if (isTablet)
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
            }*/
        }


        void btnSync_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => Sync.SyncTrans (this.Activity)
                ).ContinueWith(task => this.Activity.RunOnUiThread (() => { 
                    ((Main)this.Activity).HideProgressBar ();
                    Toast.MakeText (this.Activity.ApplicationContext, this.Activity.GetString (Resource.String.SynchronizationComplete), ToastLength.Short).Show ();
                }));
        }

        void btnListInvoices_Click(object sender, EventArgs e)
        {
            LinearLayout l2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
            l2.Visibility = ViewStates.Visible;

            RelativeLayout f1 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment1);
            RelativeLayout f2 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment2);
            RelativeLayout f3 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);

            if (f2 != null && f3 != null)
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment2, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
                            
                InvoiceInfoFragment fragmentInvoice = InvoiceInfoFragment.NewInstance(0);
                ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment3, fragmentInvoice);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                fragmentInvoice.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(fragmentDetails.InvoiceSaved);
            }
            else
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment1, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }

            /*if (isTablet)
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
            }*/
        }

        void btnListItems_Click(object sender, EventArgs e)
        {
            LinearLayout l2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
            l2.Visibility = ViewStates.Visible;

            RelativeLayout f1 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment1);
            RelativeLayout f2 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment2);
            RelativeLayout f3 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);

            if (f2 != null && f3 != null)
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Items);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment2, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                ItemFragment fragmentItem = ItemFragment.NewInstance(0);
                ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment3, fragmentItem);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                //fragmentInvoice.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(fragmentDetails.InvoiceSaved);
            }
            else
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Items);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment1, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }

            /*if (isTablet)
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
            }*/
        }

        void btnListCustomers_Click(object sender, EventArgs e)
        {
            LinearLayout l2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
            l2.Visibility = ViewStates.Visible;

            RelativeLayout f1 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment1);
            RelativeLayout f2 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment2);
            RelativeLayout f3 = Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);

            if (f2 != null && f3 != null)
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Customers);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment2, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                CustomerFragment fragmentCustomers = CustomerFragment.NewInstance(0,new string[] { });
                ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment3, fragmentCustomers);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();

                //fragmentInvoice.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(fragmentDetails.InvoiceSaved);
            }
            else
            {
                DetailsFragment fragmentDetails = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Customers);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment1, fragmentDetails);
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }

            /*if (isTablet)
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
            }*/
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
