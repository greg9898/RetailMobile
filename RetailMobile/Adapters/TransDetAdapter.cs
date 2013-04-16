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
        public delegate void QtysChangedDeletegate();
        public event QtysChangedDeletegate QtysChangedEvent;

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

            TextView lblItemCode = (TextView)view.FindViewById(Resource.Id.lblDtrn_ItemCode);
            TextView lblItemDesc = (TextView)view.FindViewById(Resource.Id.tbItemDesc);
            EditText tbDtrn_qty1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_qty1);
            EditText tbDtrn_unit_price = view.FindViewById<EditText>(Resource.Id.tbDtrn_unit_price);
            EditText tbDtrn_disc_line1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_disc_line1);
            EditText tbDtrn_net_value = view.FindViewById<EditText>(Resource.Id.tbDtrn_net_value);
            EditText tbDtrn_vat_value = view.FindViewById<EditText>(Resource.Id.tbDtrn_vat_value);

            Library.TransDet detail = dataSource[position];

            tbDtrn_net_value.Tag = position;
            tbDtrn_vat_value.Tag = position;
            tbDtrn_qty1.Tag = position;
            tbDtrn_disc_line1.Tag = position;

            lblItemCode.Text = detail.ItemCode;
            lblItemDesc.Text = detail.ItemDesc;
            tbDtrn_qty1.Text = detail.DtrnQty1.ToString();
            tbDtrn_unit_price.Text = detail.DtrnUnitPrice.ToString();
            tbDtrn_disc_line1.Text = detail.DtrnDiscLine1.ToString();
//            tbDtrn_disc_line1.Visibility = ViewStates.Gone;
            tbDtrn_net_value.Text = detail.DtrnNetValue.ToString();
            tbDtrn_vat_value.Text = detail.DtrnVatValue.ToString();

            tbDtrn_net_value.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(netValueChanged);
            tbDtrn_vat_value.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(vatValueChanged);
            tbDtrn_qty1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_qty1_TextChanged);
            tbDtrn_disc_line1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_disc_line1_TextChanged);

            return view;
        }

        void tbDtrn_qty1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            Library.TransDet detail = dataSource[index];
            detail.DtrnQty1 = double.Parse(e.Text.ToString());
            
            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

        void tbDtrn_disc_line1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            Library.TransDet detail = dataSource[index];
            detail.DtrnDiscLine1 = double.Parse((sender as EditText).Text);
            
            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

        void netValueChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            Library.TransDet detail = dataSource[index];
            detail.DtrnNetValue = double.Parse((sender as EditText).Text);

            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

        void vatValueChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            Library.TransDet detail = dataSource[index];
            detail.DtrnVatValue = double.Parse((sender as EditText).Text);
            
            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

    }
}