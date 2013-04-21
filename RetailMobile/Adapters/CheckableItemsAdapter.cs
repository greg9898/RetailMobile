using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace RetailMobile
{
	public class CheckableItemsAdapter : ArrayAdapter<Library.ItemInfo>
	{
		private Activity context = null;
		private Library.ItemInfoList _ItemInfoList;
		private Dictionary<int, int> _checkedItemIds = new Dictionary<int, int>();
		public delegate void SingleItemSelectedDeletegate();
		
		public event SingleItemSelectedDeletegate SingleItemSelectedEvent;
		
		public CheckableItemsAdapter(Activity context, Library.ItemInfoList list)
			: base(context, Resource.Layout.item_row_checkable, list)
		{
			this.context = context;
			
			_ItemInfoList = list;
		}
		
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			
			if (view == null)
			{
				view = context.LayoutInflater.Inflate(Resource.Layout.item_row_checkable, null);
			}
			
			Library.ItemInfo item = _ItemInfoList[position];
			
			CheckBox itemBox = view.FindViewById<CheckBox>(Resource.Id.checkBox);
			EditText tbQty = view.FindViewById<EditText>(Resource.Id.tbQty);
			TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
			TextView tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);
			TextView tbItemLastBuyDate = (TextView)view.FindViewById(Resource.Id.tbItemLastBuyDate);
			TextView tbItemSaleVal = (TextView)view.FindViewById(Resource.Id.tbItemSaleVal);
			TextView tbItemQtyLeft = (TextView)view.FindViewById(Resource.Id.tbItemQtyLeft);
			RelativeLayout layout_checkable_item = view.FindViewById<RelativeLayout>(Resource.Id.layout_checkable_item_info);
			
			tbItemCode.Text = item.item_cod;
			tbItemName.Text = item.ItemDesc;
			tbItemLastBuyDate.Text = item.ItemLastBuyDate.ToString(PreferencesUtil.DateFormatDateOnly);
			tbItemQtyLeft.Text = item.ItemQtyLeft.ToString(PreferencesUtil.DecimalFormat);
			tbItemSaleVal.Text = item.ItemSaleVal1.ToString(PreferencesUtil.DecimalFormat);
			
			itemBox.Tag = position;
			itemBox.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs>(itemBox_CheckedChange);
			
			layout_checkable_item.Tag = position;
			layout_checkable_item.Click += new EventHandler(layout_checkable_item_Click);
			
			tbQty.Tag = position;
			tbQty.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbQty_TextChanged);
			
			return view;
		}
		
		void tbQty_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			int index = int.Parse(((EditText)sender).Tag.ToString());
			Library.ItemInfo item = _ItemInfoList[index];
			item.ItemQty = int.Parse(((EditText)sender).Text);
		}
		
		void layout_checkable_item_Click(object sender, EventArgs e)
		{
			RelativeLayout layout_checkable_item = (RelativeLayout)sender;
			int index = int.Parse(layout_checkable_item.Tag.ToString());
			_checkedItemIds.Clear();
			Library.ItemInfo item = _ItemInfoList[index];
			int itemId = (int)item.ItemId;
			_checkedItemIds.Add(itemId, item.ItemQty);
			
			if (SingleItemSelectedEvent != null)
			{
				SingleItemSelectedEvent();
			}
		}
		
		void itemBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			CheckBox itemBox = (CheckBox)sender;
			int index = int.Parse(itemBox.Tag.ToString());
			Library.ItemInfo item = _ItemInfoList[index];
			int itemId = (int)item.ItemId;
			if (e.IsChecked)
			{
				if (!_checkedItemIds.ContainsKey(itemId))
				{
					_checkedItemIds.Add(itemId, item.ItemQty);
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