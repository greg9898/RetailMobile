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
    public class InvoiceDetailsAdapter : ArrayAdapter<Library.TransDet>
    {
        Activity context = null;
        Library.TransDetList detailsList;

        public InvoiceDetailsAdapter(Activity context, int rowResourceID, Library.TransDetList _list)
            : base(context, rowResourceID, _list)
        {
            this.context = context;

            detailsList = _list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.detailsList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.InvoiceDetailRow, null);

            TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.lblDetItemCode);
            TextView tbItemDesc = (TextView)view.FindViewById(Resource.Id.lblDetItemDesc);
            TextView tbUnitPrice = (TextView)view.FindViewById(Resource.Id.tbDetUnitPrice);
            TextView tbQty1 = (TextView)view.FindViewById(Resource.Id.tbDetQty);
            TextView tbDiscVal = (TextView)view.FindViewById(Resource.Id.tbDetDisc);
            TextView tbNetVal = (TextView)view.FindViewById(Resource.Id.tbDetNetValue);
            TextView tbVatVal = (TextView)view.FindViewById(Resource.Id.tblDetVatValue);

            if (tbItemCode != null)
                tbItemCode.Text = item.ItemCode;
            if (tbItemDesc != null)
                tbItemDesc.Text = item.ItemDesc;
            if (tbUnitPrice != null)
                tbUnitPrice.Text = item.DtrnUnitPrice.ToString("c2");
            if (tbQty1 != null)
                tbQty1.Text = item.DtrnQty1.ToString("f3");
            if (tbDiscVal != null)
                tbDiscVal.Text = item.DtrnDiscLine1.ToString("c2");
            if (tbNetVal != null)
                tbNetVal.Text = item.DtrnNetValue.ToString("c2");
            if (tbVatVal != null)
                tbVatVal.Text = item.DtrnVatValue.ToString("c2");

            return view;
        }
    }
}