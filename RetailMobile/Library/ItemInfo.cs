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
using Android.Util;

namespace RetailMobile.Library
{
    public class ItemInfo
    {
        public int ItemId { get; set; }

        public int ItemQty
        {
            get;
            set;
        }

        public string item_cod{ get; set; }
        /// <summary>
        /// Item name
        /// </summary>
        /// <value>The item desc.</value>
        public string ItemDesc { get; set; }

        public string item_long_desc { get; set; }
        /// <summary>
        /// Unit price
        /// </summary>
        /// <value>The item_sale_val1.</value>
        public decimal ItemSaleVal1 { get; set; }

        public decimal ItemQtyLeft { get; set; }

        public int ItemVatId { get; set; }

        public DateTime ItemLastBuyDate{ get; set; }

        public Android.Graphics.Bitmap ItemImage;

        public ItemInfo()
        {
            ItemQty = 1;
        }

        public static ItemInfo GetItem(Context ctx, decimal itemID)
        {
            ItemInfo info = new ItemInfo();

            using (IConnection conn = Sync.GetConnection(ctx))
            {
                /*IPreparedStatement ps = conn.PrepareStatement(@"SELECT item_id,
item_cod,
item_desc,
item_long_des,
item_ret_val1,
item_sale_val1 ,
item_buy_val1
FROM items WHERE item_id = :ItemID");*/

                IPreparedStatement ps = conn.PrepareStatement(@"SELECT
id,
item_cod,
item_desc,
item_alter_desc,
unit_price,
item_qty_left ,
item_vat,
item_ctg_id,
item_ctg_disc,
item_image
FROM ritems WHERE id = :ItemID");
                ps.Set("ItemID", itemID.ToString());

                IResultSet result = ps.ExecuteQuery();

                if (result.Next())
                {
                    /*info.ItemId = Convert.ToInt64(result.GetDouble("item_id"));
info.item_cod = result.GetString("item_cod");
info.item_desc = result.GetString("item_desc");
info.item_long_desc = result.GetString("item_long_des");
info.item_ret_val1 = Convert.ToDecimal(result.GetDouble("item_ret_val1"));
info.item_sale_val1 = Convert.ToDecimal(result.GetDouble("item_sale_val1"));
info.item_buy_val1 = Convert.ToDecimal(result.GetDouble("item_buy_val1"));*/
                    //info.ItemId = Convert.ToInt64(result.GetDouble("id"));
                    info.ItemId = result.GetInt("id");
                    info.item_cod = result.GetString("item_cod");
                    info.ItemDesc = result.GetString("item_desc");
                    info.item_long_desc = result.GetString("item_alter_desc");
                    info.ItemSaleVal1 = Convert.ToDecimal(result.GetDouble("unit_price"));
                    info.ItemQtyLeft = Convert.ToDecimal(result.GetDouble("item_qty_left"));
                    info.ItemVatId = result.GetInt("item_vat");

                    byte[] signatureBytes = result.GetBytes("item_image");
                    try
                    {
                        if (signatureBytes.Length > 0)
                        {
                            info.ItemImage = Android.Graphics.BitmapFactory.DecodeByteArray(signatureBytes,
                            0, signatureBytes.Length);
                        }
                        else
                        {
                            info.ItemImage = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        info.ItemImage = null;
                    }

                    Log.Debug("item_vat", "item_vat=" + info.ItemVatId);
                }

                ps.Close();
                conn.Release();
            }

            return info;
        }
    }
}