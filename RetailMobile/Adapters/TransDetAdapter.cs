using System;

using Android.App;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;

namespace RetailMobile
{
    public class TransDetAdapter : ArrayAdapter<Library.TransDet>
    { 
		private static EditText lastFocusedControl;
        Activity context = null;
        EditText tbDtrn_qty1;
        EditText tbDtrn_disc_line1;
        TransDetList dataSource;
        public delegate void QtysChangedDeletegate();

        public delegate void QtysSelectedDeletegate(SelectedDetail detail);
        
        public event QtysChangedDeletegate QtysChangedEvent;
        public event QtysSelectedDeletegate DetailFieldSelectedEvent;

        public TransDetAdapter(Activity context, TransDetList list)
            : base(context, Resource.Layout.TransDetRow, list)
        {
            this.context = context;

            dataSource = list;
        }

		public void AddValue()
		{
			if( lastFocusedControl != null)
			{
				lastFocusedControl.Text =  (int.Parse(lastFocusedControl.Text) + 1).ToString();
			}
		}

		public void SubstractValue()
		{
			if( lastFocusedControl != null)
			{
				lastFocusedControl.Text =  (int.Parse(lastFocusedControl.Text) - 1).ToString();
			}
		}

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TransDetRow, null);

            TextView lblItemCode = (TextView)view.FindViewById(Resource.Id.lblDtrn_ItemCode);
            TextView lblItemDesc = (TextView)view.FindViewById(Resource.Id.tbItemDesc);
            tbDtrn_qty1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_qty1);
            TextView lblDtrn_unit_price = view.FindViewById<TextView>(Resource.Id.lblDtrn_unit_price);
            tbDtrn_disc_line1 = view.FindViewById<EditText>(Resource.Id.tbDtrn_disc_line1);
            TextView lblDtrn_net_value = view.FindViewById<TextView>(Resource.Id.lblDtrn_net_value);
            TextView lblDtrn_vat_value = view.FindViewById<TextView>(Resource.Id.lblDtrn_vat_value);

			//tbDtrn_qty1.Click += new EventHandler((o,e)=>{TransDetAdapter.lastFocusedControl = (EditText)o;});
			/*tbDtrn_qty1.Touch += new EventHandler<View.TouchEventArgs>((o,e)=>
				{
				if(MotionEventActions.Up == e.Event.Action) {
					TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
					TransDetAdapter.lastFocusedControl = (EditText)o;
					TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
				}

			});
			*/
			tbDtrn_qty1.Touch += new EventHandler<View.TouchEventArgs>(EditTextTouchUp);
			tbDtrn_disc_line1.Touch += new EventHandler<View.TouchEventArgs>(EditTextTouchUp);

            Library.TransDet detail = dataSource[position];

            tbDtrn_qty1.Tag = position;
            tbDtrn_disc_line1.Tag = position;

            lblItemCode.Text = detail.ItemCode;
            lblItemDesc.Text = detail.ItemDesc;
            tbDtrn_qty1.Text = detail.DtrnQty1.ToString();
            lblDtrn_unit_price.Text = detail.DtrnUnitPrice.ToString();
            tbDtrn_disc_line1.Text = detail.DtrnDiscLine1.ToString();
            lblDtrn_net_value.Text = detail.DtrnNetValue.ToString();
            lblDtrn_vat_value.Text = detail.DtrnVatValue.ToString();

            tbDtrn_qty1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_qty1_TextChanged);
            tbDtrn_disc_line1.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbDtrn_disc_line1_TextChanged);
            tbDtrn_qty1.FocusChange += tbQty_HandleFocusChange;
            tbDtrn_disc_line1.FocusChange += tbQty_HandleFocusChange;

            return view;
        }

		private void EditTextTouchUp(object sender, View.TouchEventArgs e)
		{			
			if(MotionEventActions.Up == e.Event.Action) {
				if(TransDetAdapter.lastFocusedControl != null)
					TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_normal);
				TransDetAdapter.lastFocusedControl = (EditText)sender;
				TransDetAdapter.lastFocusedControl.SetBackgroundResource(Resource.Drawable.my_edit_text_background_focused);
			}
		}

        void tbQty_HandleFocusChange(object sender, View.FocusChangeEventArgs e)
        {          
            if (DetailFieldSelectedEvent != null)
            {
                if (e.HasFocus)
                {
                    int index = int.Parse(((EditText)sender).Tag.ToString());

                    if (sender.Equals(tbDtrn_qty1))
                    {                      
                        DetailFieldSelectedEvent(new SelectedDetail(){  
                            Position = index,
                            TransDetail = dataSource[index],
                            Property = "QTY"
                        });
                    } else  if (sender.Equals(tbDtrn_disc_line1))
                    {
                        DetailFieldSelectedEvent(new SelectedDetail(){  
                            Position = index,
                            TransDetail = dataSource[index], 
                            Property = "DISC"
                        });
                    }
                } else
                {
                    DetailFieldSelectedEvent(null);
                }
            }
        }

        void tbDtrn_qty1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            TransDet detail = dataSource[index];
            string qtyText = e.Text.ToString().Trim();
            detail.DtrnQty1 = qtyText != "" ? double.Parse(qtyText) : 0;
            
            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

        void tbDtrn_disc_line1_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int index = int.Parse(((EditText)sender).Tag.ToString());
            TransDet detail = dataSource[index];
            detail.DtrnDiscLine1 = double.Parse((sender as EditText).Text);
            
            if (QtysChangedEvent != null)
            {
                QtysChangedEvent();
            }
        }

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