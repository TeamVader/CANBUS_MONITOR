#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;




namespace CANBUS_MONITOR
{
    class CAN_Monitor_Functions
    {
        public static string _connstring;

        public static bool checkusername(string name)
        {
            try
            {
                SqlCeConnection con;
                con = new SqlCeConnection(_connstring);
                SqlCeDataReader read;
                string sqlselect = "SELECT * FROM tbluser " + "WHERE name='" + name + "';";
                SqlCeCommand cmd = new SqlCeCommand(sqlselect, con);
                con.Open();
                read = cmd.ExecuteReader();
                bool hasrows = read.Read();
                if (hasrows)
                {


                    return true;



                }
                else
                {

                    return false;
                }
            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        public static DataTable Databaseadapter(SqlCeDataAdapter sda , string tablename)
        {
            using(SqlCeConnection cn = new SqlCeConnection(_connstring))
            {
            //MessageBox.Show(tablename);
            sda = new SqlCeDataAdapter(string.Format("SELECT * FROM {0}", tablename ), cn);
            sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            SqlCeCommandBuilder sbuilder = new SqlCeCommandBuilder(sda);
            DataTable dt = new DataTable();


            try
            {


                sda.Fill(dt);
                

                
            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail with filling dataset");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {


            }
            return dt;
            }
        }

        public static bool checkuser(string name, string pwd)
        {
            try
            {
                SqlCeConnection con;
                con = new SqlCeConnection(_connstring);
                SqlCeDataReader reader;
                string sqlselect = "SELECT * FROM tbluser " + "WHERE name='" + name + "' AND pwd='" + pwd + "';";
                SqlCeCommand cmd = new SqlCeCommand(sqlselect, con);
                con.Open();
                reader = cmd.ExecuteReader();
                bool hasrows = reader.Read();
                if (hasrows)
                {


                    return true;



                }
                else
                {

                    return false;
                }
            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        /*public static void insertseries(Chart chartgraph, ComboBox chartstyle, string seriename)
        {
            chartgraph.Series.Clear();

            chartgraph.Series.Add(seriename);
            chartgraph.Series[0].ChartArea = "chartarea";
            chartgraph.Series[0].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), chartstyle.Text, true);

            chartgraph.Series[0].Points.AddXY(10, 12);
            chartgraph.Series[0].Points.AddXY(6, 2);
            chartgraph.Series[0].Points.AddXY(5, 8);
            chartgraph.Series[0].Points.AddXY(2, 15);
            chartgraph.Series[0].Points.AddXY(3, 22);
            chartgraph.Series[0].ToolTip = "Anfangskapital :#VALY\nJahre :#VALX";
        }*/

        public static void insertuser(string name, string pwd)
        {
            SqlCeConnection con;
            try
            {

                con = new SqlCeConnection(_connstring);



                SqlCeCommand cmd = new SqlCeCommand("INSERT INTO tbluser(name,pwd) VALUES(@NAMEs,@PWDs)", con);

                //cmd.Parameters.AddWithValue("@USERID", "1");
                cmd.Parameters.AddWithValue("@NAMEs", name);
                cmd.Parameters.AddWithValue("@PWDs", pwd);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }

            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                //con.Close();

            }


        }

        public static List<string> GetTables()
        {
            using (SqlCeConnection connection = new SqlCeConnection(_connstring))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                return TableNames;
            }
        }

        public static void update_combobox(DataTable dt,ComboBox combobox, int columnnumber)
        {

            //DisplayData(dt);
            
            //CAN_Monitor_Functions.filldatagrid( Datagridtest, "TNT");
            combobox.ItemsSource = dt.DefaultView;
            //combobox.val = dt.Columns[2].ToString();
            combobox.DisplayMemberPath = dt.Columns[columnnumber].ToString();
            //combobox.SelectedValuePath = dt.Columns[0].ToString();
        }

        
        public static DataTable Get_Table_Names()
        {

            SqlCeConnection cn = new SqlCeConnection(_connstring);
            DataTable dt = new DataTable();
            
            try
            {

                cn.Open();
                dt = cn.GetSchema("Tables");
                

            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();


            }

            return dt;
            
        }

        public static void Create_Configuration_Table(string configname)
        {
            using (SqlCeConnection cn = new SqlCeConnection(_connstring))
            {
                SqlCeCommand cmd;
                string sql = string.Format("CREATE TABLE {0} (Configkeyid INT IDENTITY NOT NULL PRIMARY KEY, CANID INT , BinaryID NVARCHAR(40),Assembly NVARCHAR(40) ,Description NVARCHAR(40))", configname);
                // + "userid int not null, "                                                                             VARBINARY(4)
                // + "name nvarchar(40),"
                //+ "pwd nvarchar(40))";
                //MessageBox.Show(_connstring);
                cmd = new SqlCeCommand(sql, cn);

                try
                {

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlCeException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "fail");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();

                }
            }
        }
        // usertablecreate für table in sql datenbank erstellen
        public static void Create_Measuring_Table(string name)
        {
            using (SqlCeConnection cn = new SqlCeConnection(_connstring))
            {
                SqlCeCommand cmd;
                string sql = string.Format("CREATE TABLE {0} (keyid INT IDENTITY NOT NULL PRIMARY KEY ,CANID INT,DLC INT, DATA NUMERIC(21,0))", name);
                // + "userid int not null, "
                // + "name nvarchar(40),"
                //+ "pwd nvarchar(40))";
                //MessageBox.Show(_connstring);
                cmd = new SqlCeCommand(sql, cn);

                try
                {

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlCeException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "fail");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();

                }
            }

        }

        public static void usertablecreate()
        {
            SqlCeConnection cn = new SqlCeConnection(_connstring);
            SqlCeCommand cmd;
            string sql = "CREATE TABLE tbluser (userid INT IDENTITY NOT NULL PRIMARY KEY ,name NVARCHAR(40),pwd NVARCHAR(40))";
            // + "userid int not null, "
            // + "name nvarchar(40),"
            //+ "pwd nvarchar(40))";
            //MessageBox.Show(_connstring);
            cmd = new SqlCeCommand(sql, cn);

            try
            {

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message, "fail");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();

            }

        }
        // dbcreate datenbank für user erstellen
        public static void dbcreate()
        {
            try
            {
                string psswd = "1234";
                string path = "C:\\ProgramData\\Stark_Industries";
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string connstring = path + "\\CAN_DB.sdf";
                _connstring = string.Format("Data Source = {0} ; Password = {1}", connstring, psswd);
                //_connstring = string.Format("Data Source = {0}" , connstring);

                
                if (System.IO.File.Exists(connstring))
                {

                }

                else
                {







                    //System.IO.File.Delete("user.sdf");



                    using (SqlCeEngine engine = new SqlCeEngine(_connstring))
                    {
                        engine.CreateDatabase();
                        usertablecreate();
                        Create_Configuration_Table("Train1");
                        Create_Configuration_Table("Train2");
                        Create_Measuring_Table("TNT");
                        Create_Measuring_Table("TNT2");
                        insertuser("admin", "admin");
                    }

                    //engine.LocalConnectionString = "Datasource = C:\\Test.sdf;Password = '<hallo>';";

                    //engine.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }


        }

        public static void filldatagrid_binding(DataGrid datagrid, DataTable datatable)
        {
                try
                {
                    datagrid.ItemsSource = datatable.DefaultView;
                }
                
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    

                }
            
        }
        public static void filldatagrid(DataGrid datagrid, String datatable)
        {
            using (SqlCeConnection cn = new SqlCeConnection(_connstring))
            {
                SqlCeDataAdapter sda = new SqlCeDataAdapter(string.Format("SELECT * FROM {0}", datatable), cn);
                DataTable dt = new DataTable();


                try
                {

                    cn.Open();
                    sda.Fill(dt);
                    datagrid.ItemsSource = dt.DefaultView;
                }
                catch (SqlCeException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "fail with filling dataset");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();

                }
            }
        }
        public static void SQL_Query(DataTable datatable, String SQLcommand)
        {
            using (SqlCeConnection cn = new SqlCeConnection(_connstring))
            {
                SqlCeDataAdapter sda = new SqlCeDataAdapter(SQLcommand, cn);



                try
                {

                    cn.Open();
                    sda.Fill(datatable);

                }
                catch (SqlCeException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "fail with filling dataset");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();

                }
            }
        }

        public static DataTable MakeRecordTable()
        // Create a new DataTable named NewProducts. 
        {
            DataTable newRecord = new DataTable("NewRecord");
            
            // Add three column objects to the table. 
            DataColumn productID = new DataColumn();
            productID.DataType = System.Type.GetType("System.Int32");
            productID.ColumnName = "CANID";
            //productID.AutoIncrement = true;
            newRecord.Columns.Add(productID);

            DataColumn DLC = new DataColumn();
            DLC.DataType = System.Type.GetType("System.Int32");
            DLC.ColumnName = "DLC";
            newRecord.Columns.Add(DLC);

            DataColumn DataByte = new DataColumn();
            DataByte.DataType = System.Type.GetType("System.Decimal");
            DataByte.ColumnName = "DATA";
            newRecord.Columns.Add(DataByte);
            
                
           
            
            // Add some new rows to the collection. 
            /*
            DataRow row = newRecord.NewRow();
            row["Name"] = "CC-101-WH";
            row["ProductNumber"] = "Cyclocomputer - White";

            newRecord.Rows.Add(row);
            row = newRecord.NewRow();
            row["Name"] = "CC-101-BK";
            row["ProductNumber"] = "Cyclocomputer - Black";

            newRecord.Rows.Add(row);
            row = newRecord.NewRow();
            row["Name"] = "CC-101-ST";
            row["ProductNumber"] = "Cyclocomputer - Stainless";
            newRecord.Rows.Add(row);
            newRecord.AcceptChanges();
            */
            // Return the new DataTable.  
            return newRecord;
        }

        public static void insertRecordRows(DataTable datatable, Byte[] bytearray)
        {
            DataRow row;
            int offset = 4;
            ulong data = 0;
            row = datatable.NewRow();
            if ((bytearray[1] & 0x20) == 0x20)
            {
                //Extended ID
                row["CANID"] = (bytearray[2] << 3) | (bytearray[3] >> 5) | (bytearray[4] << 19) | (bytearray[5] << 11) | ((bytearray[3] & 0x03) << 27);
                offset += 2;
            }
            else
            {
                //Standard ID
                row["CANID"] = (bytearray[2] << 3) | (bytearray[3] >> 5);
            }
            row["DLC"] = (bytearray[1] & 0x0F);

            for (int i = 0; i < (bytearray[1] & 0x0F); i++)
            {
                
                data += ((ulong)bytearray[offset + i] << (i * 8));
            }
           // Debug.Print(data.ToString("X16"));
            Debug.Print(data.ToString());


            row["DATA"] = data;

            datatable.Rows.Add(row);
            datatable.AcceptChanges();
        }


        public static void BulkInsertfromRecord(string table_name, string connection_string,DataTable datatable)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connection_string))
            {
                SqlCeCommand cmd = new SqlCeCommand();
                SqlCeResultSet rs;
                SqlCeUpdatableRecord rec;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = table_name;
                cmd.CommandType = CommandType.TableDirect;

                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                for(int i=0;i<datatable.Rows.Count;i++)
                {
               
                    rec = rs.CreateRecord();
                    rec.SetInt32(1, (Int32)datatable.Rows[i][0]);
                    rec.SetInt32(2, (Int32)datatable.Rows[i][1]);
                    rec.SetDecimal(3, (Decimal)datatable.Rows[i][2]);
                   /* rec.SetString(4, (String)datatable.Rows[i][3]);
                    rec.SetString(5, (String)datatable.Rows[i][4]);
                    rec.SetString(6, (String)datatable.Rows[i][5]);
                    rec.SetString(7, (String)datatable.Rows[i][6]);  OLD VALUES IN BYTES
                    rec.SetString(8, (String)datatable.Rows[i][7]);
                    rec.SetString(9, (String)datatable.Rows[i][8]);*/ 

                    rs.Insert(rec);
                    //rec.SetValues((obj)datatable.Rows[i]);
                }
                datatable.Clear();
                rs.Close();
                rs.Dispose();
                cmd.Dispose();
            }
        }

        public static void BulkInsertFromCSV(string file_path, string table_name, string connection_string, Dictionary<string, Type> data_types, BackgroundWorker backgroundworker)  //, BackgroundWorker backgroundworker
        {
            string line;
            List<string> column_names = new List<string>();
            using (StreamReader reader = new StreamReader(file_path))
            {
                line = reader.ReadLine();
                string[] texts = line.Split(' ');
                foreach (string txt in texts)
                {
                    //MessageBox.Show(txt);
                    column_names.Add(txt);
                }

                using (SqlCeConnection conn = new SqlCeConnection(connection_string))
                {
                    SqlCeCommand cmd = new SqlCeCommand();
                    SqlCeResultSet rs;
                    SqlCeUpdatableRecord rec;
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = table_name;
                    cmd.CommandType = CommandType.TableDirect;

                    rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                   // (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
                   
                    while ((line = reader.ReadLine()) != null)
                    {
                        texts = line.Split(' '); // '/t'

                        rec = rs.CreateRecord();

                        for (int j = 0; j < column_names.Count; ++j)
                        {
                            string columnName = column_names[j];

                            int ordinal = rec.GetOrdinal(columnName);
                            string param_value = "";
                            if (j < texts.Length)
                            {
                                param_value = texts[j];
                            }

                            Type data_type = data_types[columnName];
                            if (data_type == typeof(Int32))
                            {
                                Int32 value = 0;
                                int.TryParse(param_value, out value);
                                rec.SetInt32(ordinal, value);
                            }
                            else if (data_type == typeof(Int64))
                            {
                                Int64 value = 0;
                                Int64.TryParse(param_value, out value);
                                rec.SetInt64(ordinal, value);
                            }
                            else if (data_type == typeof(Int16))
                            {
                                Int16 value = 0;
                                Int16.TryParse(param_value, out value);
                                rec.SetInt16(ordinal, value);
                            }
                            else if (data_type == typeof(string))
                            {
                                rec.SetString(ordinal, param_value);
                            }
                            else if (data_type == typeof(double))
                            {
                                double value = 0;
                                double.TryParse(param_value, out value);
                                rec.SetDouble(ordinal, value);
                            }
                            else if (data_type == typeof(float))
                            {
                                float value = 0;
                                float.TryParse(param_value, out value);
                                rec.SetFloat(ordinal, value);
                            }
                            else if (data_type == typeof(DateTime))
                            {
                                DateTime value;
                                if (DateTime.TryParse(param_value, out value))
                                    rec.SetDateTime(ordinal, value);
                            }
                            else if (data_type == typeof(decimal))
                            {
                                decimal value;
                                decimal.TryParse(param_value, out value);
                                rec.SetDecimal(ordinal, value);
                            }
                            else if (data_type == typeof(Byte))
                            {
                                int temp;
                                int.TryParse(param_value, out temp);
                                Byte[] value = BitConverter.GetBytes(temp);
                                //Byte[].TryParse(param_value, out value);
                                //System.Buffer.BlockCopy(param_value.ToCharArray(), 0, value, 0, 4);
                                //value = GetBytes(param_value);
                               // MessageBox.Show(Convert.ToString(value[0], 16).PadLeft(2, '0'));
                                
                                //value =BitConverter. param_value;
                               // rec.SetByte(ordinal,  value[0]);
                                //rec.set
                                rec.SetBytes(ordinal,0, value,0,value.Length);
                            }
                        }
                       // MessageBox.Show( rec.ToString());
                        rs.Insert(rec);
                    }

                    rs.Close();
                    rs.Dispose();
                    cmd.Dispose();

                }
            }
        }


        public static byte[] GetBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }
        


        #if (DEBUG)
         public static void DisplayData(System.Data.DataTable table)
         {
            foreach (System.Data.DataRow row in table.Rows)
              {
              foreach (System.Data.DataColumn col in table.Columns)
                 {
                    MessageBox.Show(string.Format("{0} = {1}", col.ColumnName, row[col]));
                 }
                //Console.WriteLine("============================");
              }
         }
        #endif


    }
}
