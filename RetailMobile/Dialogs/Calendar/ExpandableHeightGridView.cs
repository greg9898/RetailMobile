
using Android.Widget;
using Android.Content;
using Android.Util;

namespace RetailMobile
{
    public class ExpandableHeightGridView : GridView
    {        
        bool expanded = false;
        
        public ExpandableHeightGridView(Context context):base(context)
        {
        }
        
        public ExpandableHeightGridView(Context context, IAttributeSet attrs)   :base(context, attrs)
        {
         
        }
        
        public ExpandableHeightGridView(Context context, IAttributeSet attrs, int defStyle) :base(context, attrs, defStyle)
        {
           
        }
        
        public bool IsExpanded()
        {
            return expanded;
        }
        
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (IsExpanded())
            {
                // Calculate entire height by providing a very large height hint.
                // But do not use the highest 2 bits of this integer; those are
                // reserved for the MeasureSpec mode.
                int expandSpec = MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, Android.Views.MeasureSpecMode.AtMost);
                base.OnMeasure(widthMeasureSpec, expandSpec);

                this.LayoutParameters.Height = MeasuredHeight;
            } else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }

        public void SetExpanded(bool isExpanded)
        {
            this.expanded = isExpanded;
        }
    }
}

