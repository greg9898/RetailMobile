using System.Collections.Generic;

using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class CategoryList : List<Category>
    {
        public static CategoryList GetCategoryList(Context ctx, int categTbl)
        {
            CategoryList items = new CategoryList();
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
FROM " + tableName);
                
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    items.Add(new Category(){
                        Id = result.GetInt("id"),
                        ItemCategDesc = result.GetString("item_categ_desc")}
                    );
                }
                
                ps.Close();
                conn.Release();
            }
            
            return items;
        }
    }
}