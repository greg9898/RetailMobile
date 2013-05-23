using System;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Content;

namespace RetailMobile
{
    public class InvoiceInfoFragment : BaseFragment
    {
        const int SAVE_BUTTON = 764;
        Library.TransHed header;
        TransDetAdapter detailsAdapter;
        ListView lvDetails;
        EditText tbCustDesc;
        EditText tbHtrnExpln;
        EditText tbHtrnID;
        EditText tbCustCode;
        EditText tbCustAddress;
        EditText tbCustPhone;
        EditText tbCustDebt;
        EditText tbHtrnDate;
        EditText tbHtrnNetValue;
        EditText tbHtrnVatValue;
        EditText tbHtrnTotValue;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public static InvoiceInfoFragment NewInstance(long objId)
        {
            var detailsFrag = new InvoiceInfoFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }

        public Library.TransHed Header
        {
            get
            {
                return header;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.InvoiceScreen, container, false);

            header = new Library.TransHed();

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(SAVE_BUTTON, this.Activity.GetString(Resource.String.btnSave), Resource.Drawable.save_48);
            actionBar.SetTitle(this.Activity.GetString(Resource.String.lblInvoice));
            
            Button btnSearchItems = v.FindViewById<Button>(Resource.Id.btnSearchItems);
            Button btnSearchCustomer = v.FindViewById<Button>(Resource.Id.btnSearchCustomer);
            Button btnAddValue = v.FindViewById<Button>(Resource.Id.btnAddValue);
            Button btnSubstractValue = v.FindViewById<Button>(Resource.Id.btnSubstractValue);
            btnSearchCustomer.Click += new EventHandler(btnSearchCustomer_Click);
            btnSearchItems.Click += new EventHandler(btnSearchItems_Click);
            btnAddValue.Click += btnAddValue_Click;
            btnSubstractValue.Click += btnSubstractValue_Click;
            
            lvDetails = v.FindViewById<ListView>(Resource.Id.lvDetails);
            lvDetails.AddHeaderView(inflater.Inflate (Resource.Layout.TransDetRow_header, null));

            tbCustCode = v.FindViewById<EditText>(Resource.Id.tbCustCode1);
            tbHtrnID = v.FindViewById<EditText>(Resource.Id.tbHtrnID);
            tbCustDesc = v.FindViewById<EditText>(Resource.Id.tbCustName1);
            tbCustAddress = v.FindViewById<EditText>(Resource.Id.tbCustAddress);
            tbCustPhone = v.FindViewById<EditText>(Resource.Id.tbCustPhone);
            tbCustDebt = v.FindViewById<EditText>(Resource.Id.tbCustDebt);
            tbHtrnExpln = v.FindViewById<EditText>(Resource.Id.tbHtrnExpln);
            tbHtrnDate = v.FindViewById<EditText>(Resource.Id.tbHtrnDate);
            tbHtrnNetValue = v.FindViewById<EditText>(Resource.Id.tbHtrnNetValue);
            tbHtrnVatValue = v.FindViewById<EditText>(Resource.Id.tbHtrnVatValue);
            tbHtrnTotValue = v.FindViewById<EditText>(Resource.Id.tbHtrnTotValue);
            
            tbCustCode.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbCustCode_TextChanged);
            tbHtrnID.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbHtrnID_TextChanged);
            tbHtrnID.Text = this.ObjectId.ToString();
            
            tbHtrnDate.Focusable = false;
            tbHtrnDate.Click += (object sender, EventArgs e) => {
                ShowCalendar(v.Context, header.TransDate);
            };

            LoadDetailsAdapter();

//            if (this.ObjectId > 0)
//            {
//                tbCustCode .Enabled = false;
//                tbHtrnID .Enabled = false;
//                tbCustDesc.Enabled = false;
//                tbCustAddress .Enabled = false;
//                tbCustPhone.Enabled = false;
//                tbCustDebt .Enabled = false;
//                tbHtrnExpln .Enabled = false;
//                tbHtrnDate .Enabled = false;
//                tbHtrnNetValue .Enabled = false;
//                tbHtrnVatValue.Enabled = false;
//                tbHtrnTotValue.Enabled = false;
//
//                tbCustCode.Enabled = false;
//                tbHtrnID.Enabled = false;
//
//                tbHtrnDate.Enabled = false;
//                btnSearchCustomer.Enabled = false;
//                btnSearchItems.Enabled = false;
//                lvDetails.Clickable = false;
//                lvDetails.Focusable = false;
//                lvDetails.FocusableInTouchMode = false;
//                btnAddValue.Enabled = false;
//                btnSubstractValue.Enabled = false;
//           
//                HeaderViewListAdapter adapter = (HeaderViewListAdapter)lvDetails.Adapter;
//                TransDetAdapter origAdapter = (TransDetAdapter)adapter.WrappedAdapter;
//                origAdapter.Disable();
//            }

            return v;
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

        void ShowCalendar(Android.Content.Context ctx, DateTime currentDate)
        {
            CalendarView calendarDlg = new CalendarView(ctx, currentDate);
            calendarDlg.DateSlected += new CalendarView.DateSelectedDelegate(d => {
				//PreferencesUtil.DateFormatDateOnly
				tbHtrnDate.Text = d.ToShortDateString ();
				header.TransDate = DateTime.Parse (tbHtrnDate.Text);
				DateTime now = DateTime.Now;
				header.TransDate = new DateTime (header.TransDate.Year, header.TransDate.Month, header.TransDate.Day, now.Hour, now.Minute, now.Second);
			});
            calendarDlg.Show();
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

        void InitInvoiceScreen()
        {
            header = new Library.TransHed();
            FillInvoiceFields();
            FillCustomerFields(new Library.CustomerInfo ());
            
            LoadDetailsAdapter();
        }

        void FillInvoiceFields()
        {
            tbHtrnExpln.Text = header.HtrnExpl;
            tbHtrnDate.Text = header.TransDateText;
            tbHtrnNetValue.Text = header.HtrnNetVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnVatValue.Text = header.HtrnVatVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnTotValue.Text = header.HtrnTotValue.ToString(PreferencesUtil.DecimalFormat);
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

            if (tbHtrnNetValue.Text.Trim() == "")
            {
                tbHtrnNetValue.Text = "0";
            }
            if (tbHtrnVatValue.Text == "")
            {
                tbHtrnVatValue.Text = "0";
            }

            header.HtrnExpl = tbHtrnExpln.Text;
            header.TransDate = DateTime.Parse(tbHtrnDate.Text);
            DateTime now = DateTime.Now;
            header.TransDate = new DateTime(header.TransDate.Year, header.TransDate.Month, header.TransDate.Day, now.Hour, now.Minute, now.Second);
            
            header.Save(Activity);
            InitInvoiceScreen();
        }

        void tbHtrnID_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            header = Library.TransHed.GetTransHed(Activity, double.Parse(((EditText)sender).Text));
            if (header != null && tbHtrnExpln != null)
            {
                FillInvoiceFields();
                LoadCustomerData(header.CstId);
            }
        }

        bool isCustChanging = false;

        void tbCustCode_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (isCustChanging)
            {
                return;
            }
            
            Log.Debug("tbCustCode_TextChanged", "tbCustCode_TextChanged text=" + ((EditText)sender).Text);
            CustomerInfo c = CustomerInfo.GetCustomer(Activity, ((EditText)sender).Text);
            
            if (tbCustDesc != null)
            {
                tbCustDesc.Text = c.Name;
                tbCustAddress.Text = c.CustAddress;
                tbCustDebt.Text = c.CustDebt.ToString();
                tbCustPhone.Text = c.CustPhone;
                header.CstId = c.CustID;
            }
        }

        void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            CustomerSelectDialog custDlg = new CustomerSelectDialog(Activity, Resource.Style.cust_dialogWrap);
            custDlg.Window.SetLayout(WindowManagerLayoutParams.FillParent, WindowManagerLayoutParams.FillParent);

            custDlg.DismissEvent += (s, ee) =>
            {
                header.CstId = custDlg.CustId;
                Log.Debug("btnSearchCustomer_Click", " header.cst_id =" + custDlg.CustId);
                LoadCustomerData(custDlg.CustId);
            };
            custDlg.Show();
        }

        private void LoadCustomerData(double custId)
        {
            Library.CustomerInfo c = Library.CustomerInfo.GetCustomer(Activity, custId);
            
            if (tbCustDesc != null)
            {
                FillCustomerFields(c);
                
                LoadDetailsAdapter();
            }
        }

        private void FillCustomerFields(Library.CustomerInfo c)
        {
            isCustChanging = true;
            
            tbCustDesc.Text = c.Name;
            tbCustAddress.Text = c.CustAddress;
            tbCustCode.Text = c.Code;
            tbCustDebt.Text = c.CustDebt.ToString();
            tbCustPhone.Text = c.CustPhone;
            
            isCustChanging = false;
        }

        void FillHeaderCalcValues()
        {
            tbHtrnNetValue.Text = header.HtrnNetVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnVatValue.Text = header.HtrnVatVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnTotValue.Text = header.HtrnTotValue.ToString(PreferencesUtil.DecimalFormat);
        }

        TransDetAdapter.SelectedDetail selectedDetail = null;

        public  void LoadDetailsAdapter()
        {
            detailsAdapter = new TransDetAdapter(Activity, header.TransDetList, this);
            detailsAdapter.QtysChangedEvent += () => 
            {
                FillHeaderCalcValues();
                detailsAdapter.NotifyDataSetChanged();

                if (TransDetAdapter.lastFocusedControl != null)
                {
                    TransDetAdapter.lastFocusedControl.RequestFocus();
                }
            };
            detailsAdapter.DetailFieldSelectedEvent += (selDetail) => {
                selectedDetail = selDetail;
            };
            
            lvDetails.SetAdapter(detailsAdapter);

            FillHeaderCalcValues();
        }

        void btnSubstractValue_Click(object sender, EventArgs e)
        {
            detailsAdapter.SubstractValue();
        }

        void btnAddValue_Click(object sender, EventArgs e)
        {
            detailsAdapter.AddValue();
        }

        void btnSearchItems_Click(object sender, EventArgs e)
        {
            ItemsSelectDialog dialogItems = new ItemsSelectDialog(Activity, Resource.Style.cust_dialog, header);
            dialogItems.DismissEvent += (s, ee) =>
            {
                foreach (int itemId in dialogItems.CheckedItemIds.Keys)
                {
                    TransDet transDet = new TransDet();
                    transDet.LoadItemInfo(Activity, itemId, dialogItems.CheckedItemIds[itemId], header.CstId);
                    Log.Debug("btnOKItem_Click", itemId + " " + transDet.ItemDesc);
                    header.TransDetList.Add(transDet);
                }
                
                LoadDetailsAdapter();
            };
            
            dialogItems.Show();
        }
    }
}
