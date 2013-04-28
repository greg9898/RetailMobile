using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace RetailMobile
{
    internal class DetailsFragment : Android.Support.V4.App.ListFragment
    {
		const int ADD_BUTTON = 12287;
		RetailMobile.Fragments.ItemActionBar actionBar;
        System.Collections.ICollection _list = null;
        private int _currentObjId = -1;
        bool _isDualPane = true;
        public static DetailsFragment NewInstance(int objId)
        {
            var detailsFrag = new DetailsFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutInt("idLvl1", objId);
            return detailsFrag;
        }
        public int ParentObjId
        {
            get { return Arguments.GetInt("idLvl1", -1); }
        }

		public override void OnViewCreated (View p0, Bundle p1)
		{
			base.OnViewCreated (p0, p1);

			actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarList);
			actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
			actionBar.AddButtonRight(ADD_BUTTON,"",Resource.Drawable.add_48);
		}

		void ActionBarButtonClicked(int id)
		{
			if(id == ADD_BUTTON)
			{
				try
				{
					var ft = FragmentManager.BeginTransaction();
					//ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
					ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(0));
					ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
					ft.Commit();
				}
				catch(Exception ex)
				{
					Log.Error("exception",ex.Message);
				}
			}
		}

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Android.Util.Log.Debug("DetailsFragment", "OnActivityCreated");
            base.OnActivityCreated(savedInstanceState);

            if (savedInstanceState != null)
            {
                _currentObjId = savedInstanceState.GetInt("idLvl2", -1);
            }

            ListView.ChoiceMode = ChoiceMode.Single;

            switch (ParentObjId)
            {
                case (int)Base.MenuItems.Items:
				actionBar.SetTitle(this.Activity.GetString(Resource.String.miItems));
                    Library.ItemInfoList itemInfoList = Library.ItemInfoList.GetItemInfoList(this.Activity);
                    _list = itemInfoList;
                    ListAdapter = new ItemsInfoAdapter(Activity, itemInfoList);
                    break;
                case (int)Base.MenuItems.Customers:
				actionBar.SetTitle(this.Activity.GetString(Resource.String.miCustomers));
                    Library.CustomerInfoList custInfoList = Library.CustomerInfoList.GetCustomerInfoList(this.Activity,new Library.CustomerInfoList.Criteria());
                    _list = custInfoList;
                    ListAdapter = new CustomersAdapter(Activity, custInfoList);
                    break;
                case (int)Base.MenuItems.Invoices:
				actionBar.SetTitle(this.Activity.GetString(Resource.String.miInvoice));
                    Library.TransHedList thedList = Library.TransHedList.GetTransHedList(this.Activity);
                    _list = thedList;
                    ListAdapter = new TransHedAdapter(Activity, thedList);
                    break;
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("idLvl1", ParentObjId);
            outState.PutInt("idLvl2", _currentObjId);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowDetails(position);
        }

        public void ShowDetails(int index)
        {
            if (index == -1)
            {
                return;
            }

            if (_isDualPane)
            {
                // We can display everything in place with fragments.
                // Have the list highlight this item and show the data.
                ListView.SetItemChecked(index, true);
                var detailFragment = (BaseFragment)FragmentManager.FindFragmentById(Resource.Id.detailInfo_fragment);

                switch (ParentObjId)
                {
                    case (int)Base.MenuItems.Items:
                        long itemId = (long)((Library.ItemInfoList)_list)[index].ItemId;

                        if (detailFragment == null || detailFragment.ObjectId != itemId)
                        {
                            var ft = FragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.detailInfo_fragment, ItemFragment.NewInstance(itemId));
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();
                        }
                        break;
                    case (int)Base.MenuItems.Customers:
                        long custId = (long)((Library.CustomerInfoList)_list)[index].CustID;

                        if (detailFragment == null || detailFragment.ObjectId != custId)
                        {
                            var ft = FragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.detailInfo_fragment,CustomerFragment.NewInstance(custId));
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();
                        }
                        break;
                    case (int)Base.MenuItems.Invoices:
                        long invoiceId = (long)((Library.TransHedList)_list)[index].HtrnId;//?

                        if (detailFragment == null || detailFragment.ObjectId != invoiceId)
                        {
                            Log.Debug("detailsfragment ShowDetails", "invoiceId =" + invoiceId);
                            var ft = FragmentManager.BeginTransaction();
                            //ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
                            ft.Replace(Resource.Id.detailInfo_fragment, InvoiceInfoFragment.NewInstance(invoiceId));
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();
                        }
                        break;
                }
            }
            else
            {
                // Otherwise we need to launch a new Activity to display
                // the dialog fragment with selected text.
                var intent = new Intent();
                intent.SetClass(Activity, typeof(DetailsActivity));
                intent.PutExtra("idLvl2", index);
                StartActivity(intent);
            }
        }
    }
}