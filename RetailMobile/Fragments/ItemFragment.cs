using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RetailMobile.Library;
using Android.Util;

namespace RetailMobile
{
    public class ItemFragment : BaseFragment
    {
        ItemInfo item;
        RetailMobile.Fragments.ItemActionBar actionBar;

        public static ItemFragment NewInstance(long objId)
        {
            var detailsFrag = new ItemFragment { Arguments = new Bundle() };
            detailsFrag.Arguments.PutLong("ObjectId", objId);
            return detailsFrag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            actionBar = (RetailMobile.Fragments.ItemActionBar)this.Activity.SupportFragmentManager.FindFragmentById(Resource.Id.ActionBar);
            actionBar.SetTitle(this.Activity.GetString (Resource.String.miItems));

            item = ItemInfo.GetItem(Activity, ObjectId);

            View view = inflater.Inflate(Resource.Layout.ItemDetails, container, false);

            EditText tbItemCode = (EditText)view.FindViewById(Resource.Id.tbItemCode);
            EditText tbItemName = (EditText)view.FindViewById(Resource.Id.tbItemName);
            EditText tbLongDesc = (EditText)view.FindViewById(Resource.Id.tbLongDesc);
            EditText tbItemSaleVal = (EditText)view.FindViewById(Resource.Id.tbItemSaleVal);
            EditText tbItemQtyLeft = (EditText)view.FindViewById(Resource.Id.tbItemRetVal);
            ImageView imgItemDetail = (ImageView)view.FindViewById(Resource.Id.imgItemDetail);
            Button btnViewFullSize = (Button)view.FindViewById(Resource.Id.btnViewImage);

            tbItemCode.Text = item.item_cod;
            tbItemName.Text = item.ItemDesc;
            tbLongDesc.Text = item.item_long_desc;
            tbItemQtyLeft.Text = item.ItemQtyLeft.ToString("######0.0##");
            tbItemSaleVal.Text = item.ItemSaleVal1.ToString("######0.0##"); 
            imgItemDetail.SetImageBitmap(item.ItemImage);

            imgItemDetail.Click += new EventHandler(imgItemDetail_Click);
            btnViewFullSize.Click += new EventHandler(btnViewImage_Click);

            return view;
        }

        void btnViewImage_Click(object sender, EventArgs e)
        {

            string root = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "DCIM");
            //string root = Android.OS.Environment.DirectoryPictures.ToString();
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(root);
            if (!myDir.Exists)
                myDir.Create();
            //myDir.Equals,
            string fname = "imageRetail.jpg";
            string saveFile = System.IO.Path.Combine(myDir.FullName, fname);
            System.IO.FileInfo fi = new System.IO.FileInfo(saveFile);


            if (fi.Exists)
                fi.Delete();
            try
            {
                //System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile);

                System.IO.FileStream fs = new System.IO.FileStream(saveFile, System.IO.FileMode.Create);
                //ItemInfo.GetItem().ItemImage.Compress
                item.ItemImage.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, fs);

                fs.Flush();
                fs.Close();

                Intent intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.Parse("file://" + saveFile), "image/*");
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Log.Debug("btnViewImage_Click", ex.Message);
            }
        }

        void imgItemDetail_Click(object sender, EventArgs e)
        {
            ImageView myImage = new ImageView(Activity);
            myImage.SetImageResource(Resource.Drawable.night);

            Dialog dlgImg = new Dialog(Activity);
            dlgImg.Window.RequestFeature(WindowFeatures.NoTitle);
            dlgImg.SetContentView(myImage);

            myImage.Click += (s,ee) => {
                dlgImg.Dismiss();
            };
            dlgImg.Show();
        }
    }
}