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

                ps = conn.PrepareStatement(@"SELECT dtrn_id, htrn_id, trans_det.item_id, items.item_cod, items.item_desc, dtrn_unit_price, dtrn_qty1, dtrn_net_value, dtrn_vat_value 
                            FROM trans_det 
                            JOIN items ON items.item_id = trans_det.item_id
                            WHERE htrn_id = :htrn_id
");
                ps.Set("htrn_id", htrnId);

                result = ps.ExecuteQuery();

                if (info.TransDetList == null)
                    info.TransDetList = new TransDetList();

                while (result.Next())
                {
                    TransDet det = new TransDet();
                    det.DtrnId = result.GetDouble("dtrn_id");
                    det.HtrnId = result.GetDouble("htrn_id");
                    det.ItemId = result.GetDouble("item_id");
                    det.ItemCode = result.GetString("item_cod");
                    det.ItemDesc = result.GetString("item_desc");
                    det.DtrnUnitPrice = result.GetDouble("dtrn_unit_price");
                    det.DtrnQty1 = result.GetDouble("dtrn_qty1");
                    det.DtrnNetValue = result.GetDouble("dtrn_net_value");
                    det.DtrnVatValue = result.GetDouble("dtrn_vat_value");
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