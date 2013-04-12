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
using Android.Content.Res;

namespace RetailMobile
{
    public class MainMenuAdapter : ArrayAdapter<string>
    {
        Activity context = null;

        public MainMenuAdapter(Activity context)
            : base(context, Resource.Layout.MainMenuRow, Enum.GetNames(typeof(Base.MenuItems)))
        {
            this.context = context;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.MainMenuRow, null);

            TextView tvMenuCaption = view.FindViewById<TextView>(Resource.Id.tvMenuCaption);
            ImageView ivMenuIcon = view.FindViewById<ImageView>(Resource.Id.ivMenuIcon);

            switch (position)
            {
                case (int)Base.MenuItems.Items:
                    tvMenuCaption.Text = this.context.GetString(Resource.String.tvMenuCaptionItems);
                    ivMenuIcon.SetImageResource(Resource.Drawable.itemsIcon);
                    break;
                case (int)Base.MenuItems.Customers:
                    tvMenuCaption.Text = this.context.GetString(Resource.String.tvMenuCaptionCustomers);
                    ivMenuIcon.SetImageResource(Resource.Drawable.customersIcon);
                    break;
                case (int)Base.MenuItems.Invoices:
                    tvMenuCaption.Text = this.context.GetString(Resource.String.tvMenuCaptionInvoices);
                    ivMenuIcon.SetImageResource(Resource.Drawable.invoiceIcon);
                    break;
                default:
                    tvMenuCaption.Text =Base.MenuItems.CheckableItemsTest.ToString(); //Enum.GetName(typeof(TitlesFragment), (TitlesFragment.MenuItems)position);
                    break;
            }

            return view;
        }
    }
}