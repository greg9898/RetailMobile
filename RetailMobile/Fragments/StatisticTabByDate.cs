using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using System.Globalization;

namespace RetailMobile
{
    public class StatisticTabByDate : BaseFragment
    {
        EditText tbStatisticDateFrom;
        EditText tbStatisticDateTo;
        Button btnStatisticSearch;
        ScrollView svStatistic ;

        public static StatisticTabByDate NewInstance(long objId)
        {
            var detailsFrag = new StatisticTabByDate { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);

            return detailsFrag;
        }

        public int CustId
        {
            get { return Arguments.GetInt("ObjectId", -1); }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            View view = inflater.Inflate(Resource.Layout.StatisticTabByDate, container, false);
                     
            tbStatisticDateFrom = view.FindViewById<EditText>(Resource.Id.tbStatisticDateFrom);
            tbStatisticDateTo = view.FindViewById<EditText>(Resource.Id.tbStatisticDateTo);
            btnStatisticSearch = view.FindViewById<Button>(Resource.Id.btnStatisticSearch);

            if (btnStatisticSearch != null)
            {
                btnStatisticSearch.Click += btnStatisticSearch_Click;
            }

            if (tbStatisticDateFrom != null)
            {
                tbStatisticDateFrom.Focusable = false;
                tbStatisticDateFrom.Click += (object s, EventArgs e) => {
                    ShowCalendar(view.Context, tbStatisticDateFrom);
                };
            }

            if (tbStatisticDateTo != null)
            {
                tbStatisticDateTo.Focusable = false;
                tbStatisticDateTo.Click += (object s, EventArgs e) => {
                    ShowCalendar(view.Context, tbStatisticDateTo);
                };
            }

            return view;
        }

        void   btnStatisticSearch_Click(object sender, EventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            DateTime.TryParseExact(tbStatisticDateFrom.Text, PreferencesUtil.DateFormatDateOnly, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtFrom);
            DateTime.TryParseExact(tbStatisticDateTo.Text, PreferencesUtil.DateFormatDateOnly, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtTo);

            TransCustList transCustList = TransCustList.GetTransCustListStatistic(this.Activity.ApplicationContext, new TransCustList.Criteria()
            {  
                CustId = this.CustId, 
                DateFrom = dtFrom,
                DateTo = dtTo,
            });
            
            svStatistic = (ScrollView)this.Activity.FindViewById(Resource.Id.svStatisticByDate);
            var tblLayout = CreateTableReport(transCustList);
            svStatistic.AddView(tblLayout);
        }

        void ShowCalendar(Context ctx, TextView tvDate)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(tvDate.Text, PreferencesUtil.DateFormatDateOnly, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Now.Date;
            }

            CalendarView calendarDlg = new CalendarView(ctx, dt);
            calendarDlg.DateSlected += new CalendarView.DateSelectedDelegate(d => {
				tvDate.Text = d.ToString (PreferencesUtil.DateFormatDateOnly);
			});
            calendarDlg.Show();
        }

        static void SetCellStyle(TextView tv)
        {
            tv.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            tv.SetPadding(10, 5, 5, 2);
            tv.SetBackgroundResource(Resource.Drawable.table_cell_bg);
        }

        static void SetHeaderCellStyle(TextView tvH)
        {
            tvH.Typeface = Android.Graphics.Typeface.DefaultBold;
            tvH.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            tvH.SetPadding(10, 5, 5, 2);
            tvH.SetBackgroundResource(Resource.Drawable.table_header_cell_bg);
            tvH.SetTextColor(Android.Content.Res.ColorStateList.ValueOf (Android.Graphics.Color.White));
        }

        TableLayout CreateTableReport(TransCustList transCustList)
        {
            Context ctx = this.Activity.ApplicationContext;
            TableLayout tblLayout = new TableLayout(ctx);
			
            TableRow rowHeader = new TableRow(ctx);
			
            TextView tvItemKategH = new TextView(ctx);
            tvItemKategH.Text = "???";//view.Resources.GetText(Resource.String.tvHeaderItemKategory);
            SetHeaderCellStyle(tvItemKategH);
            rowHeader.AddView(tvItemKategH);
			
            TextView tvCreditH = new TextView(ctx);
            tvCreditH.Text = this.Activity.Resources.GetText(Resource.String.tvHeaderStatisticCredit);
            SetHeaderCellStyle(tvCreditH);
            rowHeader.AddView(tvCreditH);
			
            TextView tvDebitH = new TextView(ctx);
            tvDebitH.Text = this.Activity.Resources.GetText(Resource.String.tvHeaderStatisticDebit);
            SetHeaderCellStyle(tvDebitH);
            rowHeader.AddView(tvDebitH);

            TextView tvCreditMinusDebitH = new TextView(ctx);
            tvCreditMinusDebitH.Text = this.Activity.Resources.GetText(Resource.String.tvHeaderStatisticCreditMinusDebit);
            SetHeaderCellStyle(tvCreditMinusDebitH);
            rowHeader.AddView(tvCreditMinusDebitH);
			
            tblLayout.AddView(rowHeader);
			
            foreach (TransCust st in transCustList)
            {
                TableRow row = new TableRow(ctx);
				
                TextView tvItemKateg = new TextView(ctx);
                tvItemKateg.Text = "???";
                SetCellStyle(tvItemKateg);
                row.AddView(tvItemKateg);
				
                TextView tvCredit = new TextView(ctx);
                tvCredit.Text = st.Credit.ToString(PreferencesUtil.CurrencyFormat);
                SetCellStyle(tvCredit);
                row.AddView(tvCredit);
				
                TextView tvDebit = new TextView(ctx);
                tvDebit.Text = st.Debit.ToString(PreferencesUtil.CurrencyFormat);
                SetCellStyle(tvDebit);
                row.AddView(tvDebit);
                
                TextView tvCreditMinusDebit = new TextView(ctx);
                tvCreditMinusDebit.Text = st.CreditMinusDebit.ToString(PreferencesUtil.CurrencyFormat);
                SetCellStyle(tvCreditMinusDebit);
                row.AddView(tvCreditMinusDebit);
				
                tblLayout.AddView(row);
            }
			
            return tblLayout;
        }
    }
}