using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Com.Jjoe64.Graphview; 
using Android.Util;

namespace RetailMobile
{
	public class LoginFragment : BaseFragment
    {
        TextView tbUsername;
        TextView tbPassword;
		Button btnLogin;

		public LoginFragment()
		{
		}

       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginFragment, container, false);

			tbUsername = (TextView)view.FindViewById(Resource.Id.tbUsername);
			tbPassword = (TextView)view.FindViewById(Resource.Id.tbPassword);
			btnLogin = (Button)view.FindViewById(Resource.Id.btnLogin);
			btnLogin.Click += new EventHandler(btnLogin_Click);
            
			return view;
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
			string username = tbUsername.Text;
			string password = tbPassword.Text;

			User user = User.Login(this.Activity, username, password);
			if(user != null)
			{
				Common.CurrentDealerID = user.deal_id;
				this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Visible;
				Sync.Synchronize(this.Activity);

				DetailsFragment details = DetailsFragment.NewInstance((int)Base.MenuItems.Invoices);
				var ft = FragmentManager.BeginTransaction();
				ft.Replace(Resource.Id.details_fragment, details);
				ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
				ft.Commit();
				
				this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;

				ft = FragmentManager.BeginTransaction();
				//ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
				ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
				ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
				ft.Commit();
			}
        }

    }
}