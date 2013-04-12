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
			TransHed header1 = new TransHed();
			header1.HtrnId = 1;
			header1.CstId = 2;
			header1.HtrnDocnum = 45;
			header1.HtrnExpl = "zaiko";
			header1.HtrnDate = new DateTime(2111, 12, 22);
			header1.HtrnEntryDate = new DateTime(2012, 11, 23);
			header1.UserId = 4;
			header1.Save(ctx);
			
			TransHed header2 = new TransHed();
			header2.HtrnId = 2;
			header2.CstId = 33;
			header2.HtrnDocnum = 451;
			header2.HtrnExpl = " na zaiko mucunkata";
			header2.UserId = 4;
			header2.HtrnDate = new DateTime(2011, 12, 3);
			header2.HtrnEntryDate = new DateTime(2012, 11, 13);
			header2.HtrnNetVal = 100;
			header2.HtrnVatVal = 12.13;
			header2.Save(ctx);
			
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