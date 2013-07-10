using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;
using Android.Views.InputMethods;

namespace RetailMobile
{
    public class LoginFragment : BaseFragment
    {
        TextView tbUsername;
        TextView tbPassword;
        Button btnLogin;
        bool isTablet;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
        }

        private void InitActionBar()
        {
            actionBar = (RetailMobile.Fragments.ItemActionBar)this.FragmentManager.FindFragmentById(Resource.Id.ActionBar1);
            actionBar.ClearButtons();
            actionBar.AddButtonLeft(1,"",Resource.Drawable.settings_48);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginFragment, container, false);

            InitActionBar();

            this.tbUsername = (TextView)view.FindViewById(Resource.Id.tbUsername);
            this.tbPassword = (TextView)view.FindViewById(Resource.Id.tbPassword);
            this.btnLogin = (Button)view.FindViewById(Resource.Id.btnLogin);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            
            return view; 
        }

        private void ActionBarButtonClicked(int buttonID)
        {
            if (buttonID == 1)
            {
                SettingsClicked();
            }
        }

        void SettingsClicked()
        {
            RelativeLayout f3 = this.Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);
            if(f3 != null)
            {
                LinearLayout f2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
                f2.Visibility = ViewStates.Gone;
                f3.Visibility = ViewStates.Visible;

                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment3, new SettingsFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.AddToBackStack("Settings");
                ft.Commit();
            }
            else
            {
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment1, new SettingsFragment());
                ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                ft.AddToBackStack("Settings");
                ft.Commit();
            }
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            string username = this.tbUsername.Text;
            string password = this.tbPassword.Text;

            if (LoginFragment.Login(this.Activity, username, password))
            {
                RelativeLayout f1 = this.Activity.FindViewById<RelativeLayout>(Resource.Id.fragment1);
                RelativeLayout f2 = this.Activity.FindViewById<RelativeLayout>(Resource.Id.fragment2);
                RelativeLayout f3 = this.Activity.FindViewById<RelativeLayout>(Resource.Id.fragment3);
                LinearLayout l2 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout2);
                LinearLayout l3 = this.Activity.FindViewById<LinearLayout>(Resource.Id.layout3);

                if (f2 != null && f3 != null)
                {
                    f2.Visibility = ViewStates.Visible;
                    f3.Visibility = ViewStates.Visible;
                    l2.Visibility = ViewStates.Visible;
                    l3.Visibility = ViewStates.Visible;

                    var ft = FragmentManager.BeginTransaction();
                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment1, new MainMenuFragment());
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();

                    DetailsFragment details = DetailsFragment.NewInstance((int)MainMenu.MenuItems.Invoices);
                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment2, details);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();

                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment3, InvoiceInfoFragment.NewInstance(0));
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();

                }
                else
                {
                    var ft = FragmentManager.BeginTransaction();
                    ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.fragment1, InvoiceInfoFragment.NewInstance(0));
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    ft.Commit();
                }

                InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(
                    Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(tbPassword.WindowToken, 0);
                imm.HideSoftInputFromWindow(tbUsername.WindowToken, 0);

                /*if (isTablet)
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
                        View layoutMenu = this.Activity.FindViewById<LinearLayout>(Resource.Id.LayoutMenu);
                        if(layoutMenu != null)
                            layoutMenu.Visibility = ViewStates.Visible;              
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
                }*/
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