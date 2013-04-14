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
	htrn_Id, 
	cst_Id, 
	user_id, 
	htrn_docnum, 
	htrn_expl, 
	htrn_net_val, 
	htrn_vat_val, 
	htrn_date  
FROM trans_hed");
				IResultSet result = ps.ExecuteQuery();

				while (result.Next())
				{
					TransHed header = new TransHed();
					header.Fetch(result);

					headers.Add(header);
				}

				ps.Close();
			}

			return headers;
		}
	}
}