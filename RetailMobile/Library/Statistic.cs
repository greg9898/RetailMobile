using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class Statistic
    {
        #region Properties 

        public int CstId { get; set; }

        public int ItemKateg { get; set; }

        public string ItemKategDesc { get; set; }

        public int  Month { get; set; }

        public double  AmountCurr { get; set; }

        public double  AmountPrev { get; set; }

        public string AmountCurrText
        { 
            get
            {
                return AmountCurr.ToString(Common.CurrencyFormat);
            }
        }

        public string AmountPrevText
        { 
            get
            {
                return AmountPrev.ToString(Common.CurrencyFormat);
            }
        }
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
            ItemKategDesc = result.GetString("item_categ_desc");
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
    amount_prev,
	ritem_categ.item_categ_desc
FROM rstatistic
JOIN ritem_categ ON ritem_categ.id = rstatistic.item_kateg
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