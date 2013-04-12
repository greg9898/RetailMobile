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
using Android.Support.V4.App;
using RetailMobile.Library;

namespace RetailMobile
{
    public class CustomerFragment : BaseFragment
    {
        TextView tbCustCode;
        TextView tbCustName;

        CustomerInfo customer;

        public static CustomerFragment NewInstance(long objId)
        {
            var detailsFrag = new CustomerFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }  

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CustomerDetails, container, false);

            customer = CustomerInfo.GetCustomer(this.Activity, base.ObjectId);

            tbCustCode = (TextView)view.FindViewById(Resource.Id.tbCustomerCode);
            tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustomerName);
            Button btnSave = (Button)view.FindViewById(Resource.Id.btnSave);
            btnSave.Click += new EventHandler(btnSave_Click);
            //get item by ItemId ..
            tbCustCode.Text = customer.Code;
            tbCustName.Text = customer.Name;

            return view;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                customer.Code = tbCustCode.Text;
                customer.Name = tbCustName.Text;

                customer.Save(this.Activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}