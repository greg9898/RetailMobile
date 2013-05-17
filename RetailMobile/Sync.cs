using System;
using Android.Content;
using Com.Ianywhere.Ultralitejni12;
using Android.Util;

namespace RetailMobile
{
    public class Sync
    {
        public static string DatabaseName = "RetailMobile1.udb";

        public static IConnection GetConnection(Context ctx)
        {
            IConnection DBConnection = null;
            IConfigPersistent Config = DatabaseManager.CreateConfigurationFileAndroid(DatabaseName, ctx);

            // Connect to the database - CreateDatabase creates a new database
            DBConnection = DatabaseManager.Connect(Config);
            //DBConnection = DatabaseManager.CreateDatabase(Config);
            return DBConnection;
        }

        const string createTableRcustomer = @"
CREATE TABLE IF NOT EXISTS rcustomer (
id INTEGER NOT NULL,
cst_cod VARCHAR(40) NOT NULL,
cst_desc VARCHAR(120) NOT NULL,
cst_ypol NUMERIC(16,6) NULL,
cst_kat_disc INTEGER NULL,
cst_tax_num VARCHAR(30) NULL,
cst_trus_id NUMERIC(6,0) NULL,
cst_addr VARCHAR(120) NULL,
cst_city VARCHAR(60) NULL,
cst_zip VARCHAR(10) NULL,
cst_phone VARCHAR(30) NULL,
cst_gsm VARCHAR(30) NULL,
cst_comments VARCHAR(2000) NULL,
PRIMARY KEY ( id ASC ))";
        const string createTableRdisc = @"
CREATE TABLE IF NOT EXISTS rdisc (
rdisc_id INTEGER NOT NULL,
cst_kat_disc INTEGER NOT NULL,
item_ctg_disc INTEGER NOT NULL,
rdisc_per NUMERIC(16,6) NOT NULL,
PRIMARY KEY ( rdisc_id ASC ))";
        const string createTableRitems = @"
CREATE TABLE IF NOT EXISTS ritems (
id INTEGER NOT NULL,
item_desc VARCHAR(120) NOT NULL,
unit_price NUMERIC(15,2) NULL,
item_cod VARCHAR(50) NULL,
item_vat INTEGER NULL,
item_qty_left NUMERIC(16,6) NULL,
item_ctg_id INTEGER NULL,
item_ctg_disc INTEGER NULL,
item_ctg2_id INTEGER NULL,
item_alter_desc VARCHAR(120) NULL,
item_image LONG BINARY,
PRIMARY KEY ( id ASC ))";
        const string createTableRtransHed = @"CREATE TABLE IF NOT EXISTS rtrans_hed (
id INTEGER NOT NULL DEFAULT AUTOINCREMENT,
cust_id INTEGER NOT NULL,
trans_date DATE NOT NULL,
vouch_id INTEGER NOT NULL,
voser_id INTEGER NOT NULL,
docnum NUMERIC(7,0) NOT NULL,
htrn_explanation VARCHAR(200) NULL,
PRIMARY KEY ( id ASC )
)";
        const string createTableRtransDet = @"CREATE TABLE IF NOT EXISTS rtrans_det (
id INTEGER NOT NULL DEFAULT AUTOINCREMENT,
htrn_id INTEGER NOT NULL,
dtrn_num INTEGER NOT NULL,
item_id INTEGER NOT NULL,
qty1 NUMERIC(16,6) NOT NULL,
unit_price NUMERIC(16,6) NULL,
disc_line1 NUMERIC(16,6) NULL,
net_value NUMERIC(16,6) NULL,
vat_value NUMERIC(16,6) NULL,
PRIMARY KEY ( id ASC )
)";
        const string CreateTableRstatistic = @"CREATE TABLE IF NOT EXISTS rstatistic (
cst_id INTEGER NOT NULL,
item_kateg INTEGER NOT NULL,
month INTEGER NOT NULL,
amount_curr NUMERIC(30,6) NULL,
amount_prev NUMERIC(30,6) NULL,
PRIMARY KEY ( cst_id ASC, item_kateg ASC, month ASC )
)";
        const string CreateTableRtransCust = @"CREATE TABLE IF NOT EXISTS rtranscust (
id INTEGER NOT NULL,
cst_id INTEGER NULL,
vouch_id INTEGER NULL,
voser_id INTEGER NULL,
docnum INTEGER NULL,
dtrn_type CHAR(1) NULL,
dtrn_net_value DECIMAL(14,2) NULL,
dtrn_vat_value DECIMAL(14,2) NULL,
dtrn_date datetime NULL,
htrn_id INTEGER NULL,
PRIMARY KEY ( id ASC )
)";

        public static void GenerateDatabase(Context ctx)
        {
            IConnection DBConnection = null;
            IConfigPersistent Config;
            IPreparedStatement PreparedStatement;
            try
            {
                // Create a configuration for the connection - parameters are the database name and the Android context
                Config = DatabaseManager.CreateConfigurationFileAndroid(DatabaseName, ctx);
                bool connected = false;
                try
                {
                    DBConnection = DatabaseManager.Connect(Config);
                    connected = true;
                }
                catch (Exception ex)
                {
                    Log.Error("Connection error", ex.Message);
                    connected = false;
                }
                if (connected)
                    return;

                // Connect to the database - CreateDatabase creates a new database
                DBConnection = DatabaseManager.CreateDatabase(Config);

                PreparedStatement = DBConnection.PrepareStatement(createTableRcustomer);
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(createTableRdisc);
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS ritem_categ (
id INTEGER NOT NULL,
item_categ_desc VARCHAR(120) NOT NULL,
PRIMARY KEY ( id ASC )
) ");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"
CREATE TABLE IF NOT EXISTS ritem_categ2 (
id INTEGER NOT NULL,
item_categ_desc VARCHAR(120) NULL,
PRIMARY KEY ( id ASC )
)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS ritemlast (
id INTEGER NOT NULL,
cst_id INTEGER NOT NULL,
item_id INTEGER NOT NULL,
last_date DATE NOT NULL,
PRIMARY KEY ( id ASC, item_id ASC )
)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(createTableRitems);
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS rmemo (
id INTEGER NOT NULL DEFAULT AUTOINCREMENT,
memo VARCHAR(4000) NULL,
memo_date TIMESTAMP NULL,
cst_id INTEGER NOT NULL,
PRIMARY KEY ( id ASC )
)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(CreateTableRstatistic);
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(CreateTableRtransCust);
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS rtrustees (
trus_id NUMERIC(6,0) NOT NULL,
trus_cod VARCHAR(10) NOT NULL,
trus_desc VARCHAR(120) NOT NULL,
PRIMARY KEY ( trus_id ASC )
)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS rusers (
deal_id integer NOT NULL,
user_id integer NOT NULL,
login_name VARCHAR(120) NOT NULL,
user_pass VARCHAR(120) NOT NULL,
user_active VARCHAR(120) NOT NULL,
PRIMARY KEY ( deal_id ASC )
)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(createTableRtransDet);

                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(createTableRtransHed);

                PreparedStatement.Execute();
                PreparedStatement.Close();

                PreparedStatement = DBConnection.PrepareStatement(@" CREATE PUBLICATION pblUploadTrans (
TABLE rtrans_hed, TABLE rtrans_det)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                //TABLE contact,
                PreparedStatement = DBConnection.PrepareStatement(@" CREATE PUBLICATION pblUsers (
TABLE rusers)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                //TABLE contact,
                PreparedStatement = DBConnection.PrepareStatement(@" CREATE PUBLICATION pblMain1 (
TABLE rcustomer,
TABLE rdisc,
TABLE ritem_categ,
TABLE ritem_categ2,
TABLE ritemlast,
TABLE ritems,
TABLE rmemo,
TABLE rstatistic,
TABLE rtranscust,
TABLE rtrustees,
TABLE rtrans_det,
TABLE rtrans_hed,
TABLE rusers)");
                PreparedStatement.Execute();
                PreparedStatement.Close();

                DBConnection.Commit();
            }
            catch (Java.Lang.Exception ex)
            {
                // Log any errors to the device debug log
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            }
            finally
            {
                DBConnection.Release();
            }
        }

        public static void Synchronize(Context ctx)
        {
            IConnection cn = GetConnection(ctx);
            try
            {
                //SyncParms.HTTP_STREAM, "sa", "Courier109"
                SyncParms syncParams = cn.CreateSyncParms(0, "sa", PreferencesUtil.SyncModel);
                syncParams.Publications = "pblMain1";

                IStreamHTTPParms streamParams = syncParams.StreamParms;
                streamParams.Host = PreferencesUtil.IP;
                streamParams.Port = PreferencesUtil.Port;
                syncParams.AuthenticationParms = Common.CurrentDealerID.ToString();

                cn.Synchronize(syncParams);
                cn.Commit();

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            }
            finally
            {
                cn.Release();
            }
        }

        public static void SyncUsers(Context ctx)
        {
            IConnection cn = GetConnection(ctx);
            try
            {
                //SyncParms.HTTP_STREAM, "sa", "Courier109"
                SyncParms syncParams = cn.CreateSyncParms(0, "sa", PreferencesUtil.SyncModel);
                syncParams.Publications = "pblUsers";

                IStreamHTTPParms streamParams = syncParams.StreamParms;
                streamParams.Host = PreferencesUtil.IP;
                streamParams.Port = PreferencesUtil.Port;

                cn.Synchronize(syncParams);
                cn.Commit();

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            }
            finally
            {
                cn.Release();
            }
        }

        public static void SyncTrans(Context ctx)
        {
            IConnection cn = GetConnection(ctx);
            try
            {
                //SyncParms.HTTP_STREAM, "sa", "Courier109"
                SyncParms syncParams = cn.CreateSyncParms(0, "sa", PreferencesUtil.SyncModel);
                syncParams.Publications = "pblUploadTrans";

                IStreamHTTPParms streamParams = syncParams.StreamParms;
                streamParams.Host = PreferencesUtil.IP;
                streamParams.Port = PreferencesUtil.Port;

                cn.Synchronize(syncParams);
                cn.Commit();

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            }
            finally
            {
                cn.Release();
            }
        }
    }
}