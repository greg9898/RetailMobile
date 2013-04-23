using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Com.Jjoe64.Graphview; 
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

        void DrawGraph(View view, System.Collections.Generic.IList<Statistic> statList)
        {
            GraphView.GraphViewData[] statGraphData = new GraphView.GraphViewData[statList.Count];
            string[] horizontalLabels = new string[statList.Count];
            string[] verticalLabels = new string[statList.Count];
          
            for (int i = 0; i < statList.Count; i++)
            {
                Statistic st = statList[i];
                statGraphData[i] = new GraphView.GraphViewData(st.ItemKateg, st.AmountCurr);
                horizontalLabels[i] = "Kateg " + st.ItemKateg.ToString();
                verticalLabels[i] = "AmountCurr " + st.Month.ToString();
            }

            // init example series data
            GraphViewSeries exampleSeries = new GraphViewSeries(statGraphData);
            // graph with dynamically genereated horizontal and vertical labels
            GraphView graphView = new BarGraphView(Activity, "GraphViewDemo");
            graphView.SetHorizontalLabels(horizontalLabels);
            graphView.SetVerticalLabels(verticalLabels);
            graphView.AddSeries(exampleSeries);
            graphView.SetScalable(false);
            //            graphView.SetViewPort(0, 10);
//            graphView.Orientation = Orientation.Vertical;
            LinearLayout layout = (LinearLayout)view.FindViewById(Resource.Id.graph1);
            layout.AddView(graphView);
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
            s.AmountCurr = 10;
            s.AmountPrev = 22;
            s.ItemKateg = 5;
            s.Month = 7;
            statList.Add(s);
                        
            s = new Statistic();
            s.CstId = customer.CustID;
            s.AmountCurr = 51;
            s.AmountPrev = 31;
            s.ItemKateg = 3;
            s.Month = 1;
            statList.Add(s);

            DrawGraph(view, statList);
                      
            return view;
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