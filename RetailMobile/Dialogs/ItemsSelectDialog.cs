using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Views.InputMethods;
using RetailMobile.Library;

namespace RetailMobile
{
    public class ItemsSelectDialog : Dialog
    {
        const int OK_BUTTON = 53345;
        const int CANCEL_BUTTON = 53346;
        Context currentContext;
        Activity activity;
        TransHed transHed;
        ListView lvItems;
        EditText tbSearch;
        EditText tbRetVal;
        Spinner cbCateg1;
        Spinner cbCateg2;
        Dictionary<int, double> _checkedItems = new Dictionary<int, double>();
        CheckableItemsAdapter adapterItems;
        ItemInfoList itemInfoList;
        bool scrollLoading = false;
        int currentPage = 0;
        int previousTotal = 0;
        ImageView imgItemSelected;
        Button btnShowImage;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public ItemsSelectDialog(Activity context, int theme, TransHed header) : base(context, theme)
        {
            currentContext = context;
            transHed = header;
            activity = context;
            //SetTitle(context.GetString (Resource.String.miItems));
            SetContentView(Resource.Layout.dialog_item_search);

            actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarDialog1);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.ClearButtons();
            actionBar.AddButtonRight(OK_BUTTON, currentContext.GetString(Resource.String.btnOK), Resource.Drawable.tick_16);
            actionBar.AddButtonLeft(CANCEL_BUTTON, currentContext.GetString(Resource.String.btnClose), Resource.Drawable.close_icon64);
            actionBar.SetTitle(currentContext.GetString(Resource.String.miItems));
            
            lvItems = FindViewById<ListView>(Resource.Id.lvItems);
            tbSearch = FindViewById<EditText>(Resource.Id.tbSearch);
            tbRetVal = FindViewById<EditText>(Resource.Id.tbRetVal);
            cbCateg1 = FindViewById<Spinner>(Resource.Id.cbCateg1);
            cbCateg2 = FindViewById<Spinner>(Resource.Id.cbCateg2);
            imgItemSelected = FindViewById<ImageView>(Resource.Id.imgItemSelected);
            btnShowImage = FindViewById<Button>(Resource.Id.btnShowImage);

            btnShowImage.Click += new EventHandler(btnShowImage_Click);

            tbSearch.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            tbRetVal.AfterTextChanged += new EventHandler<Android.Text.AfterTextChangedEventArgs>(tbSearch_AfterTextChanged);
            
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

            adapterItems = new CheckableItemsAdapter(activity, new ItemInfoList());
            adapterItems.ItemImageSelected += new CheckableItemsAdapter.ItemImageSelectedDelegate(ItemImageSelected);
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

        public override void Hide()
        {
            /*InputMethodManager imm = (InputMethodManager)currentContext.GetSystemService(
                Android.Content.Context.InputMethodService);
            imm.HideSoftInputFromWindow(tbSearch.WindowToken, 0);
            imm.HideSoftInputFromWindow(tbRetVal.WindowToken, 0);*/
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
            if (imgItemSelected.Drawable != null)
            {
                Android.Graphics.Bitmap bmp = ((Android.Graphics.Drawables.BitmapDrawable)imgItemSelected.Drawable).Bitmap;
                imgItemSelected.SetImageBitmap(image);
                bmp.Recycle();
            }
            else
            {
                imgItemSelected.SetImageBitmap(image);
            }
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

        void btnShowImage_Click(object sender, EventArgs e)
        {
            if (imgItemSelected.Drawable == null)
                return;

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
            {
                Java.IO.File test = new Java.IO.File(saveFile);
                test.Delete(); 
                //fi.Delete();
            }

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

        void ReloadItems()
        {
            currentPage = 0;
            int cbCateg1Id = ((SpinnerAdapter<int, string>)cbCateg1.Adapter).GetSelectedValue(cbCateg1.SelectedItemPosition);
            int cbCateg2Id = ((SpinnerAdapter<int, string>)cbCateg2.Adapter).GetSelectedValue(cbCateg2.SelectedItemPosition);
            decimal retVal = 0;
            decimal.TryParse(tbRetVal.Text.Trim (), out retVal);
                        
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

            if (itemInfoList != null && itemInfoList.CurrentCriteria.Category1 == crit.Category1 && itemInfoList.CurrentCriteria.Category2 == crit.Category2 && 
                itemInfoList.CurrentCriteria.CstId == crit.CstId && itemInfoList.CurrentCriteria.ItemDesc == crit.ItemDesc && itemInfoList.CurrentCriteria.RetVal == crit.RetVal)
            {
                return;
            }

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
                //ItemInfo itemInfo = ItemInfo.GetItem(currentContext, item.ItemId, false);
                //imgItemSelected.SetImageBitmap(itemInfo.ItemImage);
            };

            GC.Collect();
        }

        public Dictionary<int, double> CheckedItemIds
        {
            get { return _checkedItems; }
        }
    }
}