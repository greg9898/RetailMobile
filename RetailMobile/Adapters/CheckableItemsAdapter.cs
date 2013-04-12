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
    public class CheckableItemsAdapter : ArrayAdapter<Library.ItemInfo>
    {
        Activity context = null;
        Library.ItemInfoList _ItemInfoList;
        private Dictionary<int, int> _checkedItemIds = new Dictionary<int, int>();
        EditText tbQty;

        public CheckableItemsAdapter(Activity context, Library.ItemInfoList _list)
            : base(context, Resource.Layout.item_row_muliple_select, _list)
        {
            this.context = context;

            _ItemInfoList = _list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Library.ItemInfo item = _ItemInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.item_row_muliple_select, null);

            TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
            TextView tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);
            CheckBox itemBox = view.FindViewById<CheckBox>(Resource.Id.checkBox);
            RelativeLayout layout_checkable_item = view.FindViewById<RelativeLayout>(Resource.Id.layout_checkable_item_info);
            tbQty = view.FindViewById<EditText>(Resource.Id.tbQty);

            tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.item_desc;

            itemBox.Tag = item.item_id.ToString();
            itemBox.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs>(itemBox_CheckedChange);

            layout_checkable_item.Tag = item.item_id.ToString();
            layout_checkable_item.Click += new EventHandler(layout_checkable_item_Click);

            return view;
        }

        void layout_checkable_item_Click(object sender, EventArgs e)
        {
            RelativeLayout layout_checkable_item = (RelativeLayout)sender;
            int itemId = int.Parse(layout_checkable_item.Tag.ToString());

            _checkedItemIds.Clear();
            _checkedItemIds.Add(itemId, Convert.ToInt32(tbQty.Text));
        }

        void itemBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox itemBox = (CheckBox)sender;
            int itemId = int.Parse(itemBox.Tag.ToString());
            Android.Util.Log.Debug(this.Class.Name, "itemBox_CheckedChange itemId from tag:" + itemBox.Tag + " tbQty=" + tbQty.Text);

            if (e.IsChecked)
            {
                if (!_checkedItemIds.ContainsKey(itemId))
                {
                    _checkedItemIds.Add(itemId, Convert.ToInt32(tbQty.Text));
                }
            }
            else
            {
                if (_checkedItemIds.ContainsKey(itemId))
                {
                    _checkedItemIds.Remove(itemId);
                }
            }
        }

        public Dictionary<int, int> CheckedItemIds
        {
            get { return _checkedItemIds; }
        }
    }
}