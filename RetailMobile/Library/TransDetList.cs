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

namespace RetailMobile.Library
{
    public class TransDetList:List<TransDet>
    {
        public TransDet GetByItemId(int itemId)
        {
            foreach (TransDet d in this)
            {
                if (d.ItemId == itemId)
                {
                    return d;
                }
            }

            return null;
        }
    }
}