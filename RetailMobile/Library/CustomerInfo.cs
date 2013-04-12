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
	public class CustomerInfo
	{
		public bool IsNew = true;
		public double CustID { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string CustTaxNum { get; set; }
		public string CustAddress { get; set; }
		public decimal CustDebt { get; set; }
		public string CustPhone { get; set; }

		public static CustomerInfo GetCustomer (Context ctx, string code)
		{
			CustomerInfo info = new CustomerInfo ();

			using (IConnection conn = Sync.GetConnection(ctx)) {
				IPreparedStatement ps = conn.PrepareStatement ("SELECT cst_id, cst_cod, cst_desc FROM customers WHERE cst_cod = :Code");
				ps.Set ("Code", code);

				IResultSet result = ps.ExecuteQuery ();

				if (result.Next ()) {
					info.CustID = result.GetDouble ("cst_id");
					info.Code = result.GetString ("cst_cod");
					info.Name = result.GetString ("cst_desc");
					info.IsNew = false;
				}

				result.Close ();
				ps.Close ();
				conn.Commit ();
				conn.Release ();
			}

			return info;
		}

		public static CustomerInfo GetCustomer (Context ctx, double custID)
		{
			CustomerInfo info = new CustomerInfo ();

			if (custID == 0) {
				return info;
			}

			using (IConnection conn = Sync.GetConnection(ctx)) {
				IPreparedStatement ps = conn.PrepareStatement ("SELECT cst_id, cst_cod, cst_desc FROM customers WHERE cst_id = :CustID");
				ps.Set ("CustID", custID);

				IResultSet result = ps.ExecuteQuery ();

				if (result.Next ()) {
					info.CustID = result.GetDouble ("cst_id");
					info.Code = result.GetString ("cst_cod");
					info.Name = result.GetString ("cst_desc");
					info.IsNew = false;
				}

				result.Close ();
				ps.Close ();
				conn.Commit ();
				conn.Release ();
			}

			return info;
		}

		public void Save (Context ctx)
		{
			CustomerInfo info = new CustomerInfo ();

			using (IConnection conn = Sync.GetConnection(ctx)) {
				IPreparedStatement ps;
				if (IsNew) {
					ps = conn.PrepareStatement (@"INSERT INTO customers (cst_id, cst_cod, cst_desc) VALUES (:cst_id, :cst_cod, :cst_desc); SELECT last_insert_id();");
				} else {
					ps = conn.PrepareStatement (@"UPDATE customers SET cst_cod = :cst_cod, cst_desc = :cst_desc WHERE cst_id = :cst_id");
				}
				ps.Set ("cst_id", CustID.ToString ());
				ps.Set ("cst_cod", Code.ToString ());
				ps.Set ("cst_desc", Name.ToString ());
				if (IsNew) {
					IResultSet set = ps.ExecuteQuery ();
					if (set.Next ()) {
						CustID = set.GetDouble (0);
					}
				} else {
					ps.Execute ();
				}

				ps.Close ();

				conn.Commit ();
				conn.Release ();
			}

		}

	}
}