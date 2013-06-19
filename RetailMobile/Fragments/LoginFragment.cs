using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class LoginFragment : BaseFragment
    {
        TextView tbUsername;
        TextView tbPassword;
        Button btnLogin;
        bool isTablet;

        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginFragment, container, false);
            isTablet = Common.isTabletDevice(this.Activity);

            if (isTablet && !Common.isPortrait(this.Activity))
            {
                RetailMobile.Fragments.ItemActionBar actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
                actionBar.SetTitle(this.Activity.GetString(Resource.String.btnLogin));
                actionBar.ClearButtons();
            }

            this.tbUsername = (TextView)view.FindViewById(Resource.Id.tbUsername);
            this.tbPassword = (TextView)view.FindViewById(Resource.Id.tbPassword);
            this.btnLogin = (Button)view.FindViewById(Resource.Id.btnLogin);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            
            return view; 
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            string username = this.tbUsername.Text;
            string password = this.tbPassword.Text;

            if (LoginFragment.Login(this.Activity, username, password))
            {
                if (isTablet)
                {
                    this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutList).Visibility = ViewStates.Visible;
                    this.Activity.FindViewById<FrameLayout>(Resource.Id.details_fragment).Visibility = ViewStates.Visible;
                    this.Activity.FindViewById<LinearLayout>(Resource.Id.layoutDetails).Visibility = ViewStates.Visible;

                    DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                    var ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.details_fragment, details);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();

                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();

                    if (this.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
                    {    
                        this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu).Visibility = ViewStates.Visible;              
                    }
                    else
                    { 
                        RetailMobile.Fragments.ActionBar myActionBar = (RetailMobile.Fragments.ActionBar)Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarMain);       

                        myActionBar.ButtonMenuVisibility = ViewStates.Visible;
                        myActionBar.ButtonSettingsVisibility = ViewStates.Gone;
                    }
                }
                else
                {
//                    var intent = new Android.Content.Intent();
//                    intent.SetClass(this.Activity, typeof(TransactionFragmentActivity));
//                    StartActivity(intent);

                    var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.actionbar_phone_fragment, new RetailMobile.Fragments.ItemActionBar(), "ItemActionBar");
                    ft.Replace(Resource.Id.content_phone_fragment, InvoiceInfoFragment.NewInstance(0));
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }               
            }
            else
            {
                Toast.MakeText(Activity.ApplicationContext, Activity.GetString(Resource.String.InvalidUserOrPass), ToastLength.Short).Show();
            }
        }

        public static bool Login(Android.Content.Context ctx, string username, string password)
        {
            bool isSuccessful = false;

            User user = User.Login(ctx, username, password);
            if (user != null)
            {
                Common.CurrentDealerID = user.deal_id;

                isSuccessful = true;
                PreferencesUtil.Username = username;
                PreferencesUtil.Password = password;
                PreferencesUtil.SavePreferences(ctx);
            }

            return isSuccessful;
        }
    }
}