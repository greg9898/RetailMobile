using System;
using Android.App;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;

namespace RetailMobile
{
    public class TransDetAdapter : ArrayAdapter<Library.TransDet>, GestureDetector.IOnGestureListener, View.IOnTouchListener
    { 
        bool disabled = false;
        public static EditText lastFocusedControl;
        Android.Support.V4.App.FragmentActivity context = null;
        EditText tbDtrn_qty1;
        EditText tbDtrn_disc_line1;
        TransDetList dataSource;
        InvoiceInfoFragment parentView;
        private GestureDetector _gestureDetector;

        public delegate void QtysChangedDeletegate();

        public delegate void QtysSelectedDeletegate(SelectedDetail detail);

        public event QtysChangedDeletegate QtysChangedEvent;
        public event QtysSelectedDeletegate DetailFieldSelectedEvent;

        public TransDetAdapter(Android.Support.V4.App.FragmentActivity context, TransDetList list, InvoiceInfoFragment baseFragment)
            : base(context, Resource.Layout.TransDetRow, list)
        {
            this.context = context;

            dataSource = list;
            parentView = baseFragment;
        }

        public void AddValue()
        {
            if (lastFocusedControl != null)
            {
                string lastFocusedControlText = lastFocusedControl.Text;
                lastFocusedControlText = lastFocusedControlText.Replace(",",".").Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                lastFocusedControl.Text = (decimal.Parse(lastFocusedControlText) + 1).ToString();
            }
        }

        public void SubstractValue()
        {
            if (lastFocusedControl != null)
            {
                string lastFocusedControlText = lastFocusedControl.Text;
                lastFocusedControlText = lastFocusedControlText.Replace(",",".").Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                lastFocusedControl.Text = (decimal.Parse(lastFocusedControlText) - 1).ToString();
            }
        }
        static EditText lastEdit = null;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            ViewHolder holder;

            TransDet detail = dataSource[position];

            if (detail == null)
            {
                return view;
            }

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TransDetRow, null);
                holder = new ViewHolder();
                holder.position = position;

                holder.lblItemCode = (TextView)view.FindViewById(Resource.Id.lblDtrn_ItemCode);
                holder.lblItemDesc = (TextView)view.FindViewById(Resource.Id.tbItemDesc);
                holder.tbDtrn_qty1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_qty1);
                holder.lblDtrn_unit_price = view.FindViewById<TextView>(Resource.Id.lblDtrn_unit_price);
                holder.tbDtrn_disc_line1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_disc_line1);
                holder.lblDtrn_net_value = view.FindViewById<TextView>(Resource.Id.lblDtrn_net_value);
                holder.lblDtrn_vat_value = view.FindViewById<TextView>(Resource.Id.lblDtrn_vat_value);

                tbDtrn_disc_line1 = holder.tbDtrn_disc_line1;
                tbDtrn_qty1 = holder.tbDtrn_qty1;

                view.Tag = holder;

                view.LongClick += rowView_HandleLongClick;

                holder.tbDtrn_qty1.Tag = holder;
                holder.tbDtrn_qty1.FocusableInTouchMode = true;
                holder.tbDtrn_qty1.Text = detail.DtrnQty1.ToString("#######0.0");
                holder.tbDtrn_qty1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_qty1_TextChanged);            
                //holder.tbDtrn_qty1.FocusChange += new EventHandler(tbQty_HandleFocusChange);
                holder.tbDtrn_qty1.FocusChange += tbQty_HandleFocusChange;
                //holder.tbDtrn_qty1.SetOnTouchListener(new View.IOnTouchListener());
                //holder.tbDtrn_qty1.Touch += new EventHandler<View.TouchEventArgs>(EditTextTouchUp);
                //holder.tbDtrn_qty1.on
                //holder.tbDtrn_qty1.
                /*GestureDetector gestureDetector = new GestureDetector(new itge
                GestureDetector gestureDetector = new GestureDetector(new GestureDetector.SimpleOnGestureListener() {
                    public boolean onDoubleTap(MotionEvent e) {
                        Log.e("", "Open new activty here");
                        return true;
                    }
                });

                tv.setOnTouchListener(new OnTouchListener() {
                    public boolean onTouch(View v, MotionEvent event) {
                        return gestureDetector.onTouchEvent(event);
                    }
                });*/
                _gestureDetector = new GestureDetector(this);
                _gestureDetector.DoubleTap += new EventHandler<GestureDetector.DoubleTapEventArgs>((o,e)=>{
                    //((EditText)o).SelectAll();
                    Android.Util.Log.Debug("double tap", "Double tapped");
                    if(lastFocusedControl != null)
                    {
                        Android.Util.Log.Debug("double tap", "Last focused not null");
                        lastFocusedControl.SelectAll();

                        lastFocusedControl.PostDelayed(new Action(()=>{lastFocusedControl.SelectAll();}),500);
                    }
                });

               // holder.tbDtrn_qty1.Touch += new EventHandler<View.TouchEventArgs>((o,e)=>
               // {
                    //lastEdit = ((EditText)o);
                  //  _gestureDetector.OnTouchEvent(e.Event);

               // });
                holder.tbDtrn_qty1.SetOnTouchListener(this);

                //holder.tbDtrn_disc_line1.Tag = position;
                holder.tbDtrn_disc_line1.Tag = holder;
                holder.tbDtrn_disc_line1.FocusableInTouchMode = true;
                holder.tbDtrn_disc_line1.Text = detail.DtrnDiscLine1.ToString();
                holder.tbDtrn_disc_line1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_disc_line1_TextChanged);
                holder.tbDtrn_disc_line1.FocusChange += tbQty_HandleFocusChange;
                holder.Datasource = detail;
                //holder.tbDtrn_disc_line1.Touch += new EventHandler<View.TouchEventArgs>(EditTextTouchUp);
            }
            else
            {            
                holder = (ViewHolder)view.Tag;
            }

            holder.lblItemCode.Text = detail.ItemCode;
            holder.lblItemDesc.Text = detail.ItemDesc;

            holder.lblDtrn_unit_price.Text = detail.DtrnUnitPrice.ToString(Common.CurrencyFormat);

            holder.lblDtrn_net_value.Text = detail.DtrnNetValue.ToString(Common.CurrencyFormat);
            holder.lblDtrn_vat_value.Text = detail.DtrnVatValue.ToString(Common.CurrencyFormat);

            if (disabled)
            {
                holder.tbDtrn_disc_line1.Enabled = false;
                holder.tbDtrn_qty1.Enabled = false;
                holder.tbDtrn_disc_line1.Focusable = false;
                holder.tbDtrn_qty1.Focusable = false;
            }

            return view;
        }

        public void Disable()
        {
            disabled = true;
        }

        class ViewHolder:Java.Lang.Object
        {
            public TransDet Datasource;
            public TextView lblItemCode ;
            public TextView lblItemDesc ;
            public EditText tbDtrn_qty1;
            public TextView lblDtrn_unit_price  ;
            public EditText tbDtrn_disc_line1;
            public TextView lblDtrn_net_value ;
            public TextView lblDtrn_vat_value;
            public int position;

            public void RefreshRow()
            {
                if (lblDtrn_net_value != null && lblDtrn_vat_value != null)
                {
                    lblDtrn_net_value.Text = Datasource.DtrnNetValue.ToString(Common.CurrencyFormat);
                    lblDtrn_vat_value.Text = Datasource.DtrnVatValue.ToString(Common.CurrencyFormat);
                }
            }
        }

        void rowView_HandleLongClick(object sender, View.LongClickEventArgs e)
        {
            View rowView = sender as View;
            int position = ((ViewHolder)rowView.Tag).position;
            string question = string.Format(context.Resources.GetText( Resource.String.delete_detail_question), dataSource[position].ItemDesc);

            new DialogDeleteDetail(question, position, this).Show(context.SupportFragmentManager, "dialog");
        }

        public class DialogDeleteDetail : Android.Support.V4.App.DialogFragment
        {
            TransDetAdapter adapter;
            int position;
            string questionDelYesNo;

            public DialogDeleteDetail(string questionDelete, int pos, TransDetAdapter adapter)
            {
                position = pos;
                questionDelYesNo = questionDelete;
                this.adapter = adapter;
            }

            public override Android.App.Dialog OnCreateDialog(Android.OS.Bundle savedInstanceState)
            {
                var builder = new Android.App.AlertDialog.Builder(Activity);
                builder.SetMessage(questionDelYesNo);
                builder.SetPositiveButton(Resources.GetText( Resource.String.Yes), delegate(object sender, Android.Content.DialogClickEventArgs args)
                {
                    TransDet d = adapter.parentView.Header.TransDetList[position];

                    //                    adapter.Remove(d);
                    //                    adapter.NotifyDataSetChanged();
                    adapter.parentView.Header.MarkDetailDeleted(d);
                    adapter.parentView.LoadDetailsAdapter();
                });
                builder.SetNegativeButton(Resources.GetText( Resource.String.No), (sender, args) =>
                {
                    this.Dismiss();
                });

                return builder.Create();
            }
        }

        void EditText_Click(object sender, EventArgs e)
        {
            if (TransDetAdapter.lastFocusedControl != null)
            {
                TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
            }

            TransDetAdapter.lastFocusedControl = (EditText)sender;
            TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
        }

        private void EditTextTouchUp(object sender, View.TouchEventArgs e)
        {           
            //          if (MotionEventActions.Up == e.Event.Action ) {  disallow text edit ?
            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    if (TransDetAdapter.lastFocusedControl != null)
                        TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
                    TransDetAdapter.lastFocusedControl = (EditText)sender;
                    TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
                    TransDetAdapter.lastFocusedControl.RequestFocus();

                    EditText yourEditText = (EditText)sender;
                    Android.Views.InputMethods.InputMethodManager imm = (Android.Views.InputMethods.InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService);
                    imm.ShowSoftInput(yourEditText, Android.Views.InputMethods.ShowFlags.Implicit);
                    break;
            }
        }

        #region IOnTouchListener implementation

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    if (TransDetAdapter.lastFocusedControl != null)
                        TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
                    TransDetAdapter.lastFocusedControl = (EditText)v;
                    TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
                    TransDetAdapter.lastFocusedControl.RequestFocus();

                    EditText yourEditText = (EditText)v;
                    Android.Views.InputMethods.InputMethodManager imm = (Android.Views.InputMethods.InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService);
                    imm.ShowSoftInput(yourEditText, Android.Views.InputMethods.ShowFlags.Implicit);
                    break;
            }
            _gestureDetector.OnTouchEvent(e);
            return true;
        }

        #endregion

        void tbQty_HandleFocusChange(object sender, View.FocusChangeEventArgs e)
        {          
            if (DetailFieldSelectedEvent != null)
            {
                if (e.HasFocus)
                {
                    //int index = int.Parse(((EditText)sender).Tag.ToString ());
                    int index = ((ViewHolder)((EditText)sender).Tag).position;

                    if (sender.Equals(tbDtrn_qty1))
                    {                      
                        DetailFieldSelectedEvent(new SelectedDetail (){  
                            Position = index,
                            TransDetail = dataSource[index],
                            Property = "QTY"
                        });
                    }
                    else if (sender.Equals(tbDtrn_disc_line1))
                    {
                        DetailFieldSelectedEvent(new SelectedDetail (){  
                            Position = index,
                            TransDetail = dataSource[index], 
                            Property = "DISC"
                        });
                    }

                }
                else
                {
                    DetailFieldSelectedEvent(null);
                }
            }
        }

        void tbDtrn_qty1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
            //int index = int.Parse(((EditText)sender).Tag.ToString ());
            string q = e.Text.ToString().Trim().Replace(",", ".").Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int index = ((ViewHolder)((EditText)sender).Tag).position;
            TransDet detail = dataSource[index];
            //string qtyText = e.Text.ToString().Trim();
            string qtyText = q;
            detail.DtrnQty1 = qtyText != "" ? double.Parse(qtyText) : 0;

            ((ViewHolder)((EditText)sender).Tag).RefreshRow();

            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
            }
            catch(Exception ex)
            {

            }
        }

        void tbDtrn_disc_line1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                string q = (sender as EditText).Text.Trim().Replace(",", ".").Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            //int index = int.Parse(((EditText)sender).Tag.ToString ());
            int index = ((ViewHolder)((EditText)sender).Tag).position;
            TransDet detail = dataSource[index];
            //detail.DtrnDiscLine1 = double.Parse((sender as EditText).Text);
                detail.DtrnDiscLine1 = double.Parse(q);

            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
            }
            catch(Exception ex)
            {
            }
        }

     
        #region IOnGestureListener implementation

        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {

        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {

        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        #endregion

        public class SelectedDetail
        {
            public TransDet TransDetail = null;
            public int Position = -1;
            public string Property = "";

            public static SelectedDetail Empty()
            {
                SelectedDetail d = new SelectedDetail();
                d.TransDetail = null;
                d.Position = -1;
                d.Property = "";
                return d;
            }
        }
    }
}