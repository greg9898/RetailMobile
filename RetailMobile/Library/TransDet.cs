
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

        public long ItemId { get; set; }

        public double DtrnQty1 { get; set; }

        public double DtrnUnitPrice { get; set; }

        public double DtrnDiscLine1 { get; set; }

        public double DtrnDiscLine2 { get; set; }

        public double DtrnNetValue { get; set; }

        public double DtrnVatValue { get; set; }

        public double ItemVatValue { get; set; }

        public int ItemVatId { get; set; }

        public void LoadItemInfo(Context ctx, decimal itemId, int qty, double cstId)
        {
            Log.Debug("LoadItemInfo", "itemID=" + itemId + ",qty=" + qty);

            ItemInfo item = ItemInfo.GetItem(ctx, itemId);

            ItemId = (int)item.ItemId;
            ItemCode = item.item_cod;
            ItemDesc = item.item_desc;
            DtrnQty1 = qty;

//            DtrnUnitPrice = GetItemPrice(ctx, ItemId);
//            ItemVatId = GetItemVatId(ctx, ItemId);
            DtrnUnitPrice = (double)item.item_sale_val1;
            ItemVatId = item.ItemVatId;

            switch (ItemVatId)
            {
                case 1:
                    ItemVatValue = 0;
                    break;
                case 2:
                    ItemVatValue = 13;
                    break;
                case 3:
                    ItemVatValue = 23;
                    break;
            }
        
            CalcValues(ctx, cstId);
        }

        void CalcValues(Context ctx, double cstId)
        {
            double discValue1 = 0;
            double discValue2 = 0;
            if (cstId > 0)
            {
//                DtrnDiscLine1 = GetDiscount(ctx, ItemId, (long)cstId);
//                DtrnDiscLine2 = GetDiscount2(ctx, ItemId, (long)cstId);
                if (DtrnDiscLine1 > 0)
                {
                    discValue1 = System.Math.Round((DtrnQty1 * DtrnUnitPrice) * (DtrnDiscLine1 / 100), 2);
                }
                if (DtrnDiscLine2 > 0)
                {
                    discValue2 = System.Math.Round(((1 * DtrnUnitPrice) - discValue1) * (DtrnDiscLine2 / 100), 2);
                }
            } else
            {
                DtrnDiscLine1 = 0;
                DtrnDiscLine2 = 0;
            }
            DtrnNetValue = System.Math.Round(((DtrnQty1 * DtrnUnitPrice) - discValue1) - discValue2, 2);
            DtrnVatValue = System.Math.Round((DtrnNetValue) * ((100 + ItemVatValue) / 100) - DtrnNetValue, 2);
        }

        public void Save(IConnection conn, TransHed header)
        {
            CustomerInfo info = new CustomerInfo();

            IPreparedStatement ps;
            if (IsNew)
            {
                ps = conn.PrepareStatement(@"INSERT INTO trans_det
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
            } else
            {
                ps = conn.PrepareStatement(@"UPDATE trans_det SET 
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

            ps.Set("htrn_id", header.HtrnId);
            ps.Set("dtrn_num", DtrnNum);
            ps.Set("item_id", ItemId);
            ps.Set("dtrn_unit_price", DtrnUnitPrice);
            ps.Set("dtrn_disc_line1", DtrnDiscLine1);
            ps.Set("dtrn_net_value", DtrnNetValue);
            ps.Set("dtrn_vat_value", DtrnVatValue);

            if (IsNew)
            {
                ps.Execute();
                ps = conn.PrepareStatement(@"SELECT TOP 1 dtrn_id FROM trans_det ORDER BY dtrn_id DESC");

                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    DtrnId = rs.GetDouble("dtrn_id");
                }
            } else
            {
                ps.Execute();
            }

            ps.Close();

        }

        public double GetDiscount(Context ctx, long itemId, long cstId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                const string query = @"
select disc_per from disc 
where cst_kat_disc = (select cst_kat_disc from customer where  id = :cst_id)
  and item_ctg_disc = (select item_ctg_disc from items where id = :item_id) ";

                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("item_id", itemId);
                ps.Set("cst_id", cstId);
                double discount = 0;

                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    discount = rs.GetDouble("disc_per");
                }
            
                ps.Close();
                conn.Commit();
                conn.Release();

                return discount;
            }
        }

        public double GetDiscount2(Context ctx, long itemId, long cstId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                const string query = @"
select disc_per2 from disc 
where cst_kat_disc = (select cst_kat_disc from customer where  id = :cst_id)
  and item_ctg_disc = (select item_ctg_disc from items where id = :item_id) ";
            
                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("item_id", itemId);
                ps.Set("cst_id", cstId);
                double discount = 0;
            
                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    discount = rs.GetDouble("disc_per2");
                }
            
                ps.Close();
                conn.Commit();
                conn.Release();
            
                return discount;
            }
        }

        public double GetItemPrice(Context ctx, long itemId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                IPreparedStatement ps = conn.PrepareStatement("select item_sale_val1 from items where item_id = :item_id ");//unit_price ?
                ps.Set("item_id", itemId);
                double unit_price = 0;
            
                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    unit_price = rs.GetDouble("item_sale_val1");
                }
            
                ps.Close();
                conn.Commit();
                conn.Release();
                
                return unit_price;
            }
        }
        
        public int GetItemVatId(Context ctx, long itemId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                const string query = @"select vat_id from items where item_id = :item_id";
            
                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("item_id", itemId);
                int vatId = 0;
            
                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    vatId = (int)rs.GetDouble("vat_id");
                }
            
                ps.Close();
                conn.Commit();
                conn.Release();
            
                return vatId;
            }
        }
    }
}