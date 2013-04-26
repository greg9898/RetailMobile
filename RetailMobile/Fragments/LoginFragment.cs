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
		RetailMobile.Fragments.ItemActionBar actionBar;
        TextView tbUsername;
        TextView tbPassword;
		Button btnLogin;

		public LoginFragment()
		{
		}

       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginFragment, container, false);

			actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
			actionBar.SetTitle(this.Activity.GetString(Resource.String.btnLogin));
			actionBar.ClearButtons();

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

			if(LoginFragment.Login(this.Activity, username, password))
			{
				this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Visible;
				this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
				this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;
				this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
				
				DetailsFragment details = DetailsFragment.NewInstance((int)Base.MenuItems.Invoices);
				var ft = FragmentManager.BeginTransaction();
				ft.Replace(Resource.Id.details_fragment, details);
				ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
				ft.Commit();
				
				ft = FragmentManager.BeginTransaction();
				//ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
				ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
				ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
				ft.Commit();
			}

        }

		public static bool Login(Android.Content.Context ctx, string username, string password)
		{
			bool isSuccessful = false;

			User user = User.Login(ctx, username, password);
			if(user != null)
			{
				Common.CurrentDealerID = user.deal_id;

				Sync.Synchronize(ctx);

				isSuccessful = true;
				PreferencesUtil.Username = username;
				PreferencesUtil.Password = password;
				PreferencesUtil.SavePreferences(ctx);
			}

			return isSuccessful;
		}

    }
}