
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

        public string[] Days
        {
            get
            {
                return days;
            }
            set
            {
                days = value;
            }
        }
        
        public CalendarAdapter(Context c, Calendar monthCalendar)
        {
            month = monthCalendar;
            selectedDate = (Calendar)monthCalendar.Clone();
            mContext = c;
            month.Set(CalendarField.DayOfMonth, 1);
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
            
            // disable empty days from the beginning
            if (days[position] == "")
            {
                dayView.Clickable = false;
                dayView.Focusable = false;
            } else
            {
                // mark current day as focused
                if (month.Get(CalendarField.Year) == selectedDate.Get(CalendarField.Year) && month.Get(CalendarField.Month) == selectedDate.Get(CalendarField.Month)
                    && days[position] == "" + selectedDate.Get(CalendarField.DayOfMonth))
                {
                    v.SetBackgroundResource(Resource.Drawable.button_selector);
                } else
                {
                    v.SetBackgroundResource(Resource.Color.light_blue);
                }
            }

            dayView.Text = days[position];
            
            // create date string for comparison
            string date = days[position];
            
            if (date.Length == 1)
            {
                date = "0" + date;
            }

            string monthStr = "" + (month.Get(CalendarField.Month) + 1);
            if (monthStr.Length == 1)
            {
                monthStr = "0" + monthStr;
            }
            
            // show icon if date is not empty and it exists in the items array
            ImageView iw = (ImageView)v.FindViewById(Resource.Id.date_icon);
            if (date.Length > 0 && items != null && items.Contains(date))
            {
                iw.Visibility = ViewStates.Visible;
                ;
            } else
            {
                iw.Visibility = ViewStates.Invisible;
            }
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

        public void SetItems(List<string> items)
        {
            for (int i = 0; i != items.Count; i++)
            {
                if (items[i].Length == 1)
                {
                    items[i] = "0" + items[i];
                }
            }

            this.items = items;
        }

        public void RefreshDays()
        {
            // clear items
            items.Clear();
            
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

