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

namespace RetailMobile
{
    [Activity(Label = "Items List")]
    //[Activity(Label = "Main menu", MainLauncher = true, Icon = "@drawable/icon")]
    public class ItemsList : ListActivity
    {
        Library.ItemInfoList _ItemInfoList;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ItemsList);
            RegisterForContextMenu(this.ListView);

        }

        protected override void OnResume()
        {
            base.OnResume();

            LoadData();
        }

        private void LoadData()
        {
            _ItemInfoList = Library.ItemInfoList.GetItemInfoList(this);
            ItemsListAdapter adapter = new ItemsListAdapter(this, Resource.Layout.ItemInfoRow, _ItemInfoList);
            ListAdapter = adapter;
            /*DAL.ItemInfoList.GetItemInfoList("", (o, e) =>
            {
                if (e.Error != null)
                {
                    Android.Util.Log.Error("GetItemInfoList", string.Format("An error has occurred: {0}", e.Error.Message));
                }
                else
                {
                    _ItemInfoList = e.Object;
                    ItemsListAdapter adapter = new ItemsListAdapter(this, Resource.Layout.ItemInfoRow, _ItemInfoList);
                    ListAdapter = adapter;
                }
            });*/
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var ItemDetailsScreen = new Intent(this, typeof(ItemDetails));
            ItemDetailsScreen.PutExtra("ItemID", _ItemInfoList[position].item_id.ToString());
            StartActivity(ItemDetailsScreen);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //var menuItemNewItem = menu.Add(0, 1, 1, Resource.String.miAddItem);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 1)
            {
                var ItemDetailsScreen = new Intent(this, typeof(ItemDetails));
                StartActivity(ItemDetailsScreen);
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            //MenuInflater.Inflate(Resource.Menu.ItemListContextMenu,menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            /*AdapterView.AdapterContextMenuInfo mi = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            switch (item.ItemId)
            {
                case Resource.Id.miNew:
                    //add new 
                    var ItemDetailsScreen = new Intent(this, typeof(ItemDetails));
                    StartActivity(ItemDetailsScreen);
                    return true;
                case Resource.Id.miEdit:
                    //edit
                    var ItemDetailsScreenEdit = new Intent(this, typeof(ItemDetails));
                    ItemDetailsScreenEdit.PutExtra("ItemID", _ItemInfoList[mi.Position].ItemID);
                    StartActivity(ItemDetailsScreenEdit);
                    return true;
                case Resource.Id.miDelete:
                    //delete
                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }*/
            return base.OnContextItemSelected(item);
        }
    }
}