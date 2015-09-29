using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
    public partial class CANopen
    {

        #region COB ID FUNCTION CODES
        //Default COB ID Function Codes in the canopen DS301
        private const byte NMT = 0x0;
        private const byte SYNC = 0x1;
        private const byte TIME_STAMP = 0x2;
        private const byte TPDO1 = 0x3;
        private const byte RPDO1 = 0x4;
        private const byte TPDO2 = 0x5;
        private const byte RPDO2 = 0x6;
        private const byte TPDO3 = 0x7;
        private const byte RPDO3 = 0x8;
        private const byte TPDO4 = 0x9;
        private const byte RPDO4 = 0xA;
        private const byte TSDO = 0xB;
        private const byte RSDO = 0xC;
        private const byte NMT_EC = 0xE;
        private const byte LSS = 0xF;

        public class Default_Identifier_Setup
        {
            /// <summary>
            /// Communication profile DS-301
            ///</summary>
            public static Dictionary<Byte, string> Default_Identifier_Setup_Dict = new Dictionary<Byte, string>()
            {
                {NMT,"NMT"},
                {SYNC,"EMCY"},
                {TIME_STAMP,"TIME"},
                {TPDO1,"TPDO1"},
                {RPDO1,"RPDO1"},
                {TPDO2,"TPDO2"},
                {RPDO2,"RPDO2"},
                {TPDO3,"TPDO3"},
                {RPDO3,"RPDO3"},
                {TPDO4,"TPDO4"},
                {RPDO4,"RPDO4"},
                {TSDO,"TSDO"},
                {RSDO,"RSDO"},
                {NMT_EC,"NMT_EC"},
                {LSS,"LSS"}
              
            };

            public static string GetDescription(Byte ID)
            {
                // Try to get the result in the static Dictionary
                string result;
                if (Default_Identifier_Setup_Dict.TryGetValue(ID, out result))
                {
                    return result;
                }
                else
                {
                    return "n/a";
                }
            }

        }

        #endregion


        #region NODE STATES
        public class Default_NODE_STATES
        {
            public const byte Boot_Up = 0x00;
            public const byte Disconnected = 0x01;
            public const byte Connecting = 0x02;
            public const byte Preparing = 0x02;
            public const byte Stopped = 0x04;
            public const byte Operational = 0x05;
            public const byte Pre_operational = 0x7F;
            public const byte Unknown_state = 0x0F;

            public static Dictionary<Byte, string> Default_NODES_STATES_Dict = new Dictionary<Byte, string>()
            {
                {Boot_Up,"Boot_Up"},
                //{Disconnected,"Disconnected"},
                //{Connecting,"Connecting"},
                //{Preparing,"Preparing"},
                {Stopped,"Stopped"},
                {Operational,"Operational"},
                {Pre_operational,"Pre_operational"},
                {Unknown_state,"Unknown_state"},

            };

            public static string GetDescription(Byte ID)
            {
                // Try to get the result in the static Dictionary
                string result;
                if (Default_NODES_STATES_Dict.TryGetValue(ID, out result))
                {
                    return result;
                }
                else
                {
                    return "n/a";
                }
            }
        }
       #endregion


        #region NMT COMMAND SPECIFIER


         public class Default_NMT_CS
        {
            /// <summary>
            /// NMT Command Specifier 
            /// </summary>
            private const byte NMT_Start_Node = 0x01;
            private const byte NMT_Stop_Node = 0x02;
            private const byte NMT_Enter_PreOperational = 0x80;
            private const byte NMT_Reset_Node = 0x81;
            private const byte NMT_Reset_Comunication = 0x82;

             // Byte Command CS and NodeID or 0 for all Nodes
            /// <summary>
            /// Communication profile DS-301
            ///</summary>
            public static Dictionary<Byte, string> Default_NMT_CS_Dict = new Dictionary<Byte, string>()
            {
                {NMT_Start_Node,"Start Node"},
                {NMT_Stop_Node,"Stop Node"},
                {NMT_Enter_PreOperational,"Pre-Operational"},
                {NMT_Reset_Node,"Reset Node"},
                {NMT_Reset_Comunication,"Reset Communication"},
            };

            public static string GetDescription(Byte ID)
            {
                // Try to get the result in the static Dictionary
                string result;
                if (Default_NMT_CS_Dict.TryGetValue(ID, out result))
                {
                    return result;
                }
                else
                {
                    return "n/a";
                }
            }

        }

        #endregion

        #region SDO COMMANDS
        public class Default_SDO_COMMANDS
        {
            private const byte SDO_Master_Read_From_Slave    = 0x40;
            private const byte SDO_Master_Write_To_Slave     = 0x23;
            private const byte SDO_Succes_Read_Response      = 0x43;
            private const byte SDO_Succes_Write_Response     = 0x60;
            private const byte SDO_Abort_Operation           = 0x80;

            static Dictionary<Byte, string> Default_SDO_CS_Dict = new Dictionary<Byte, string>()
            {
                {SDO_Master_Read_From_Slave,"Master Read From Slave"},
                {SDO_Master_Write_To_Slave,"Master Write To SLave"},
                {SDO_Succes_Read_Response,"Successfull Read Response"},
                {SDO_Succes_Write_Response,"Successfull Write Response"},
                {SDO_Abort_Operation,"Abort Communication"},
            };

            public static string GetDescription(Byte ID)
            {
                // Try to get the result in the static Dictionary
                string result;
                if (Default_SDO_CS_Dict.TryGetValue(ID, out result))
                {
                    return result;
                }
                else
                {
                    return "n/a";
                }
            }
        }
        #endregion

        #region EMERGENCY CODES
        public class Default_EMCY
        {
                
       

                public const UInt16 NoError = 0x0000;
                public const UInt16 GenericError = 0x1000;

                public const UInt16 CurrentError = 0x2000;
                public const UInt16 Current_device_input_side = 0x2100;
                public const UInt16 Current_inside_the_device  = 0x2200;
                public const UInt16 Current_device_output_side = 0x2300;

                public const UInt16 VoltageError = 0x3000;
                public const UInt16 Mains_Voltage = 0x3100;
                public const UInt16 Voltage_inside_the_device = 0x3200;
                public const UInt16 Output_Voltage = 0x3300;

                public const UInt16 TemperatureError = 0x4000;
                public const UInt16 Ambient_Temperature = 0x4100;
                public const UInt16 Device_Temperature = 0x4200;

                public const UInt16 HardwareError = 0x5000;

                public const UInt16 SoftwareError = 0x6000;
                public const UInt16 Internal_Software = 0x6100;
                public const UInt16 User_Software = 0x6200;
                public const UInt16 Data_Set = 0x6300;

                public const UInt16 ModuleError = 0x7000;

                public const UInt16 Monitoring = 0x8000;
                public const UInt16 Communication = 0x8100;
                public const UInt16 CAN_Overrun_Objects_lost = 0x8110;
                public const UInt16  CAN_in_Error_Passive_Mode = 0x8120;
                public const UInt16  Life_Guard_Error_or_Heartbeat_Error = 0x8130;
                public const UInt16 recovered_from_bus_off = 0x8140;
                public const UInt16  Transmit_COB_ID_collision = 0x8150;
                public const UInt16  Protocol_Error = 0x8200;
                public const UInt16 PDO_not_processed_due_to_length_error = 0x8210;
                public const UInt16  PDO_length_exceeded = 0x8220;

                public const UInt16 ExternalError = 0x9000;
                public const UInt16 Additional_Functions = 0xF000;
                public const UInt16 DevSpecificError = 0xFF00;


                static Dictionary<UInt16, string> Default_EMCY_Dict = new Dictionary<UInt16, string>()
            {
                {NoError,"NoError"},
                {GenericError,"GenericError"},

                {CurrentError,"CurrentError"},
                {Current_device_input_side,"Current_device_input_side"},
                {Current_inside_the_device,"Current_inside_the_device"},
                {Current_device_output_side,"Current_device_output_side"},

                {VoltageError ,"VoltageError"},
                {Mains_Voltage,"Mains_Voltage"},
                {Voltage_inside_the_device,"Voltage_inside_the_device"},
                {Output_Voltage,"Output_Voltage"},

                {TemperatureError,"TemperatureError"},
                {Ambient_Temperature,"Ambient_Temperature"},
                {Device_Temperature,"Device_Temperature"},

                {HardwareError,"HardwareError"},

                {SoftwareError,"SoftwareError"},
                {Internal_Software ,"Internal_Software"},
                {User_Software,"User_Software"},
                {Data_Set,"Data_Set"},

                {ModuleError,"ModuleError"},

                {Monitoring,"Monitoring"},
                {Communication,"Communication"},
                {CAN_Overrun_Objects_lost,"CAN_Overrun_Objects_lost"},
                {CAN_in_Error_Passive_Mode,"CAN_in_Error_Passive_Mode"},
                {Life_Guard_Error_or_Heartbeat_Error,"Life_Guard_Error_or_Heartbeat_Error"},
                { recovered_from_bus_off,"recovered_from_bus_off"},
                {Transmit_COB_ID_collision,"Transmit_COB_ID_collision"},
                {Protocol_Error,"Protocol_Error"},
                {PDO_not_processed_due_to_length_error,"PDO_not_processed_due_to_length_error"},
                {PDO_length_exceeded,"PDO_length_exceeded"},

                {ExternalError,"ExternalError"},
                {Additional_Functions,"Additional_Functions"},
                {DevSpecificError,"DevSpecificError"},
            };

            public static string GetDescription(UInt16 ID)
            {
                // Try to get the result in the static Dictionary
                string result;
                if (Default_EMCY_Dict.TryGetValue((ushort)(ID & 0xFFF0), out result))
                {
                    return result;
                }
                else
                {
                    if (Default_EMCY_Dict.TryGetValue((ushort)(ID & 0xFF00), out result))
                    {
                        return result;
                    }
                    else
                    {
                        return "n/a";
                    }
                }
            }
        }
        #endregion

    }
}
