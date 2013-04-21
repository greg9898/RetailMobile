using System;

using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
	public class TransHed
	{
		public bool IsNew = true;
		
		public int HtrnId { get; set; }
		//public int comp_id { get; set; }
		//public int bran_id { get; set; }
		//public int store_id { get; set; }
		//public int per_id { get; set; }
		
		public DateTime TransDate { get; set; }
		
		public long CstId { get; set; }
		
		public int HtrnDocnum { get; set; }
		
		public double UserId { get; set; }
		/// <summary>
		/// no taxes
		/// </summary>
		public double HtrnNetVal
		{
			get
			{
				if (TransDetList == null)
				{
					return 0;
				}
				
				double htrnNetVal = 0;
				
				foreach (TransDet d in TransDetList)
				{
					htrnNetVal += d.DtrnNetValue;
				}
				
				return htrnNetVal;
			}
		}
		/// <summary>
		/// value added tax
		/// </summary>
		public double HtrnVatVal
		{
			get
			{
				if (TransDetList == null)
				{
					return 0;
				}
				
				double htrnVatVal = 0;
				
				foreach (TransDet d in TransDetList)
				{
					htrnVatVal += d.DtrnVatValue;
				}
				return htrnVatVal;
			}
		}
		
		public DateTime HtrnEntryDate { get; set; }
		
		public string HtrnExpl { get; set; }
		
		public TransDetList TransDetList { get; set; }
		
		public string CstName{ get; set; }
		
		#region Customer Info
		public string CustCod;
		public string CustName;
		#endregion
		
		public double HtrnTotValue
		{
			get
			{
				return HtrnNetVal + HtrnVatVal;
			}
		}
		
		public TransHed()
		{
			TransDetList = new Library.TransDetList();
			TransDate = DateTime.Now;
			HtrnEntryDate = DateTime.Now;
		}
		
		public void Save(Context ctx)
		{
			CustomerInfo info = new CustomerInfo();
			
			using (IConnection conn = Sync.GetConnection(ctx))
			{
				IPreparedStatement ps;
				if (IsNew)
				{
					ps = conn.PrepareStatement(@"
INSERT INTO rtrans_hed
(
cust_id, trans_date, vouch_id, voser_id, docnum, htrn_explanation
)
VALUES
(
:cst_id,
:htrn_date,
:vouch_id,
:voser_id,
:htrn_docnum,
:htrn_expl
)");
					//SELECT last_insert_id();
				} else
				{
					ps = conn.PrepareStatement(@"
UPDATE rtrans_hed SET
cust_id = :cst_id,
trans_date = :htrn_date,
vouch_id = :vouch_id,
voser_id = :voser_id,
docnum = :htrn_docnum,
htrn_explanation = :htrn_expl
WHERE id = :htrn_id");
					
					ps.Set("htrnId", HtrnId);
				}
				
				//ps.Set("comp_id", comp_id);
				//ps.Set("bran_id", bran_id);
				//ps.Set("store_id", store_id);
				//ps.Set("per_id", per_id);
				ps.Set("htrn_date", TransDate.ToString("yyyy-MM-dd HH:mm:ss"));
				ps.Set("cst_id", CstId);
				ps.Set("vouch_id", 1);
				ps.Set("voser_id", 1);
				ps.Set("htrn_docnum", HtrnDocnum);
				//ps.Set("user_id", UserId);
				//ps.Set("htrn_entry_date", HtrnEntryDate.ToString("yyyy-MM-dd HH:mm:ss"));
				ps.Set("htrn_expl", HtrnExpl);
				//ps.Set("htrn_net_val", HtrnNetVal);
				//ps.Set("htrn_vat_val", HtrnVatVal);
				
				if (IsNew)
				{
					ps.Execute();
					
					ps = conn.PrepareStatement(@"SELECT TOP 1 id FROM rtrans_hed ORDER BY id DESC");
					
					IResultSet rs = ps.ExecuteQuery();
					if (rs.Next())
					{
						HtrnId = rs.GetInt("id");
					}
				} else
				{
					ps.Execute();
				}
				
				ps.Close();
				
				if (TransDetList != null)
				{
					foreach (var detail in TransDetList)
					{
						detail.Save(conn, this);
					}
				}
				
				conn.Commit();
				conn.Release();
			}
		}
		
		internal static void FetchDetails(TransHed transHed, IConnection conn)
		{
			IPreparedStatement ps = conn.PrepareStatement(@"
SELECT rtrans_det.id, rtrans_det.htrn_id, rtrans_det.dtrn_num, rtrans_det.item_id,
ritems.item_cod, ritems.item_desc,
rtrans_det.qty1, rtrans_det.unit_price, rtrans_det.disc_line1, rtrans_det.net_value, rtrans_det.vat_value
FROM rtrans_det
JOIN ritems ON ritems.id = rtrans_det.item_id
WHERE htrn_id = :htrn_id ");
			ps.Set("htrn_id", transHed.HtrnId);
			
			IResultSet result = ps.ExecuteQuery();
			
			if (transHed.TransDetList == null)
			{
				transHed.TransDetList = new TransDetList();
			}
			
			while (result.Next())
			{
				TransDet d = new TransDet();
				d.Fetch(result);
				transHed.TransDetList.Add(d);
			}
			
			
			result.Close();
			ps.Close();
		}
		
		public static TransHed GetTransHed(Context ctx, double htrnId)
		{
			TransHed transHed = new TransHed();
			
			if (htrnId == 0)
			{
				return transHed;
			}
			
			using (IConnection conn = Sync.GetConnection(ctx))
			{
				string query = @"
SELECT
rtrans_hed.id, cust_id, trans_date, vouch_id, voser_id, docnum, htrn_explanation,
rcustomer.cst_desc
FROM rtrans_hed
LEFT OUTER JOIN rcustomer ON rcustomer.id = rtrans_hed.cust_id
WHERE rtrans_hed.id = :htrnId ";
				
				IPreparedStatement ps = conn.PrepareStatement(query);
				ps.Set("htrnId", htrnId);
				
				IResultSet result = ps.ExecuteQuery();
				
				if (result.Next())
				{
					transHed.Fetch(result);
				}
				
				result.Close();
				ps.Close();
				
				FetchDetails(transHed, conn);
				
				conn.Commit();
				conn.Release();
			}
			
			return transHed;
		}
		
		public void Fetch(IResultSet result)
		{
			HtrnId = result.GetInt("id");
			CstId = result.GetInt("cust_id");
			HtrnExpl = result.GetString("htrn_explanation");
			HtrnDocnum = result.GetInt("docnum");
			TransDate = Common.JavaDateToDatetime(result.GetDate("trans_date"));
			CstName = result.GetString("cst_desc");
			
			IsNew = false;
		}
	}
}