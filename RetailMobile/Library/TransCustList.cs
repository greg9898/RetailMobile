using System.Collections.Generic;
using Android.Content;
using Com.Ianywhere.Ultralitejni12;
using System;
using Android.Util;

namespace RetailMobile.Library
{
    public class TransCustList : List<TransCust>
    {
        public class Criteria
        {
            public DateTime DateFrom;
            public DateTime DateTo;
            public int CustId;
            public string CustName;
        }

        public static TransCustList GetTransCustList(Context ctx, Criteria c)
        {
            TransCustList items = new TransCustList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {    
                string query = @"SELECT 
id,
cst_id,
vouch_id,
voser_id,
docnum,
dtrn_type,
dtrn_net_value,
dtrn_vat_value,
dtrn_date,
htrn_id 
FROM rTransCust
WHERE 1 =1 ";

                if (c.CustId > 0)
                {                  
                    query += " AND cst_id = :cst_id";
                }

                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("cst_id", c.CustId);
                
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    items.Add(TransCust.GetTransCust (result));
                }
                
                ps.Close();
                conn.Release();
            }
            
            return items;
        }

        public static TransCustList GetTransCustListStatistic(Context ctx, Criteria c)
        {
            TransCustList items = new TransCustList();
            using (IConnection conn = Sync.GetConnection(ctx))
            {   
                string dateCondition = "";

                if (c.DateFrom > DateTime.MinValue)
                {
                    dateCondition += " AND date(dtrn_date) >= :date_from ";
                }

                if (c.DateTo > DateTime.MinValue)
                {
                    dateCondition += " AND date(dtrn_date) <= :date_to ";
                }

                string query = @" 
SELECT cst_id,
       sum(case when dtrn_type = 1 " + dateCondition + @" then coalesce(dtrn_net_value,0) + coalesce(dtrn_vat_value,0) else 0 end) as credit,
       sum(case when dtrn_type = 2 " + dateCondition + @" then coalesce(dtrn_net_value,0) + coalesce(dtrn_vat_value,0) else 0 end) as debit,
       sum(case when dtrn_type = 1 then coalesce(dtrn_net_value,0) + coalesce(dtrn_vat_value,0) else 0 end) -
       sum(case when dtrn_type = 2 then coalesce(dtrn_net_value,0) + coalesce(dtrn_vat_value,0) else 0 end) as crdb,
       rcustomer.cst_desc
FROM rtranscust
JOIN rcustomer ON rcustomer.id = cst_id
WHERE 1 = 1 ";

                if (c.CustId > 0)
                {                  
                    query += " AND cst_id = :cst_id ";
                }

                if (c.CustName != "")
                {                  
                    query += " AND rcustomer.cst_desc like \'" + c.CustName + "%\'";
                }

                query += @" 
group by cst_id,
         rcustomer.cst_desc ";

                IPreparedStatement ps = conn.PrepareStatement(query);
               
                if (c.CustId > 0)
                {
                    ps.Set("cst_id", c.CustId);
                }

                if (c.DateFrom > DateTime.MinValue)
                {
                    ps.Set("date_from", c.DateFrom.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (c.DateTo > DateTime.MinValue)
                {
                    ps.Set("date_to", c.DateTo.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                }

//                Log.Debug("rtranscust", query);
                IResultSet result = ps.ExecuteQuery();

                while (result.Next())
                {
                    items.Add(TransCust.GetTransCustStat (result));
                }
               
                ps.Close();
                conn.Release();
            }

            return items;
        }
    }
}