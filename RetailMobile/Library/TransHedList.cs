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
    public class TransHedList : List<TransHed>
    {
        internal static TransHedList GetTransHedList(Context ctx)
        {   
            TransHedList headers = new TransHedList();
            
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                IPreparedStatement ps = conn.PrepareStatement(@"
SELECT 
    rtrans_hed.id, rtrans_hed.cust_id, trans_date, docnum, htrn_explanation,
    rcustomer.cst_desc  
FROM rtrans_hed
LEFT OUTER JOIN rcustomer ON rcustomer.id = rtrans_hed.cust_id ");
                IResultSet result = ps.ExecuteQuery();
                
                while (result.Next())
                {
                    TransHed header = new TransHed();
                    header.Fetch(result);
                    
                    headers.Add(header);
                }

                result.Close();
                ps.Close();

                foreach (TransHed h in headers)
                {
                    TransHed.FetchDetails(h, conn);
                }

                conn.Release();
            }
            
            return headers;
        }

        public bool Contains(int id)
        {
            foreach (TransHed h in this)
            {
                if (h.HtrnId == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}