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
        TabHost mTabHost;
        TextView tbCustCode;
        TextView tbCustName;
        CustomerInfo customer;
        RetailMobile.Fragments.ItemActionBar actionBar;
        StatisticTabMonthly statisticTabMonthly;
        StatisticTabByDate statisticTabByDate;
        View viewCust;

        public static CustomerFragment NewInstance(long objId)
        {
            var detailsFrag = new CustomerFragment
            {
                Arguments = new Bundle ()
            };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }

        public int CustId
        {
            get { return Arguments.GetInt("ObjectId", -1); }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CustomerDetails, container, false);
            viewCust = view;

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.SetTitle(this.Activity.GetString (Resource.String.miCustomers));
            view.FindViewById<FrameLayout>(Resource.Id.realtabcontent).Visibility = ViewStates.Gone;

            customer = CustomerInfo.GetCustomer(Activity, ObjectId);
            
            tbCustCode = (TextView)view.FindViewById(Resource.Id.tbCustomerCode);
            tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustomerName);
            Button btnSave = (Button)view.FindViewById(Resource.Id.btnSave);
            btnSave.Click += new EventHandler(btnSave_Click);
            tbCustCode.Text = customer.Code;
            tbCustName.Text = customer.Name;

            statisticTabMonthly = StatisticTabMonthly.NewInstance(CustId);
            statisticTabByDate = StatisticTabByDate.NewInstance(CustId);

            mTabHost = (TabHost)view.FindViewById(Resource.Id.tabhost);
            mTabHost.TabChanged += tabHost_HandleTabChanged; 
            mTabHost.Setup();
            InitializeTab(view);
            
            return view;
        }

        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);

            mTabHost = (TabHost)this.Activity.FindViewById(Resource.Id.tabhost);
        }

        void tabHost_HandleTabChanged(object sender, TabHost.TabChangeEventArgs e)
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
		/*
     * Initialize the tabs and set views and identifiers for the tabs
     */
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
		/*
     * adds the fragment to the FrameLayout
     */
        public void PushFragments(Android.Support.V4.App.Fragment fragment)
        {

            var ft = FragmentManager.BeginTransaction();		
            ft.Replace(Resource.Id.realtabcontent, fragment);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            viewCust.FindViewById<FrameLayout>(Resource.Id.realtabcontent).Visibility = ViewStates.Visible;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                customer.Code = tbCustCode.Text;
                customer.Name = tbCustName.Text;
                
                customer.Save(Activity);
            }
            catch (Exception ex)
            {
                Log.Error("Save customer failed", ex.Message);
                throw ex;
            }
        }
    }
}