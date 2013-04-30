
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Text.Format;
using Android.Widget;
using Java.Util;

namespace RetailMobile
{
    public class CalendarView : Dialog
    {        
        Context currentContext;
        public delegate void DateSelectedDelegate(System.DateTime date);
        public event DateSelectedDelegate DateSlected;
        
        public CalendarView(Context context, System.DateTime currentDate):base(context, Resource.Style.cust_dialog)
        {
            currentContext = context;
//            Window.RequestFeature(Android.Views.WindowFeatures.NoTitle);

            if (currentDate > new System.DateTime(1970, 1, 1, 0, 0, 0))
            {
                currentDate = currentDate.AddMonths(-1);

                month.Set(currentDate.Year, currentDate.Month, currentDate.Day);
            }
        }
        
        Calendar  month = Calendar.Instance;
        CalendarAdapter calendarAdapter;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.calendar);
//            month = Calendar.Instance;
            
            calendarAdapter = new CalendarAdapter(currentContext, month);

            GridView gridview = (GridView)FindViewById(Resource.Id.gridview);
            gridview.SetAdapter(calendarAdapter);

            TextView title = (TextView)FindViewById(Resource.Id.title);
            title.Text = DateFormat.Format("MMMM yyyy", month);
            
            TextView previous = (TextView)FindViewById(Resource.Id.previous);
            
            previous.Click += (sender, e) => {
                if (month.Get(CalendarField.Month) == month.GetActualMinimum(CalendarField.Month))
                {
                    month.Set((month.Get(CalendarField.Year) - 1), month.GetActualMaximum(CalendarField.Month), 1);
                } else
                {
                    month.Set(CalendarField.Month, month.Get(CalendarField.Month) - 1);
                }
                RefreshCalendar();
            };
            
            TextView next = (TextView)FindViewById(Resource.Id.next);
            next.Click += (sender, e) => {
                if (month.Get(CalendarField.Month) == month.GetActualMaximum(CalendarField.Month))
                {
                    month.Set((month.Get(CalendarField.Year) + 1), month.GetActualMinimum(CalendarField.Month), 1);
                } else
                {
                    month.Set(CalendarField.Month, month.Get(CalendarField.Month) + 1);
                }

                RefreshCalendar();                    
            };
            
            gridview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
            {   
                TextView date = (TextView)e.View.FindViewById(Resource.Id.date);
                    
                if (date != null && date.Text != "")
                {                        
                    Intent intent = new Intent();
                    string day = date.Text;
                      
                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }
                    // return chosen date as string format
                    string dateS = DateFormat.Format("yyyy-MM", month) + "-" + day;
                    // setResult(RESULT_OK, intent);
//                    selectedDate = DateHelper.GetDate(dateS);
                    System.DateTime selectedDate = System.DateTime.Parse(dateS);

                    if (DateSlected != null)
                        DateSlected(selectedDate);
                    Dismiss();
                }
                    
            };
        }
        
        public void RefreshCalendar()
        {
            TextView title = (TextView)FindViewById(Resource.Id.title);
            
            calendarAdapter.RefreshDays();
            calendarAdapter.NotifyDataSetChanged();
            
            title.Text = DateFormat.Format("MMMM yyyy", month);
        }
    }
}

