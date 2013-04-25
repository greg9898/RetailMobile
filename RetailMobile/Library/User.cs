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
	public class User
	{
		public int deal_id { get; set; }
		public int user_id { get; set; }
		public string login_name { get; set; }
		public string user_pass { get; set; }

		public static User Login (Context ctx, string username, string password)
		{
			User user = null;
			using (IConnection conn = Sync.GetConnection(ctx)) {
				IPreparedStatement ps = conn.PrepareStatement (@"SELECT deal_id,
user_id,
login_name,
user_pass,
user_active
FROM rusers 
WHERE user_active = 1 AND login_name = :login_name AND user_pass = :user_pass ");
				ps.Set ("login_name", username);
				ps.Set ("user_pass", password);
				
				IResultSet result = ps.ExecuteQuery ();
				
				if (result.Next ()) {
					user = new User();
					user.deal_id = result.GetInt("deal_id");
					user.user_id = result.GetInt ("user_id");
					user.login_name = result.GetString ("login_name");
					user.user_pass = result.GetString ("user_pass");
				}
				
				result.Close ();
				ps.Close ();
				conn.Commit ();
				conn.Release ();
			}
			
			return user;
		}
			
	}
		
}