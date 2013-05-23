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
        private const int OK_BUTTON = 53345;
        private const int CANCEL_BUTTON = 53346;
        Context currentContext;
        Activity activity;
        TransHed transHed;
        ListView lvItems;
        EditText tbSearch;
        EditText tbRetVal;
        Button btnClose;
        Spinner cbCateg1;
        Spinner cbCateg2;
        Dictionary<int, int> _checkedItems = new Dictionary<int, int>();
        CheckableItemsAdapter adapterItems;
        ItemInfoList itemInfoList;
        bool scrollLoading = false;
        private int currentPage = 0;
        private int previousTotal = 0;
        ImageView imgItemSelected;
        Button btnShowImage;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public ItemsSelectDialog(Activity context, int theme, TransHed header)
            : base(context, theme)
        {
            currentContext = context;
            transHed = header;
            activity = context;
            //SetTitle(context.GetString (Resource.String.miItems));
            SetContentView(Resource.Layout.dialog_item_search);
            // this.Window.SetLayout(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);

            actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarDialog1);
            //actionBar = FindViewById<RetailMobile.Fragments.ItemActionBar>(Resource.Id.ActionBarDialog);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(OK_BUTTON, currentContext.GetString(Resource.String.btnOK), Resource.Drawable.tick_16);
            actionBar.AddButtonLeft(CANCEL_BUTTON, currentContext.GetString(Resource.String.btnClose), Resource.Drawable.close_icon64);
            actionBar.SetTitle(currentContext.GetString(Resource.String.miItems));
            
            Button btnOK = FindViewById<Button>(Resource.Id.btnOK);
            btnClose = FindViewById<Button>(Resource.Id.btnClose);
            lvItems = FindViewById<ListView>(Resource.Id.lvItems);
            tbSearch = FindViewById<EditText>(Resource.Id.tbSearch);
            tbRetVal = FindViewById<EditText>(Resource.Id.tbRetVal);
            cbCateg1 = FindViewById<Spinner>(Resource.Id.cbCateg1);
            cbCateg2 = FindViewById<Spinner>(Resource.Id.cbCateg2);
            imgItemSelected = FindViewById<ImageView>(Resource.Id.imgItemSelected);
            btnShowImage = FindViewById<Button>(Resource.Id.btnShowImage);

            btnOK.Visibility = ViewStates.Gone;
            btnClose.Visibility = ViewStates.Gone;

            btnShowImage.Click += new EventHandler(btnShowImage_Click);

            tbSearch.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            tbRetVal.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            btnClose.Click += new EventHandler(btnCloseItems_Click);
            
            List<KeyValuePair<int, string>> categ1List = AddCategoryList(1);
            List<KeyValuePair<int, string>> categ2List = AddCategoryList(2);
            
            categ1List.Insert(0, new KeyValuePair<int, string>(0, activity.GetString(Resource.String.SpinnerAll)));
            categ2List.Insert(0, new KeyValuePair<int, string>(0, activity.GetString(Resource.String.SpinnerAll)));
            
            SpinnerAdapter<int, string> categ1Adapter = new SpinnerAdapter<int, string>(activity, categ1List);
            SpinnerAdapter<int, string> categ2Adapter = new SpinnerAdapter<int, string>(activity, categ2List);
            cbCateg1.Adapter = categ1Adapter;
            cbCateg2.Adapter = categ2Adapter;
            cbCateg1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(cbCateg1_ItemSelected);
            cbCateg2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(cbCateg1_ItemSelected);
            
            lvItems.FocusChange += new EventHandler<View.FocusChangeEventArgs>(lvItems_FocusChange);
            lvItems.AddHeaderView(context.LayoutInflater.Inflate (Resource.Layout.item_row_checkable_header, null));
            
            btnOK.Click += new EventHandler(btnOKItem_Click);

//          itemInfoList = new ItemInfoList ();
//		    itemInfoList.LoadItems (this.Context); no need of initial loading here

            adapterItems = new CheckableItemsAdapter(activity, new ItemInfoList());
            adapterItems.ItemImageSelected += new CheckableItemsAdapter.ItemImageSelectedDelegate(ItemImageSelected);
            //adapterItems.LoadData(0);
            lvItems.Adapter = adapterItems;

            adapterItems.SingleItemSelectedEvent += () =>
            {
                _checkedItems = adapterItems.CheckedItemIds;
				
                Dismiss();
            };
            adapterItems.SingleItemFocusedEvent += (item) =>
            {
                TextView lblItemSelectedInfo = FindViewById<TextView>(Resource.Id.lblItemSelectedInfo);
                //ImageView imgItemSelected = FindViewById<ImageView>(Resource.Id.imgItemSelected);
                lblItemSelectedInfo.Text = item.ItemDesc;
                imgItemSelected.SetImageResource(Resource.Drawable.night);//todo
            };

            //ItemInfoList.LoadAdapterItems(context,0,adapterItems, new Library.ItemInfoList.Criteria());

            lvItems.Scroll += new EventHandler<AbsListView.ScrollEventArgs>((o,e) => {
				if (scrollLoading && e.TotalItemCount > previousTotal) {
					scrollLoading = false;
					previousTotal = e.TotalItemCount;
					currentPage++;
				}
				
				if (!scrollLoading && (e.FirstVisibleItem + e.VisibleItemCount) + 10 >= e.TotalItemCount && e.TotalItemCount >= 10) {
//					((IScrollLoadble)lvItems.Adapter).LoadData(currentPage);
					HeaderViewListAdapter adapter = (HeaderViewListAdapter)lvItems.Adapter;
					IScrollLoadble origAdapter = (IScrollLoadble)adapter.WrappedAdapter;
					origAdapter.LoadData (currentPage);

					scrollLoading = true;
				}
			});

        }

        void ActionBarButtonClicked(int id)
        {
            if (id == OK_BUTTON)
            {
                try
                {
                    HeaderViewListAdapter adapter = (HeaderViewListAdapter)lvItems.Adapter;
                    CheckableItemsAdapter origAdapter = (CheckableItemsAdapter)adapter.WrappedAdapter;

                    if (origAdapter.CheckedItemIds.Count > 0)
                    {
                        _checkedItems = origAdapter.CheckedItemIds;

                        Dismiss();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("ActionBarButtonClicked SAVE_BUTTON", "ActionBarButtonClicked SAVE_BUTTON " + ex.Message);
                }
            }
            else
                if (id == CANCEL_BUTTON)
            {
                Cancel();
            }
        }


        public void ItemImageSelected(Android.Graphics.Bitmap image)
        {
            imgItemSelected.SetImageBitmap(image);
        }

        void lvItems_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                Log.Debug("lvItems_FocusChange", "lvItems_FocusChange if(e.HasFocus)");
            }
        }

        List<KeyValuePair<int, string>> AddCategoryList(int categTbl)
        {
            List<KeyValuePair<int, string>> categComboList = new List<KeyValuePair<int, string>>();
            CategoryList categList = CategoryList.GetCategoryList(activity, categTbl);
            
            foreach (Category c in categList)
            {
                categComboList.Add(new KeyValuePair<int, string> (c.Id, c.ItemCategDesc));
            }
            
            return categComboList;
        }

        void cbCateg1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ReloadItems();
        }

        void btnOKItem_Click(object sender, EventArgs e)
        {
            HeaderViewListAdapter adapter = (HeaderViewListAdapter)lvItems.Adapter;
            CheckableItemsAdapter origAdapter = (CheckableItemsAdapter)adapter.WrappedAdapter;

            if (origAdapter.CheckedItemIds.Count > 0)
            {
                _checkedItems = origAdapter.CheckedItemIds;
            
                Dismiss();
            }
        }

        void btnShowImage_Click(object sender, EventArgs e)
        {
            Android.Graphics.Bitmap bmp = ((Android.Graphics.Drawables.BitmapDrawable)imgItemSelected.Drawable).Bitmap;
            if (bmp == null)
                return;
            //imgItemSelected.get
            string root = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "DCIM");
            //string root = Android.OS.Environment.DirectoryPictures.ToString();
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(root);
            if (!myDir.Exists)
                myDir.Create();
            //myDir.Equals,
            string fname = "imageRetail.jpg";
            string saveFile = System.IO.Path.Combine(myDir.FullName, fname);
            System.IO.FileInfo fi = new System.IO.FileInfo(saveFile);


            if (fi.Exists)
                fi.Delete();
            try
            {
                //System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile);

                System.IO.FileStream fs = new System.IO.FileStream(saveFile, System.IO.FileMode.Create);
                //ItemInfo.GetItem().ItemImage.Compress
                bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, fs);

                fs.Flush();
                fs.Close();

                Intent intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.Parse("file://" + saveFile), "image/*");
                currentContext.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Log.Debug("btnViewImage_Click", ex.Message);
            }
        }

        void btnCloseItems_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        void tbSearch_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            ReloadItems();
        }

        public override void Dismiss()
        {
            Android.Support.V4.App.FragmentTransaction ft = ((Android.Support.V4.App.FragmentActivity)activity).SupportFragmentManager.BeginTransaction();
            ft.Remove(actionBar);
            ft.Commit();

            base.Dismiss();
        }

        private void ReloadItems()
        {
            currentPage = 0;
            int cbCateg1Id = ((SpinnerAdapter<int, string>)cbCateg1.Adapter).GetSelectedValue(cbCateg1.SelectedItemPosition);
            int cbCateg2Id = ((SpinnerAdapter<int, string>)cbCateg2.Adapter).GetSelectedValue(cbCateg2.SelectedItemPosition);
            decimal retVal = 0;
            decimal.TryParse(tbRetVal.Text.Trim (), out retVal);
            
            Log.Debug("ReloadItems tbSearch.Text=", tbSearch.Text);
            Log.Debug("ReloadItems cbCateg1Value=", cbCateg1Id.ToString());
            Log.Debug("ReloadItems cbCateg2Value=", cbCateg2Id.ToString());
            Log.Debug("ReloadItems retVal=", retVal.ToString());
            
            if (cbCateg1Id == 0 & cbCateg2Id == 0 && retVal == 0 && tbSearch.Text == "")
            {
                lvItems.Adapter = new CheckableItemsAdapter(activity, new Library.ItemInfoList());
                return;
            }
            
            ItemInfoList.Criteria crit = new ItemInfoList.Criteria()
            {
                ItemDesc = tbSearch.Text,
                Category1 = cbCateg1Id,
                Category2 = cbCateg2Id,
                RetVal = retVal,
                CstId = transHed.CstId
            };

            //itemInfoList = ItemInfoList.GetItemInfoList(activity,crit);
            itemInfoList = new ItemInfoList();
            itemInfoList.CurrentCriteria = crit;

            adapterItems = new CheckableItemsAdapter(activity, itemInfoList);
            adapterItems.ItemImageSelected += new CheckableItemsAdapter.ItemImageSelectedDelegate(ItemImageSelected);
            adapterItems.LoadData(0);
            /*ItemInfoList.LoadAdapterItems(currentContext,0,adapterItems,new ItemInfoList.Criteria()
			                              {
				ItemDesc = tbSearch.Text,
				Category1 = cbCateg1Id,
				Category2 = cbCateg2Id,
				RetVal = retVal,
				CstId = transHed.CstId
			});*/

            lvItems.Adapter = adapterItems;
            currentPage = 1;//	lvItems.LastVisiblePosition

            adapterItems.SingleItemSelectedEvent += () =>
            {
                _checkedItems = adapterItems.CheckedItemIds;
				
                Dismiss();
            };
            adapterItems.SingleItemFocusedEvent += (item) =>
            {
                TextView lblItemSelectedInfo = FindViewById<TextView>(Resource.Id.lblItemSelectedInfo);
                ImageView imgItemSelected = FindViewById<ImageView>(Resource.Id.imgItemSelected);
                lblItemSelectedInfo.Text = item.ItemDesc;
                //tincho da go vidi
                ItemInfo itemInfo = ItemInfo.GetItem(currentContext, item.ItemId);
                imgItemSelected.SetImageBitmap(itemInfo.ItemImage);
            };
        }

        public Dictionary<int, int> CheckedItemIds
        {
            get { return _checkedItems; }
        }
    }
}