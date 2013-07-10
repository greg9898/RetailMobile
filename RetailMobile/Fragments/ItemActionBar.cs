using System;
using System.Collections.Generic;
using Android.OS;
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
        LinearLayout leftButtons;
        LinearLayout rightButtons;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.items_action_bar, container, false);
            titleView = v.FindViewById<TextView>(Resource.Id.lblCaption);
            ButtonsAdded = new List<int>();
            leftButtons = v.FindViewById<LinearLayout>(Resource.Id.LeftButtons);
            rightButtons = v.FindViewById<LinearLayout>(Resource.Id.RightButtons);
            //btnSync = v.FindViewById<Button>(Resource.Id.btnSync);
            //btnSync.Click += new EventHandler(btnSync_Click);
            return v;
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public void SetTitle(string title)
        {
            if (titleView != null)
                titleView.Text = title;
        }

        public void AddButtonLeft(int id, string text, int resourceID)
        {
            AddButton(leftButtons, id, text, resourceID);
        }

        public void AddButtonRight(int id, string text, int resourceID)
        {
            AddButton(rightButtons, id, text, resourceID);
        }

        public void ClearButtons()
        {
            if (ButtonsAdded != null)
                ButtonsAdded.Clear();
            if (leftButtons != null)
                leftButtons.RemoveAllViews();
            if (rightButtons != null)
                rightButtons.RemoveAllViews();
        }

        void AddButton(ViewGroup parent, int id, string text, int resourceID)
        {
            if (ButtonsAdded == null)
                ButtonsAdded = new List<int>();

            if (ButtonsAdded.Contains(id))
                return;

            RelativeLayout rlButton = new RelativeLayout(this.Activity);
            rlButton.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.FillParent);
            parent.AddView(rlButton);

            Button btn = new Button(this.Activity, null, Resource.Style.menuButtonStyle);
            btn.LayoutParameters = ActionBar.ButtonLayoutParams(this.Resources);

            if (resourceID == 0)
            {
                btn.Text = text;
            }
            else
            {
                btn.SetBackgroundResource(resourceID);
            }
            btn.Tag = id;
            btn.Click += BtnClick; 
            rlButton.AddView(btn);
            ButtonsAdded.Add(id);
            //btn.LayoutParameters = new RelativeLayout.LayoutParams();
        }

        void BtnClick(object sender, EventArgs e)
        {
            if (ActionButtonClicked != null)
                ActionButtonClicked((int)((Button)sender).Tag);
        }
    }
}