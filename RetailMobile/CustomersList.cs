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
using RetailMobile.Library;

namespace RetailMobile
{
    [Activity(Label = "Customers List")]
    //[Activity(Label = "Main menu", MainLauncher = true, Icon = "@drawable/icon")]
    public class CustomersList : ListActivity
    {
        Library.CustomerInfoList _customerInfoList;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CustomersList);
            RegisterForContextMenu(this.ListView);

        }

        protected override void OnResume()
        {
            base.OnResume();

            LoadData();
        }

        private void LoadData()
        {
            _customerInfoList = Library.CustomerInfoList.GetCustomerInfoList(this, new Library.CustomerInfoList.Criteria());
            CustomersListAdapter adapter = new CustomersListAdapter(this, Resource.Layout.CustomerInfoRow, _customerInfoList);
            ListAdapter = adapter;
            /*DAL.CustomerInfoList.GetCustomerInfoList(new CriteriaJ(this), (o, e) =>
            {
                if (e.Error != null)
                {
                    Android.Util.Log.Error("GetCustomerInfoList", string.Format("An error has occurred: {0}", e.Error.Message));
                }
                else
                {
                    _customerInfoList = e.Object;
                    CustomersListAdapter adapter = new CustomersListAdapter(this, Resource.Layout.CustomerInfoRow, _customerInfoList);
                    ListAdapter = adapter;
                }
            });*/
        }

        public static IListAdapter LoadCustomerData(Activity context)
        {
            CustomersListAdapter adapter = null;
            Library.CustomerInfoList customerInfoList = Library.CustomerInfoList.GetCustomerInfoList(context, new Library.CustomerInfoList.Criteria());
            adapter = new CustomersListAdapter(context, Resource.Layout.CustomerInfoRow, customerInfoList);
            /*Library.CustomerInfoList.GetCustomerInfoList(new CriteriaJ(context), (o, e) =>
            {
                if (e.Error != null)
                {
                    Android.Util.Log.Error("GetCustomerInfoList", string.Format("An error has occurred: {0}", e.Error.Message));
                }
                else
                {
                    DAL.CustomerInfoList customerInfoList = e.Object;
                    adapter = new CustomersListAdapter(context, Resource.Layout.CustomerInfoRow, customerInfoList);
                }
            });*/

            return adapter;
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var customerDetailsScreen = new Intent(this, typeof(CustomerDetails));
            customerDetailsScreen.PutExtra("CustID", _customerInfoList[position].CustID.ToString());
            StartActivity(customerDetailsScreen);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var menuItemNewCustomer = menu.Add(0, 1, 1, Resource.String.miAddCustomer);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 1)
            {
                var customerDetailsScreen = new Intent(this, typeof(CustomerDetails));
                StartActivity(customerDetailsScreen);
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            MenuInflater.Inflate(Resource.Menu.CustomerListContextMenu, menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo mi = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            switch (item.ItemId)
            {
                case Resource.Id.miNew:
                    //add new 
                    var customerDetailsScreen = new Intent(this, typeof(CustomerDetails));
                    StartActivity(customerDetailsScreen);
                    return true;
                case Resource.Id.miEdit:
                    //edit
                    var customerDetailsScreenEdit = new Intent(this, typeof(CustomerDetails));
                    customerDetailsScreenEdit.PutExtra("CustID", _customerInfoList[mi.Position].CustID.ToString());
                    StartActivity(customerDetailsScreenEdit);
                    return true;
                case Resource.Id.miDelete:
                    //delete
                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }

        }
    }
}