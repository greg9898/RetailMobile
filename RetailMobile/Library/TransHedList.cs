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
	rtrans_hed.id, rtrans_hed.cust_id, trans_date, vouch_id, voser_id, docnum, htrn_explanation, rcustomer.cst_desc  
FROM rtrans_hed
LEFT OUTER JOIN rcustomer ON rcustomer.id = rtrans_hed.cust_id
");
				IResultSet result = ps.ExecuteQuery();
				
				while (result.Next())
				{
					TransHed header = new TransHed();
					header.Fetch(result);
					
					headers.Add(header);
				}
				
				ps.Close();
				conn.Release ();
			}
			
			return headers;
		}
	}
}