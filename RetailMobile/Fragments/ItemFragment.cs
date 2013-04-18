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
			
			ItemInfo item = ItemInfo.GetItem(this.Activity, base.ObjectId);
			
			View view = inflater.Inflate(Resource.Layout.ItemDetails, container, false);
			
			EditText tbItemCode = (EditText)view.FindViewById(Resource.Id.tbItemCode);
			EditText tbItemName = (EditText)view.FindViewById(Resource.Id.tbItemName);
			EditText tbLongDesc = (EditText)view.FindViewById(Resource.Id.tbLongDesc);
			EditText tbItemSaleVal = (EditText)view.FindViewById(Resource.Id.tbItemSaleVal);
			EditText tbItemRetVal = (EditText)view.FindViewById(Resource.Id.tbItemRetVal);
			
			tbItemCode.Text = item.item_cod;
			tbItemName.Text = item.item_desc;
			tbLongDesc.Text = item.item_long_desc;
			//tbItemRetVal.Text = item.item_ret_val1.ToString("######0.0##");
			tbItemRetVal.Text = item.item_qty_left.ToString("######0.0##");
			tbItemSaleVal.Text = item.item_sale_val1.ToString("######0.0##");
			
			return view;
		}
	}
}