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
using Android.Util;

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
    htrn_Id, 
    trans_hed.cst_Id, 
    htrn_docnum, 
    htrn_net_val, 
    htrn_vat_val, 
    htrn_date,
    customers.cst_desc  
FROM trans_hed
LEFT OUTER JOIN customers ON customers.cst_id = trans_hed.cst_id ");
                IResultSet result = ps.ExecuteQuery();

                while (result.Next())
                {
                    TransHed header = new TransHed();
                    header.FetchInfo(result);

                    headers.Add(header);
                }

                ps.Close();
                conn.Release();
            }

            return headers;
        }
    }
}