using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
	public class CheckableItemsAdapter : ArrayAdapter<Library.ItemInfo>, IScrollLoadble
	{
		Activity context = null;
		ItemInfoList _itemInfoList;

		private Dictionary<int, int> _checkedItemIds = new Dictionary<int, int> ();
		public delegate void SingleItemSelectedDeletegate ();

		public delegate void SingleItemFocusedDeletegate (ItemInfo item);

		public event SingleItemFocusedDeletegate SingleItemFocusedEvent;
		public event SingleItemSelectedDeletegate SingleItemSelectedEvent;
        
		public CheckableItemsAdapter (Activity context, Library.ItemInfoList list)
            : base(context, Resource.Layout.item_row_checkable, list)
		{
			this.context = context;
            
			_itemInfoList = list;
		}
        
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
            
			if (view == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.item_row_checkable, null);
			}
            
			//Library.ItemInfo item = _ItemInfoList[position];
			Library.ItemInfo item = this.GetItem (position);
			if (item == null)
				return view;
            
			try {
				CheckBox itemBox = view.FindViewById<CheckBox> (Resource.Id.checkBox);
				EditText tbQty = view.FindViewById<EditText> (Resource.Id.tbQty);
				TextView tbItemCode = (TextView)view.FindViewById (Resource.Id.tbItemCode);
				TextView tbItemName = (TextView)view.FindViewById (Resource.Id.tbItemName);
				TextView tbItemLastBuyDate = (TextView)view.FindViewById (Resource.Id.tbItemLastBuyDate);
				TextView tbItemSaleVal = (TextView)view.FindViewById (Resource.Id.tbItemSaleVal);
				TextView tbItemQtyLeft = (TextView)view.FindViewById (Resource.Id.tbItemQtyLeft);
				RelativeLayout layout_checkable_item = view.FindViewById<RelativeLayout> (Resource.Id.layout_checkable_item_info);
            
				if (tbItemCode != null)
					tbItemCode.Text = item.item_cod;
				if (tbItemName != null)
					tbItemName.Text = item.ItemDesc;
				if (tbItemLastBuyDate != null)
					tbItemLastBuyDate.Text = item.ItemLastBuyDate.ToString (PreferencesUtil.DateFormatDateOnly);
				if (tbItemQtyLeft != null)
					tbItemQtyLeft.Text = item.ItemQtyLeft.ToString (PreferencesUtil.DecimalFormat);
				if (tbItemSaleVal != null)
					tbItemSaleVal.Text = item.ItemSaleVal1.ToString (PreferencesUtil.DecimalFormat);
            
				itemBox.Tag = position;
				itemBox.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs> (itemBox_CheckedChange);
            
				layout_checkable_item.Tag = position;
				layout_checkable_item.Touch += new EventHandler<View.TouchEventArgs> (layout_checkable_item_Touch);
            
				tbQty.Tag = position;
				tbQty.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs> (tbQty_TextChanged);
			} catch (Exception ex) {
				Log.Error ("exception", ex.Message);
			}
			return view;
		}

		/* variable for counting two successive up-down events */
		int clickCount = 0;
		/*variable for storing the time of first click*/
		DateTime startTime;
		/* constant for defining the time duration between the click that can be considered as double-tap */
		const long MAX_DURATION = 500;

		void layout_checkable_item_Touch (object sender, View.TouchEventArgs e)
		{
			switch (e.Event.Action & MotionEventActions.Mask) {
			case MotionEventActions.Up:
				clickCount++;
                    
				RelativeLayout layout_checkable_item = (RelativeLayout)sender;
				int index = int.Parse (layout_checkable_item.Tag.ToString ());
                    //ItemInfo item = _ItemInfoList[index];
				ItemInfo item = this.GetItem (index);

				if (clickCount == 1) {
					startTime = DateTime.Now;   

					if (SingleItemFocusedEvent != null) {
						SingleItemFocusedEvent (item);
					}
				} else if (clickCount == 2) {
					long duration = (long)new  TimeSpan ((DateTime.Now - startTime).Ticks).TotalMilliseconds;

					if (duration <= MAX_DURATION) {
						_checkedItemIds.Clear ();
						int itemId = (int)item.ItemId;
						_checkedItemIds.Add (itemId, item.ItemQty);
                            
						if (SingleItemSelectedEvent != null) {
							SingleItemSelectedEvent ();
						}

						clickCount = 0;
						duration = 0;
					} else {
						clickCount = 1;
						startTime = DateTime.Now;

						if (SingleItemFocusedEvent != null) {
							SingleItemFocusedEvent (item);
						}
					}
				}
				break;    
			}
		}
        
		void tbQty_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			int index = int.Parse (((EditText)sender).Tag.ToString ());
			//Library.ItemInfo item = _ItemInfoList[index];
			Library.ItemInfo item = this.GetItem (index);
			int res = 0;
			if (int.TryParse (((EditText)sender).Text, out res)) {
				item.ItemQty = res;

				if (_checkedItemIds.ContainsKey (item.ItemId)) {
					_checkedItemIds [item.ItemId] = item.ItemQty;
				}
			}            
		}

		void itemBox_CheckedChange (object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			CheckBox itemBox = (CheckBox)sender;
			int index = int.Parse (itemBox.Tag.ToString ());
			//Library.ItemInfo item = _ItemInfoList[index];
			Library.ItemInfo item = this.GetItem (index);
			int itemId = (int)item.ItemId;
			if (e.IsChecked) {
				if (!_checkedItemIds.ContainsKey (itemId)) {
					_checkedItemIds.Add (itemId, item.ItemQty);
				}
			} else {
				if (_checkedItemIds.ContainsKey (itemId)) {
					_checkedItemIds.Remove (itemId);
				}
			}
		}
        
		public Dictionary<int, int> CheckedItemIds {
			get { return _checkedItemIds; }
		}
		
		#region IScrollLoadble implementation


		public void LoadData (int page)
		{
			if (_itemInfoList.CurrentCriteria == null)
				_itemInfoList.CurrentCriteria = new ItemInfoList.Criteria ();

			ItemInfoList.LoadAdapterItems (context, page, this, _itemInfoList.CurrentCriteria);
			this.NotifyDataSetChanged ();
		}


		#endregion

	}
}