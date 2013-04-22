using System.Collections.Generic;

using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class StatisticList : List<Statistic>
    {
        public static StatisticList GetStatisticList(Context ctx, int cstId)
        {
            StatisticList items = new StatisticList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {                 
                IPreparedStatement ps = conn.PrepareStatement(@"
SELECT 
    cst_id, 
    item_kateg,
    month,
    amount_curr,
    amount_prev
FROM rstatistic 
WHERE cst_id = :cstId");
                
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    Statistic s = new Statistic();
                    s.Fetch(result);
                    items.Add(s);
                }
                
                ps.Close();
                conn.Release();
            }
            
            return items;
        }
    }
}