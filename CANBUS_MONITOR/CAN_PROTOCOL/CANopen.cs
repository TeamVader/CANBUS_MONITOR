using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
    /*COB:
    Communication Object. A unit of transportation in a CAN network. Data must be sent across a CAN
    Network inside a COB. There are 2048 different COB's in a CAN network. A COB can contain at most
    8 bytes of data.
    COB-ID:
    Each COB is uniquely identified in a CAN network by a number called the COB Identifier (COB-ID).
    The COB-ID determines the priority of that COB for the MAC sub-layer.*/


    public class CANopen
    {

        public class Service_ID
        {
            static Dictionary<UInt32, string> ID_Service_Dictionary = new Dictionary<uint, string>()
            {
                {0x000,"NMT"},
            };
        }
        

        [global::System.AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        sealed class BitfieldLengthAttribute : Attribute
        {
            uint length;

            public BitfieldLengthAttribute(uint length)
            {
                this.length = length;
            }

            public uint Length { get { return length; } }
        }




        public struct COB_ID //1-2047
        {
            [BitfieldLength(7)] //0-6
            public uint Node_ID;
            [BitfieldLength(4)] //7-10
            public uint Function;
            
            
        }

        public const Int16 NMT_node_control = 0; //0h All Slave receive
        //Identifier standard length 11 bit

        public const Int16 SYNC = 128; //Not implented
        public const Int16 time = 256; //100h  

        //Process Data Objects Standard Message with 8 Databytes
        public const Int16 PDO = 500; //181h-580h

        //SDO (Service Data Objects) is used for Receive_Transmission of variable Data length
        public const Int16 SDO_Transmit = 1408; //580h +Node ID
        public const Int16 SDO_Receive = 1536; //600h +Node ID

        public const Int16 Heartbeat_Message = 1792;   //701h-780h plus ID CAN NODE
        //Example CAN NODE Nr 100 sends Heartbeat Message with Identifier : 764h / 1892

        /// <summary>
        /// Test
        /// </summary>
        public class Emergency
        {
            public const Int32 NoError = 0x0000;
            public const Int32 NodefinedError = 0x1000;
            public const Int32 CurrentError = 0x2000;
            public const Int32 VoltageError = 0x3000;
            public const Int32 TemperatureError = 0x4000;
            public const Int32 HardwareError = 0x5000;
            public const Int32 SoftwareError = 0x6000;
            public const Int32 ModuleError = 0x7000;
            public const Int32 CommunicationError = 0x8000;
            public const Int32 ExternalError = 0x9000;
            public const Int32 DevSpecificError = 0xFF00;
        }
        

        public static class PrimitiveConversion
            {
                public static long ToLong<T>(T t) where T : struct
                {
                    long r = 0;
                    int offset = 0;

                    // For every field suitably attributed with a BitfieldLength
                    foreach (System.Reflection.FieldInfo f in t.GetType().GetFields())
                    {
                        object[] attrs = f.GetCustomAttributes(typeof(BitfieldLengthAttribute), false);
                        if (attrs.Length == 1)
                        {
                            uint fieldLength = ((BitfieldLengthAttribute)attrs[0]).Length;

                            // Calculate a bitmask of the desired length
                            long mask = 0;
                            for (int i = 0; i < fieldLength; i++)
                                mask |= 1 << i;

                            r |= ((UInt32)f.GetValue(t) & mask) << offset;

                            offset += (int)fieldLength;
                        }
                    }

                    return r;
                }
            }

    }
}
