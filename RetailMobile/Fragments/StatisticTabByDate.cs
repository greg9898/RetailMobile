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
using Android.Support.V4.App;
using RetailMobile.Library;

namespace RetailMobile
{
	public class StatisticTabByDate : BaseFragment
	{
		StatisticList allStatList;

		public static StatisticTabByDate NewInstance (long objId)
		{
			var detailsFrag = new StatisticTabByDate { Arguments = new Bundle() };
			detailsFrag.Arguments.PutLong ("ObjectId", objId);

			return detailsFrag;
		}   

		public int CustId {
			get { return Arguments.GetInt ("ObjectId", -1); }
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (container == null) {
				// Currently in a layout without a container, so no reason to create our view.
				return null;
			}

			View view = inflater.Inflate (Resource.Layout.StatisticTabByDate, container, false);
            
			allStatList = StatisticList.GetStatisticList (Activity, CustId);
			
			DebugAddStatistics ();

			ScrollView svStatistic = (ScrollView)view.FindViewById (Resource.Id.svStatisticByDate);
			var tblLayout = CreateTableReport (view, allStatList);
//			svStatistic.AddView (tblLayout);
			TextView tv = new TextView (view.Context);
			tv.Text = "Tab2 by date TODO..";
			svStatistic.AddView (tv);
			return view;
		}
		static void SetCellStyle (TextView tv)
		{
			tv.SetTextSize (Android.Util.ComplexUnitType.Dip, 12);
			tv.SetPadding (10, 5, 5, 2);
			tv.SetBackgroundResource (Resource.Drawable.table_cell_bg);
		}
		
		static void SetHeaderCellStyle (TextView tvH)
		{
			tvH.Typeface = Android.Graphics.Typeface.DefaultBold;
			tvH.SetTextSize (Android.Util.ComplexUnitType.Dip, 12);
			tvH.SetPadding (10, 5, 5, 2);
			tvH.SetBackgroundResource (Resource.Drawable.table_header_cell_bg);
			tvH.SetTextColor (Android.Content.Res.ColorStateList.ValueOf (Android.Graphics.Color.White));
		}
		
		static TableLayout CreateTableReport (View view, StatisticList statList)
		{
			TableLayout tblLayout = new TableLayout (view.Context);
			
			TableRow rowHeader = new TableRow (view.Context);
			
			TextView tvItemKategH = new TextView (view.Context);
			tvItemKategH.Text = view.Resources.GetText (Resource.String.tvHeaderItemKategory);
			SetHeaderCellStyle (tvItemKategH);
			rowHeader.AddView (tvItemKategH);
			
			TextView tvAmountCurrH = new TextView (view.Context);
			tvAmountCurrH.Text = DateTime.Now.ToString ("MMM yyyy");
			SetHeaderCellStyle (tvAmountCurrH);
			rowHeader.AddView (tvAmountCurrH);
			
			TextView tvAmountPrevH = new TextView (view.Context);
			tvAmountPrevH.Text = DateTime.Now.AddYears (-1).ToString ("MMM yyyy");
			SetHeaderCellStyle (tvAmountPrevH);
			rowHeader.AddView (tvAmountPrevH);
			
			tblLayout.AddView (rowHeader);
			
			foreach (Statistic st in statList) {
				TableRow row = new TableRow (view.Context);
				
				TextView tvItemKateg = new TextView (view.Context);
				tvItemKateg.Text = st.ItemKategDesc;
				SetCellStyle (tvItemKateg);
				row.AddView (tvItemKateg);
				
				TextView tvAmountCurr = new TextView (view.Context);
				tvAmountCurr.Text = st.AmountCurr.ToString ();
				SetCellStyle (tvAmountCurr);
				row.AddView (tvAmountCurr);
				
				TextView tvAmountPrev = new TextView (view.Context);
				tvAmountPrev.Text = st.AmountPrev.ToString ();
				SetCellStyle (tvAmountPrev);
				row.AddView (tvAmountPrev);
				
				tblLayout.AddView (row);
			}
			
			return tblLayout;
		}

		void DebugAddStatistics ()
		{
			Statistic s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 7;
			s.ItemKateg = 5;
			s.Month = 5;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 13;
			s.AmountPrev = 17;
			s.ItemKateg = 2;
			s.Month = 2;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 27;
			s.ItemKateg = 5;
			s.Month = 6;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 110;
			s.AmountPrev = 22;
			s.ItemKateg = 5;
			s.Month = 7;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 151;
			s.AmountPrev = 231;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 51;
			s.AmountPrev = 31;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 7;
			s.ItemKateg = 5;
			s.Month = 5;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 13;
			s.AmountPrev = 17;
			s.ItemKateg = 2;
			s.Month = 2;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 27;
			s.ItemKateg = 5;
			s.Month = 6;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 110;
			s.AmountPrev = 22;
			s.ItemKateg = 5;
			s.Month = 7;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 151;
			s.AmountPrev = 231;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 51;
			s.AmountPrev = 31;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 7;
			s.ItemKateg = 5;
			s.Month = 5;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 13;
			s.AmountPrev = 17;
			s.ItemKateg = 2;
			s.Month = 2;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 27;
			s.ItemKateg = 5;
			s.Month = 6;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 110;
			s.AmountPrev = 22;
			s.ItemKateg = 5;
			s.Month = 7;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 151;
			s.AmountPrev = 231;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 51;
			s.AmountPrev = 31;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 10;
			s.AmountPrev = 27;
			s.ItemKateg = 5;
			s.Month = 5;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 13;
			s.AmountPrev = 17;
			s.ItemKateg = 2;
			s.Month = 2;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 210;
			s.AmountPrev = 27;
			s.ItemKateg = 5;
			s.Month = 6;
			allStatList.Add (s);
			
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 110;
			s.AmountPrev = 22;
			s.ItemKateg = 5;
			s.Month = 7;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 151;
			s.AmountPrev = 231;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
			s = new Statistic ();
			s.CstId = CustId;
			s.AmountCurr = 51;
			s.AmountPrev = 31;
			s.ItemKateg = 3;
			s.Month = 1;
			allStatList.Add (s);
		}
	}
}