using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class Statistic
    {
        #region Properties 

        public int CstId { get; set; }

        public int ItemKateg { get; set; }

        public int  Month { get; set; }

        public double  AmountCurr { get; set; }

        public double  AmountPrev { get; set; }

        #endregion
        
        public Statistic()
        {
        }

        internal void Fetch(IResultSet result)
        {
            CstId = result.GetInt("cst_id");
            ItemKateg = result.GetInt("item_kateg");
            Month = result.GetInt("month");
            AmountCurr = result.GetDouble("amount_curr");
            AmountPrev = result.GetDouble("amount_prev");
        }
        
        public static Statistic GetStatistic(Context ctx, int cstId)
        {
            Statistic info = new Statistic();
          
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
                ps.Set("cstId", cstId);
                
                IResultSet result = ps.ExecuteQuery();
                
                if (result.Next())
                {
                    info.Fetch(result);
                }
                
                ps.Close();
                conn.Release();
            }
            
            return info;
        }
    }
}