using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class CheckableItemsAdapter : ArrayAdapter<Library.ItemInfo>, IScrollLoadble, View.IOnTouchListener
    {
        public static EditText lastFocusedControl;

        public delegate void ItemImageSelectedDelegate(Android.Graphics.Bitmap image);

        public event  ItemImageSelectedDelegate  ItemImageSelected;

        Activity context;
        ItemInfoList _itemInfoList;
        public ItemsSelectDialog Parent;
        Dictionary<int, double> _checkedItemIds = new Dictionary<int, double>();

        public delegate void SingleItemSelectedDeletegate();

        public delegate void SingleItemFocusedDeletegate(ItemInfo item);

        public event SingleItemFocusedDeletegate SingleItemFocusedEvent;
        public event SingleItemSelectedDeletegate SingleItemSelectedEvent;

        public CheckableItemsAdapter(Activity context, Library.ItemInfoList list)
            : base(context, Resource.Layout.item_row_checkable, list)
        {
            this.context = context;
            
            _itemInfoList = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            ViewHolder holder;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.item_row_checkable, null);

                holder = new ViewHolder();
                holder.itemBox = view.FindViewById<CheckBox>(Resource.Id.checkBox);
                holder.btnItemImage = view.FindViewById<Button>(Resource.Id.btnItemImage);
                holder.tbQty = view.FindViewById<EditText>(Resource.Id.tbQty);
                holder.tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
                holder.tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);
                holder.tbItemLastBuyDate = (TextView)view.FindViewById(Resource.Id.tbItemLastBuyDate);
                holder.tbItemSaleVal = (TextView)view.FindViewById(Resource.Id.tbItemSaleVal);
                holder.tbItemQtyLeft = (TextView)view.FindViewById(Resource.Id.tbItemQtyLeft);
                holder.layout_checkable_item = view.FindViewById<RelativeLayout>(Resource.Id.layout_checkable_item_info);

                holder.tbQty.SetOnTouchListener(this);

                view.Tag = holder;
            }
            else
            {			
                holder = (ViewHolder)view.Tag;
            }
            
            ItemInfo item = this.GetItem(position);

            if (item == null)
            {
                return view;
            }
		
            try
            {
                if (holder.btnItemImage != null)
                {
                    holder.btnItemImage.SetBackgroundDrawable(new Android.Graphics.Drawables.BitmapDrawable(item.ItemImage));
                    holder.btnItemImage.Tag = item.ItemId;
                    holder.btnItemImage.Click += new EventHandler(btnItemImageClicked);
                }

                if (holder.tbItemCode != null)
                    holder.tbItemCode.Text = item.item_cod;
                if (holder.tbItemName != null)
                    holder.tbItemName.Text = item.ItemDesc;
                if (holder.tbItemLastBuyDate != null)
                    holder.tbItemLastBuyDate.Text = item.ItemLastBuyDate.ToString(Common.DateFormatDateOnly);
                if (holder.tbItemQtyLeft != null)
                    holder.tbItemQtyLeft.Text = item.ItemQtyLeft.ToString(Common.DecimalFormat);
                if (holder.tbItemSaleVal != null)
                    holder.tbItemSaleVal.Text = item.ItemSaleVal1.ToString(Common.DecimalFormat);
				
                holder.itemBox.CheckedChange -= new EventHandler<CompoundButton.CheckedChangeEventArgs>(itemBox_CheckedChange);
                holder.itemBox.Checked = _checkedItemIds.ContainsKey(item.ItemId);
                holder.itemBox.Tag = position;
                holder.itemBox.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs>(itemBox_CheckedChange);
                holder.position = position;
            
                holder.layout_checkable_item.Tag = position;
                holder.layout_checkable_item.Touch += new EventHandler<View.TouchEventArgs>(layout_checkable_item_Touch);
            
                holder.tbQty.Tag = holder;//position;
                holder.tbQty.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbQty_TextChanged);
            }
            catch (Exception ex)
            {
                Log.Error("CheckableItemsAdapter getView", ex.Message);
            }

            GC.Collect();
            return view;
        }

        public void btnItemImageClicked(object sender, EventArgs e)
        {
            //show image in default image viewer
            //_itemInfoList.show//
            if (ItemImageSelected != null)
            {
                int itemID = ((int)((Button)sender).Tag);
                Android.Graphics.Bitmap image = ItemInfo.GetItemImage(context, itemID);
                //Android.Graphics.Bitmap image = ((Android.Graphics.Drawables.BitmapDrawable)((Button)sender).Background).Bitmap;
                ItemImageSelected(image);
            }
        }

        class ViewHolder:Java.Lang.Object
        {
            public Button btnItemImage;
            public	CheckBox itemBox;
            public	EditText tbQty ;
            public	TextView tbItemCode;
            public	TextView tbItemName ;
            public	TextView tbItemLastBuyDate;
            public	TextView tbItemSaleVal ;
            public	TextView tbItemQtyLeft;
            public	RelativeLayout layout_checkable_item;
            public int position = 0;
        }
		/* variable for counting two successive up-down events */
        int clickCount = 0;
		/*variable for storing the time of first click*/
        DateTime startTime;
		/* constant for defining the time duration between the click that can be considered as double-tap */
        const long MAX_DURATION = 500;

        void layout_checkable_item_Touch(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    clickCount++;
                    
                    RelativeLayout layout_checkable_item = (RelativeLayout)sender;
                    int index = int.Parse(layout_checkable_item.Tag.ToString ());
                    //ItemInfo item = _ItemInfoList[index];
                    ItemInfo item = this.GetItem(index);

                    if (clickCount == 1)
                    {
                        startTime = DateTime.Now;   

                        if (SingleItemFocusedEvent != null)
                        {
                            SingleItemFocusedEvent(item);
                        }
                    }
                    else if (clickCount == 2)
                    {
                        long duration = (long)new  TimeSpan((DateTime.Now - startTime).Ticks).TotalMilliseconds;

                        if (duration <= MAX_DURATION)
                        {
                            _checkedItemIds.Clear();
                            int itemId = (int)item.ItemId;
                            _checkedItemIds.Add(itemId, item.ItemQty);
                            
                            if (SingleItemSelectedEvent != null)
                            {
                                SingleItemSelectedEvent();
                            }

                            clickCount = 0;
                        }
                        else
                        {
                            clickCount = 1;
                            startTime = DateTime.Now;

                            if (SingleItemFocusedEvent != null)
                            {
                                SingleItemFocusedEvent(item);
                            }
                        }
                    }
                    break;    
            }
        }

        void tbQty_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //int index = int.Parse(((EditText)sender).Tag.ToString ());
            ViewHolder holder = ((ViewHolder)((EditText)sender).Tag);
            int index = int.Parse(holder.position.ToString ());

            Library.ItemInfo item = this.GetItem(index);
            double res = 0;
            string q = ((EditText)sender).Text;
            q = q.ToString().Trim().Replace(",", ".").Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (double.TryParse(q, out res))
            {
                item.ItemQty = res;

                if (_checkedItemIds.ContainsKey(item.ItemId))
                {
                    _checkedItemIds[item.ItemId] = item.ItemQty;
                }

                if (!holder.itemBox.Checked)
                    holder.itemBox.Checked = true;
            }            
        }

        void itemBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox itemBox = (CheckBox)sender;
            int index = int.Parse(itemBox.Tag.ToString ());
            //Library.ItemInfo item = _ItemInfoList[index];
            Library.ItemInfo item = this.GetItem(index);
            int itemId = (int)item.ItemId;
            if (e.IsChecked)
            {
                if (!_checkedItemIds.ContainsKey(itemId))
                {
                    _checkedItemIds.Add(itemId, item.ItemQty);
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

        public Dictionary<int, double> CheckedItemIds
        {
            get { return _checkedItemIds; }
        }
		#region IScrollLoadble implementation


        public void LoadData(int page)
        {
            ItemInfoList.LoadAdapterItems(context, page, this, _itemInfoList.CurrentCriteria);
            this.NotifyDataSetChanged();
        }
		#endregion

        #region IOnTouchListener implementation

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    if (lastFocusedControl != null)
                        lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
                    lastFocusedControl = (EditText)v;
                    lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
                    lastFocusedControl.RequestFocus();

                    EditText yourEditText = (EditText)v;
                    Android.Views.InputMethods.InputMethodManager imm = (Android.Views.InputMethods.InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService);
                    imm.ShowSoftInput(yourEditText, Android.Views.InputMethods.ShowFlags.Forced);
                    if (lastFocusedControl != null)
                        lastFocusedControl.PostDelayed(new Action(()=>{lastFocusedControl.SelectAll();}), 100);
                    break;

            }

            return true;
        }
        #endregion

    }
}