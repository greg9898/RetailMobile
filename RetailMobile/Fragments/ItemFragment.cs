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
using Android.Support.V4.App;
using RetailMobile.Library;

namespace RetailMobile
{
    public class ItemFragment : BaseFragment
    {
        public static BaseFragment NewInstance(long objId)
        {
            var detailsFrag = new ItemFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }
            
            ItemInfo item = ItemInfo.GetItem(Activity, ObjectId);
            
            View view = inflater.Inflate(Resource.Layout.ItemDetails, container, false);
            
            EditText tbItemCode = (EditText)view.FindViewById(Resource.Id.tbItemCode);
            EditText tbItemName = (EditText)view.FindViewById(Resource.Id.tbItemName);
            EditText tbLongDesc = (EditText)view.FindViewById(Resource.Id.tbLongDesc);
            EditText tbItemSaleVal = (EditText)view.FindViewById(Resource.Id.tbItemSaleVal);
            EditText tbItemQtyLeft = (EditText)view.FindViewById(Resource.Id.tbItemRetVal);
            ImageView imgItemDetail = (ImageView)view.FindViewById(Resource.Id.imgItemDetail);
            
            tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.ItemDesc;
            tbLongDesc.Text = item.item_long_desc;
            tbItemQtyLeft.Text = item.ItemQtyLeft.ToString("######0.0##");
            tbItemSaleVal.Text = item.ItemSaleVal1.ToString("######0.0##");

            imgItemDetail.Click += new EventHandler(imgItemDetail_Click);
            
            return view;
        }

        void imgItemDetail_Click(object sender, EventArgs e)
        {
            ImageView myImage = new ImageView(Activity);
            myImage.SetImageResource(Resource.Drawable.night);
//            myImage.Click - close

            Dialog dlgImg = new Dialog(Activity);
            dlgImg.Window.RequestFeature(WindowFeatures.NoTitle);
            dlgImg.SetContentView(myImage);
            dlgImg.Show();
        }
    }
}