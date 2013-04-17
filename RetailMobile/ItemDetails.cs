using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RetailMobile.Library;

namespace RetailMobile
{
    [Activity(Label = "Items Details")]
    public class ItemDetails : Activity
    {
        Library.ItemInfo _Item;
        EditText tbItemCode;
        EditText tbItemName;
        //SimpleApp.BindingManager bindingManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ItemDetails);

            //bindingManager = new SimpleApp.BindingManager(this);

            tbItemCode = (EditText)FindViewById(Resource.Id.tbItemCode);
            tbItemName = (EditText)FindViewById(Resource.Id.tbItemName);

            int ItemID = Intent.GetIntExtra("ItemID", 0);
            _Item = Library.ItemInfo.GetItem(this, ItemID);
            DataBind();
            /*DAL.ItemInfo.GetItemInfo(new CriteriaJ(this, ItemID), (o, e) =>
            {
                if (e.Error != null)
                {
                    throw (e.Error);
                }
                else
                {
                    _Item = e.Object;
                    DataBind();
                }
            });*/

            //_Item.Code = "0012";
            //_Item.Name = "Zaeka Roger";
        }


        private void DataBind()
        {
            tbItemCode.Text = _Item.item_cod;
            tbItemName.Text = _Item.item_desc;
            //bindingManager.Bindings.Add(new SimpleApp.Binding(tbItemCode, "Text", _Item, DAL.ItemInfo.ItemCodProperty.Name));
            //bindingManager.Bindings.Add(new SimpleApp.Binding(tbItemName, "Text", _Item, DAL.ItemInfo.ItemDescProperty.Name));

        }

    }
}

