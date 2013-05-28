using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;

namespace RetailMobile
{
    public class StatisticTabMonthly : BaseFragment
    {
        StatisticList allStatList;

        public static StatisticTabMonthly NewInstance(long objId)
        {
            var detailsFrag = new StatisticTabMonthly { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);

            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            View view = inflater.Inflate(Resource.Layout.StatisticTabMonthly, container, false);
            
            allStatList = StatisticList.GetStatisticListThisMonth(Activity, ObjectId);
			
//            DebugAddStatistics();
			
            ScrollView svStatistic = (ScrollView)view.FindViewById(Resource.Id.svStatistic);
            var tblLayout = CreateTableReport(view, allStatList);
            svStatistic.AddView(tblLayout);		
            return view;
        }

        static void SetCellStyle(TextView tv)
        {
            tv.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            tv.SetPadding(10, 5, 5, 2);
            tv.SetTextColor(Android.Content.Res.ColorStateList.ValueOf (Android.Graphics.Color.White));
            //			tv.SetBackgroundResource (Resource.Drawable.table_cell_bg);
        }

        static void SetHeaderCellStyle(TextView tvH)
        {
            tvH.Typeface = Android.Graphics.Typeface.DefaultBold;
            tvH.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            tvH.SetPadding(10, 5, 5, 2);
//			tvH.SetBackgroundResource (Resource.Drawable.table_header_cell_bg);
            tvH.SetTextColor(Android.Content.Res.ColorStateList.ValueOf (Android.Graphics.Color.White));
        }

        static TableLayout CreateTableReport(View view, StatisticList statList)
        {
            TableLayout.LayoutParams fillParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent, 1.0f);
            TableLayout tblLayout = new TableLayout(view.Context);
            tblLayout.LayoutParameters = fillParams;
            tblLayout.StretchAllColumns = true;

            TableRow rowHeader = new TableRow(view.Context);
            rowHeader.SetBackgroundResource(Resource.Drawable.actionbar_background);
			
            TextView tvItemKategH = new TextView(view.Context);
            tvItemKategH.Text = view.Resources.GetText(Resource.String.tvHeaderItemKategory);
            SetHeaderCellStyle(tvItemKategH);
            rowHeader.AddView(tvItemKategH);
			
            TextView tvAmountCurrH = new TextView(view.Context);
            tvAmountCurrH.Text = DateTime.Now.ToString("MMM yyyy");
            tvAmountCurrH.Gravity = GravityFlags.Right;
            SetHeaderCellStyle(tvAmountCurrH);
            rowHeader.AddView(tvAmountCurrH);
			
            TextView tvAmountPrevH = new TextView(view.Context);
            tvAmountPrevH.Text = DateTime.Now.AddYears(-1).ToString("MMM yyyy");
            tvAmountPrevH.Gravity = GravityFlags.Right;
            SetHeaderCellStyle(tvAmountPrevH);
            rowHeader.AddView(tvAmountPrevH);
			
            tblLayout.AddView(rowHeader);
			
            foreach (Statistic st in statList)
            {
                TableRow row = new TableRow(view.Context);
                row.SetBackgroundResource(Resource.Drawable.button_selector);
				
                TextView tvItemKateg = new TextView(view.Context);
                tvItemKateg.Text = st.ItemKategDesc;
                StatisticTabMonthly.SetCellStyle(tvItemKateg);
                row.AddView(tvItemKateg);
				
                TextView tvAmountCurr = new TextView(view.Context);
                tvAmountCurr.Gravity = GravityFlags.Right;
                tvAmountCurr.Text = st.AmountCurrText;
                StatisticTabMonthly.SetCellStyle(tvAmountCurr);
                row.AddView(tvAmountCurr);
				
                TextView tvAmountPrev = new TextView(view.Context);
                tvAmountPrev.Gravity = GravityFlags.Right;
                tvAmountPrev.Text = st.AmountPrevText;
                StatisticTabMonthly.SetCellStyle(tvAmountPrev);
                row.AddView(tvAmountPrev);
				
                tblLayout.AddView(row);
            }
			
            return tblLayout;
        }
    }
}