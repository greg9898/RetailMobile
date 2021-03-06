using Android.App;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class TransHedAdapter : ArrayAdapter<Library.TransHed>, IScrollLoadble
    {
        Activity context;
        Library.TransHedList TransHedList;

        public TransHedAdapter(Activity context, Library.TransHedList list)
            : base(context, Resource.Layout.TransHedRow, list)
        {
            this.context = context;

            TransHedList = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //var item = this.TransHedList[position];
            var item = this.GetItem(position);
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TransHedRow, null);
          
            TextView tbHtrnDate = (TextView)view.FindViewById(Resource.Id.tbHtrnDate);
            TextView tbHtrnNetVal = (TextView)view.FindViewById(Resource.Id.tbHtrnNetVal);
            TextView tbHtrnVatVal = (TextView)view.FindViewById(Resource.Id.tbHtrnVatVal);
            TextView tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustName);

            tbHtrnDate.Text = item.TransDate.ToString(Common.DateFormatDateOnly);
            tbHtrnNetVal.Text = item.HtrnNetVal.ToString(Common.DecimalFormat);
            tbHtrnVatVal.Text = item.HtrnVatVal.ToString(Common.DecimalFormat);
            tbCustName.Text = item.CstName;

            return view;
        }

        public void InsertIntoTransHedList(TransHed h, int index)
        {
            TransHedList.Insert(index, h);
        }

        public bool ContainsId(int id)
        {
            return TransHedList.Contains(id);
        }
		#region IScrollLoadble implementation

        public void LoadData(int page)
        {

        }
		#endregion
    }
}