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
    public class ItemInfoList : List<ItemInfo>
    {
        public static ItemInfoList GetItemInfoList(Context ctx)
        {
            return GetItemInfoList(ctx, new Criteria());
        }
        
        public static ItemInfoList GetItemInfoList(Context ctx, Criteria c)
        {
            ItemInfoList items = new ItemInfoList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                //string query = "SELECT item_id, item_cod, item_desc FROM items WHERE 1=1 ";
                string joinLastDate = "";
                string fields = "";

                if (c.CstId > 0)
                {
                    fields += @",
    ritemlast.last_date";
                    joinLastDate = @"
LEFT OUTER JOIN ritemlast ON ritemlast.item_id = ritems.id AND ritemlast.cst_id = " + c.CstId;
                }

                string query = @"
SELECT top 50 
    ritems.id, 
    ritems.item_cod, 
    ritems.item_desc,
    ritems.item_qty_left " +
                    fields + @" 
FROM ritems" +
                    joinLastDate + @" 
WHERE 1 = 1 ";
                
                if (c.ItemDesc != "")
                {
                    query += " AND ritems.item_desc like \'" + c.ItemDesc + "%\'";
                }
                
                if (c.Category1 != 0)
                {
                    query += " AND ritems.item_ctg_id = " + c.Category1;
                }
                
                if (c.Category2 != 0)
                {
                    query += " AND ritems.item_ctg2_id = " + c.Category2;               
                }
                
                if (c.RetVal != 0)
                {
//                    query += " AND ritems.item_qty_left = " + c.RetVal;
                }
                
				query += " ORDER BY ritems.item_desc ";

                IPreparedStatement ps = conn.PrepareStatement(query);
                
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    ItemInfo item = new ItemInfo()
                    {
                        ItemId = result.GetInt("id"),
                        item_cod = result.GetString("item_cod"),
                        ItemDesc = result.GetString("item_desc"),
                        ItemQtyLeft = Convert.ToDecimal(result.GetDouble("item_qty_left"))
                    };

                    if (c.CstId > 0)
                    {
                        item.ItemLastBuyDate = Common.JavaDateToDatetime(result.GetDate("last_date"));
                    }
                    
                    items.Add(item);
                }
                
                ps.Close();
                conn.Release();
            }
            
            return items;
        }
        
        public class Criteria
        {
            public string ItemDesc;
            public int Category1;
            public int Category2;
            public decimal RetVal;
            public long CstId;
            
            public Criteria()
            {
            }
        }
    }
}