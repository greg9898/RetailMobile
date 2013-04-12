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
    public class TransDetAdapter : ArrayAdapter<Library.TransDet>
    {
        Activity context = null;
        Library.TransDetList dataSource;

        public TransDetAdapter(Activity context, Library.TransDetList list)
            : base(context, Resource.Layout.TransDetRow, list)
        {
            this.context = context;

            dataSource = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TransDetRow, null);

            TextView lblItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
            TextView lblItemDesc = (TextView)view.FindViewById(Resource.Id.tbItemDesc);
            EditText tbDtrn_qty1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_qty1);
            EditText tbDtrn_unit_price = view.FindViewById<EditText>(Resource.Id.tbDtrn_unit_price);
            EditText tbDtrn_disc_line1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_disc_line1);
            EditText tbDtrn_net_value = view.FindViewById<EditText>(Resource.Id.tbDtrn_net_value);
            EditText tbDtrn_vat_value = view.FindViewById<EditText>(Resource.Id.tbDtrn_vat_value);

            Library.TransDet detail = dataSource[position];
            lblItemCode.Text = detail.ItemCode;
            lblItemDesc.Text = detail.ItemDesc;
            tbDtrn_qty1.Text = detail.DtrnQty1.ToString();
            tbDtrn_unit_price.Text = detail.dtrn_unit_price.ToString();
//            tbDtrn_disc_line1.Text = detail.dtrn_disc_line1.ToString();
            tbDtrn_disc_line1.Visibility = ViewStates.Gone;
            tbDtrn_net_value.Text = detail.dtrn_net_value.ToString();
            tbDtrn_vat_value.Text = detail.dtrn_vat_value.ToString();

            return view;
        }
    }
}