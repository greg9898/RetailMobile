
using System.Collections.Generic;

using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace RetailMobile
{
    public class CalendarAdapter : BaseAdapter
    {
        static int FIRST_DAY_OF_WEEK = 0; // Sunday = 0, Monday = 1
        
        Context mContext;
        Calendar month;
        Calendar selectedDate;
        List<string> items;
        // references to our items
        public string[] days;

        public CalendarAdapter(Context c, Calendar monthCalendar)
        {
            month = monthCalendar;
            selectedDate = (Calendar)monthCalendar.Clone();
            mContext = c;
//            month.Set(CalendarField.DayOfMonth, 1);
            this.items = new List<string>();

            RefreshDays();
        }

        #region implemented abstract members of BaseAdapter

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView;
            TextView dayView;

            if (convertView == null)
            { // if it's not recycled, initialize some attributes
                LayoutInflater vi = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);
                v = vi.Inflate(Resource.Layout.calendar_item, null);
            }

            dayView = (TextView)v.FindViewById(Resource.Id.date);
            ImageView iw = (ImageView)v.FindViewById(Resource.Id.date_icon);

            // disable empty days from the beginning
            if (days[position] == "")
            {
                iw.Visibility = ViewStates.Invisible;
                dayView.Clickable = false;
                dayView.Focusable = false;
            } else
            {
                // mark current day as focused
                if (month.Get(CalendarField.Year) == selectedDate.Get(CalendarField.Year) && month.Get(CalendarField.Month) == selectedDate.Get(CalendarField.Month)
                    && days[position] == "" + selectedDate.Get(CalendarField.DayOfMonth))
                {
                    v.SetBackgroundResource(Resource.Drawable.button_selector);
                    iw.Visibility = ViewStates.Visible;
                } else
                {
                    v.SetBackgroundResource(Resource.Color.light_blue);
                    iw.Visibility = ViewStates.Invisible;
                }
            }

            dayView.Text = days[position];

            return v;
        }

        public override int Count
        {
            get
            {
                return days.Length;
            }
        }

        #endregion

        public void RefreshDays()
        {            
            int lastDay = month.GetActualMaximum(CalendarField.DayOfMonth);
            int firstDay = month.Get(CalendarField.DayOfWeek);
           
            // figure size of the array
            if (firstDay == 1)
            {
                days = new string[lastDay + (FIRST_DAY_OF_WEEK * 6)];
            } else
            {
                days = new string[lastDay + firstDay - (FIRST_DAY_OF_WEEK + 1)];
            }
            
            int j;
            
            // populate empty days before first real day
            if (firstDay > 1)
            {
                for (j = 0; j < firstDay - FIRST_DAY_OF_WEEK; j++)
                {
                    days[j] = "";
                }
            } else
            {
                for (j = 0; j < FIRST_DAY_OF_WEEK * 6; j++)
                {
                    days[j] = "";
                }

                j = FIRST_DAY_OF_WEEK * 6 + 1; // sunday => 1, monday => 7
            }
            
            // populate days
            int dayNumber = 1;
            for (int i = j - 1; i < days.Length; i++)
            {
                days[i] = "" + dayNumber;
                dayNumber++;
            }
        }
    }
}

