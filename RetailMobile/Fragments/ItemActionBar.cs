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

namespace RetailMobile.Fragments
{
    public class ItemActionBar : Android.Support.V4.App.Fragment
    {
		List<int> ButtonsAdded;
		public delegate void ActionButtonCLickedDelegate(int buttonID);
		public event ActionButtonCLickedDelegate ActionButtonClicked;
		TextView titleView;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			View v = inflater.Inflate(Resource.Layout.items_action_bar, container, true);
			titleView = v.FindViewById<TextView>(Resource.Id.lblCaption);
			ButtonsAdded = new List<int>();
            //btnSync = v.FindViewById<Button>(Resource.Id.btnSync);
            //btnSync.Click += new EventHandler(btnSync_Click);
            return v;
        }

		public void SetTitle(string title)
		{
			titleView.Text = title;
		}

		public void AddButtonLeft(int id, string text, int resourceID)
		{
			LinearLayout layout = this.Activity.FindViewById<LinearLayout>(Resource.Id.LeftButtons);
			AddButton(layout, id, text, resourceID);
		}

		public void AddButtonRight(int id, string text, int resourceID)
		{
			LinearLayout layout = this.Activity.FindViewById<LinearLayout>(Resource.Id.RightButtons);
			AddButton(layout, id, text, resourceID);
		}

		public void ClearButtons()
		{
			LinearLayout layoutL = this.Activity.FindViewById<LinearLayout>(Resource.Id.LeftButtons);
			LinearLayout layoutR = this.Activity.FindViewById<LinearLayout>(Resource.Id.RightButtons);
			layoutL.RemoveAllViews();
			layoutR.RemoveAllViews();
		}

		private void AddButton(LinearLayout parent, int id, string text, int resourceID)
		{
			if(ButtonsAdded.Contains(id))
				return;

			RelativeLayout rlButton = new RelativeLayout(this.Activity);
			rlButton.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,ViewGroup.LayoutParams.FillParent);
			parent.AddView(rlButton);

			Button btn = new Button(this.Activity,null,Resource.Style.menuButtonStyle);
			btn.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,ViewGroup.LayoutParams.WrapContent);
			((RelativeLayout.LayoutParams)btn.LayoutParameters).AddRule(LayoutRules.CenterVertical);
			((RelativeLayout.LayoutParams)btn.LayoutParameters).LeftMargin = 2;
			((RelativeLayout.LayoutParams)btn.LayoutParameters).RightMargin = 2;
			btn.Text = text;
			btn.Tag = id;
			btn.Click += new EventHandler(ActionButton_Clicked);
			rlButton.AddView(btn);
			ButtonsAdded.Add(id);
			//btn.LayoutParameters = new RelativeLayout.LayoutParams();
		}

		private void ActionButton_Clicked(object sender, EventArgs e)
		{
			if(ActionButtonClicked != null)
				ActionButtonClicked((int)((Button)sender).Tag);
		}
    }
}