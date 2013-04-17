using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace RetailMobile
{
    public class InvoiceInfoFragment : BaseFragment
    {
        Library.TransHed header;
        TransDetAdapter detailsAdapter;
        ListView lvDetails;
        private EditText tbCustDesc;
        private EditText tbHtrnExpln;
        private EditText tbHtrnID;
        private EditText tbCustCode;
        private EditText tbCustAddress;
        private EditText tbCustPhone;
        private EditText tbCustDebt;
        private EditText tbHtrnDate;
        private EditText tbHtrnNetValue;
        private EditText tbHtrnVatValue;
        private EditText tbHtrnTotValue;

        public static BaseFragment NewInstance(long objId)
        {
            var detailsFrag = new InvoiceInfoFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.InvoiceScreen, container, false);

            header = new Library.TransHed();

            Button btnSearchItems = v.FindViewById<Button>(Resource.Id.btnSearchItems);
            Button btnSearchCustomer = v.FindViewById<Button>(Resource.Id.btnSearchCustomer);
            Button btnSave = v.FindViewById<Button>(Resource.Id.btnSave);
            btnSearchCustomer.Click += new EventHandler(btnSearchCustomer_Click);
            btnSearchItems.Click += new EventHandler(btnSearchItems_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            lvDetails = v.FindViewById<ListView>(Resource.Id.lvDetails);
            lvDetails.AddHeaderView(inflater.Inflate(Resource.Layout.TransDetRow_header, null));
            LoadDetailsAdapter();

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

            if (tbCustCode != null)
                tbCustCode.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbCustCode_TextChanged);
            if (tbHtrnID != null)
                tbHtrnID.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbHtrnID_TextChanged);
                    
            tbHtrnID.Text = base.ObjectId.ToString();

            return v;
        }

        private void InitInvoiceScreen()
        {
            header = new Library.TransHed();
            FillInvoiceFields();
            FillCustomerFields(new Library.CustomerInfo());

            LoadDetailsAdapter();
        }

        private void FillInvoiceFields()
        {
            tbHtrnExpln.Text = header.HtrnExpl;
            tbHtrnDate.Text = header.HtrnDate.ToString();
            tbHtrnNetValue.Text = header.HtrnNetVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnVatValue.Text = header.HtrnVatVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnTotValue.Text = header.HtrnTotValue.ToString(PreferencesUtil.DecimalFormat);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (tbHtrnNetValue.Text.Trim() == "")
            {
                tbHtrnNetValue.Text = "0";
            }
            if (tbHtrnVatValue.Text == "")
            {
                tbHtrnVatValue.Text = "0";
            }
            header.HtrnExpl = tbHtrnExpln.Text;
            header.HtrnDate = DateTime.Parse(tbHtrnDate.Text);
            header.HtrnNetVal = double.Parse(tbHtrnNetValue.Text);
            header.HtrnVatVal = double.Parse(tbHtrnVatValue.Text);
            
            header.Save(this.Activity);
            InitInvoiceScreen();
        }

        void tbHtrnID_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            header = Library.TransHed.GetTransHed(this.Activity, double.Parse(((EditText)sender).Text));
            if (header != null && tbHtrnExpln != null)
            {
                FillInvoiceFields();
                LoadCustomerData(header.CstId);
                LoadDetailsAdapter();
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
            Library.CustomerInfo c = Library.CustomerInfo.GetCustomer(this.Activity, ((EditText)sender).Text);

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
            CustomerSelectDialog custDlg = new CustomerSelectDialog(this.Activity, Resource.Style.cust_dialogWrap);
            custDlg.DismissEvent += (s, ee) =>
            {
                header.CstId = (double)custDlg.CustId;
                Log.Debug("btnSearchCustomer_Click", " header.cst_id =" + custDlg.CustId);
                LoadCustomerData(custDlg.CustId);
            };
            custDlg.Show();
        }

        private void LoadCustomerData(double custId)
        {
            Library.CustomerInfo c = Library.CustomerInfo.GetCustomer(this.Activity, custId);

            if (tbCustDesc != null)
            {
                FillCustomerFields(c);
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
            header.CalcValues();
            tbHtrnNetValue.Text = header.HtrnNetVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnVatValue.Text = header.HtrnVatVal.ToString(PreferencesUtil.DecimalFormat);
            tbHtrnTotValue.Text = header.HtrnTotValue.ToString(PreferencesUtil.DecimalFormat);
        }

        void LoadDetailsAdapter()
        {
            detailsAdapter = new TransDetAdapter(Activity, header.TransDetList);
            detailsAdapter.QtysChangedEvent += () => 
            {
                FillHeaderCalcValues();

            };

            lvDetails.SetAdapter(detailsAdapter);
        }

        void btnSearchItems_Click(object sender, EventArgs e)
        {
            ItemsSelectDialog dialogItems = new ItemsSelectDialog(this.Activity, Resource.Style.cust_dialog);
            dialogItems.DismissEvent += (s, ee) =>
            {
                foreach (int itemId in dialogItems.CheckedItemIds.Keys)
                {

                    Library.TransDet transDet = new Library.TransDet();
                    transDet.LoadItemInfo(this.Activity, itemId, dialogItems.CheckedItemIds[itemId], header.CstId);
                    Android.Util.Log.Debug("btnOKItem_Click", itemId + " " + transDet.ItemDesc);
                    header.TransDetList.Add(transDet);
                }
 
                FillHeaderCalcValues();
                LoadDetailsAdapter();
            };

            dialogItems.Show();
        }
    }
}
