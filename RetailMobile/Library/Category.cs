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
    public class Category
    {
        public int Id { get; set; }
        
        public string ItemCategDesc{ get; set; }

        public Category()
        {
        }
        
        public static Category GetCategory(Context ctx, long categId, int categTbl)
        {
            Category info = new Category();
            
            using (IConnection conn = Sync.GetConnection(ctx))
            {               
                string tableName = "ritem_categ";
               
                if (categTbl == 2)
                {
                    tableName += "2";
                }

                IPreparedStatement ps = conn.PrepareStatement(@"SELECT 
id, 
item_categ_desc
FROM " + tableName + " WHERE id = :CategId");
                ps.Set("categId", categId);
                
                IResultSet result = ps.ExecuteQuery();
                
                if (result.Next())
                {
                    info.Id = result.GetInt("id");
                    info.ItemCategDesc = result.GetString("item_categ_desc");
                }

                result.Close();
                ps.Close();
                conn.Release();
            }
            
            return info;
        }
    }
}