using System;

using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
	public class TransHed
	{
		public bool IsNew = true;

		public double HtrnId { get; set; }
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

		public double HtrnTotValue
		{
			get
			{
				return HtrnNetVal + HtrnVatVal;
			}
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
INSERT INTO trans_hed
	(	
		htrn_date,
		cst_id,
		htrn_docnum,
		user_id,
		htrn_entry_date,
        htrn_expl,
    htrn_net_val,
    htrn_vat_val
	)
VALUES
	(	
		:htrn_date,
		:cst_id,
		:htrn_docnum,
		:user_id,
		:htrn_entry_date,
        :htrn_expl,
    :htrn_net_val,
    :htrn_vat_val
)");
					//SELECT last_insert_id();
				} else
				{
					ps = conn.PrepareStatement(@"
UPDATE trans_hed SET 
htrn_date,
		cst_id = :cst_id,
		htrn_docnum = :htrn_docnum,
		user_id = :user_id,
		htrn_entry_date = :htrn_entry_date,
        htrn_expl = :htrn_expl, 
    htrn_net_val = :htrn_net_val,
    htrn_vat_val = :htrn_vat_val
WHERE htrn_id = :htrn_id");
					
					ps.Set("htrnId", HtrnId);
				}

				//ps.Set("comp_id", comp_id);
				//ps.Set("bran_id", bran_id);
				//ps.Set("store_id", store_id);
				//ps.Set("per_id", per_id);
				ps.Set("htrn_date", HtrnDate.ToString("yyyy-MM-dd HH:mm:ss"));
				ps.Set("cst_id", CstId);
				ps.Set("htrn_docnum", HtrnDocnum);
				ps.Set("user_id", UserId);
				ps.Set("htrn_entry_date", HtrnEntryDate.ToString("yyyy-MM-dd HH:mm:ss"));
				ps.Set("htrn_expl", HtrnExpl);
				ps.Set("htrn_net_val", HtrnNetVal);
				ps.Set("htrn_vat_val", HtrnVatVal);
             
				if (IsNew)
				{
					ps.Execute();

					ps = conn.PrepareStatement(@"SELECT TOP 1 htrn_id FROM trans_hed ORDER BY htrn_id DESC");

					IResultSet rs = ps.ExecuteQuery();
					if (rs.Next())
					{
						HtrnId = rs.GetDouble("htrn_id");
					}
				} else
				{
					ps.Execute();
				}

				ps.Close();

				conn.Commit();
				conn.Release();
			}

			if (TransDetList != null)
			{
				foreach (var detail in TransDetList)
				{
					detail.Save(ctx, this);
				}
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
    htrn_id,    
    cst_id,
    htrn_docnum,
    user_id,
    htrn_expl,
    htrn_net_val,
    htrn_vat_val,
    htrn_date 
FROM trans_hed WHERE htrn_id = :htrnId");
				ps.Set("htrnId", htrnId);

				IResultSet result = ps.ExecuteQuery();

				if (result.Next())
				{
					info.Fetch(result);
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
			HtrnId = result.GetDouble("htrn_id");
			CstId = result.GetDouble("cst_id");
			HtrnExpl = result.GetString("htrn_expl");
			HtrnDocnum = result.GetInt("htrn_docnum");
			UserId = result.GetDouble("user_id");
			HtrnNetVal = result.GetDouble("htrn_net_val");
			HtrnVatVal = result.GetDouble("htrn_vat_val");
			;
			HtrnDate = Sync.JavaDateToDatetime(result.GetDate("htrn_date"));

			IsNew = false;
		}
	}
}