using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class CustomerFragment : BaseFragment
    {
        const int SAVE_BUTTON = 765;
        TabHost mTabHost;
        TextView tbCustCode;
        TextView tbCustName;
        CustomerInfo customer;
        RetailMobile.Fragments.ItemActionBar actionBar;
        StatisticTabMonthly statisticTabMonthly;
        StatisticTabByDate statisticTabByDate;
        View viewCust;
        string[] customerNames;

        public static CustomerFragment NewInstance(long objId, string[] custNames)
        {
            var detailsFrag = new CustomerFragment
            {
                Arguments = new Bundle ()
            };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            detailsFrag.customerNames = custNames;
            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CustomerDetails, container, false);
            viewCust = view;

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.SetTitle(this.Activity.GetString (Resource.String.miCustomers));
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(SAVE_BUTTON, this.Activity.GetString(Resource.String.btnSave), Resource.Drawable.save_48);
            view.FindViewById<FrameLayout>(Resource.Id.realtabcontent).Visibility = ViewStates.Gone;

            customer = CustomerInfo.GetCustomer(Activity, this.ObjectId);
            
            tbCustCode = (TextView)view.FindViewById(Resource.Id.tbCustomerCode);
            tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustomerName);
         
            tbCustCode.Text = customer.Code;
            tbCustName.Text = customer.Name;

            statisticTabMonthly = StatisticTabMonthly.NewInstance(this.ObjectId);
            statisticTabByDate = StatisticTabByDate.NewInstance(this.ObjectId, customerNames);

            mTabHost = (TabHost)view.FindViewById(Resource.Id.tabhost);
            mTabHost.TabChanged += TabHostHandleTabChanged; 
            mTabHost.Setup();
            InitializeTab(view);

            GC.Collect();
            return view;
        }

        void ActionBarButtonClicked(int id)
        {
            if (id == SAVE_BUTTON)
            {
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    Log.Error("ActionBarButtonClicked SAVE_BUTTON", "ActionBarButtonClicked SAVE_BUTTON " + ex.Message);
                }
            }
        }

        public override void OnDestroyView()
        {
            actionBar.ActionButtonClicked -= new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            base.OnDestroyView();
        }

        public override void OnPause()
        {
            actionBar.ActionButtonClicked -= new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            base.OnPause();
        }

        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);

            mTabHost = (TabHost)this.Activity.FindViewById(Resource.Id.tabhost);
        }

        void TabHostHandleTabChanged(object sender, TabHost.TabChangeEventArgs e)
        {
            if (e.TabId.Equals("Monthly"))
            {
                PushFragments(statisticTabMonthly);
            }
            else if (e.TabId.Equals("By Date"))
            {
                PushFragments(statisticTabByDate);
            }
        }
        /// <summary>
        /// Initialize the tabs and set views and identifiers for the tabs
        /// </summary>
        /// <param name="view">View.</param>
        public void InitializeTab(View view)
        {		
            TabHost.TabSpec spec1 = mTabHost.NewTabSpec("Monthly");
            spec1.SetContent(Resource.Id.realtabcontent);
            spec1.SetIndicator(this.Activity. LayoutInflater.Inflate( Resource.Layout.statistic_tab_button_month, null));//"Monthly", Resource.Drawable.statistic_date48);
            mTabHost.AddTab(spec1);		
		
            TabHost.TabSpec spec2 = mTabHost.NewTabSpec("By Date");
            spec2.SetContent(Resource.Id.realtabcontent);
            spec2.SetIndicator(this.Activity. LayoutInflater.Inflate( Resource.Layout.statistic_tab_button_date, null));//"By Date", Resource.Drawable.statistic_month);
            mTabHost.AddTab(spec2);

            for (int i=0; i<mTabHost.TabWidget.ChildCount; i++)
            {
                View tab = mTabHost.TabWidget.GetChildAt(i);
                tab.SetBackgroundResource(Resource.Drawable.main_button_selector);
                tab.SetMinimumHeight(18);
            }

            mTabHost.SetCurrentTabByTag("By Date");
            mTabHost.SetCurrentTabByTag("Monthly");
        }

        public void PushFragments(Android.Support.V4.App.Fragment fragment)
        {

            var ft = FragmentManager.BeginTransaction();		
            ft.Replace(Resource.Id.realtabcontent, fragment);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            viewCust.FindViewById<FrameLayout>(Resource.Id.realtabcontent).Visibility = ViewStates.Visible;
        }

        void Save()
        {
            try
            {
                customer.Code = tbCustCode.Text;
                customer.Name = tbCustName.Text;
                
                customer.Save(Activity);
                Toast.MakeText(this.Activity.ApplicationContext, this.Activity.GetString(Resource.String.CustomerSavedSuccessMsg), ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Log.Error("Save customer failed", ex.Message);
                throw;
            }
        }
    }
}