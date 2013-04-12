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
using RetailMobile.Library;

namespace RetailMobile
{
    public class ItemsSelectDialog : Dialog
    {
        Activity activity;
        private ListView lvItems;
        private EditText tbSearch;
        private EditText tbRetVal;
        private Button btnClose;
        private Spinner cbCateg1;
        private Spinner cbCateg2;
        private Dictionary<int, int> _checkedItems = new Dictionary<int, int>();

        public ItemsSelectDialog(Activity context, int theme)
            : base(context, theme)
        {
            activity = context;
            //dialogCustomers.Window.SetLayout(Android.Widget.RelativeLayout.LayoutParams.FillParent, Android.Widget.RelativeLayout.LayoutParams.FillParent);
            SetTitle(context.GetString(Resource.String.miItems));
            SetContentView(Resource.Layout.dialog_item_search);

            Button btnOK = FindViewById<Button>(Resource.Id.btnOK);
            btnClose = FindViewById<Button>(Resource.Id.btnClose);
            lvItems = FindViewById<ListView>(Resource.Id.lvItems);
            tbSearch = FindViewById<EditText>(Resource.Id.tbSearch);
            tbRetVal = FindViewById<EditText>(Resource.Id.tbRetVal);
            cbCateg1 = FindViewById<Spinner>(Resource.Id.cbCateg1);
            cbCateg2 = FindViewById<Spinner>(Resource.Id.cbCateg2);

            tbSearch.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            tbRetVal.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            btnClose.Click += new EventHandler(btnCloseItems_Click);

            List<KeyValuePair<int, string>> categ1List;
            List<KeyValuePair<int, string>> categ2List;
            DebugAddGategLists(out categ1List, out categ2List);

            categ1List.Insert(0, new KeyValuePair<int, string>(0, "All"));
            categ2List.Insert(0, new KeyValuePair<int, string>(0, "All"));

            SpinnerAdapter<int, string> categ1Adapter = new SpinnerAdapter<int, string>(activity, categ1List);
            SpinnerAdapter<int, string> categ2Adapter = new SpinnerAdapter<int, string>(activity, categ2List);
            cbCateg1.Adapter = categ1Adapter;
            cbCateg2.Adapter = categ2Adapter;
            cbCateg1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(cbCateg1_ItemSelected);
            cbCateg2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(cbCateg1_ItemSelected);

            btnOK.Click += new EventHandler(btnOKItem_Click);
        }

        private static void DebugAddGategLists(out List<KeyValuePair<int, string>> categ1List, out List<KeyValuePair<int, string>> categ2List)
        {
            categ1List = new List<KeyValuePair<int, string>>();
            categ2List = new List<KeyValuePair<int, string>>();
            for (int i = 1; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    categ1List.Add(new KeyValuePair<int, string>(i, "value " + i));
                }
                if (i % 3 == 0)
                {
                    categ2List.Add(new KeyValuePair<int, string>(i, "value  " + i));
                }
            }
        }

        void cbCateg1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ReloadItems();
        }

        void btnOKItem_Click(object sender, EventArgs e)
        {
            _checkedItems = ((CheckableItemsAdapter)lvItems.Adapter).CheckedItemIds;

            Dismiss();//?
        }

        void btnCloseItems_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        void tbSearch_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            ReloadItems();
        }

        private void ReloadItems()
        {
            int cbCateg1Value = ((SpinnerAdapter<int, string>)cbCateg1.Adapter).GetSelectedValue(cbCateg1.SelectedItemPosition);
            int cbCateg2Value = ((SpinnerAdapter<int, string>)cbCateg2.Adapter).GetSelectedValue(cbCateg2.SelectedItemPosition);
            decimal retVal = 0;
            decimal.TryParse(tbRetVal.Text.Trim(), out retVal);

            Log.Debug("ReloadItems tbSearch.Text=", tbSearch.Text);
            Log.Debug("ReloadItems cbCateg1Value=", cbCateg1Value.ToString());
            Log.Debug("ReloadItems cbCateg2Value=", cbCateg2Value.ToString());
            Log.Debug("ReloadItems retVal=", retVal.ToString());

            if (cbCateg1Value == 0 & cbCateg2Value == 0 && retVal == 0 && tbSearch.Text == "")
            {
                lvItems.Adapter = new CheckableItemsAdapter(activity, new Library.ItemInfoList());
                return;
            }

            Library.ItemInfoList itemInfoList = Library.ItemInfoList.GetItemInfoList(activity, new Library.ItemInfoList.Criteria()
            {
                ItemDesc = tbSearch.Text,
                Category1 = cbCateg1Value,
                Category2 = cbCateg2Value,
                RetVal = retVal
            });
            lvItems.Adapter = new CheckableItemsAdapter(activity, itemInfoList);
        }

        public Dictionary<int, int> CheckedItemIds
        {
            get { return _checkedItems; }
        }
    }
}