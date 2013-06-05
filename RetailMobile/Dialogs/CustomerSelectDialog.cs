using System;
using Android.App;
using Android.Widget;
using Android.Util;

namespace RetailMobile
{
    public class CustomerSelectDialog : Dialog
    {
        const int OK_BUTTON = 52345;
        const int CANCEL_BUTTON = 52346;
        Activity activity;
        ListView lvCustomers;
        EditText tbCustCode;
        EditText tbCustName;
        long _custId;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public CustomerSelectDialog(Activity context, int theme)
            : base(context, theme)
        {
            activity = context;
            //dialogCustomers.Window.SetLayout(Android.Widget.RelativeLayout.LayoutParams.FillParent, Android.Widget.RelativeLayout.LayoutParams.FillParent);
//            SetTitle(context.GetString(Resource.String.miCustomers));
            SetContentView(Resource.Layout.dialog_customer_search);

            actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarDialogCust);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(OK_BUTTON, activity.GetString(Resource.String.btnOK), Resource.Drawable.tick_16);
            actionBar.AddButtonLeft(CANCEL_BUTTON, activity.GetString(Resource.String.btnClose), Resource.Drawable.close_icon64);
            actionBar.SetTitle(activity.GetString(Resource.String.miCustomers));

            lvCustomers = FindViewById<ListView>(Resource.Id.lvCustomers);
            tbCustCode = FindViewById<EditText>(Resource.Id.tbCustCode);
            tbCustName = FindViewById<EditText>(Resource.Id.tbCustName);

            tbCustCode.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            tbCustName.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);

            lvCustomers.ChoiceMode = ChoiceMode.Single;
            lvCustomers.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(lvCustomers_ItemClick);

            Library.CustomerInfoList custInfoList = Library.CustomerInfoList.GetCustomerInfoList(activity, new Library.CustomerInfoList.Criteria()
            {
                CustCode = tbCustCode.Text,
                CustName = tbCustName.Text
            });
            lvCustomers.Adapter = new CustomersAdapter(activity, custInfoList);
        }

        void ActionBarButtonClicked(int id)
        {
            if (id == OK_BUTTON)
            {
                try
                {
                    if (_custId > 0)
                    {
                        Dismiss();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("ActionBarButtonClicked SAVE_BUTTON", "ActionBarButtonClicked SAVE_BUTTON " + ex.Message);
                }
            }
            else if (id == CANCEL_BUTTON)
            {
                Cancel();
            }
        }

        public override void Dismiss()
        {
            Android.Support.V4.App.FragmentTransaction ft = ((Android.Support.V4.App.FragmentActivity)activity).SupportFragmentManager.BeginTransaction();
            ft.Remove(actionBar);
            ft.Commit();

            base.Dismiss();
        }

        void lvCustomers_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Library.CustomerInfo c = ((CustomersAdapter)lvCustomers.Adapter).GetItem(e.Position);
            _custId = c.CustID;
            Log.Debug("lvCustomers_ItemClick", "_custId=" + _custId);
            if (_custId > 0)
            {
                Dismiss();
            }
        }

        void tbSearch_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            Library.CustomerInfoList custInfoList = Library.CustomerInfoList.GetCustomerInfoList(activity, new Library.CustomerInfoList.Criteria()
            {
                CustCode = tbCustCode.Text,
                CustName = tbCustName.Text
            });
            lvCustomers.Adapter = new CustomersAdapter(activity, custInfoList);
        }

        public long CustId
        {
            get { return _custId; }
        }
    }
}