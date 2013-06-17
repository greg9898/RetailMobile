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

            bool isTablet = Common.isTabletDevice(this.Activity);

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(ControlIds.INVOICE_SAVE_BUTTON, this.Activity.GetString(Resource.String.btnSave), Resource.Drawable.save_48);
            actionBar.SetTitle(this.Activity.GetString(Resource.String.lblInvoice));

            if (isTablet)
            {
                if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                {
                    actionBar.AddButtonLeft(ControlIds.INVOICE_ADD_BUTTON, "", Resource.Drawable.add_48);
                }
            }
            else
            {
                actionBar.AddButtonLeft(ControlIds.INVOICE_MAINMENU_BUTTON, "", Resource.Drawable.menu_32);
                actionBar.AddButtonLeft(ControlIds.INVOICE_ADD_BUTTON, "", Resource.Drawable.add_48);
            }
            
            invoiceTabHeader = InvoiceTabHeader.NewInstance(this);
            invoiceTabDetails = InvoiceTabDetails.NewInstance(this);
            invoiceTabDetails.DetailsChanged += invoiceTabDetails_DetailsChanged;
            invoiceTabHeader.CustomerChanged += invoiceTabHeader_CustomerChanged;

            tabHost = (TabHost)view.FindViewById(Resource.Id.tabhost);
            tabHost.Setup();
            InitializeTab();   

            if (Common.isPortrait(this.Activity))
            {
//                MainMenuPopup.InitPopupMenu(this.Activity, actionBar.Id);
//                InitPopupMenu();
            }
            else
            {

            }             

            return v;
        }

        void InitPopupMenu()
        {
            int layoutWidth = (Resources.DisplayMetrics.WidthPixels * 31) / 100;
            int layoutHeight = 4 * ((int)Resources.GetDimension(Resource.Dimension.main_menu_icon_size) + 2);
            RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(layoutWidth, layoutHeight);
            lp.AddRule(LayoutRules.Below, actionBar.Id);
            lp.TopMargin = (int)Resources.GetDimension(Resource.Dimension.action_bar_height);

            RelativeLayout popupMenu = this.Activity.FindViewById<RelativeLayout>(Resource.Id.popupMenu);
            Log.Debug("invoiceInfoFr InitPopupMenu", "popupMenu=", popupMenu);
            popupMenu.LayoutParameters = lp;
            popupMenu.SetBackgroundResource(Resource.Drawable.actionbar_background);

            MainMenuFragment mainmenupopup_fragment = (MainMenuFragment)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.mainmenupopup_fragment);  
            mainmenupopup_fragment.IsPopupMenu = true;

            Button btnSettings = this.Activity.FindViewById<Button>(Resource.Id.btnSettingsMain);
            btnSettings.Touch += (object sender, View.TouchEventArgs e) => { 
                switch (e.Event.Action & MotionEventActions.Mask)
                {
                    case MotionEventActions.Up:
                        popupMenu.Visibility = ViewStates.Gone;
                        SettingsClicked();
                        break;
                }
            };
        }

        void SettingsClicked()
        {
            if (Common.isTabletDevice(this.Activity))
            {                
                this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Gone;
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.detailInfo_fragment, new SettingsFragment());

                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.Commit();
            }
            else
            {
                var intent = new Android.Content.Intent();
                intent.SetClass(this.Activity, typeof(SettingsFragmentActivity));
                StartActivity(intent);
            }
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
            spec2.SetContent(Resource.Id.tab2);
            spec2.SetIndicator(this.Activity. LayoutInflater.Inflate( Resource.Layout.invoice_tab_button_details, null));
            tabHost.AddTab(spec2);

            for (int i = 0; i < tabHost.TabWidget.ChildCount; i++)
            {
                View tab = tabHost.TabWidget.GetChildAt(i);
                tab.SetBackgroundResource(Resource.Drawable.main_button_selector);
                tab.SetMinimumHeight(18);
            }

            var ft = FragmentManager.BeginTransaction();        
            ft.Replace(Resource.Id.realtabcontent, invoiceTabHeader);
            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft.Commit();

            var ft1 = FragmentManager.BeginTransaction();        
            ft1.Replace(Resource.Id.tab2, invoiceTabDetails);
            ft1.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
            ft1.Commit(); 
        }

        void ActionBarButtonClicked(int id)
        {
            switch (id)
            {
                case ControlIds.INVOICE_SAVE_BUTTON:            
                    try
                    {
                        Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ActionBarButtonClicked SAVE_BUTTON", "ActionBarButtonClicked SAVE_BUTTON " + ex.Message);
                    }
                    break;
                case ControlIds.INVOICE_ADD_BUTTON:
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
                    break;
                case ControlIds.INVOICE_MAINMENU_BUTTON:
                    RelativeLayout popupMenu = this.Activity.FindViewById<RelativeLayout>(Resource.Id.popupMenu);
                    popupMenu.Visibility = popupMenu.Visibility == ViewStates.Visible ? ViewStates.Gone : ViewStates.Visible;
                    break;
            }
        }

        void ResetInvoiceScreen()
        {
            header = new Library.TransHed();

            invoiceTabHeader.InitHeader();
            invoiceTabDetails.LoadDetailsAdapter();
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
            bool isNew = header.IsNew;
            header.Save(Activity);

            if (isNew && InvoiceSaved != null)
                InvoiceSaved(header.HtrnId);

            ResetInvoiceScreen();
        }
    }
}
