using System;
using Android.Content;
using Android.OS;
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

        bool scrollLoading = false;
        private int currentPage = 0;
        private int previousTotal = 0;
		//IScrollLoadble LoadableAdapter;

        public override void OnViewCreated(View p0, Bundle p1)
        {
            base.OnViewCreated(p0, p1);

            actionBar = (RetailMobile.Fragments.ItemActionBar)((Android.Support.V4.App.FragmentActivity)this.Activity).SupportFragmentManager.FindFragmentById(Resource.Id.ActionBarList);
            actionBar.ActionButtonClicked += new RetailMobile.Fragments.ItemActionBar.ActionButtonCLickedDelegate(ActionBarButtonClicked);
            actionBar.AddButtonRight(ADD_BUTTON, "", Resource.Drawable.add_48);

            //this.ListView.SetOnScrollListener(new EndlessScrollListener((IScrollLoadble)this.ListView.Adapter));
            //this.ListView.Scroll += new EventHandler<AbsListView.ScrollEventArgs>();

            this.ListView.Scroll += new EventHandler<AbsListView.ScrollEventArgs>((o,e) => {
				if (scrollLoading && e.TotalItemCount > previousTotal) {
					scrollLoading = false;
					previousTotal = e.TotalItemCount;
					currentPage++;
				}
				
				if (!scrollLoading && (e.FirstVisibleItem + e.VisibleItemCount) + 10 >= e.TotalItemCount && e.TotalItemCount >= 10) {
					((IScrollLoadble)this.ListView.Adapter).LoadData (currentPage);

					scrollLoading = true;
				}
			});
        }

        void ActionBarButtonClicked(int id)
        {
            if (id == ADD_BUTTON)
            {
                try
                {
                    var ft = FragmentManager.BeginTransaction();
                    //ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
                    InvoiceInfoFragment invoiceFragment = InvoiceInfoFragment.NewInstance(0);
                    ft.Replace(Resource.Id.detailInfo_fragment, invoiceFragment);
                    ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                    invoiceFragment.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(InvoiceSaved);
                    ft.Commit();
                }
                catch (Exception ex)
                {
                    Log.Error("exception", ex.Message);
                }
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Log.Debug("DetailsFragment", "OnActivityCreated");
            base.OnActivityCreated(savedInstanceState);

            if (savedInstanceState != null)
            {
                _currentObjId = savedInstanceState.GetInt("idLvl2", -1);
            }

            ListView.ChoiceMode = ChoiceMode.Single;

            switch (ParentObjId)
            {
                case (int)MainMenu.MenuItems.Items:
                    actionBar.SetTitle(this.Activity.GetString (Resource.String.miItems));
                    Library.ItemInfoList itemInfoList = new RetailMobile.Library.ItemInfoList();
                    //Library.ItemInfoList itemInfoList = Library.ItemInfoList.GetItemInfoList(this.Activity);
                    _list = itemInfoList;
                    ListAdapter = new ItemsInfoAdapter(Activity, itemInfoList);
                    ((IScrollLoadble)ListAdapter).LoadData(0);
					//Library.ItemInfoList.LoadAdapterItems(this.Activity, (ItemsInfoAdapter)ListAdapter);
                    break;
                case (int)MainMenu.MenuItems.Customers:
                    actionBar.SetTitle(this.Activity.GetString (Resource.String.miCustomers));
                    Library.CustomerInfoList custInfoList = Library.CustomerInfoList.GetCustomerInfoList(this.Activity, new Library.CustomerInfoList.Criteria());
                    _list = custInfoList;
                    ListAdapter = new CustomersAdapter(Activity, custInfoList);
                    break;
                case (int)MainMenu.MenuItems.Invoices:
                    actionBar.SetTitle(this.Activity.GetString (Resource.String.miInvoice));
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

        public void InvoiceSaved(int id)
        {
            Library.TransHed header = Library.TransHed.GetTransHed(this.Activity, id);
            ((TransHedAdapter)ListAdapter).Insert(header, 0);
            ((TransHedAdapter)ListAdapter).NotifyDataSetChanged();

            //((Library.TransHedList)_list).Insert(0,header);
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
                    case (int)MainMenu.MenuItems.Items:
                        long itemId = ((ItemsInfoAdapter)ListAdapter).GetItem(index).ItemId;
                       // long itemId = (long)((Library.ItemInfoList)_list)[index].ItemId;

                        if (detailFragment == null || detailFragment.ObjectId != itemId)
                        {
                            var ft = FragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.detailInfo_fragment, ItemFragment.NewInstance(itemId));
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();
                        }
                        break;
                    case (int)MainMenu.MenuItems.Customers:
                        long custId = (long)((Library.CustomerInfoList)_list)[index].CustID;

                        if (detailFragment == null || detailFragment.ObjectId != custId)
                        {
                            Library.CustomerInfoList custList = (Library.CustomerInfoList)_list;
                            System.Collections.Generic.List<string> custNamesList = new System.Collections.Generic.List<string>(custList.Count);
                            custList.ForEach(c=>custNamesList.Add( c.Name));
                            var ft = FragmentManager.BeginTransaction();
                            ft.Replace(Resource.Id.detailInfo_fragment, CustomerFragment.NewInstance(custId, custNamesList.ToArray()));
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();
                        }
                        break;
                    case (int)MainMenu.MenuItems.Invoices:
                        //long invoiceId = (long)((Library.TransHedList)_list)[index].HtrnId;//?
                        long invoiceId = (long)((TransHedAdapter)this.ListView.Adapter).GetItem(index).HtrnId;

                        if (detailFragment == null || detailFragment.ObjectId != invoiceId)
                        {
                            Log.Debug("detailsfragment ShowDetails", "invoiceId =" + invoiceId);
                            var ft = FragmentManager.BeginTransaction();
                            //ft.Replace(Resource.Id.detailInfo_fragment, InvoiceFragment.NewInstance(invoiceId));
                            InvoiceInfoFragment invoiceFragment = InvoiceInfoFragment.NewInstance(invoiceId);
                            ft.Replace(Resource.Id.detailInfo_fragment, invoiceFragment);
                            ft.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade);
                            ft.Commit();

                            invoiceFragment.InvoiceSaved += new InvoiceInfoFragment.InvoiceSavedDelegate(InvoiceSaved);
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