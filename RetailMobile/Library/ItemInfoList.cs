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
            ItemInfoList items = new ItemInfoList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                IPreparedStatement ps = conn.PrepareStatement("SELECT item_id, item_cod, item_desc FROM items");

                IResultSet result = ps.ExecuteQuery();

                while (result.Next())
                {
                    ItemInfo item = new ItemInfo();
                    item.ItemId = Convert.ToDecimal(result.GetDouble("item_id"));
                    item.item_cod = result.GetString("item_cod");
                    item.item_cod = result.GetString("item_desc");

                    items.Add(item);
                }

                ps.Close();
            }

            return items;
        }

        public static ItemInfoList GetItemInfoList(Context ctx, Criteria c)
        {
            ItemInfoList items = new ItemInfoList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                string query = "SELECT item_id, item_cod, item_desc FROM items WHERE 1=1 ";

                if (c.ItemDesc != "")
                {
                    query += " AND item_desc like \'" + c.ItemDesc + "%\'";
                }

                if (c.Category1 != 0)
                {
                    throw new NotImplementedException("GetItemInfoList c.Category1 != 0");
                }

                if (c.Category2 != 0)
                {
                    throw new NotImplementedException("GetItemInfoList c.Category2 != 0");
                }

                if (c.RetVal != 0)
                {
                    query += " AND item_ret_val1 = " + c.RetVal;
                }
                IPreparedStatement ps = conn.PrepareStatement(query);

                IResultSet result = ps.ExecuteQuery();

                while (result.Next())
                {
                    ItemInfo item = new ItemInfo();
                    item.ItemId = Convert.ToDecimal(result.GetDouble("item_id"));
                    item.item_cod = result.GetString("item_cod");
                    item.item_desc = result.GetString("item_desc");

                    items.Add(item);
                }

                ps.Close();
            }

            return items;
        }

        public class Criteria
        {
            public string ItemDesc;
            public int Category1;
            public int Category2;
            public decimal RetVal;

            public Criteria() { }
            public Criteria(string desc, int category1, int category2, decimal retVal)
            {
                ItemDesc = desc;
                Category1 = category1;
                Category2 = category2;
                RetVal = retVal;
            }
        }
    }
}