using Android.Content;
using Com.Ianywhere.Ultralitejni12;
using Android.Util;

namespace RetailMobile.Library
{
    public class TransDet
    {

        double dtrnQty1;
        double dtrnDiscLine1;
        double dtrnDiscLine2;
        public bool IsNew = true;
        public bool IsDeleted = false;

        public string ItemCode { get; set; }

        public string ItemDesc { get; set; }

        public int DtrnId { get; set; }

        public int HtrnId { get; set; }

        public int DtrnNum { get; set; }

        public int ItemId { get; set; }

        public double DtrnQty1
        {
            get
            {
                return dtrnQty1;
            }
            set
            {
                dtrnQty1 = value;
                CalcValues();
            }
        }

        public double DtrnUnitPrice { get; set; }

        public double DtrnDiscLine1
        {
            get
            {
                return dtrnDiscLine1;
            }
            set
            {
                dtrnDiscLine1 = value;

                if (dtrnDiscLine1 > 0)
                {
                    dtrnDiscValue1 = System.Math.Round((DtrnQty1 * DtrnUnitPrice) * (dtrnDiscLine1 / 100), 2);
                }
                
                CalcValues();
            }
        }

        public double DtrnDiscLine2
        {
            get
            {
                return dtrnDiscLine2;
            }
            set
            {
                dtrnDiscLine2 = value;

                if (dtrnDiscLine2 > 0)
                {
                    dtrnDiscValue2 = System.Math.Round(((1 * DtrnUnitPrice) - dtrnDiscValue1) * (dtrnDiscLine2 / 100), 2);
                }
                
                CalcValues();
            }
        }

        public double DtrnNetValue { get; set; }

        public double DtrnVatValue { get; set; }

        public double ItemVatValue
        { 
            get
            {           
                switch (ItemVatId)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 13;
                    case 3:
                        return 23;
                } 

                return 0;
            }
        }

        public int ItemVatId { get; set; }

        public void LoadItemInfo(Context ctx, decimal itemId, double qty, double cstId)
        {
            Log.Debug("LoadItemInfo", "itemID=" + itemId + ",qty=" + qty);
            
            ItemInfo item = ItemInfo.GetItem(ctx, itemId, false);
            
            ItemId = item.ItemId;
            ItemCode = item.item_cod;
            ItemDesc = item.ItemDesc;
            dtrnQty1 = qty;
            
            //            DtrnUnitPrice = GetItemPrice(ctx, ItemId);
            //            ItemVatId = GetItemVatId(ctx, ItemId);
            DtrnUnitPrice = (double)item.ItemSaleVal1;
            ItemVatId = item.ItemVatId;
                       
            CalcDiscount(ctx, cstId);
        }

        double dtrnDiscValue1;
        double dtrnDiscValue2;

        internal void CalcDiscount(Context ctx, double cstId)
        {
            dtrnDiscValue1 = 0;
            dtrnDiscValue2 = 0;

            if (cstId > 0)
            {
                dtrnDiscLine1 = GetDiscount(ctx, ItemId, (long)cstId);
                dtrnDiscLine2 = GetDiscount2(ctx, ItemId, (long)cstId);

                if (dtrnDiscLine1 > 0)
                {
                    dtrnDiscValue1 = System.Math.Round((DtrnQty1 * DtrnUnitPrice) * (dtrnDiscLine1 / 100), 2);
                }
                if (dtrnDiscLine2 > 0)
                {
                    dtrnDiscValue2 = System.Math.Round(((1 * DtrnUnitPrice) - dtrnDiscValue1) * (dtrnDiscLine2 / 100), 2);
                }
            }
            else
            {
                dtrnDiscLine1 = 0;
                dtrnDiscLine2 = 0;
            }
            
            CalcValues();
        }

        void CalcValues()
        {
            DtrnNetValue = System.Math.Round(((DtrnQty1 * DtrnUnitPrice) - dtrnDiscValue1) - dtrnDiscValue2, 2);
            DtrnVatValue = System.Math.Round((DtrnNetValue) * ((100 + ItemVatValue) / 100) - DtrnNetValue, 2);
        }

        public void Fetch(IResultSet result)
        {
            DtrnId = result.GetInt("id");
            HtrnId = result.GetInt("htrn_id");
            ItemId = result.GetInt("item_id");
            ItemCode = result.GetString("item_cod");
            ItemDesc = result.GetString("item_desc");
            DtrnUnitPrice = result.GetDouble("unit_price");
            dtrnQty1 = result.GetDouble("qty1");
            dtrnDiscLine1 = result.GetDouble("disc_line1");
            DtrnNetValue = result.GetDouble("net_value");
            DtrnVatValue = result.GetDouble("vat_value");
            ItemVatId = result.GetInt("item_vat");
            DtrnNum = result.GetInt("dtrn_num");

            IsNew = false;
        }

        public void Save(IConnection conn, TransHed header)
        {
            CustomerInfo info = new CustomerInfo();
            
            IPreparedStatement ps;

            if (IsNew)
            {
                if (IsDeleted)
                {
                    return;
                }

                ps = conn.PrepareStatement(@"INSERT INTO rtrans_det
    (   
        htrn_id, dtrn_num, item_id, qty1, unit_price, disc_line1, net_value, vat_value
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
       ,:dtrn_vat_value )");

                ps.Set("htrn_id", header.HtrnId);
                ps.Set("dtrn_num", DtrnNum);
                ps.Set("item_id", ItemId);
                ps.Set("dtrn_qty1", DtrnQty1);
                ps.Set("dtrn_unit_price", DtrnUnitPrice);
                ps.Set("dtrn_disc_line1", DtrnDiscLine1);
                ps.Set("dtrn_net_value", DtrnNetValue);
                ps.Set("dtrn_vat_value", DtrnVatValue);

                ps.Execute();

                ps = conn.PrepareStatement(@"SELECT TOP 1 id FROM rtrans_det ORDER BY id DESC");

                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    DtrnId = rs.GetInt("id");
                }
            }
            else if (IsDeleted)
            {
                ps = conn.PrepareStatement(@"
DELETE FROM rtrans_det WHERE id = :dtrn_id ");
                ps.Set("dtrn_id", DtrnId);
                ps.Execute();
            }
            else
            {
                ps = conn.PrepareStatement(@"
UPDATE rtrans_det SET 
            htrn_id = :htrn_id
           ,dtrn_num = :dtrn_num
           ,item_id = :item_id
           ,qty1 = :dtrn_qty1
           ,unit_price = :dtrn_unit_price
           ,disc_line1 = :dtrn_disc_line1
           ,net_value = :dtrn_net_value
           ,vat_value = :dtrn_vat_value
WHERE id = :dtrn_id ");

                ps.Set("dtrn_id", DtrnId);
                ps.Set("htrn_id", header.HtrnId);
                ps.Set("dtrn_num", DtrnNum);
                ps.Set("item_id", ItemId);
                ps.Set("dtrn_qty1", DtrnQty1);
                ps.Set("dtrn_unit_price", DtrnUnitPrice);
                ps.Set("dtrn_disc_line1", DtrnDiscLine1);
                ps.Set("dtrn_net_value", DtrnNetValue);
                ps.Set("dtrn_vat_value", DtrnVatValue);

                ps.Execute();
            }
                             
            ps.Close();            
        }

        public double GetDiscount(Context ctx, long itemId, long cstId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                const string query = @"
select rdisc_per from rdisc 
where cst_kat_disc = (select cst_kat_disc from rcustomer where  id = :cst_id)
  and item_ctg_disc = (select item_ctg_disc from ritems where id = :item_id) ";
                
                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("item_id", itemId);
                ps.Set("cst_id", cstId);
                double discount = 0;
                
                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    discount = rs.GetDouble("rdisc_per");
                }
                
                ps.Close();
                conn.Commit();
                conn.Release();
                
                return discount;
            }
        }
        /// <summary>
        /// TODO: add column disc_per2 into disc table
        /// </summary>
        /// <returns>The discount2.</returns>
        /// <param name="ctx">Context.</param>
        /// <param name="itemId">Item identifier.</param>
        /// <param name="cstId">Cst identifier.</param>
        public double GetDiscount2(Context ctx, long itemId, long cstId)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                const string query = @"
select rdisc_per from rdisc 
where cst_kat_disc = (select cst_kat_disc from rcustomer where  id = :cst_id)
  and item_ctg_disc = (select item_ctg_disc from ritems where id = :item_id) ";
                
                IPreparedStatement ps = conn.PrepareStatement(query);
                ps.Set("item_id", itemId);
                ps.Set("cst_id", cstId);
                double discount = 0;
                
                IResultSet rs = ps.ExecuteQuery();
                if (rs.Next())
                {
                    discount = rs.GetDouble("rdisc_per");
                }
                
                ps.Close();
                conn.Commit();
                conn.Release();
                
                return discount;
            }
        }
//      public double GetItemPrice(Context ctx, long itemId)
//      {
//          using (IConnection conn = Sync.GetConnection(ctx))
//          {
//              IPreparedStatement ps = conn.PrepareStatement("select item_sale_val1 from items where item_id = :item_id ");//unit_price ?
//              ps.Set("item_id", itemId);
//              double unit_price = 0;
//              
//              IResultSet rs = ps.ExecuteQuery();
//              if (rs.Next())
//              {
//                  unit_price = rs.GetDouble("item_sale_val1");
//              }
//              
//              ps.Close();
//              conn.Commit();
//              conn.Release();
//              
//              return unit_price;
//          }
//      }
//      
//      public int GetItemVatId(Context ctx, long itemId)
//      {
//          using (IConnection conn = Sync.GetConnection(ctx))
//          {
//              const string query = @"select vat_id from items where item_id = :item_id";
//              
//              IPreparedStatement ps = conn.PrepareStatement(query);
//              ps.Set("item_id", itemId);
//              int vatId = 0;
//              
//              IResultSet rs = ps.ExecuteQuery();
//              if (rs.Next())
//              {
//                  vatId = (int)rs.GetDouble("vat_id");
//              }
//              
//              ps.Close();
//              conn.Commit();
//              conn.Release();
//              
//              return vatId;
//          }
//      }
    }
}