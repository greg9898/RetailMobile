
using Android.Content;
using Com.Ianywhere.Ultralitejni12;
using Android.Util;

namespace RetailMobile.Library
{
	public class TransDet
	{
		public bool IsNew = true;

		public string ItemCode { get; set; }
		public string ItemDesc { get; set; }

		public double DtrnId { get; set; }
		public double HtrnId { get; set; }
		public int DtrnNum { get; set; }
		public double ItemId { get; set; }
		public double DtrnQty1 { get; set; }
		public double dtrn_unit_price { get; set; }
		public double dtrn_disc_line1 { get; set; }
		public double dtrn_net_value { get; set; }
		public double dtrn_vat_value { get; set; }

		public void LoadItemInfo (Context ctx, decimal itemID, int qty)
		{
			Log.Debug ("LoadItemInfo", "itemID=" + itemID + ",qty=" + qty);

			ItemInfo item = ItemInfo.GetItem (ctx, itemID);

			ItemId = (int)item.item_id;
			ItemCode = item.item_cod;
			ItemDesc = item.item_desc;
			DtrnQty1 = qty;
		}

		public void Save (IConnection conn, TransHed header)
		{
			CustomerInfo info = new CustomerInfo ();


				IPreparedStatement ps;
				if (IsNew) {
					ps = conn.PrepareStatement (@"INSERT INTO trans_det
	(	
		htrn_id
           ,dtrn_num
           ,item_id
           ,dtrn_qty1
           ,dtrn_unit_price
           ,dtrn_disc_line1
           ,dtrn_net_value
           ,dtrn_vat_value
	)
VALUES
	(	
		:htrn_id
           ,:dtrn_num
           ,:item_id
           ,:dtrn_qty1
           ,:dtrn_unit_price
           ,:dtrn_disc_line1
           ,:dtrn_net_value
           ,:dtrn_vat_value
)");
				} else {
					ps = conn.PrepareStatement (@"UPDATE trans_det SET 
            htrn_id = :htrn_id
           ,dtrn_num = :dtrn_num
           ,item_id = :item_id
           ,dtrn_qty1 = :dtrn_qty1
           ,dtrn_unit_price = :dtrn_unit_price
           ,dtrn_disc_line1 = :dtrn_disc_line1
           ,dtrn_net_value = :dtrn_net_value
           ,dtrn_vat_value = :dtrn_vat_value
WHERE dtrn_id = :dtrn_id
");
                    ps.Set("dtrn_vat_value", DtrnId);
				}

				ps.Set ("htrn_id", header.HtrnId);
				ps.Set ("dtrn_num", DtrnNum);
				ps.Set ("item_id", ItemId);
				ps.Set ("dtrn_unit_price", dtrn_unit_price);
				ps.Set ("dtrn_disc_line1", dtrn_disc_line1);
				ps.Set ("dtrn_net_value", dtrn_net_value);
				ps.Set ("dtrn_vat_value", dtrn_vat_value);

				if (IsNew) {
					ps.Execute ();
                    ps = conn.PrepareStatement(@"SELECT TOP 1 dtrn_id FROM trans_det ORDER BY dtrn_id DESC");

                    IResultSet rs = ps.ExecuteQuery();
                    if (rs.Next())
                    {
                        DtrnId = rs.GetDouble("dtrn_id");
                    }
				} else {
					ps.Execute ();
				}

				ps.Close ();

		}
	}
}