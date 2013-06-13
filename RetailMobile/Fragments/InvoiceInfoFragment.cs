using System;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using RetailMobile.Fragments;

namespace RetailMobile
{
    public class InvoiceInfoFragment : BaseFragment
    {
        public delegate void InvoiceSavedDelegate(int id);

        public event InvoiceSavedDelegate InvoiceSaved;

        TransHed header = new TransHed();
        TabHost tabHost;
        ItemActionBar actionBar;
        View view;
        InvoiceTabHeader invoiceTabHeader;
        InvoiceTabDetails invoiceTabDetails;
        bool isTabletLand;

        public static InvoiceInfoFragment NewInstance(long objId)
        {
            var detailsFrag = new InvoiceInfoFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }

        public Library.TransHed Header
        {
            get { return header;}
            set{ header = value;}
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.InvoiceScreen, container, false);
            view = v;

            isTabletLand = this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu) != null;

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(ControlIds.INVOICE_SAVE_BUTTON, this.Activity.GetString(Resource.String.btnSave), Resource.Drawable.save_48);
            actionBar.SetTitle(this.Activity.GetString(Resource.String.lblInvoice));

            if (!isTabletLand)
            {
                actionBar.AddButtonLeft(ControlIds.DETAILS_ADD_BUTTON, "", Resource.Drawable.add_48);
            }
            
            invoiceTabHeader = InvoiceTabHeader.NewInstance(this);
            invoiceTabDetails = InvoiceTabDetails.NewInstance(this);
            invoiceTabDetails.DetailsChanged += invoiceTabDetails_DetailsChanged;
            invoiceTabHeader.CustomerChanged += invoiceTabHeader_CustomerChanged;

            tabHost = (TabHost)view.FindViewById(Resource.Id.tabhost);
            tabHost.TabChanged += TabHostHandleTabChanged; 
            tabHost.Setup();
            InitializeTab();               

            return v;
        }

        public  void LoadDetailsAdapter()
        {
            invoiceTabDetails.LoadDetailsAdapter();
        }

        void invoiceTabHeader_CustomerChanged()
        {            
            LoadDetailsAdapter();
        }

        void invoiceTabDetails_DetailsChanged()
        {
            invoiceTabHeader.FillHeaderCalcValues();
        }

        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);

            tabHost = (TabHost)this.Activity.FindViewById(Resource.Id.tabhost);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            actionBar.ActionButtonClicked -= new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
        }

        public override void OnPause()
        {
            base.OnPause();
            actionBar.ActionButtonClicked -= new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
        }

        public void InitializeTab()
        {       
            TabHost.TabSpec spec1 = tabHost.NewTabSpec("Info");
            spec1.SetContent(Resource.Id.realtabcontent);
            spec1.SetIndicator(this.Activity. LayoutInflater.Inflate( Resource.Layout.invoice_tab_button_header, null));
            tabHost.AddTab(spec1);     

            TabHost.TabSpec spec2 = tabHost.NewTabSpec("Details");
            spec2.SetContent(Resource.Id.realtabcontent);
            spec2.SetIndicator(this.Activity. LayoutInflater.Inflate( Resource.Layout.invoice_tab_button_details, null));
            tabHost.AddTab(spec2);

            for (int i = 0; i < tabHost.TabWidget.ChildCount; i++)
            {
                View tab = tabHost.TabWidget.GetChildAt(i);
                tab.SetBackgroundResource(Resource.Drawable.main_button_selector);
                tab.SetMinimumHeight(18);
            }

            tabHost.SetCurrentTabByTag("Details");
            tabHost.SetCurrentTabByTag("Info");
        }

        void TabHostHandleTabChanged(object sender, TabHost.TabChangeEventArgs e)
        {
            if (e.TabId.Equals("Info"))
            {
                PushFragments(invoiceTabHeader);
            }
            else if (e.TabId.Equals("Details"))
            {
                PushFragments(invoiceTabDetails);
            }
        }

        public void PushFragments(Android.Support.V4.App.Fragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();        
            ft.Replace(Resource.Id.realtabcontent, fragment);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            view.FindViewById<FrameLayout>(Resource.Id.realtabcontent).Visibility = ViewStates.Visible;
        }

        void ActionBarButtonClicked(int id)
        {
            if (id == ControlIds.INVOICE_SAVE_BUTTON)
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
            else  if (id == ControlIds.DETAILS_ADD_BUTTON)
            {
                try
                {
                    var ft = FragmentManager.BeginTransaction();
                    //ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
                    InvoiceInfoFragment invoiceFragment = InvoiceInfoFragment.NewInstance(0);
                    ft.Replace(Resource.Id.detailInfo_fragment, invoiceFragment);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    invoiceFragment.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(InvoiceSaved);
                    ft.Commit();
                }
                catch (Exception ex)
                {
                    Log.Error("exception", ex.Message);
                }
            }
        }

        void ResetInvoiceScreen()
        {
            header = new Library.TransHed();

            invoiceTabHeader.FillInvoiceFields();
            invoiceTabHeader.FillCustomerFields(new Library.CustomerInfo ());

            invoiceTabDetails.LoadDetailsAdapter();

            tabHost.SetCurrentTabByTag("Details");
            tabHost.SetCurrentTabByTag("Info");
        }

        void Save()
        {
            if (header.CstId == 0)
            {
                Toast.MakeText(this.Activity, "Select customer!", ToastLength.Short).Show();
                return;
            }

            if (header.TransDetList == null || header.TransDetList.Count == 0)
            {
                Toast.MakeText(this.Activity, "Choose details!", ToastLength.Short).Show();
                return;
            }
            
            invoiceTabHeader.SetDataToHeader();
           
            header.Save(Activity);

            if (InvoiceSaved != null)
                InvoiceSaved(header.HtrnId);

            ResetInvoiceScreen();
        }
    }
}