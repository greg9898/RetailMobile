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

namespace RetailMobile
{
    [Activity(Label = "Item Search")]
    public class ItemSearch : ListActivity
    {
        Library.ItemInfoList _ItemInfoList;
        Spinner cbItemCateg1;
        Spinner cbItemCateg2;
        EditText tbItemCodeFilter;
        EditText tbItemDescFilter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ItemSearch);
            cbItemCateg1 = FindViewById<Spinner>(Resource.Id.cbItemCateg1);
            cbItemCateg2 = FindViewById<Spinner>(Resource.Id.cbItemCateg2);
            tbItemCodeFilter = FindViewById<EditText>(Resource.Id.tbFilterCode);
            tbItemDescFilter = FindViewById<EditText>(Resource.Id.tbFilterDesc);

            tbItemDescFilter.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>((s, e) => { SearchItems(); });
            tbItemCodeFilter.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>((s, e) => { SearchItems(); });

            /*Library.BindableList<int, string> listCateg1 = Library.BindableList<int, string>.GetList(DAL.DBUtil.TableItemCateg1Name, "Cat1ID", "Cat1Desc", this);
            listCateg1.Insert(0, new KeyValuePair<int, string>(-1, "All"));
            cbItemCateg1.Adapter = new SpinnerAdapter<int, string>(this, listCateg1);
            cbItemCateg1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>((s, e) => { SearchItems(); });

            DAL.BindableList<int, string> listCateg2 = DAL.BindableList<int, string>.GetList(DAL.DBUtil.TableItemCateg2Name, "Cat2ID", "Cat2Desc", this);
            listCateg2.Insert(0, new KeyValuePair<int, string>(-1, "All"));
            cbItemCateg2.Adapter = new SpinnerAdapter<int, string>(this, listCateg2);
            cbItemCateg2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>((s, e) => { SearchItems(); });*/

            SearchItems();
        }

        private void SearchItems()
        {
            string searchFilter = " 1=1 ";

            int categ1SelectedValue = ((SpinnerAdapter<int, string>)cbItemCateg1.Adapter).GetSelectedValue(cbItemCateg1.SelectedItemPosition);
            if (categ1SelectedValue != -1)
                searchFilter += " AND Cat1ID = " + categ1SelectedValue.ToString();

            int categ2SelectedValue = ((SpinnerAdapter<int, string>)cbItemCateg2.Adapter).GetSelectedValue(cbItemCateg2.SelectedItemPosition);
            if (categ2SelectedValue != -1)
                searchFilter += " AND Cat2ID = " + categ2SelectedValue.ToString();

            if (string.IsNullOrEmpty(tbItemCodeFilter.Text) == false)
            {
                searchFilter += " AND ItemCod LIKE '%" + tbItemCodeFilter.Text + "%'";
            }

            if (string.IsNullOrEmpty(tbItemDescFilter.Text) == false)
            {
                searchFilter += " AND ItemDesc LIKE '%" + tbItemDescFilter.Text + "%'";
            }

            _ItemInfoList = Library.ItemInfoList.GetItemInfoList(this);
            ItemsSearchAdapter adapter = new ItemsSearchAdapter(this, Resource.Layout.ItemSearchRow, _ItemInfoList);
            ListAdapter = adapter;
            /* DAL.ItemInfoList.GetItemInfoList(searchFilter, (o, e) =>
             {
                 if (e.Error != null)
                 {

                 }
                 else
                 {
                     _ItemInfoList = e.Object;
                     ItemsSearchAdapter adapter = new ItemsSearchAdapter(this, Resource.Layout.ItemSearchRow, _ItemInfoList);
                     ListAdapter = adapter;
                 }
             });*/
        }

    }
}
