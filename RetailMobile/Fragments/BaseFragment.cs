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

namespace RetailMobile
{
    public class BaseFragment : Android.Support.V4.App.Fragment
    {
        //private static BaseFragment NewInstance(long objId)
        //{
        //    var detailsFrag = new BaseFragment { Arguments = new Bundle() };
        //    detailsFrag.Arguments.PutLong("ObjectId", objId);
        //    return detailsFrag;
        //}

        public long ObjectId
        {
            get { return Arguments.GetLong("ObjectId", 0); }
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