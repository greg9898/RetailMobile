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
    public class ItemsSearchAdapter : ArrayAdapter<Library.ItemInfo>
    {
        Activity context = null;
        Library.ItemInfoList ItemInfoList;

        public ItemsSearchAdapter(Activity context, int rowResourceID, Library.ItemInfoList _list)
            : base(context, rowResourceID, _list)
        {
            this.context = context;

            ItemInfoList = _list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.ItemInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ItemSearchRow, null);

            TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbSearchItemCode);
            TextView tbItemDesc = (TextView)view.FindViewById(Resource.Id.tbSearchItemName);
            TextView tbItemLongDesc = (TextView)view.FindViewById(Resource.Id.tbItemLongDesc);
            TextView tbItemCateg1 = (TextView)view.FindViewById(Resource.Id.tbItemCateg1);
            TextView tbItemCateg2 = (TextView)view.FindViewById(Resource.Id.tbItemCateg2);
            TextView tbItemSaleVal = (TextView)view.FindViewById(Resource.Id.tbItemSaleVal);
            TextView tbItemRetVal = (TextView)view.FindViewById(Resource.Id.tbItemRetVal);
            TextView tbItemQtyLeft = (TextView)view.FindViewById(Resource.Id.tbItemQtyLeft);
            ImageView ivItemImage = (ImageView)view.FindViewById(Resource.Id.imgItem);

            if (tbItemCode != null)
                tbItemCode.Text = item.item_cod;
            if (tbItemDesc != null)
                tbItemDesc.Text = item.item_desc;
            /*if (tbItemLongDesc != null)
                tbItemLongDesc.Text = item.ItemLongDesc;
            if (tbItemCateg1 != null)
                tbItemCateg1.Text = item.Cat1ID.ToString();
            if (tbItemCateg2 != null)
                tbItemCateg2.Text = item.Cat2ID.ToString();
            if (tbItemSaleVal!= null)
                tbItemSaleVal.Text = item.ItemSaleVal1.ToString("c2");
            if (tbItemRetVal != null)
                tbItemRetVal.Text = item.ItemRetVal1.ToString("c2");
            if (tbItemQtyLeft != null)
                tbItemQtyLeft.Text = item.ItemQtyLeft.ToString();*/


            //demo code
            if (item.item_cod == "0001")
            {
                ivItemImage.SetImageResource(Resource.Drawable.kamenitza);
                tbItemQtyLeft.Text = "16";
            }
            if (item.item_cod == "0002")
            {
                ivItemImage.SetImageResource(Resource.Drawable.zagorka);
                tbItemQtyLeft.Text = "29";
            }

            return view;
        }
    }
}