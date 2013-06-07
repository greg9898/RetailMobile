using System.Collections.Generic;

namespace RetailMobile.Library
{
    public class TransDetList:List<TransDet>
    {
        public TransDet GetByItemId(int itemId)
        {
            foreach (TransDet d in this)
            {
                if (d.ItemId == itemId)
                {
                    return d;
                }
            }

            return null;
        }
    }
}