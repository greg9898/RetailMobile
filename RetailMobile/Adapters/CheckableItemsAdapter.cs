using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

namespace RetailMobile
{
    public class CheckableItemsAdapter : ArrayAdapter<Library.ItemInfo>
    {
        private  Activity context = null;
        private  Library.ItemInfoList _ItemInfoList;
        private Dictionary<int, int> _checkedItemIds = new Dictionary<int, int>();
        private   EditText tbQty;
        public delegate void SingleItemSelectedDeletegate();

        public event SingleItemSelectedDeletegate SingleItemSelectedEvent;

        public CheckableItemsAdapter(Activity context, Library.ItemInfoList list)
            : base(context, Resource.Layout.item_row_muliple_select, list)
        {
            this.context = context;

            _ItemInfoList = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Library.ItemInfo item = _ItemInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.item_row_muliple_select, null);

            TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
            TextView tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);
            TextView tbItemLastBuyDate = (TextView)view.FindViewById(Resource.Id.tbItemLastBuyDate);
            TextView tbItemSaleVal = (TextView)view.FindViewById(Resource.Id.tbItemSaleVal);
            TextView tbItemRetVal = (TextView)view.FindViewById(Resource.Id.tbItemRetVal);
            CheckBox itemBox = view.FindViewById<CheckBox>(Resource.Id.checkBox);
            RelativeLayout layout_checkable_item = view.FindViewById<RelativeLayout>(Resource.Id.layout_checkable_item_info);
            tbQty = view.FindViewById<EditText>(Resource.Id.tbQty);

            tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.item_desc;
            tbItemLastBuyDate.Text = item.ItemLastBuyDate.ToString();
            tbItemRetVal.Text = item.item_ret_val1.ToString(PreferencesUtil.DecimalFormat);
            tbItemSaleVal.Text = item.item_sale_val1.ToString(PreferencesUtil.DecimalFormat);

            itemBox.Tag = item.ItemId.ToString();
            itemBox.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs>(itemBox_CheckedChange);

            layout_checkable_item.Tag = item.ItemId.ToString();
            layout_checkable_item.Click += new EventHandler(layout_checkable_item_Click);

            tbQty.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbQty_TextChanged);

            return view;
        }

        void tbQty_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
           
        }

        void layout_checkable_item_Click(object sender, EventArgs e)
        {
            RelativeLayout layout_checkable_item = (RelativeLayout)sender;
            int itemId = int.Parse(layout_checkable_item.Tag.ToString());

            _checkedItemIds.Clear();
            _checkedItemIds.Add(itemId, Convert.ToInt32(tbQty.Text));

            if (SingleItemSelectedEvent != null)
            {
                SingleItemSelectedEvent();
            }
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
            } else
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