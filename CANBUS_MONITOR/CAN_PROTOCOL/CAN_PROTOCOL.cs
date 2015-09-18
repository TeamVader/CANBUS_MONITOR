using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
    class CAN_OPEN_PROTOCOL
    {
        //
        public const Int16  NMT_node_control = 0; //0h All Slave receive
        //Identifier standard length 11 bit

        public const Int16 SYNC= 128 ; //Not implented
        public const Int16 time = 256 ; //100h  
        
        //Process Data Objects Standard Message with 8 Databytes
        public const Int16 PDO = 500 ;

        //SDO (Service Data Objects) is used for Receive_Transmission of variable Data length
        public const Int16 SDO_Transmit = 1408; //580h +Node ID
        public const Int16 SDO_Receive = 1536; //600h +Node ID

        public const Int16 Heartbeat_Message = 1792;   //700h plus ID CAN NODE
        //Example CAN NODE Nr 100 sends Heartbeat Message with Identifier : 764h / 1892

        /// <summary>
        /// Test
        /// </summary>
        public class Emergency
        {
            public const Int32 NoError          = 0x0000;
            public const Int32 NodefinedError   = 0x1000;
            public const Int32 CurrentError     = 0x2000;
            public const Int32 VoltageError     = 0x3000;
            public const Int32 TemperatureError = 0x4000;
            public const Int32 HardwareError    = 0x5000;
            public const Int32 SoftwareError    = 0x6000;
            public const Int32 ModuleError      = 0x7000;
            public const Int32 CommunicationError = 0x8000;
            public const Int32 ExternalError    = 0x9000;
            public const Int32 DevSpecificError = 0xFF00;      
        }
        
        
    }

    public class CAN_MESSAGE
    {

        /// <summary>
        /// CAN ID CLASS
        /// </summary>
        public class CAN_ID
        {
            public CAN_ID()
            {
            }
            public Byte SIDL { get; set; }
            public Byte SIDH { get; set; }
            public Byte EID0 { get; set; }
            public Byte EID8 { get; set; }
        }

        /// <summary>
        /// CAN DATA CLASS
        /// </summary>
        public class CAN_DATA
        {
            public CAN_DATA()
            {
            }
            public Byte D0 { get; set; }
            public Byte D1 { get; set; }
            public Byte D2 { get; set; }
            public Byte D3 { get; set; }
            public Byte D4 { get; set; }
            public Byte D5 { get; set; }
            public Byte D6 { get; set; }
            public Byte D7 { get; set; }
        }
    }
}
