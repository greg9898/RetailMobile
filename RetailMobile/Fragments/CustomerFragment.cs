using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
//using Com.Jjoe64.Graphview;
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


        /*    StatisticList statList = StatisticList.GetStatisticList(Activity, customer.CustID);

            // init example series data
            GraphViewSeries exampleSeries = new GraphViewSeries(new GraphView.GraphViewData[] {
                          new GraphView.GraphViewData(1, 2.0d)
                    ,     new GraphView.GraphViewData(2, 1.5d)
                        , new GraphView.GraphViewData(2.5, 3.0d) // another frequency                        
                        , new GraphView.GraphViewData(2.7, 3.5d) // another frequency
                        , new GraphView.GraphViewData(3, 2.5d)
                        , new GraphView.GraphViewData(4, 1.0d)
                        , new GraphView.GraphViewData(5, 3.0d)
            });
            
            // graph with dynamically genereated horizontal and vertical labels
            GraphView graphView = new BarGraphView(Activity, "GraphViewDemo");
            graphView.AddSeries(exampleSeries); // data
            graphView.SetScalable(true);
            graphView.Orientation = Orientation.Horizontal;

            LinearLayout layout = (LinearLayout)view.FindViewById(Resource.Id.graph1);
            layout.AddView(graphView);*/
          
//            // custom static labels
//            graphView.SetHorizontalLabels(new String[]
//            {
//                "2 days ago",
//                "yesterday",
//                "today",
//                "tomorrow"
//            });
//            graphView.SetVerticalLabels(new String[] {"high", "middle", "low"});
//            graphView.AddSeries(exampleSeries); // data
//            

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