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

namespace RetailMobile
{
    public class CustomersListAdapter : ArrayAdapter<Library.CustomerInfo>
    {
        Activity context = null;
        Library.CustomerInfoList customerInfoList;

        public CustomersListAdapter(Activity context, int rowResourceID, Library.CustomerInfoList _list)
            : base(context, rowResourceID, _list)
        {
            this.context = context;

            customerInfoList = _list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.customerInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomerInfoRow, null);

            TextView tbCustCode = (TextView)view.FindViewById(Resource.Id.tbCustCode);
            TextView tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustName);
            TextView tbCustTaxNum = (TextView)view.FindViewById(Resource.Id.tbCustTaxNum);
            TextView tbCustDebt = (TextView)view.FindViewById(Resource.Id.tbCustDebt);

            tbCustCode.Text = item.Code;
            tbCustName.Text = item.Name;
            tbCustTaxNum.Text = item.CustTaxNum;
            tbCustDebt.Text = item.CustDebt.ToString();

            return view;
        }
    }
}