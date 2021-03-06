using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Widget;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class ItemInfoList : List<ItemInfo>
    {
        public Criteria CurrentCriteria = new Criteria();
        int lastLoadedID = 0;

        public ItemInfoList()
        {
            lastLoadedID = 0;
        }

        public static ItemInfoList GetItemInfoList(Context ctx)
        {
            return GetItemInfoList(ctx, new Criteria());
        }

        public void LoadItems(Context ctx)
        {
            Criteria c = CurrentCriteria;
            using (IConnection conn = Sync.GetConnection(ctx))
            {
//				IPreparedStatement ps1 = conn.PrepareStatement ("select * from ritemlast");
//							
//				IResultSet result1 = ps1.ExecuteQuery ();
//				
//				while (result1.Next()) {
//					Log.Debug ("", result1.GetInt (0) + " " + result1.GetInt (1));
//				}
			
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
SELECT TOP 30
    ritems.ID, 
    ritems.item_cod, 
    ritems.item_desc,
    ritems.item_image,
    ritems.item_qty_left " +
                    fields + @" 
FROM ritems" +
                    joinLastDate + @" 
WHERE 1 = 1  ";
				
                if (c.ItemDesc != "")
                {
//					query += " AND ritems.item_desc like \'" + c.ItemDesc + "%\'";
                    query += " AND ritems.item_desc like :ItemDesc ";
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
                if (c.ItemDesc != "")
                    ps.Set("ItemDesc", c.ItemDesc);
				
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

                    byte[] signatureBytes = result.GetBytes("item_image");
                    try
                    {
                        if (signatureBytes.Length > 0)
                        {
                            Android.Graphics.Bitmap img = Android.Graphics.BitmapFactory.DecodeByteArray(signatureBytes,
                            0, signatureBytes.Length);
                            item.ItemImage = Android.Graphics.Bitmap.CreateScaledBitmap(img,64,64,true);
                            img.Recycle();
                            img = null;
                        }
                        else
                        {
                            item.ItemImage = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        item.ItemImage = null;
                    }
					
                    if (c.CstId > 0)
                    {
                        item.ItemLastBuyDate = Common.JavaDateToDatetime(result.GetDate ("last_date"));
                    }

                    lastLoadedID = item.ItemId;
                    Add(item);
                }
                result.Close();
                ps.Close();
                conn.Release();
            }
        }

        public static void LoadAdapterItems(Context ctx, int page, ArrayAdapter<ItemInfo> adapter, Criteria c)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                string joinLastDate = "";
                string fields = "";
				
                if (c.CstId > 0)
                {
                    fields += @",
    ritemlast.last_date";//OUTER
                    joinLastDate = @"
LEFT JOIN ritemlast ON ritemlast.item_id = ritems.id AND ritemlast.cst_id = " + c.CstId;
                }
                int offset = 1 + page * 30;
                string query = @"
SELECT TOP 30 START AT " + offset + @" 
    ritems.ID, 
    ritems.item_cod, 
    ritems.item_desc,
    ritems.item_image,
    ritems.item_qty_left " +
                    fields + @" 
FROM ritems" +
                    joinLastDate + @" 
WHERE 1 = 1  ";
				
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
                Log.Debug("select items", query);
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
					
                    byte[] signatureBytes = result.GetBytes("item_image");
                    try
                    {
                        if (signatureBytes.Length > 0)
                        {
                            Android.Graphics.Bitmap img = Android.Graphics.BitmapFactory.DecodeByteArray(signatureBytes,
                                                                                                         0, signatureBytes.Length);
                            item.ItemImage = Android.Graphics.Bitmap.CreateScaledBitmap(img,64,64,true);
                            img.Recycle();
                            img = null;

                        }
                        else
                        {
                            item.ItemImage = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        item.ItemImage = null;
                    }

                    if (c.CstId > 0)
                    {
                        item.ItemLastBuyDate = Common.JavaDateToDatetime(result.GetDate ("last_date"));
                    }
					
                    adapter.Add(item);
                }
				
                result.Close();
                ps.Close();
                conn.Release();
            }
        }

        public static ItemInfoList GetItemInfoList(Context ctx, Criteria c)
        {
            ItemInfoList items = new ItemInfoList();
            items.CurrentCriteria = c;
            items.LoadItems(ctx);
            return items;
        }

        public class Criteria
        {
            public string ItemDesc = "";
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