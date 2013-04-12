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
    [Activity(Label = "Invoice Screen")]
    public class InvoiceScreen : Activity
    {
        public const int ITEMS_SEARCH = 2010;
        //SimpleApp.BindingManager bindingManager;
        Library.TransHed _header;

        TextView tbCustCode;
        TextView tbCustDesc;
        TextView tbHtrnID;
        TextView tbHtrnDate;
        TextView tbHtrnExpln;
        TextView tbHtrnNetValue;
        TextView tbHtrnVatValue;
        TextView tbHtrnTotValue;
        Button btnSearchItems;
        ListView lvDetails;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.InvoiceScreen);

            LoadControls();

            //bindingManager = new SimpleApp.BindingManager(this);

            /*Library.TransHed.NewTransHed((o, a) =>
            {
                if (a.Error == null)
                {
                    _header = a.Object;
                    DataBind();
                }
                else
                {
                    //show error
                }
            });*/

            _header = new Library.TransHed();
            DataBind();

        }

        private void LoadControls()
        {
            btnSearchItems = FindViewById<Button>(Resource.Id.btnSearchItems);
            btnSearchItems.Click += new EventHandler(btnSearchItems_Click);

            tbCustCode = FindViewById<TextView>(Resource.Id.tbCustCode);
            tbCustDesc = FindViewById<TextView>(Resource.Id.tbCustName);
            tbHtrnID = FindViewById<TextView>(Resource.Id.tbHtrnID);
            tbHtrnDate = FindViewById<TextView>(Resource.Id.tbHtrnDate);
            tbHtrnExpln = FindViewById<TextView>(Resource.Id.tbHtrnExpln);
            tbHtrnNetValue = FindViewById<TextView>(Resource.Id.tbHtrnNetValue);
            tbHtrnVatValue = FindViewById<TextView>(Resource.Id.tbHtrnVatValue);
            tbHtrnTotValue = FindViewById<TextView>(Resource.Id.tbHtrnTotValue);

            lvDetails = FindViewById<ListView>(Resource.Id.lvDetails);
        }

        private void DataBind()
        {
            /*if (tbCustCode != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbCustCode, "Text", _header, DAL.TransHed.CustCodeProperty.Name));
            if (tbCustDesc != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbCustDesc, "Text", _header, DAL.TransHed.CustNameProperty.Name));
            if (tbHtrnID != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnID, "Text", _header, DAL.TransHed.HtrnIDProperty.Name));
            if (tbHtrnDate != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnDate, "Text", _header, DAL.TransHed.HtrnDateProperty.Name));
            if (tbHtrnExpln != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnExpln, "Text", _header, DAL.TransHed.HtrnExplProperty.Name));
            if (tbHtrnNetValue != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnNetValue, "Text", _header, DAL.TransHed.HtrnNetValueProperty.Name));
            if (tbHtrnVatValue != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnVatValue, "Text", _header, DAL.TransHed.HtrnVatValueProperty.Name));
            if (tbHtrnTotValue != null)
                bindingManager.Bindings.Add(new SimpleApp.Binding(tbHtrnTotValue, "Text", _header, DAL.TransHed.HtrnTotValueProperty.Name));
            */

            InvoiceDetailsAdapter adapter = new InvoiceDetailsAdapter(this, Resource.Layout.InvoiceDetailRow, _header.TransDetList);
            lvDetails.Adapter = adapter;
        }

        void btnSearchItems_Click(object sender, EventArgs e)
        {
            var ItemSearchScreen = new Intent(this, typeof(ItemSearch));
            StartActivityForResult(ItemSearchScreen, ITEMS_SEARCH);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == ITEMS_SEARCH)
            {
                //fill selected items in details list
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

    }
}