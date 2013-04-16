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
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class ItemInfo
    {
        public decimal ItemId { get; set; }

        public int ItemQty
        {
            get;
            set;
        }

        public string item_cod{ get; set; }

        public string item_desc { get; set; }

        public string item_long_desc { get; set; }

        public decimal item_ret_val1 { get; set; }

        public decimal item_sale_val1 { get; set; }

        public decimal item_buy_val1 { get; set; }

        public DateTime ItemLastBuyDate{ get; set; }

        public ItemInfo()
        {
            ItemQty = 1;
        }

        public static ItemInfo GetItem(Context ctx, decimal itemID)
        {
            ItemInfo info = new ItemInfo();

            using (IConnection conn = Sync.GetConnection(ctx))
            {
                IPreparedStatement ps = conn.PrepareStatement(@"SELECT item_id, 
item_cod, 
item_desc, 
item_long_des, 
item_ret_val1, 
item_sale_val1 ,
item_buy_val1
FROM items WHERE item_id = :ItemID");
                ps.Set("ItemID", itemID.ToString());

                IResultSet result = ps.ExecuteQuery();

                if (result.Next())
                {
                    info.ItemId = Convert.ToDecimal(result.GetDouble("item_id"));
                    info.item_cod = result.GetString("item_cod");
                    info.item_desc = result.GetString("item_desc");
                    info.item_long_desc = result.GetString("item_long_des");
                    info.item_ret_val1 = Convert.ToDecimal(result.GetDouble("item_ret_val1"));
                    info.item_sale_val1 = Convert.ToDecimal(result.GetDouble("item_sale_val1"));
                    info.item_buy_val1 = Convert.ToDecimal(result.GetDouble("item_buy_val1"));
                }

                ps.Close();
            }

            return info;
        }
    }
}