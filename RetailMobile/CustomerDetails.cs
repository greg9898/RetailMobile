using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RetailMobile.Library;

namespace RetailMobile
{
    [Activity(Label = "Customers Details")]
    public class CustomerDetails : Activity
    {
        CustomerInfo _customer;
        EditText tbCustomerCode;
        EditText tbCustomerName;
        EditText tbCustTaxNum;
        TextView lblCustTaxNumError;
        //SimpleApp.BindingManager bindingManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.CustomerDetails);

            //bindingManager = new SimpleApp.BindingManager(this);

            int custID = Intent.GetIntExtra("CustID", 0);
            if (custID == 0)
            {
                _customer = new CustomerInfo();
            }
            else
            {
                _customer = CustomerInfo.GetCustomer(this, custID);
            }

            tbCustomerCode = (EditText)FindViewById(Resource.Id.tbCustomerCode);
            tbCustomerName = (EditText)FindViewById(Resource.Id.tbCustomerName);
            tbCustTaxNum = (EditText)FindViewById(Resource.Id.tbCustTaxNum);
            lblCustTaxNumError = (TextView)FindViewById(Resource.Id.lblCustTaxNumError);

            //_customer.Code = "0012";
            //_customer.Name = "Zaeka Roger";

            DataBind();


            //((Button)FindViewById(Resource.Id.btnSave)).Click += new EventHandler(Activity1_Click);

        }

        private void DataBind()
        {

            //            bindingManager.Bindings.Add(new SimpleApp.Binding(tbCustomerCode, "Text", _customer, DAL.CustomerEdit.CustCodeProperty.Name));
            //            bindingManager.Bindings.Add(new SimpleApp.Binding(tbCustomerName, "Text", _customer, DAL.CustomerEdit.CustDescProperty.Name));
            //            bindingManager.Bindings.Add(new SimpleApp.Binding(tbCustTaxNum, "Text", _customer, DAL.CustomerEdit.CustTaxNumProperty.Name));

            //tbCustTaxNum.SetError("Tax Num Not Valid", null);

            tbCustomerCode.Text = _customer.Code;
            tbCustomerName.Text = _customer.Name;
            tbCustTaxNum.Text = _customer.CustTaxNum;
            tbCustTaxNum.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(tbCustTaxNum_TextChanged);
        }

        void tbCustTaxNum_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //bindingManager.Bindings[2].UpdateSource();

            //if (_customer.BrokenRulesCollection.Count > 0)
            //            {
            /* var res = from i in _customer.BrokenRulesCollection where i.Property == DAL.CustomerEdit.CustTaxNumProperty.Name select i;
             if (res != null)
             {
                 string errText = "";
                 foreach (var r in res)
                 {
                     errText += r.Description + "\n";
                 }
                 errText.TrimEnd('\n');
                 lblCustTaxNumError.Text = errText;
                 lblCustTaxNumError.Visibility = ViewStates.Visible;

             }
             else
                 lblCustTaxNumError.Visibility = ViewStates.Gone;*/
            //            }
            //            else
            //                lblCustTaxNumError.Visibility = ViewStates.Gone;
        }
        //void btnSaveClick(object sender, EventArgs e)
        public void btnSaveClick(View v)
        {
            //bindingManager.UpdateSourceForLastView();
            try
            {
                _customer.Save(this);
                /*_customer.BeginSave((o, arg) =>
                {
                    if (arg.Error != null)
                    {
                        AlertDialog.Builder ad = new AlertDialog.Builder(this);
                        ad.SetTitle("Error");
                        ad.SetMessage("Customer is not valid");
                        ad.SetNeutralButton("Close", (System.EventHandler<Android.Content.DialogClickEventArgs>)null);
                        ad.Show();
                    }
                    else
                        this.Finish();
                });*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

