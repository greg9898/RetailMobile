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
    public class ItemsListAdapter : ArrayAdapter<Library.ItemInfo>
    {
        Activity context = null;
        Library.ItemInfoList ItemInfoList;

        public ItemsListAdapter(Activity context, int rowResourceID, Library.ItemInfoList _list)
            : base(context, rowResourceID, _list)
        {
            this.context = context;

            ItemInfoList = _list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.ItemInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ItemInfoRow, null);

            TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
            TextView tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);

            tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.ItemDesc;

            return view;
        }
    }
}