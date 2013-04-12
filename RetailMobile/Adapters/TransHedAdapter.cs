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
    public class TransHedAdapter : ArrayAdapter<Library.TransHed>
    {
        Activity context = null;
        Library.TransHedList TransHedList;

        public TransHedAdapter(Activity context, Library.TransHedList list)
            : base(context, Resource.Layout.TransHedRow, list)
        {
            this.context = context;

            TransHedList = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.TransHedList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TransHedRow, null);

            //TextView tbCompID = (TextView)view.FindViewById(Resource.Id.tbCompID);
            TextView tbHtrnDocnum = (TextView)view.FindViewById(Resource.Id.tbHtrnDocnum);
            TextView tbHtrnExpl = (TextView)view.FindViewById(Resource.Id.tbHtrnExpl);
            TextView tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustName);

            tbHtrnDocnum.Text = item.HtrnDocnum.ToString();
            tbHtrnExpl.Text = item.HtrnExpl;
            tbCustName.Text = item.CstId.ToString();

            return view;
        }
    }
}