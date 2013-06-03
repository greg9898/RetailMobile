using System;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class TransCust
    {
        public int Id { get; set; }

        public int	CstId{ get; set; }

        public int	VouchId{ get; set; }

        public int	VoserId{ get; set; }

        public int	Docnum{ get; set; }

        public string	DtrnType{ get; set; }

        public decimal	DtrnNetValue{ get; set; }

        public decimal	DtrnVatValue{ get; set; }

        public DateTime	DtrnDate{ get; set; }

        public int	HtrnId { get; set; }

        public decimal  Credit{ get; set; }

        public decimal  Debit{ get; set; }

        public decimal  CreditMinusDebit{ get; set; }

        public   string Cst_desc
        {
            get;
            set;
        }

        public TransCust()
        {
        }

        public	static TransCust GetTransCust(IResultSet result)
        {
            return new TransCust()
            {
                Id = result.GetInt ("id"),
                CstId = result.GetInt ("cst_id"),
                VouchId = result.GetInt ("vouch_id"),
                VoserId = result.GetInt ("voser_id"),
                Docnum = result.GetInt ("docnum"),
                DtrnType = result.GetString ("dtrn_type"),
                DtrnNetValue =  (decimal)result.GetDouble  ("dtrn_net_value"),
                DtrnVatValue = (decimal)result.GetDouble ("dtrn_vat_value"),
                DtrnDate = Common.JavaDateToDatetime( result.GetDate ("dtrn_date")),
                HtrnId = result.GetInt ("htrn_id"),
            };
        }

        public  static TransCust GetTransCustStat(IResultSet result)
        {
            return new TransCust()
            {
                CstId = result.GetInt ("cst_id"),
                Credit =  (decimal)result.GetDouble  ("credit"),
                Debit = (decimal)result.GetDouble ("debit"),
                CreditMinusDebit = (decimal)result.GetDouble ("crdb"),
                Cst_desc = result.GetString("cst_desc")
            };
        }
    }
}