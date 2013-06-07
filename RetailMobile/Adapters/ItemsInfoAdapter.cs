using Android.App;
using Android.Views;
using Android.Widget;

namespace RetailMobile
{
    public class ItemsInfoAdapter : ArrayAdapter<Library.ItemInfo>, IScrollLoadble
    {
        Activity context;
        Library.ItemInfoList itemInfoList;

        public ItemsInfoAdapter(Activity context, Library.ItemInfoList list)
            : base(context, Resource.Layout.ItemInfoRow, list)
        {
            this.context = context;

            itemInfoList = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.GetItem(position);
            //var item = this.ItemInfoList[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ItemInfoRow, null);

            //TextView tbItemCode = (TextView)view.FindViewById(Resource.Id.tbItemCode);
            TextView tbItemName = (TextView)view.FindViewById(Resource.Id.tbItemName);

            //tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.ItemDesc;

            return view;
        }
		#region IScrollLoadble implementation

        public void LoadData(int page)
        {
            Library.ItemInfoList.LoadAdapterItems(context, page, this, new Library.ItemInfoList.Criteria());
            this.NotifyDataSetChanged();
        }
		#endregion
    }
}