using System;
using Android.Content;
using Com.Ianywhere.Ultralitejni12;

namespace RetailMobile
{
    public class Error
    {
        public int ErrorID;
        public string Message;
        public string Stack;

        public Error(string message, string stack)
        {
            Message = message;
            Stack = stack;
        }

        public static void LogError(Context ctx, string message, string stack)
        {
            using (IConnection conn = Sync.GetConnection(ctx))
            {
                IPreparedStatement ps;

                ps = conn.PrepareStatement(@"INSERT INTO rerrors (message, stack, error_date) VALUES (:message, :stack, :error_date)");

                ps.Set("message", message);
                ps.Set("stack", stack);
                ps.Set("error_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                ps.Execute();

                ps.Close();

                conn.Commit();
                conn.Release();
            }
            Sync.SyncErrors(ctx);
        }
    }
}

