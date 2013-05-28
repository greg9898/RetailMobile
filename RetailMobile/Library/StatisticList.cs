using System.Collections.Generic;
using Android.Content;
using Com.Ianywhere.Ultralitejni12;
using Android.Util;

namespace RetailMobile.Library
{
    public class StatisticList : List<Statistic>
    {
        public static StatisticList GetStatisticListThisMonth(Context ctx, long cstId)
        {
            StatisticList items = new StatisticList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {         
                string query = @"
SELECT 
    cst_id, 
    item_kateg,
    month,
    amount_curr,
    amount_prev,
	ritem_categ.item_categ_desc
FROM rstatistic
JOIN ritem_categ ON ritem_categ.id = rstatistic.item_kateg";

                if (cstId > 0)
                {
                    query += @"
WHERE cst_id = :cstId AND month = " + System.DateTime.Now.Month;
                }

                IPreparedStatement ps = conn.PrepareStatement(query);
                
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    Statistic s = new Statistic();
                    s.Fetch(result);
                    items.Add(s);

                    Log.Debug("StatisticList", "StatisticList CstId=" + s.CstId + " Month=" + s.Month);
                } 
                
                ps.Close();
                conn.Release();
            }
            
            return items;
        }
    }
}