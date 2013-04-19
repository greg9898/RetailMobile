using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace RetailMobile
{
    public class CustomerSelectDialog : Dialog
    {
        Activity activity;
        ListView lvCustomers;
        Button btnCloseCust;
        EditText tbCustCode;
        EditText tbCustName;
        long _custId;

        public CustomerSelectDialog(Activity context, int theme)
            : base(context, theme)
        {
            activity = context;
            //dialogCustomers.Window.SetLayout(Android.Widget.RelativeLayout.LayoutParams.FillParent, Android.Widget.RelativeLayout.LayoutParams.FillParent);
            SetTitle(context.GetString(Resource.String.miCustomers));
            SetContentView(Resource.Layout.dialog_customer_search);

            Button btnOKCust = FindViewById<Button>(Resource.Id.btnOK);
            btnCloseCust = FindViewById<Button>(Resource.Id.btnClose);
            lvCustomers = FindViewById<ListView>(Resource.Id.lvCustomers);
            tbCustCode = FindViewById<EditText>(Resource.Id.tbCustCode);
            tbCustName = FindViewById<EditText>(Resource.Id.tbCustName);

            tbCustCode.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            tbCustName.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            btnCloseCust.Click += new EventHandler(btnCloseCust_Click);

            lvCustomers.ChoiceMode = ChoiceMode.Single;
            lvCustomers.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(lvCustomers_ItemClick);

            btnOKCust.Click += new EventHandler(btnOKCust_Click);
        }

        void lvCustomers_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Library.CustomerInfo c = ((CustomersAdapter)lvCustomers.Adapter).GetItem(e.Position);
            _custId = c.CustID;
            Log.Debug("lvCustomers_ItemClick", "_custId=" + _custId);
            if (_custId > 0)
            {
                Dismiss();
            }
        }

        void tbSearch_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            Library.CustomerInfoList custInfoList = Library.CustomerInfoList.GetCustomerInfoList(activity, new Library.CustomerInfoList.Criteria()
            {
                CustCode = tbCustCode.Text,
                CustName = tbCustName.Text
            });
            lvCustomers.Adapter = new CustomersAdapter(activity, custInfoList);
        }

        void btnCloseCust_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        void btnOKCust_Click(object sender, EventArgs e)
        {
            Log.Debug("btnOKCust_Click", "_custId=" + _custId);
            if (_custId > 0)
            {
                Dismiss();
            }
        }

        public long CustId
        {
            get { return _custId; }
        }
    }
}