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
		
		public DateTime HtrnDate { get; set; }
		
		public double CstId { get; set; }
		
		public int HtrnDocnum { get; set; }
		
		public double UserId { get; set; }
		/// <summary>
		/// no taxes
		/// </summary>
		public double HtrnNetVal { get; set; }
		/// <summary>
		/// value added tax
		/// </summary>
		public double HtrnVatVal { get; set; }
		
		public DateTime HtrnEntryDate { get; set; }
		
		public string HtrnExpl { get; set; }
		
		public TransDetList TransDetList { get; set; }
		
		public string CstName{get;set;}
		
		public void CalcValues()
		{
			HtrnNetVal = 0;
			HtrnVatVal = 0;
			
			if (TransDetList == null)
			{
				return;
			}
			
			foreach (TransDet d in TransDetList)
			{
				HtrnNetVal += d.DtrnNetValue;
				HtrnVatVal += d.DtrnVatValue;
			}
		}
		
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
			HtrnDate = DateTime.Now;
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
				ps.Set("htrn_date", HtrnDate.ToString("yyyy-MM-dd HH:mm:ss"));
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
		
		public static TransHed GetTransHed(Context ctx, double htrnId)
		{
			TransHed info = new TransHed();
			
			if (htrnId == 0)
			{
				return info;
			}
			
			using (IConnection conn = Sync.GetConnection(ctx))
			{
				IPreparedStatement ps = conn.PrepareStatement(@"
SELECT 
    id, cust_id, trans_date, vouch_id, voser_id, docnum, htrn_explanation
FROM rtrans_hed WHERE id = :htrnId");
				ps.Set("htrnId", htrnId);
				
				IResultSet result = ps.ExecuteQuery();
				
				if (result.Next())
				{
					info.Fetch(result);
				}
				
				result.Close();
				
				ps = conn.PrepareStatement(@"SELECT rtrans_det.id, rtrans_det.htrn_id, rtrans_det.dtrn_num, rtrans_det.item_id, ritems.item_cod, 
							ritems.item_desc, rtrans_det.qty1, rtrans_det.unit_price, rtrans_det.disc_line1, rtrans_det.net_value, rtrans_det.vat_value
                            FROM rtrans_det 
                            JOIN ritems ON ritems.id = rtrans_det.item_id
                            WHERE htrn_id = :htrn_id
");
				ps.Set("htrn_id", htrnId);
				
				result = ps.ExecuteQuery();
				
				if (info.TransDetList == null)
					info.TransDetList = new TransDetList();
				
				while (result.Next())
				{
					TransDet det = new TransDet();
					det.DtrnId = result.GetInt("id");
					det.HtrnId = result.GetInt("htrn_id");
					det.ItemId = result.GetInt("item_id");
					det.ItemCode = result.GetString("item_cod");
					det.ItemDesc = result.GetString("item_desc");
					det.DtrnUnitPrice = result.GetDouble("unit_price");
					det.DtrnQty1 = result.GetDouble("qty1");
					det.DtrnNetValue = result.GetDouble("net_value");
					det.DtrnVatValue = result.GetDouble("vat_value");
					info.TransDetList.Add(det);
				}
				result.Close();
				ps.Close();
				conn.Commit();
				conn.Release();
			}
			
			return info;
		}
		
		public void Fetch(IResultSet result)
		{
			HtrnId = result.GetInt("id");
			CstId = result.GetInt("cust_id");
			HtrnExpl = result.GetString("htrn_explanation");
			HtrnDocnum = result.GetInt("docnum");
			HtrnDate = Sync.JavaDateToDatetime(result.GetDate("trans_date"));
			
			IsNew = false;
		}
		
		public void FetchInfo(IResultSet result)
		{
			HtrnId = result.GetInt("htrn_id");
			CstId = result.GetInt("cst_id");
			HtrnDocnum = result.GetInt("htrn_docnum");
			//HtrnNetVal = result.GetDouble("htrn_net_val");
			//HtrnVatVal = result.GetDouble("htrn_vat_val");
			
			HtrnDate = Sync.JavaDateToDatetime(result.GetDate("htrn_date"));
			CstName = result.GetString("cst_desc");
			
			IsNew = false;
		}
	}
}