using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using RetailMobile.Library;

namespace RetailMobile
{
    public class InvoiceTabDetails : BaseFragment
    {
        ListView lvDetails;
        TransDetAdapter detailsAdapter;
        TransDetAdapter.SelectedDetail selectedDetail;
        ItemsSelectDialog dialogItems;
        InvoiceInfoFragment invoiceParentView;

        public delegate void DetailsChangedDelegate();

        public event DetailsChangedDelegate DetailsChanged;

        public static InvoiceTabDetails NewInstance(InvoiceInfoFragment parentView)
        {
            var detailsFrag = new InvoiceTabDetails { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", parentView.ObjectId);
            detailsFrag.invoiceParentView = parentView;

            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                return null;
            }

            View v = inflater.Inflate(Resource.Layout.invoice_tab_details, container, false);
            Button btnSearchItems = v.FindViewById<Button>(Resource.Id.btnSearchItems);
            Button btnAddValue = v.FindViewById<Button>(Resource.Id.btnAddValue);
            Button btnSubstractValue = v.FindViewById<Button>(Resource.Id.btnSubstractValue);
            btnSearchItems.Click += btnSearchItems_Click;
            btnAddValue.Click += btnAddValue_Click;
            btnSubstractValue.Click += btnSubstractValue_Click;

            lvDetails = v.FindViewById<ListView>(Resource.Id.lvDetails);
            lvDetails.AddHeaderView(inflater.Inflate (Resource.Layout.TransDetRow_header, null));
     
            LoadDetailsAdapter();
            GC.Collect();
            return v;
        }



        public void LoadDetailsAdapter()
        {
            if (this.Activity == null)
            {
                return;
            }

            detailsAdapter = new TransDetAdapter(Activity, invoiceParentView.Header.TransDetList, this.invoiceParentView);
            detailsAdapter.QtysChangedEvent += () =>
            {
                if (DetailsChanged != null)
                {
                    DetailsChanged();
                }
                //detailsAdapter.NotifyDataSetChanged();

                //if (TransDetAdapter.lastFocusedControl != null)
                //{
                //TransDetAdapter.lastFocusedControl.RequestFocus();
                //}
            };
            detailsAdapter.DetailFieldSelectedEvent += (selDetail) => {
                selectedDetail = selDetail;
            };

            lvDetails.SetAdapter(detailsAdapter);

            if (DetailsChanged != null)
            {
                DetailsChanged();
            }
        }

        void btnSubstractValue_Click(object sender, EventArgs e)
        {
            detailsAdapter.SubstractValue();
        }

        void btnAddValue_Click(object sender, EventArgs e)
        {
            detailsAdapter.AddValue();
        }

        void btnSearchItems_Click(object sender, EventArgs e)
        {
            dialogItems = new ItemsSelectDialog(Activity, Resource.Style.actionDialog, invoiceParentView.Header);
            dialogItems.Window.SetLayout(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent);
            dialogItems.DismissEvent += (s, ee) =>
            {
                foreach (int itemId in dialogItems.CheckedItemIds.Keys)
                {
                    TransDet detOld = invoiceParentView.Header.TransDetList.GetByItemId(itemId);

                    if (detOld != null)
                    {
                        detOld.LoadItemInfo(Activity, itemId, (double)detOld.DtrnQty1 + dialogItems.CheckedItemIds[itemId], invoiceParentView.Header.CstId);
                    }
                    else
                    {
                        TransDet transDet = new TransDet();
                        transDet.LoadItemInfo(Activity, itemId, dialogItems.CheckedItemIds[itemId], invoiceParentView.Header.CstId);
                        transDet.DtrnNum = invoiceParentView.Header.TransDetList.Count + 1;
                        invoiceParentView.Header.TransDetList.Add(transDet);
                    }
                }

                LoadDetailsAdapter();
                if (DetailsChanged != null)
                {
                    DetailsChanged();
                }
            };

            dialogItems.Show();
        }
    }
}