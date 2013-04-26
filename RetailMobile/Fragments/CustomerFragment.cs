using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class CustomerFragment : BaseFragment
    {
        TextView tbCustCode;
        TextView tbCustName;
        CustomerInfo customer;
        
        public static CustomerFragment NewInstance(long objId)
        {
            var detailsFrag = new CustomerFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CustomerDetails, container, false);
            
            customer = CustomerInfo.GetCustomer(Activity, ObjectId);
            
            tbCustCode = (TextView)view.FindViewById(Resource.Id.tbCustomerCode);
            tbCustName = (TextView)view.FindViewById(Resource.Id.tbCustomerName);
            Button btnSave = (Button)view.FindViewById(Resource.Id.btnSave);
            btnSave.Click += new EventHandler(btnSave_Click);
            //get item by ItemId ..
            tbCustCode.Text = customer.Code;
            tbCustName.Text = customer.Name;
            
            
            StatisticList statList = StatisticList.GetStatisticList(Activity, customer.CustID);
            
            Statistic s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 10;
            s.AmountPrev = 7;
            s.ItemKateg = 5;
            s.Month = 2;
            statList.Add(s);
            
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 13;
            s.AmountPrev = 17;
            s.ItemKateg = 2;
            s.Month = 2;
            statList.Add(s);
            
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 10;
            s.AmountPrev = 27;
            s.ItemKateg = 5;
            s.Month = 6;
            statList.Add(s);
            
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 110;
            s.AmountPrev = 22;
            s.ItemKateg = 5;
            s.Month = 7;
            statList.Add(s);
            
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 151;
            s.AmountPrev = 231;
            s.ItemKateg = 3;
            s.Month = 1;
            statList.Add(s);
            
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 51;
            s.AmountPrev = 31;
            s.ItemKateg = 3;
            s.Month = 1;
            statList.Add(s);
            
            ScrollView svStatistic = (ScrollView)view.FindViewById(Resource.Id.svStatistic);
            var tblLayout = CreateTableReport(view, statList);
            svStatistic.AddView(tblLayout);
            
            return view;
        }
        
        static void SetCellStyle(TextView tv)
        {
            tv.SetTextSize(ComplexUnitType.Dip, 12);
            tv.SetPadding(10, 5, 5, 2);
            tv.SetBackgroundResource(Resource.Drawable.table_cell_bg);
        }
        
        static void SetHeaderCellStyle(TextView tvH)
        {
            tvH.Typeface = Android.Graphics.Typeface.DefaultBold;
            tvH.SetTextSize(ComplexUnitType.Dip, 12);
            tvH.SetPadding(10, 5, 5, 2);
            tvH.SetBackgroundResource(Resource.Drawable.table_header_cell_bg);
            tvH.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.White));
        }
        
        static void AddMonthsButtonsInFooter(View view, TableLayout tblLayout)
        {
            TableRow rowFooter = new TableRow(view.Context);
            
            LinearLayout llMonths1To4 = new LinearLayout(view.Context);
            llMonths1To4.Orientation = Orientation.Horizontal;
            rowFooter.AddView(llMonths1To4);
            
            Button btnMonth1 = new Button(view.Context);
            btnMonth1.Text = "Jan";
            btnMonth1.Tag = 1;
            btnMonth1.Click += btnMonthClick;
            llMonths1To4.AddView(btnMonth1);
            
            Button btnMonth2 = new Button(view.Context);
            btnMonth2.Text = "Feb";
            btnMonth2.Tag = 2;
            btnMonth2.Click += btnMonthClick;
            llMonths1To4.AddView(btnMonth2);
            
            Button btnMonth3 = new Button(view.Context);
            btnMonth3.Text = "Mar";
            btnMonth3.Tag = 3;
            btnMonth3.Click += btnMonthClick;
            llMonths1To4.AddView(btnMonth3);
            
            Button btnMonth4 = new Button(view.Context);
            btnMonth4.Text = "Apr";
            btnMonth4.Tag = 4;
            btnMonth4.Click += btnMonthClick;
            llMonths1To4.AddView(btnMonth4);
            
            LinearLayout llMonths5To8 = new LinearLayout(view.Context);
            llMonths5To8.Orientation = Orientation.Horizontal;
            rowFooter.AddView(llMonths5To8);
            
            Button btnMonth5 = new Button(view.Context);
            btnMonth5.Text = "May";
            btnMonth5.Tag = 5;
            btnMonth5.Click += btnMonthClick;
            llMonths5To8.AddView(btnMonth5);
            
            Button btnMonth6 = new Button(view.Context);
            btnMonth6.Text = "Jun";
            btnMonth6.Tag = 6;
            btnMonth6.Click += btnMonthClick;
            llMonths5To8.AddView(btnMonth6);
            
            Button btnMonth7 = new Button(view.Context);
            btnMonth7.Text = "Jul";
            btnMonth7.Tag = 7;
            btnMonth7.Click += btnMonthClick;
            llMonths5To8.AddView(btnMonth7);
            
            Button btnMonth8 = new Button(view.Context);
            btnMonth8.Text = "Aug";
            btnMonth8.Tag = 8;
            btnMonth8.Click += btnMonthClick;
            llMonths5To8.AddView(btnMonth8);
            
            LinearLayout llMonths9To12 = new LinearLayout(view.Context);
            llMonths9To12.Orientation = Orientation.Horizontal;
            rowFooter.AddView(llMonths9To12);
            
            Button btnMonth9 = new Button(view.Context);
            btnMonth9.Text = "Sep";
            btnMonth9.Tag = 9;
            btnMonth9.Click += btnMonthClick;
            llMonths9To12.AddView(btnMonth9);
            
            Button btnMonth10 = new Button(view.Context);
            btnMonth10.Text = "Oct";
            btnMonth10.Tag = 10;
            btnMonth10.Click += btnMonthClick;
            llMonths9To12.AddView(btnMonth10);
            
            Button btnMonth11 = new Button(view.Context);
            btnMonth11.Text = "Nov";
            btnMonth11.Tag = 11;
            btnMonth11.Click += btnMonthClick;
            llMonths9To12.AddView(btnMonth11);
            
            Button btnMonth12 = new Button(view.Context);
            btnMonth12.Text = "Dec";
            btnMonth12.Tag = 12;
            btnMonth12.Click += btnMonthClick;
            llMonths9To12.AddView(btnMonth12);
            
            tblLayout.AddView(rowFooter);
        }
        
        static void btnMonthClick(object sender, EventArgs e)
        {
            
        }
        
        static TableLayout CreateTableReport(View view, StatisticList statList)
        {
            TableLayout tblLayout = new TableLayout(view.Context);
            
            TableRow rowHeader = new TableRow(view.Context);
            
            TextView tvItemKategH = new TextView(view.Context);
            tvItemKategH.Text = view.Resources.GetText(Resource.String.tvHeaderItemKategory);
            SetHeaderCellStyle(tvItemKategH);
            rowHeader.AddView(tvItemKategH);
            
            TextView tvAmountCurrH = new TextView(view.Context);
            tvAmountCurrH.Text = view.Resources.GetText(Resource.String.tvHeaderAmountCurr);
            SetHeaderCellStyle(tvAmountCurrH);
            rowHeader.AddView(tvAmountCurrH);
            
            TextView tvAmountPrevH = new TextView(view.Context);
            tvAmountPrevH.Text = view.Resources.GetText(Resource.String.tvHeaderAmountPrev);
            SetHeaderCellStyle(tvAmountPrevH);
            rowHeader.AddView(tvAmountPrevH);
            
            tblLayout.AddView(rowHeader);
            
            foreach (Statistic st in statList)
            {
                TableRow row = new TableRow(view.Context);
                
                TextView tvItemKateg = new TextView(view.Context);
                tvItemKateg.Text = st.ItemKateg.ToString();
                SetCellStyle(tvItemKateg);
                row.AddView(tvItemKateg);
                
                TextView tvAmountCurr = new TextView(view.Context);
                tvAmountCurr.Text = st.AmountCurr.ToString();
                SetCellStyle(tvAmountCurr);
                row.AddView(tvAmountCurr);
                
                TextView tvAmountPrev = new TextView(view.Context);
                tvAmountPrev.Text = st.AmountPrev.ToString();
                SetCellStyle(tvAmountPrev);
                row.AddView(tvAmountPrev);
                
                tblLayout.AddView(row);
            }
            
            AddMonthsButtonsInFooter(view, tblLayout);
            
            return tblLayout;
        }
        
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                customer.Code = tbCustCode.Text;
                customer.Name = tbCustName.Text;
                
                customer.Save(Activity);
            } catch (Exception ex)
            {
                Log.Error("Save customer failed", ex.Message);
                throw ex;
            }
        }
        
    }
}