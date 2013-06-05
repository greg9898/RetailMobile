using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile.Library
{
    public class CustomerInfoList : List<CustomerInfo>
    {
        public static CustomerInfoList GetCustomerInfoList(Context ctx, Criteria crit)
        {
            CustomerInfoList customers = new CustomerInfoList();
			
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                string query = @"
SELECT TOP 100 id, cst_cod, cst_desc 
FROM rcustomer
WHERE 1=1 ";
                if (crit.CustCode != "")
                {
                    query += " AND cst_cod like \'" + crit.CustCode + "%\'";
                }
				
                if (crit.CustName != "")
                {
                    query += " AND cst_desc like \'" + crit.CustName + "%\'";
                }

                query += " ORDER BY cst_desc ";
				
                Log.Debug("GetCustomerInfoList", query);
                IPreparedStatement ps = conn.PrepareStatement(query);
                IResultSet result = ps.ExecuteQuery();
				
                while (result.Next())
                {
                    CustomerInfo customer = new CustomerInfo();
                    customer.CustID = result.GetInt("id");
                    customer.Code = result.GetString("cst_cod");
                    customer.Name = result.GetString("cst_desc");
					
                    customers.Add(customer);
                }
				
                ps.Close();
            }
			
            return customers;
        }

        public class Criteria
        {
            public string CustName;
            public string CustCode;

            public Criteria()
            {
            }

            public Criteria(string custName, string custCode)
            {
                CustName = custName;
                CustCode = custCode;
            }
        }
    }
}