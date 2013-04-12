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
    public class SpinnerAdapter<K, V> : BaseAdapter<KeyValuePair<K, V>>
    {
        public override KeyValuePair<K, V> this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public K GetSelectedValue(int pos)
        {
            return _list[pos].Key;
        }

        Activity context = null;
        List<KeyValuePair<K, V>> _list;

        public SpinnerAdapter(Activity context, List<KeyValuePair<K, V>> list)
            : base()
        {
            this.context = context;

            _list = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this._list[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.SpinnerRow, null);

            TextView tbValue = (TextView)view.FindViewById(Resource.Id.tbValue);
            if (tbValue != null && item.Value != null)
                tbValue.Text = item.Value.ToString();

            return view;
        }

        public override int Count
        {
            get { return _list.Count(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
    }
}