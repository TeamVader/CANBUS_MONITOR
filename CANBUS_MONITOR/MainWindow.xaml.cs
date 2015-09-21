#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlServerCe;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using System.Windows.Interop;


#if (DEBUG)
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Timers;
using System.Text.RegularExpressions;
using System.Globalization;


#endif

namespace CANBUS_MONITOR
{


    public class ListboxPIC : ListBox
    {
        
        public void addItem(string newitem)
        {
            this.Items.Add(newitem);
            if (this.Items.Count > 10)
            {
               // MessageBox.Show(this.Items.Count.ToString());
                this.Items.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

       
        #region SET YOUR USB Vendor and Product ID!
        // Vendor ID: 0x4d8 1240
        // Product ID: 0x0070 112
        //Use configuration #1, and interface #0 with endpoints 0x01 (out) and 0x81 (in).
        //All communication uses 64 byte interrupt transfers. 

        

        #endregion

        public DataTable datatables = new DataTable();
        public DataTable datatablescolumn = new DataTable();
        public DataTable dtconfiguration = new DataTable();
        public SqlCeDataAdapter sda = new SqlCeDataAdapter();
        public SqlCeCommandBuilder sbuilder = new SqlCeCommandBuilder();
        public SqlCeCommand cmdselect = new SqlCeCommand();
        private BackgroundWorker USB_Data_Gatherer = new BackgroundWorker();
        private BackgroundWorker CSV_Importer = new BackgroundWorker();
        private BackgroundWorker Datagrid_Filler = new BackgroundWorker();
        private BackgroundWorker USB_Event = new BackgroundWorker();
        private BackgroundWorker Baud_Rate_Detect = new BackgroundWorker();
        private BackgroundWorker SQLRecord_Inserter = new BackgroundWorker();
        private BackgroundWorker SQLRecord_Inserter2 = new BackgroundWorker();
        private BackgroundWorker Device_Programer = new BackgroundWorker();
        public CAN_Monitor_Classes.Connection_Monitoring CAN_Device_Notifications;
        private CAN_Monitoring_Device CAN_Device;
        private Bootloader_Device Bootloader_Device;
        private HexFile New_Firmware_Hex;
        private CAN_MESSAGE.CAN_ID CAN_ID_TEST = new CAN_MESSAGE.CAN_ID();
        private CAN_MESSAGE.CAN_DATA CAN_DATA_TEST = new CAN_MESSAGE.CAN_DATA();
        DataTable record1;
        DataTable record2;

        private Nodegroup Node_group = new Nodegroup();
        private CANopen.Node tempnode = new CANopen.Node();

        //private DispatcherTimer dispatcherTimer = new DispatcherTimer();
            // Create a 30 min timer 
        private System.Timers.Timer timer = new System.Timers.Timer();

    // Hook up the Elapsed event for the timer.
    



        //private MCP2

#if (DEBUG)
        private string time = "";
        #endif
        
        public MainWindow()
        {
            InitializeComponent();
               
               USB_Event.DoWork += USB_Event_DoWork;
               USB_Event.RunWorkerCompleted +=USB_Event_RunWorkerCompleted;

               USB_Data_Gatherer.DoWork += USB_Data_Gatherer_DoWork;
               USB_Data_Gatherer.WorkerReportsProgress = true;
               USB_Data_Gatherer.WorkerSupportsCancellation = true;
               USB_Data_Gatherer.ProgressChanged += USB_Data_Gatherer_ProgressChanged;
               USB_Data_Gatherer.RunWorkerCompleted += USB_Data_Gatherer_RunWorkerCompleted;

               SQLRecord_Inserter.DoWork += SQLRecord_Inserter_DoWork;
               SQLRecord_Inserter2.DoWork += SQLRecord_Inserter_DoWork;

               CSV_Importer.DoWork += CSV_Importer_DoWork;
               CSV_Importer.WorkerReportsProgress = true;
               CSV_Importer.WorkerSupportsCancellation = true;
               CSV_Importer.ProgressChanged += CSV_Importer_ProgressChanged;

               Baud_Rate_Detect.DoWork += Baud_Rate_Detect_DoWork;
               Baud_Rate_Detect.WorkerReportsProgress = true;
               Baud_Rate_Detect.WorkerSupportsCancellation = true;
               Baud_Rate_Detect.ProgressChanged += Baud_Rate_Detect_ProgressChanged;
               Baud_Rate_Detect.RunWorkerCompleted += Baud_Rate_Detect_RunWorkerCompleted;


               Device_Programer.DoWork += Program_Bootloader_device;
               Device_Programer.ProgressChanged += Device_Program_Progess_changed;
               Device_Programer.WorkerReportsProgress = true;
               
               Datagrid_Filler.DoWork += Datagrid_Filler_DoWork;
               Datagrid_Filler.RunWorkerCompleted += Datagrid_Filler_RunWorkerCompleted;

               CAN_Device_Notifications = new CAN_Monitor_Classes.Connection_Monitoring();
               CAN_Device_Notifications.SetRecording(false);
               CAN_Device_Notifications.SetConnection(false);
               CAN_Device_Notifications.SetConfigMode(false);
               CAN_Device_Notifications.SetisPrograming(false);
               CAN_Device_Notifications.SetNormalMode(false);

               DataContext = CAN_Device_Notifications;
               Listview_Nodes.ItemsSource = Node_group.Nodecollection;

               CAN_Device = new CAN_Monitoring_Device(0x04D8, 0x0070);
               Bootloader_Device = new Bootloader_Device(0x04D8, 0x003C);
               New_Firmware_Hex = new HexFile();
               timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
               timer.Interval = 1000;

               byte[] idtest = new byte[12];

               idtest[0] = 1;
               idtest[1] = 10;
               idtest[2] = 2;
               idtest[3] = 6;
               idtest[4] = 9;
               idtest[5] = 100;
               idtest[6] = 30;
               idtest[7] = 20;
               idtest[8] = 8;
               idtest[9] = 3;
               for (byte i = 0; i < 10; i++)
               {
                   CANopen.Node newnode = new CANopen.Node();

                   newnode.Node_ID = idtest[i];
                   newnode.Set_State(0x05);
                   Node_group.AddNode(newnode);
               }

               
             //
               
               // Add a listener for usb events
               CAN_Device.usbEvent += new CAN_Monitoring_Device.usbEventsHandler(usbEvent_receiver);
               Bootloader_Device.usbEvent += new CAN_Monitoring_Device.usbEventsHandler(usbEvent_receiver);


               // Perform an initial search for the target device
               CAN_Device.findTargetDevice();
               Bootloader_Device.findTargetDevice();
               
    

        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CAN_Monitor_Functions.dbcreate();
            datatables = CAN_Monitor_Functions.Get_Table_Names();
           // datatablescolumn = CAN_Monitor_Functions.Get_Column_Names();
            CAN_Monitor_Functions.SQL_Query(datatablescolumn, " select table_name from information_schema.columns where column_name like 'Configkeyid'");
            CAN_Monitor_Functions.update_combobox(datatables,ComboBoxtables,2);
            CAN_Monitor_Functions.update_combobox(datatablescolumn, Comboboxconfiguration, 0);
            //CAN_Monitor_Functions.DisplayData(datatablescolumn);
            CBO_OP_Mode.Items.Add("Normal Mode");
            CBO_OP_Mode.Items.Add("Sleep Mode");
            CBO_OP_Mode.Items.Add("Loopback Mode");
            CBO_OP_Mode.Items.Add("Listen Only Mode");
            CBO_OP_Mode.Items.Add("Configuration Mode");

            CBO_Bus_Speed.Items.Add("125 Kbit/sec");
            CBO_Bus_Speed.Items.Add("250 Kbit/sec");
            CBO_Bus_Speed.Items.Add("500 Kbit/sec");
            CBO_Bus_Speed.Items.Add("1000 Kbit/sec");
            //Checkbox_Autospeed.IsEnabled = false;
            record1 = CAN_Monitor_Functions.MakeRecordTable();
            record2 = CAN_Monitor_Functions.MakeRecordTable();
            timer.Enabled = true;
           // 
            CAN_Device_Informations();
            Check_isBusActive();
            Check_BusSpeed();

            CAN_ID_TEST.EID0 = 0x00;
            CAN_ID_TEST.SIDL = 0x00;
            CAN_ID_TEST.SIDH = 0x00;
            CAN_ID_TEST.EID8 = 0x00;
            CAN_DATA_TEST.D0 = 0x00;
            CAN_DATA_TEST.D1 = 0x00;
            CAN_DATA_TEST.D2 = 0x00;
            CAN_DATA_TEST.D3 = 0x00;
            CAN_DATA_TEST.D4 = 0x00;
            CAN_DATA_TEST.D5 = 0x00;
            CAN_DATA_TEST.D6 = 0x00;
            CAN_DATA_TEST.D7 = 0x00;

            

            //Run_Baud_Rate_Auto_Detect();
        }

        #region timer
         public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (CAN_Device.isDeviceAttached && !CAN_Device_Notifications.Recording && CAN_Device_Notifications.Mode == "Mode : Normal Operation Mode")
            {
                if (CAN_Device_Notifications.IsBusActive)
                {
                    //CAN_Device.MCP_2515_SPI_Write_Data(MCP2515_SPI_Commands.CAN_WRITE,
                }
                Check_isBusActive();
               
            }
             // MessageBox.Show(CAN_Device_Notifications.Mode);
        }
        #endregion 

        #region MCP_2515 Notifications

        private void Check_isBusActive()
        {
            if (CAN_Device.isDeviceAttached)
            {
                Byte temp = CAN_Device.MCP_2515_SPI_Status();
                if (temp == 0x03)
                {
                    CAN_Device_Notifications.SetisBusActive(true);
                    CAN_Device_Notifications.SetTraffic("Bus is active");


                    return;
                }

                CAN_Device_Notifications.SetisBusActive(false);
                CAN_Device_Notifications.SetTraffic("Bus is not active");
            }
            else
            {
                CAN_Device_Notifications.SetisBusActive(false);
                CAN_Device_Notifications.SetTraffic("");
            }
        }

        private void CAN_Device_Informations()
        {
            string Mode ;
            if (CAN_Device.isDeviceAttached)
            {
                Mode = CAN_Device.MCP_2515_Get_Operation_Mode();
                CAN_Device_Notifications.SetMode(Mode);

                switch (Mode)
                {
                    case  "Mode : Normal Operation Mode" :
                        CAN_Device_Notifications.SetNormalMode(true);
                        CAN_Device_Notifications.SetConfigMode(false);
                        break;
                    case "Mode : Configuration Mode":
                        CAN_Device_Notifications.SetNormalMode(false);
                        CAN_Device_Notifications.SetConfigMode(true);
                        break;
                    default:
                         CAN_Device_Notifications.SetNormalMode(false);
                        CAN_Device_Notifications.SetConfigMode(false);
                        break;
                }
            }
        }

        private void Check_Firmware_Version()
        {
            CAN_Device_Notifications.SetFirmware(CAN_Device.Check_Firmware_Version());
        }

        private void Check_BusSpeed()
        {
            if (CAN_Device.isDeviceAttached)
            {
               // CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_250kbps);

                CAN_Device_Notifications.SetSpeed(CAN_Device.MCP_2515_Get_Bus_Speed());
            }
            else
            {
                CAN_Device_Notifications.SetSpeed("");

            }
        }

        private void Run_Baud_Rate_Auto_Detect()
        {
            TabRecording.Focus();
            Baud_Rate_Detect.RunWorkerAsync();
        }

        private void Baud_Rate_Detect_DoWork(object sender, DoWorkEventArgs e)
        {
            int errorcounter = 0;
            string actual_speed = "";
            byte[] Bus_Speed_Array = new byte[] { Bus_Speed.CAN_250kbps, Bus_Speed.CAN_500kbps,Bus_Speed.CAN_125kbps , Bus_Speed.CAN_1000kbps };

            for(int j = 0;j <4;j++)
            {
            CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed_Array[j]);
            actual_speed = CAN_Device.MCP_2515_Get_Bus_Speed();
            (sender as BackgroundWorker).ReportProgress(0,actual_speed);
            (sender as BackgroundWorker).ReportProgress(0, "Checking for valid Messages..");


            for (int i = 0; i < 10; i++)
            {
                if (Error_Message())
                {
                    errorcounter++;
                //    (sender as BackgroundWorker).ReportProgress(0, string.Format("Message Error Nr {0}",i));
                }
                else
                {
                //    (sender as BackgroundWorker).ReportProgress(0,string.Format( "Valid Message Nr {0} received",i));
                }
                Thread.Sleep(500);
                
            }
            if (errorcounter == 0)
            {
                (sender as BackgroundWorker).ReportProgress(0, string.Format("No Error detected {0}", actual_speed));
                CAN_Device_Notifications.SetSpeed(actual_speed);
                break;
            }
            else
            {
                (sender as BackgroundWorker).ReportProgress(0, string.Format("{0} : {1} Error Messages received", actual_speed,errorcounter));
                errorcounter = 0;
            }
            }
                
            
        }

        private void Baud_Rate_Detect_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listboxrecording.addItem((string)e.UserState);
        }
        private void Baud_Rate_Detect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //listboxrecording.Items.Clear();
        }
        

        private bool Error_Message()
        {
            byte MRRF = new byte();
            CAN_Device.MCP_2515_SPI_Write_Data(0x00, MCP2515_Register.CANINTF);

             MRRF = CAN_Device.MCP_2515_SPI_Read(MCP2515_Register.CANINTF);

             if ((MRRF & 0x80) == 128)
             {
                 return true;
             }
             
             return false;
        }

        #endregion

        #region Bootloader Functions

        private int Query_Bootloader_Device()
        {

            Byte[] USB_packet = new Byte[65];

            int programspace = 0;
            int Program_start_adress = 0;
            USB_packet = Bootloader_Device.Query_Device_Packet();
            if (USB_packet != null)
            {
                Program_start_adress = BitConverter.ToInt32(USB_packet, 5);
                programspace = BitConverter.ToInt32(USB_packet, 9);

                ListboxPIC.addItem(" ");
                ListboxPIC.addItem(string.Format("Retrieved query data from Device"));
                ListboxPIC.addItem(string.Format("Program space : {0} Bytes (0x{0:X4})  Start Adress : 0x{1:X4}", programspace, Program_start_adress));
                return programspace;
                //Listbox_Scroll_End(ListboxPIC);
            }
            else
            {
                ListboxPIC.addItem("Failed to retrieve query data from PIC");
                return 0;
            }

        }

        private void Extended_Query_Bootloader_Device()
        {

            Byte[] USB_packet = new Byte[65];

            int Bootloader_version = 0;
            int Erase_Page_Size = 0;
            USB_packet = Bootloader_Device.Extended_Query_Device_Packet();
            if (USB_packet != null)
            {
                Erase_Page_Size = BitConverter.ToInt32(USB_packet, 12);
                Bootloader_version = BitConverter.ToInt16(USB_packet, 2);

                ListboxPIC.addItem(" ");
                ListboxPIC.addItem(string.Format("Retrieved extended query data from Device"));
                ListboxPIC.addItem(string.Format("Bootloader Version : {0}.{1}   Erase Page Size : {2} Bytes", Bootloader_version >> 8, (Bootloader_version & 0x00FF).ToString("D2"), Erase_Page_Size));
                //Listbox_Scroll_End(ListboxPIC);
            }
            else
            {
                ListboxPIC.addItem("Failed to retrieve extended query data from PIC");
            }

        }

        private void Erase_Bootloader_Device()
        {
            bool success;

             success = Bootloader_Device.Erase_Device();
             if (success)
             {
                 //ListboxPIC.addItem("");
                 //ListboxPIC.addItem("Device is successfully erased");
                 //Listbox_Scroll_End(ListboxPIC);
                 return;
             }
             ListboxPIC.addItem("");
             ListboxPIC.addItem("Erase Device failed");

        }

        private void Program_Bootloader_device( object sender, DoWorkEventArgs e)
        {


            HexFile hexdata = (HexFile)e.Argument;
            Byte[] USB_Packet = new Byte[65];
            Byte[] intBytes =new byte[4];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.PROGRAM_DEVICE; 
            int errorflag = 0;
           
            if (hexdata.ErrorCounter > 0)
            {
                (sender as BackgroundWorker).ReportProgress(0, "Error in Hexfile : cant load new firmware!");
                return;
            }
            CAN_Device_Notifications.SetisPrograming(true);
                for (int i = 0; i < hexdata.HexFileLines.Count; i++)
                // MessageBox.Show(hexdata.HexFileLines[1].Data[1].ToString());
                {
                    if (hexdata.HexFileLines[i].RecordType == HexFile.eRecordType.DataRecord && hexdata.HexFileLines[i].Address > 4000)
                    {
                        intBytes = BitConverter.GetBytes(hexdata.HexFileLines[i].Address);
                        
                        //if (BitConverter.IsLittleEndian)
                         //   Array.Reverse(intBytes);
                        USB_Packet[1] = Bootloader_USB_Commands.PROGRAM_DEVICE;
                        USB_Packet[2] = intBytes[0];
                        USB_Packet[3] = intBytes[1];
                        USB_Packet[4] = 0;
                        USB_Packet[5] = 0;
                        USB_Packet[6] = hexdata.HexFileLines[i].NumBytes;
                       // Debug.Print(string.Format("Adress : 0x{0:X4}  ", (USB_Packet[2] + (USB_Packet[3] << 8))));
                       // Debug.Print(string.Format("Number of Data Bytes : {0}  Bytes", USB_Packet[6]));
                        //Debug.Print(string.Format("Data Bytes : {0}  Bytes", hexdata.HexFileLines[i].NumBytes.ToString()));
                        //Debug.Print(string.Format("Adress : {0}", hexdata.HexFileLines[i].Address.ToString()));
                        
                        for (int j = 0; j < hexdata.HexFileLines[i].NumBytes; j++)
                        {
                            USB_Packet[(65 - hexdata.HexFileLines[i].NumBytes) + j] = hexdata.HexFileLines[i].Data[j];
                         //   Debug.Print(string.Format("Byte {0}  : 0x{1:X2}  ",j.ToString(), hexdata.HexFileLines[i].Data[j]));
                        }
                        
                        if (!Bootloader_Device.senddata(USB_Packet))
                        {
                            errorflag = 1;
                            break;
                        }
                        USB_Packet[1] = Bootloader_USB_Commands.PROGRAM_COMPLETE;
                        if (!Bootloader_Device.senddata(USB_Packet))
                        {
                            errorflag = 1;
                            break;
                        }
                         
                    }
                    //Bootloader_Device.Program_Data_Packet();
                }
              

                //Send Program Complete Command
                

                //Send Sign Flash Command
                USB_Packet[1] = Bootloader_USB_Commands.SIGN_FLASH;
                if (Bootloader_Device.senddata(USB_Packet))
                {

                    (sender as BackgroundWorker).ReportProgress(2, "Device Flash signed successfully");
                }
                else
                {
                    errorflag = 1;
                }
                USB_Packet[1] = Bootloader_USB_Commands.GET_DATA;
                USB_Packet[2] = 0x06; //APP_Signature_Adress
                USB_Packet[3] = 0x11;
                USB_Packet[4] = 0;
                USB_Packet[5] = 0;
                USB_Packet[6] = 0x02;
                if (Bootloader_Device.senddata(USB_Packet))
                {
                    USB_Packet = Bootloader_Device.readdata();
                    if (USB_Packet != null)
                    {
                        Debug.Print(string.Format("APP_SIGNATURE_VALUE : 0x{0:X4}", (USB_Packet[63] + (USB_Packet[64]<<8))  ));
                        if ((USB_Packet[63] + (USB_Packet[64] << 8)) == 0x600D)
                        {
                            (sender as BackgroundWorker).ReportProgress(3, "Device verified successfully");
                        }
                        else
                        {
                            (sender as BackgroundWorker).ReportProgress(3,"Device Flash Signature write failed");
                            errorflag = 1;
                            //ListboxPIC.addItem("Device Flash Signature write failed");
                        }
                    }
                }

                if (errorflag == 1)
                {
                    (sender as BackgroundWorker).ReportProgress(0, "Program Device failed");
                    Erase_Bootloader_Device();
                    (sender as BackgroundWorker).ReportProgress(0, "Device erased");

                    return;
                }
                else
                {
                    (sender as BackgroundWorker).ReportProgress(1, "Device is programmed");
                }

                CAN_Device_Notifications.SetisPrograming(false);

         }

        private void Device_Program_Progess_changed(object sender, ProgressChangedEventArgs e)
        {
                            ListboxPIC.addItem((string)e.UserState);
        }

        private void Listbox_Scroll_End(ListBox lsbox)
        {
            lsbox.ScrollIntoView(lsbox.Items[lsbox.Items.Count - 1]);
        }
        
        

        #endregion

        #region Button_Events

        private void Buttonclose_Click(object sender, RoutedEventArgs e)
        {
            // CAN_Monitor_Functions.clearusbdevice(CAN_Monitoring_Device);
            this.Close();
            Application.Current.Shutdown();

        }

        private void Buttonadd_Click(object sender, RoutedEventArgs e)
        {
            /*CAN_Monitor_Functions.Create_Measuring_Table(Textboxname.Text);
            datatables = CAN_Monitor_Functions.Get_Table_Names();
            CAN_Monitor_Functions.update_combobox(datatables, ComboBoxtables, 2);*/
            Random rd = new Random();

            Debug.Print("{0}",Node_group.NodeCount());

            CANopen.Node newnode = new CANopen.Node();
            newnode.Set_State(0x04);
            newnode.Node_ID = (byte)rd.Next(1, 127);
            Node_group.AddNode(newnode);
            Debug.Print("{0}", Node_group.NodeCount());

            //CAN_Device_Notifications.Setnodesize(200);
            //MessageBox.Show(newnode.State_Description);
            //newnode.Set_State(0x05);
           // MessageBox.Show(newnode.State_Description);


        }

        private void buttonstart_Click(object sender, RoutedEventArgs e)
        {


            listboxrecording.Items.Clear();

            if (!CAN_Device_Notifications.Recording && CAN_Device.isDeviceAttached)
            {
                //Check_isBusActive();
                //if (CAN_Device_Notifications.IsBusActive)
                //{
                    CAN_Device.Send_USB_Command(USB_User_Commands.Start_USB_Communication);
                    CAN_Device_Notifications.SetRecording(true);
                    USB_Data_Gatherer.RunWorkerAsync();
                //}
            }


        }

        private void buttonstop_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device_Notifications.Recording)
            {
                USB_Data_Gatherer.CancelAsync();
            }
            CAN_Device_Notifications.SetRecording(false);
            CAN_Device.Send_USB_Command(USB_User_Commands.Stop_USB_Communication);

        }




        private void Buttonmerge_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();




            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";
            dlg.InitialDirectory = "D:";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                prgbarcsv.Visibility = System.Windows.Visibility.Visible;
                if (Comboboxconfiguration.SelectedIndex != -1)
                {
                    CSV_Importer.RunWorkerAsync(dlg.FileName);
                }
            }

        }

        private void buttonbootloader_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device.isDeviceAttached)
            {
                ListboxPIC.addItem("Try to get Device in Bootloader Mode");
                CAN_Device.Send_USB_Command(USB_User_Commands.Entry_Bootloader);

                USB_Event.RunWorkerAsync();
            }
            else
            {
                ListboxPIC.addItem("Device is not attached or already in Bootloader Mode");
            }

        }

        private void buttonreset_Click(object sender, RoutedEventArgs e)
        {

            if (CAN_Device.isDeviceAttached)
            {
                CAN_Device.Send_USB_Command(USB_User_Commands.Reset_Device);
                ListboxPIC.addItem("Try to Reset CAN Device ");
                return;
            }


            if (Bootloader_Device.isDeviceAttached)
            {
                Bootloader_Device.Reset_Device();
                ListboxPIC.addItem("Try to Reset Bootloader Device ");
            }


        }

        private void buttonloadhex_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


            New_Firmware_Hex = new HexFile();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".hex";
            dlg.Filter = "HEX Files (*.hex)|*.hex";
            dlg.InitialDirectory = @"C:\Users\astark\MPLABXProjects\MCP2515DM-BM_Firmware\Source\dist\default\production";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            int i = 0;
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {

                try
                {
                    New_Firmware_Hex.LoadFile(dlg.FileName);

                    ListboxPIC.addItem(" ");
                    ListboxPIC.addItem(string.Format("New Hex File : {0} is loaded", New_Firmware_Hex.Filename.Substring(New_Firmware_Hex.Filename.LastIndexOf('\\') + 1)));
                    while (New_Firmware_Hex.HexFileLines[i].Address < 19)
                    {
                        i++;
                    }
                    i++;
                    ListboxPIC.addItem(string.Format("New Firmware Start Adress : 0x{0:X4}", New_Firmware_Hex.HexFileLines[i].Address));
                    ListboxPIC.addItem(string.Format("New Firmware Size : {0} Bytes", New_Firmware_Hex.Filesize));
                    ListboxPIC.addItem(string.Format("Number of Errors in Checksum : {0} ", New_Firmware_Hex.ErrorCounter));
                    if (New_Firmware_Hex.ErrorCounter != 0)
                    {
                        ListboxPIC.addItem("There are Errors detected in the Checksum of the Hex File ! ");
                    }

                    Listbox_Scroll_End(ListboxPIC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Fehler '{0}' beim Laden der Datei", ex.Message));
                    //return 1;
                }
                // Open document 

            }

        }

        private void buttonprogramdevice_Click(object sender, RoutedEventArgs e)
        {
            int program_space = 0;
            if (New_Firmware_Hex.Filename != "" && Bootloader_Device.isDeviceAttached && !CAN_Device_Notifications.IsPrograming)
            {
                program_space = Query_Bootloader_Device();
                Debug.Print(string.Format("Program space : {0}",program_space.ToString()));
                if (program_space <= New_Firmware_Hex.Filesize)
                {
                    ListboxPIC.addItem("Program Size to big");
                    return;
                }
                Extended_Query_Bootloader_Device();
                Erase_Bootloader_Device();
                Device_Programer.RunWorkerAsync(New_Firmware_Hex);
                

            }
            else
            {
                ListboxPIC.addItem("No HEX File selected or device not in Bootloader Mode");
            }
        }


        private void Buttonhelp_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Resources\MCP2515.pdf");
            Process P = new Process
            {
                StartInfo = { FileName = "AcroRd32.exe", Arguments = path }
            };
            P.Start();
        }

        private void ButtonsetFilter_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device_Notifications.Mode == "Mode : Configuration Mode")
            {

            }
            else
            {
                MessageBox.Show("Device must be in Configuration Mode");
            }
        }

        private void ButtonsetMask_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device_Notifications.Mode == "Mode : Configuration Mode")
            {
            }
            else
            {
                MessageBox.Show("Device must be in Configuration Mode");
            }
        }

        private void buttonresetnodes_Click(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0xff;
            CAN_ID_TEST.SIDH = 0xee;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
            Thread.Sleep(200);
        }

        private void buttonbaudrate_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device_Notifications.Mode == "Mode : Listen Only Mode")
            {
                Run_Baud_Rate_Auto_Detect();
            }
            else
            {
                listboxrecording.addItem("MCP2515 must be in listen only mode to auto detect baud rate");
            }
           
        }

        #endregion

        #region Combobox_Events

        private void ComboBoxtables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //ComboBoxItem requiredItem = (ComboBoxItem)ComboBoxtables.SelectedItem;
            //string value = requiredItem.Content.ToString();

            //MessageBox.Show(ComboBoxtables.SelectedValue.ToString());
            try
            {
                // MessageBox.Show(dt.Rows[ComboBoxtables.SelectedIndex][2].ToString());
                CAN_Monitor_Functions.filldatagrid(Datagridtest, datatables.Rows[ComboBoxtables.SelectedIndex][2].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Comboboxconfiguration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string datatablename = datatablescolumn.Rows[Comboboxconfiguration.SelectedIndex][0].ToString();
            Datagrid_Filler.RunWorkerAsync(datatablename);

            /* try
             {
                 //MessageBox.Show(Comboboxconfiguration.SelectedValue.ToString());
                 //MessageBox.Show(Comboboxconfiguration.SelectedIndex.ToString());
                 dtconfiguration = CAN_Monitor_Functions.Databaseadapter(sda, datatablescolumn.Rows[Comboboxconfiguration.SelectedIndex][0].ToString());
                 CAN_Monitor_Functions.filldatagrid_binding(Datagridconfiguration, dtconfiguration);
                 Datagridconfiguration.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                 //MessageBox.Show(dtconfiguration.TableName);



             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }*/
        }

        private void CBO_OP_Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CAN_Device.MCP_2515_Set_Operation_Mode(CBO_OP_Mode.SelectedIndex))
            {
                CAN_Device_Informations();

                
            }
            else
            {
                Debug.Print("Set Operation Mode failed");

            }
        }

        private void CBO_Bus_Speed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CBO_Bus_Speed.SelectedIndex)
            {
                case 0 :
                    CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_125kbps);        
                    break;
                case 1:
                    CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_250kbps);
                    break;
                case 2:
                    CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_500kbps);
                    break;
                case 3:
                    CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_1000kbps);
                    break;
            }
            Thread.Sleep(100);
            Check_BusSpeed();
        }
        #endregion

        #region Datagrid_events

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Datagridconfiguration_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {


        }


        private void Datagridconfiguration_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            MessageBox.Show("Hallo");
        }

        private void Datagridconfiguration_CurrentCellChanged(object sender, EventArgs e)
        {
            using (SqlCeConnection cn = new SqlCeConnection(CAN_Monitor_Functions._connstring))
            {
                try
                {
                    //MessageBox.Show(dtconfiguration.TableName);
                    sda = new SqlCeDataAdapter(string.Format("SELECT * FROM {0}", datatablescolumn.Rows[Comboboxconfiguration.SelectedIndex][0].ToString()), cn);

                    sbuilder = new SqlCeCommandBuilder(sda);

                    sda.Update(dtconfiguration);




                }
                catch (SqlCeException sqlex)
                {
                    MessageBox.Show(sqlex.Message, "fail with filling dataset");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        void Datagrid_Filler_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CAN_Monitor_Functions.filldatagrid_binding(Datagridconfiguration, dtconfiguration);
            Datagridconfiguration.Columns[0].Visibility = System.Windows.Visibility.Hidden;
        }
        void Datagrid_Filler_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //MessageBox.Show(Comboboxconfiguration.SelectedValue.ToString());
                //MessageBox.Show(Comboboxconfiguration.SelectedIndex.ToString());
                dtconfiguration = CAN_Monitor_Functions.Databaseadapter(sda, (string)e.Argument);
                //CAN_Monitor_Functions.filldatagrid_binding(Datagridconfiguration, dtconfiguration);
                //Datagridconfiguration.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                //MessageBox.Show(dtconfiguration.TableName);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        private void Can_Bus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

       

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region SQL_Database

        void SQLRecord_Inserter_DoWork(object sender, DoWorkEventArgs e)
        {
            //DataTable temp = (DataTable)e.Argument ;
            CAN_Monitor_Functions.BulkInsertfromRecord("TNT", CAN_Monitor_Functions._connstring, (DataTable)e.Argument);
        }
        #endregion

        #region USB_TASK

        void USB_Data_Gatherer_DoWork(object sender, DoWorkEventArgs e)
        {
            #if (DEBUG)
            Stopwatch stopwat = new Stopwatch();
            #endif

            byte[] readBuffer = new byte[64];
            byte dummy;
            int datalength;
            int counter = 0;
            bool extid = false;
            bool buffer1full = false;
           

            

            //int result = 0;
            int i = 0;
            #if (DEBUG)
            stopwat.Start();
            #endif
             
                    // i++;
                    //(sender as BackgroundWorker).ReportProgress(Convert.ToInt32(((double)4 / max) * 100),i);
                
               // System.Threading.Thread.Sleep(1);

           
             
             

             try
             {
                 // Find and open the usb device.
                 
                 /** CAN MESSAGE Protocoll
                  * 
                  * Byte 0: Most significant bit (bit 7) indicates the presence of a CAN message. Bit 5 indicates an extended (29 bit) identifier. Bit 4 indicates a remote transmission request (RTR). Bits 0-3 contain the data length.
                  * [Standard ID] Bytes 1-2: Byte 1 and bits 5-7 of byte 2 contain the identifier, big endian.
                  * [Extended ID] Bytes 1-4: From most significant to least significant, the message ID is in byte 1, byte 2 (bits 5-7), byte 2 (bits 0-1), byte 3, and byte 4.
                  * Next [len] bytes: If this is not an RTR message, the content of the message follows (up to 8 bytes).
                  * 
                  * 
                  * 
                  * 
                  * 
                  * 
                  * 
                  * 
                  * 
                  * **/
                 //get data over spi 0xA0
                  
                
                 while (!USB_Data_Gatherer.CancellationPending )
                 {
                    

                     // If the device hasn't sent data in the last 5 seconds,
                     // a timeout error (ec = IoTimedOut) will occur. 
                     readBuffer = CAN_Device.readdata();
                     if (readBuffer != null)
                     {
                         counter++;
                         (sender as BackgroundWorker).ReportProgress(counter, readBuffer);

                         if(counter%100==0)
                         {
                             
                            
                             //MessageBox.Show(buffer1full.ToString());
                             if (buffer1full)
                             {
                                 SQLRecord_Inserter.RunWorkerAsync(record2);
                             }
                             else
                             {
                                 SQLRecord_Inserter.RunWorkerAsync(record1);

                             }

                             buffer1full = !buffer1full;
                         }
                         if (!buffer1full)
                         {
                             CAN_Monitor_Functions.insertRecordRows(record1, readBuffer);
                         }
                         else
                         {
                             CAN_Monitor_Functions.insertRecordRows(record2, readBuffer);

                         }
                         

                     }
                     else
                     {
                         Debug.WriteLine("Device deattached");
                         break;
                     }
                     
                 }


                 
             }
             catch (Exception ex)
             {
                 
                 MessageBox.Show( ex.Message);
             }
             finally
             {

               
               
             }

             e.Cancel = true;

             CAN_Device.Send_USB_Command(USB_User_Commands.Stop_USB_Communication);
             //Thread.Sleep(1000);
             if (record2.Rows.Count > 0)
             {
                 SQLRecord_Inserter.RunWorkerAsync(record2);
             }
             else
             {
                 SQLRecord_Inserter.RunWorkerAsync(record1);

             }

             if (!CAN_Device.isDeviceAttached)
             {
                 CAN_Device.findTargetDevice();
             }
             //CAN_Device_Notifications.SetRecording(false);
            #if (DEBUG)
            stopwat.Stop();

            time = String.Format("TIME  = {0}", stopwat.Elapsed);
            //MessageBox.Show(time);
            #endif
        }



        

        

        void USB_Data_Gatherer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            byte[] readBuffer;
            readBuffer = e.UserState as byte[];
            int counter = (readBuffer[1] & 0x0F);
            string CAN_Message = "CAN Message : ";
            if (readBuffer != null)
            {

                //MessageBox.Show(readBuffer[0].ToString());
                
                //ID extended or Standard
                if((readBuffer[1] & 0x20) == 0x20)
                {
                    counter +=2;
                }
                

                for (int i = 2; i < counter+4; i++)
                {
                    CAN_Message += Convert.ToString(readBuffer[i], 16).PadLeft(2, '0') + "  ";
                    //CAN_Message += Convert.ToString(readBuffer[i], 16).PadLeft(2,'0') + "  ";
                    //lbtest.Items.Add(Convert.ToString(readBuffer[i], 2) + Convert.ToString(readBuffer[i],16));
                }
                
                listboxrecording.addItem(CAN_Message);
                lblnumber.Content = string.Format("CAN Messages received : {0} ", e.ProgressPercentage.ToString());

                //if (lbtest.Items.Count == 15)
                //{
                //  lbtest.Items.RemoveAt(lbtest.Items.Count - 15);
                //}
                //lbtest.SelectedIndex = lbtest.Items.Count - 1;
                //lbtest.ScrollIntoView(lbtest.SelectedItem);
            }
                
               // lbtest.InvalidateVisual();
                //if (lbtest.Items.Count == 40)
                //  lbtest.Items.Clear();
        }

        void USB_Data_Gatherer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CAN_Device_Notifications.SetRecording(false);
        }
        #endregion

        #region USB_Event
        void USB_Event_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
        }

        void USB_Event_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Bootloader_Device.isDeviceAttached)
            {
                ListboxPIC.addItem("Success : Device is in Bootloader Mode");

            }
            else
            {
                ListboxPIC.addItem("Failed : Device is not in Bootloader Mode");
            }
        }


        private void usbEvent_receiver(object o, EventArgs e)
        {

            if (CAN_Device.isDeviceAttached)
            {

                CAN_Device_Notifications.SetConnection(true);

                CAN_Device_Informations();
                Check_BusSpeed();
                Check_isBusActive();
                Check_Firmware_Version();
                //Checkbox_Termination_Res.IsChecked = false;

            }
            else if (Bootloader_Device.isDeviceAttached)
            {


                CAN_Device_Notifications.SetConnection(true);
                CAN_Device_Notifications.SetFirmware("");
                CAN_Device_Notifications.SetMode("Bootloader Mode");
                CAN_Device_Notifications.SetSpeed("");
                CAN_Device_Notifications.SetTraffic("");
            }

            else
            {
                CAN_Device_Notifications.SetConnection(false);

            }
            //CAN_Device_Informations();
        }

        #endregion

        #region CSV IMPORT
        void CSV_Importer_DoWork(object sender, DoWorkEventArgs e)
        {

            string filename = (string)e.Argument;
            Dictionary<string, Type> data_types = new Dictionary<string, Type>();
            //data_types["keyid"] = typeof(Int32);
            
            data_types["CANID"] = typeof(Int32);
            data_types["BinaryID"] = typeof(string);
            data_types["Assembly"] = typeof(string);
            data_types["Description"] = typeof(string);
            //MessageBox.Show(filename);
#if (DEBUG)
            Stopwatch stopwat = new Stopwatch();
#endif

            //MessageBox.Show(datatablescolumn.Rows[Comboboxconfiguration.SelectedIndex][0].ToString());

            //int result = 0;

#if (DEBUG)
            stopwat.Start();
#endif

            CAN_Monitor_Functions.BulkInsertFromCSV(filename, "Train1", CAN_Monitor_Functions._connstring, data_types, CSV_Importer);

#if (DEBUG)
            stopwat.Stop();

            time = String.Format("TIME  = {0}", stopwat.Elapsed);
            MessageBox.Show(time);
#endif
        }


        void CSV_Importer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgbarcsv.Value = e.ProgressPercentage;
            //  lbtest.Items.Clear();
        }

        void CSV_RunworkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //MessageBox.Show(Comboboxconfiguration.SelectedValue.ToString());
                //MessageBox.Show(Comboboxconfiguration.SelectedIndex.ToString());
                dtconfiguration = CAN_Monitor_Functions.Databaseadapter(sda, datatablescolumn.Rows[Comboboxconfiguration.SelectedIndex][0].ToString());
                //MessageBox.Show(dtconfiguration.TableName);
                CAN_Monitor_Functions.filldatagrid_binding(Datagridconfiguration, dtconfiguration);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
            //  lbtest.Items.Clear();
        }
        #endregion     

      


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[1] = Bootloader_USB_Commands.GET_DATA;
            USB_Packet[2] = 0x18; //APP_Signature_Adress
            USB_Packet[3] = 0x10;
            USB_Packet[4] = 0;
            USB_Packet[5] = 0;
            USB_Packet[6] = 0x20;
            if (Bootloader_Device.senddata(USB_Packet))
            {
                USB_Packet = Bootloader_Device.readdata();
                if (USB_Packet != null)
                {

                    Debug.Print(string.Format("APP_SIGNATURE_VALUE : 0x{0:X4}", (USB_Packet[63] + (USB_Packet[64]<<8))));
                    for (int i = 0; i < 65; i++)
                    {
                        Debug.Print(string.Format("Byte : 0x{0:X2}", (USB_Packet[i])));

                    }
                    //ListboxPIC.addItem("Device verified successfully");
                }
            }
           /* */
                
        }





        #region Checkbox Events

        private void CheckBoxedit_Checked(object sender, RoutedEventArgs e)
        {
            Datagridconfiguration.IsReadOnly = false;
        }

        private void CheckBoxedit_Unchecked(object sender, RoutedEventArgs e)
        {
            Datagridconfiguration.IsReadOnly = true;
        }

        private void Checkbox_Autospeed_Checked(object sender, RoutedEventArgs e)
        {
            
            CBO_Bus_Speed.IsEnabled = false;
        }

        private void Checkbox_Autospeed_Unchecked(object sender, RoutedEventArgs e)
        {
            
            CBO_Bus_Speed.IsEnabled = true;
        }

        private void Checkbox_Termination_Res_Unchecked(object sender, RoutedEventArgs e)
        {
            if(CAN_Device.isDeviceAttached && !CAN_Device_Notifications.Recording)
            CAN_Device.Send_USB_Command(USB_User_Commands.Disable_Term_resistor);
        }

        private void Checkbox_Termination_Res_Checked(object sender, RoutedEventArgs e)
        {
            if (CAN_Device.isDeviceAttached && !CAN_Device_Notifications.Recording)
            CAN_Device.Send_USB_Command(USB_User_Commands.Enable_Term_resistor);
        }

        private void Checkbox_Led25Pct_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;
            
            CAN_DATA_TEST.D0 = 0x01;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST,CAN_DATA_TEST,dlc);
        }

        private void Checkbox_Led25Pct_Unchecked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x01;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        private void Checkbox_Led50Pct_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x02;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        private void Checkbox_Led50Pct_Unchecked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x02;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        private void Checkbox_Led75Pct_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x03;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        private void Checkbox_Led75Pct_Unchecked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x03;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

       

        private void Checkbox_Led100Pct_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x04;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        private void Checkbox_Led100Pct_Unchecked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x04;
            CAN_ID_TEST.SIDH = 0xff;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
        }

        #endregion

        private void CBO_Buffernumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.Print(CBO_Buffernumber.SelectedIndex.ToString());
        }

        #region Textbox_Events
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-fA-F]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Textbox_New_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            int box_int = 0;
            UInt32 ubox_int=0;
            if (int.TryParse(Textbox_New_Value.Text, out box_int))
            {
                if (box_int <= Math.Pow(2, 29))
                {
                    Label_Binary_Value.Content = Convert.ToString(box_int, 2).PadLeft(29, '0');
                }
                else if(!Regex.IsMatch(Textbox_New_Value.Text, "^[01]+$"))
                {
                    Label_Binary_Value.Content = "Value to high";
                }
            }

            else if (UInt32.TryParse(Textbox_New_Value.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ubox_int))
            {
                if (ubox_int <= Math.Pow(2, 29))
                {

                    Label_Binary_Value.Content = Convert.ToString(ubox_int, 2).PadLeft(29, '0');
                }
                else
                {
                    Label_Binary_Value.Content = "Value to high";
                }
            }
            else if (Regex.IsMatch(Textbox_New_Value.Text, "^[01]+$"))
            {
                ubox_int = Convert.ToUInt32(Textbox_New_Value.Text, 2);
                if (ubox_int <= Math.Pow(2, 29))
                {

                    Label_Binary_Value.Content = Convert.ToString(ubox_int, 2).PadLeft(29, '0');
                }
                else
                {
                    Label_Binary_Value.Content = "Value to high";
                }
            }
            else
            {
                if (Textbox_New_Value.Text.Length > 1)
                {
                    Label_Binary_Value.Content = "Value to high";
                }
                else
                {
                    Label_Binary_Value.Content = "";
                }
            }
        }

        #endregion

        #region Radiobutton Events
        private void Radiobutton_Speed125_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x01;
            CAN_ID_TEST.SIDH = 0xee;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
            Thread.Sleep(200);
            CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_125kbps);
            Check_BusSpeed();

       }

        private void Radiobutton_Speed250_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x02;
            CAN_ID_TEST.SIDH = 0xee;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
            Thread.Sleep(200);
            CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_250kbps);
            Check_BusSpeed();

        }

        private void Radiobutton_Speed500_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x03;
            CAN_ID_TEST.SIDH = 0xee;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
            Thread.Sleep(200);
            CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_500kbps);
            Check_BusSpeed();
        }

        private void Radiobutton_Speed1000_Checked(object sender, RoutedEventArgs e)
        {
            byte dlc = 0x01;

            CAN_DATA_TEST.D0 = 0x04;
            CAN_ID_TEST.SIDH = 0xee;
            CAN_Device.Send_CAN_Message(CAN_ID_TEST, CAN_DATA_TEST, dlc);
            Thread.Sleep(200);
            CAN_Device.MCP_2515_Set_Bus_Speed(Bus_Speed.CAN_1000kbps);
            Check_BusSpeed();
        }

        #endregion

        private void buttonsetlistenmode_Click(object sender, RoutedEventArgs e)
        {
            if (CAN_Device_Notifications.Mode != "Mode : Listen Only Mode")
            {
                CAN_Device.MCP_2515_Set_Operation_Mode(3);
                Thread.Sleep(200);
                CAN_Device_Informations();
                //CAN_Device_Notifications.SetMode(CAN_Device.MCP_2515_Get_Operation_Mode());
            }
        }

        

       







    }
}
