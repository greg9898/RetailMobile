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
		
        // public static void CreateDatabase (Context ctx)
        // {
        // IConnection DBConnection = null;
        // IConfigPersistent Config;
        // IPreparedStatement PreparedStatement;
        // try {
        // // Create a configuration for the connection - parameters are the database name and the Android context
        // Config = DatabaseManager.CreateConfigurationFileAndroid (DatabaseName, ctx);
        // bool connected = false;
        // try {
        // DBConnection = DatabaseManager.Connect (Config);
        // connected = true;
        // } catch (Exception ex) {
        // connected = false;
        // }
        // if (connected)
        // return;
        //
        // // Connect to the database - CreateDatabase creates a new database
        // DBConnection = DatabaseManager.CreateDatabase (Config);
        //
        // // Create Users table
        // PreparedStatement = DBConnection.PrepareStatement (@"CREATE TABLE IF NOT EXISTS Users(
        // user_id numeric(6, 0) PRIMARY KEY DEFAULT AUTOINCREMENT,
        // user_cod varchar(15) NOT NULL,
        // user_pass varchar(60) NOT NULL,
        // first_name varchar(50) NULL,
        // last_name varchar(50) NULL,
        // login_name varchar(50) NOT NULL,
        // adname varchar(100) NULL,
        // ADSID varchar(50) NULL,
        // user_address varchar(100) NULL,
        // user_city varchar(60) NULL,
        // user_phone varchar(30) NULL,
        // user_mobile varchar(30) NULL,
        // user_group smallint NULL,
        // user_active smallint NULL)
        // ");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // // Create Items table
        // PreparedStatement = DBConnection.PrepareStatement (@" CREATE TABLE IF NOT EXISTS Items(
        // item_id NUMERIC(9,0) PRIMARY KEY DEFAULT AUTOINCREMENT,
        // comp_id numeric(6, 0) NOT NULL,
        // item_cod varchar(25) NOT NULL,
        // item_desc varchar(120) NOT NULL,
        // item_long_des varchar(200) NULL,
        // item_alter_cod varchar(30) NULL,
        // item_alter_desc varchar(60) NULL,
        // item_sound_desc varchar(60) NULL,
        // item_active varchar(1) NOT NULL,
        // vat_id numeric(6, 0) NULL,
        // cat1_id numeric(6, 0) NULL,
        // cat2_id numeric(6, 0) NULL,
        // cat3_id numeric(6, 0) NULL,
        // clsz_id numeric(6, 0) NULL,
        // item_cost_val numeric(16, 6) NULL,
        // item_lastcost_date datetime NULL,
        // meas_buy_id1 numeric(6, 0) NULL,
        // meas_buy_id2 numeric(6, 0) NULL,
        // item_buy_rel varchar(100) NULL,
        // item_buy_val1 numeric(16, 6) NULL,
        // item_buy_val2 numeric(16, 6) NULL,
        // item_buy_lastd datetime NULL,
        // meas_sale_id1 numeric(6, 0) NULL,
        // meas_sale_id2 numeric(6, 0) NULL,
        // item_sale_rel varchar(100) NULL,
        // item_sale_val1 numeric(16, 6) NULL,
        // item_sale_val2 numeric(16, 6) NULL,
        // item_sale_qty_def numeric(16, 6) NULL,
        // item_sale_lastd datetime NULL,
        // item_sale_markup numeric(5, 2) NULL,
        // item_sale_max_qty numeric(16, 6) NULL,
        // item_sale_max_disc numeric(5, 2) NULL,
        // meas_ret_id1 numeric(6, 0) NULL,
        // meas_ret_id2 numeric(6, 0) NULL,
        // item_ret_rel varchar(100) NULL,
        // item_ret_val1 numeric(16, 6) NULL,
        // item_ret_val2 numeric(16, 6) NULL,
        // item_ret_qty_def numeric(16, 6) NULL,
        // item_ret_lastd datetime NULL,
        // item_ret_markup numeric(5, 2) NULL,
        // item_ret_max_qty numeric(16, 6) NULL,
        // item_ret_max_disc numeric(5, 2) NULL,
        // item_qty_bal_q1 numeric(16, 6) NULL,
        // item_qty_bal_q2 numeric(16, 6) NULL,
        // item_qty_min_lim numeric(16, 6) NULL,
        // item_qty_ord_lim numeric(16, 6) NULL,
        // item_qty_max_lim numeric(16, 6) NULL,
        // item_itmgr_id numeric(18, 0) NULL,
        // item_comments varchar(1000) NULL,
        // TaxId numeric(6, 0) NULL,
        // item_sale_val3 numeric(16, 6) NULL,
        // item_type char(1) NULL,
        // item_orders_date datetime NULL,
        // vend_id numeric(9, 0) NULL )
        //
        //");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // // Create Customers table
        // PreparedStatement = DBConnection.PrepareStatement (@"CREATE TABLE IF NOT EXISTS customers(
        // cst_id numeric(9, 0) PRIMARY KEY DEFAULT AUTOINCREMENT,
        // comp_id numeric(6, 0) NOT NULL,
        // cst_cod varchar(40) NOT NULL,
        // cst_desc varchar(120) NOT NULL,
        // cst_alter_desc varchar(120) NULL,
        // cst_tax_num varchar(30) NULL,
        // cst_tax_stat varchar(2) NOT NULL,
        // cucat_id numeric(6, 0) NULL,
        // pay_id numeric(6, 0) NULL,
        // trus_id numeric(6, 0) NULL,
        // voc_id numeric(6, 0) NULL,
        // area_id numeric(6, 0) NULL,
        // coun_id numeric(6, 0) NULL,
        // cst_addr varchar(120) NULL,
        // cst_zip varchar(10) NULL,
        // cst_city varchar(60) NULL,
        // cst_phone varchar(30) NULL,
        // cst_gsm varchar(30) NULL,
        // cst_fax varchar(30) NULL,
        // cst_email varchar(60) NULL,
        // cst_web varchar(60) NULL,
        // cst_kepyo numeric(1, 0) NOT NULL,
        // cst_disc_perc numeric(5, 2) NULL,
        // cst_cred_lim numeric(14, 2) NULL,
        // cst_contact varchar(60) NULL,
        // cst_active varchar(1) NOT NULL,
        // cst_cstgr_id numeric(18, 0) NULL,
        // TaxID numeric(6, 0) NULL,
        // deal_id numeric(6, 0) NULL,
        // cst_comments varchar(2000) NULL,
        // AccountsId numeric(9, 0) NULL,
        // CustSalePrice numeric(1, 0) NULL,
        // CanInvoice varchar(1) NULL)
        //
        //");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // // Create TransHed table
        // PreparedStatement = DBConnection.PrepareStatement (@"CREATE TABLE IF NOT EXISTS trans_hed(
        // htrn_id numeric(9, 0) PRIMARY KEY DEFAULT AUTOINCREMENT,
        // comp_id numeric(6, 0) NULL,
        // bran_id numeric(6, 0) NULL,
        // store_id numeric(6, 0) NULL,
        // per_id numeric(6, 0) NULL,
        // htrn_date datetime NULL,
        // vend_id numeric(9, 0) NULL,
        // cst_id numeric(9, 0) NULL,
        // vouch_id numeric(6, 0) NULL,
        // voser_id numeric(6, 0) NULL,
        // htrn_docnum numeric(7, 0) NULL,
        // htrn_expl varchar(1000) NULL,
        // deal_id numeric(6, 0) NULL,
        // rout_id numeric(6, 0) NULL,
        // muadr_id numeric(6, 0) NULL,
        // htrn_del_name varchar(120) NULL,
        // htrn_del_addr varchar(120) NULL,
        // htrn_del_zip varchar(10) NULL,
        // htrn_del_city varchar(60) NULL,
        // htrn_del_phone varchar(30) NULL,
        // htrn_del_taxnum varchar(30) NULL,
        // htrn_del_trus_id numeric(6, 0) NULL,
        // htrn_print numeric(1, 0) NULL,
        // htrn_net_val numeric(14, 2) NULL,
        // htrn_vat_val numeric(14, 2) NULL,
        // htrn_cost_val numeric(14, 2) NULL,
        // htrn_tot_disc_eu numeric(14, 2) NULL,
        // htrn_tot_disc_perc numeric(5, 2) NULL,
        // htrn_rel_docs varchar(254) NULL,
        // pos_id numeric(6, 0) NULL,
        // User_id numeric(6, 0) NULL,
        // htrn_entry_date datetime NULL,
        // htrn_transformed numeric(1, 0) NULL,
        // hed_type numeric(2, 0) NULL,
        // rapid_sign varchar(320) NULL,
        // htrn_place_send varchar(400) NULL,
        // htrn_place_delivery varchar(400) NULL,
        // htrn_payment_way varchar(400) NULL,
        // htrn_entry_reason varchar(200) NULL,
        // store_id_to numeric(6, 0) NULL,
        // InvoiceCaseId numeric(18, 0) NULL,
        // htrn_order_date datetime NULL,
        // htrn_automobile_id numeric(6, 0) NULL,
        // htrn_driver_id numeric(6, 0) NULL,
        // htrn_transport_id numeric(6, 0) NULL,
        // cstmuadr_id numeric(6, 0) NULL,
        // vend_triangle_id numeric(9, 0) NULL)
        //");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // // Create TransDet table
        // PreparedStatement = DBConnection.PrepareStatement (@"CREATE TABLE IF NOT EXISTS trans_det(
        // dtrn_id numeric(9, 0) PRIMARY KEY DEFAULT AUTOINCREMENT,
        // htrn_id numeric(9, 0) NULL,
        // dtrn_num numeric(6, 0) NULL,
        // dtrn_type varchar(1) NULL,
        // item_id numeric(9, 0) NULL,
        // dtrn_imp_exp varchar(2) NULL,
        // dtrn_qty1 numeric(16, 6) NULL,
        // dtrn_qty2 numeric(16, 6) NULL,
        // dtrn_unit_price numeric(16, 6) NULL,
        // dtrn_disc_line1 numeric(5, 2) NULL,
        // dtrn_disc_val1 numeric(14, 2) NULL,
        // dtrn_disc_line2 numeric(5, 2) NULL,
        // dtrn_disc_val2 numeric(14, 2) NULL,
        // dtrn_disc_toteu numeric(14, 2) NULL,
        // dtrn_disc_totper numeric(14, 2) NULL,
        // dtrn_charge_net numeric(14, 2) NULL,
        // dtrn_charge_vat numeric(14, 2) NULL,
        // dtrn_net_value numeric(14, 2) NULL,
        // dtrn_vat_value numeric(14, 2) NULL,
        // dtrn_cost_value numeric(14, 2) NULL,
        // dtrn_memo varchar(254) NULL,
        // dtrn_disc_val3 numeric(14, 2) NULL,
        // store_id numeric(6, 0) NULL,
        // det_type numeric(2, 0) NULL,
        // dtrn_parent_id numeric(9, 0) NULL,
        // SetHeaderId numeric(6, 0) NULL,
        // dtrn_date_exp datetime NULL)
        //
        // ");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        // //IF NOT EXISTS
        // PreparedStatement = DBConnection.PrepareStatement (@" CREATE PUBLICATION pblUsers(TABLE users)");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // PreparedStatement = DBConnection.PrepareStatement (@" CREATE PUBLICATION pblMain1 (TABLE users, TABLE items, TABLE customers, TABLE trans_hed, TABLE trans_det)");
        // PreparedStatement.Execute ();
        // PreparedStatement.Close ();
        //
        // DBConnection.Commit ();
        // } catch (Java.Lang.Exception ex) {
        // // Log any errors to the device debug log
        // Android.Util.Log.Error ("UltraliteApplication", string.Format ("An error has occurred: {0}", ex.Message));
        // } finally {
        // DBConnection.Release ();
        // }
        // }
		
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
                } catch (Exception ex)
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
				
                PreparedStatement = DBConnection.PrepareStatement(@"CREATE TABLE IF NOT EXISTS rtranscust (
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
)");
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
				
                PreparedStatement = DBConnection.PrepareStatement(createTableRtransDet);
				
                PreparedStatement.Execute();
                PreparedStatement.Close();
				
                PreparedStatement = DBConnection.PrepareStatement(createTableRtransHed);
				
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
TABLE rtrans_hed)");
                PreparedStatement.Execute();
                PreparedStatement.Close();
				
                DBConnection.Commit();
            } catch (Java.Lang.Exception ex)
            {
                // Log any errors to the device debug log
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            } finally
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
				
                cn.Synchronize(syncParams);
                cn.Commit();
				
            } catch (Exception ex)
            {
                Android.Util.Log.Error("UltraliteApplication", string.Format("An error has occurred: {0}", ex.Message));
            } finally
            {
                cn.Release();
            }
        }
    }
}