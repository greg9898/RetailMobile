using Android.OS;
using Android.Views;

namespace RetailMobile
{
    public class BaseFragment : Android.Support.V4.App.Fragment
    {
//        public static BaseFragment NewInstance(long objId)
//        {
//            var detailsFrag = new BaseFragment { Arguments = new Bundle() };
//            detailsFrag.Arguments.PutLong("ObjectId", objId);
//            return detailsFrag;
//        }

        public long ObjectId
        {
            get
            { 
                if (Arguments != null)
                    return Arguments.GetLong("ObjectId", 0);
                else
                    return 0;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}