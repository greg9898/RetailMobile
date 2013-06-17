using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.Util;
using RetailMobile.Library;

namespace RetailMobile
{
    public class InvoiceTabHeader : BaseFragment
    {
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
        InvoiceInfoFragment invoiceParentView;

        public delegate void CustomerChangedDelegate();

        public event CustomerChangedDelegate CustomerChanged;

        public static InvoiceTabHeader NewInstance(InvoiceInfoFragment parentView)
        {
            var detailsFrag = new InvoiceTabHeader { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", parentView.ObjectId);
            detailsFrag.invoiceParentView = parentView;

            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                return null;
            }

            View v = inflater.Inflate(Resource.Layout.invoice_tab_header, container, false);
      			
            Button btnSearchCustomer = v.FindViewById<Button>(Resource.Id.btnSearchCustomer);
            btnSearchCustomer.Click += btnSearchCustomer_Click;

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

            //tbCustCode.TextChanged += tbCustCode_TextChanged;
            tbCustCode.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbCustCode_TextChanged);
            tbHtrnID.TextChanged += tbHtrnID_TextChanged;
            tbHtrnID.Text = this.ObjectId.ToString();

            tbHtrnDate.Focusable = false;
            tbHtrnDate.Click += (object sender, EventArgs e) => {
                ShowCalendar(v.Context, invoiceParentView.Header.TransDate);
            };     

            FillInvoiceFields();

            return v;
        }

        void tbHtrnID_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (init)
                return;
            double hedId = double.Parse(((EditText)sender).Text);

            if (invoiceParentView == null || invoiceParentView.Header.HtrnId == hedId)
            {
                return;
            }

            invoiceParentView.Header = Library.TransHed.GetTransHed(Activity, hedId);
            if (invoiceParentView.Header != null && tbHtrnExpln != null)
            {              
                LoadCustomerData(invoiceParentView.Header.CstId);
            }
        }

        bool isCustChanging = false;

        void tbCustCode_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (init)
                return;
            if (isCustChanging)
            {
                return;
            }

            if (string.IsNullOrEmpty(((EditText)sender).Text))
            {
                FillCustomerFields(new CustomerInfo());
                invoiceParentView.Header.CstId = 0;
                return;
            }

            Log.Debug("tbCustCode_TextChanged", " text=" + ((EditText)sender).Text);
            CustomerInfo c = CustomerInfo.GetCustomer(Activity, ((EditText)sender).Text);

            if (tbCustDesc != null)
            {
                FillCustomerFields(c);

                if (invoiceParentView != null)
                {
                    invoiceParentView.Header.CstId = c.CustID;
                }
            }
        }

        void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            CustomerSelectDialog custDlg = new CustomerSelectDialog(Activity, Resource.Style.actionDialog);
            custDlg.Window.SetLayout(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent);

            custDlg.DismissEvent += (s, ee) =>
            {
                if (custDlg.CustId > 0)
                {
                    invoiceParentView.Header.CstId = custDlg.CustId;
                    LoadCustomerData(custDlg.CustId);
                }
            };
            custDlg.Show();
        }

        public  void FillInvoiceFields()
        {
            if (invoiceParentView == null)
            {
                return;
            }

            tbHtrnExpln.Text = invoiceParentView.Header.HtrnExpl;
            tbHtrnDate.Text = invoiceParentView.Header.TransDateText;
      
            FillHeaderCalcValues();
        }

        void ShowCalendar(Android.Content.Context ctx, DateTime currentDate)
        {
            CalendarView calendarDlg = new CalendarView(ctx, currentDate);
            calendarDlg.DateSlected += new CalendarView.DateSelectedDelegate(d => {
                //Common.DateFormatDateOnly
                tbHtrnDate.Text = d.ToShortDateString ();
                invoiceParentView.Header.TransDate = DateTime.Parse (tbHtrnDate.Text);
                DateTime now = DateTime.Now;
                invoiceParentView.Header.TransDate = new DateTime (invoiceParentView.Header.TransDate.Year, invoiceParentView.Header.TransDate.Month,
                                                                   invoiceParentView.Header.TransDate.Day, now.Hour, now.Minute, now.Second);
            });
            calendarDlg.Show();
        }

        void LoadCustomerData(double custId)
        {
            Library.CustomerInfo c = Library.CustomerInfo.GetCustomer(Activity, custId);

            if (tbCustDesc != null)
            {
                FillCustomerFields(c);
                tbCustCode.Text = c.Code;
                FillInvoiceFields();

                if (CustomerChanged != null)
                {
                    CustomerChanged();
                }
            }
        }

        bool init;

        public void InitHeader()
        {
            init = true;
            tbHtrnID.Text = "0";
            FillInvoiceFields();
            tbCustCode.Text = "";
            FillCustomerFields(new Library.CustomerInfo ());
            init = false;
        }

        public void FillCustomerFields(Library.CustomerInfo c)
        {
            isCustChanging = true;

            tbCustDesc.Text = c.Name;
            tbCustAddress.Text = c.CustAddress;
            //NE RAZKOMENTIRAI, NE MOJE DA SE LOADVA CUSTOMER PO KOD!!!!
            //tbCustCode.Text = c.Code;
            tbCustDebt.Text = c.CustDebt.ToString();
            tbCustPhone.Text = c.CustPhone;

            isCustChanging = false;
        }

        public void FillHeaderCalcValues()
        {
            tbHtrnNetValue.Text = invoiceParentView.Header.HtrnNetVal.ToString(Common.DecimalFormat);
            tbHtrnVatValue.Text = invoiceParentView.Header.HtrnVatVal.ToString(Common.DecimalFormat);
            tbHtrnTotValue.Text = invoiceParentView.Header.HtrnTotValue.ToString(Common.DecimalFormat);
        }

        public void SetDataToHeader()
        {
            if (tbHtrnNetValue.Text.Trim() == "")
            {
                tbHtrnNetValue.Text = "0";
            }
            if (tbHtrnVatValue.Text == "")
            {
                tbHtrnVatValue.Text = "0";
            }

            invoiceParentView.Header.HtrnExpl = tbHtrnExpln.Text;
            invoiceParentView.Header.TransDate = DateTime.Parse(tbHtrnDate.Text);
            DateTime now = DateTime.Now;
            invoiceParentView.Header.TransDate = new DateTime(invoiceParentView.Header.TransDate.Year, invoiceParentView.Header.TransDate.Month,
            invoiceParentView.Header.TransDate.Day, now.Hour, now.Minute, now.Second);
        }
    }
}
