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


    public partial class CANopen
    {


        public class CanMsg
        {
            public byte[] Data = new byte[8];
            public UInt32 ID = 0;
            public byte MsgLen = 0;
            public MsgTypes MsgType = MsgTypes.MSGTYPE_STANDARD;
        }

        /// <summary>
        /// CAN Message
        /// </summary>
        [Flags]
        public enum MsgTypes : int
        {
            MSGTYPE_STANDARD = 0x00,		// Standard Frame (11 bit ID)
            MSGTYPE_RTR = 0x01,		        // Remote request
            MSGTYPE_EXTENDED = 0x02,		// CAN 2.0 B Frame (29 Bit ID)
            MSGTYPE_STATUS = 0x80,		    // Status Message
        }

        
        /*

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

         */
        
        public class CAN_FRAME
        {
            
            public UInt32 ID = 0;
            public byte Datalength = 0;
            public byte[] Data = new byte[8];
            public MsgTypes MsgType = MsgTypes.MSGTYPE_STANDARD;
        }

       

        
        /*
        private void canDispatch(CanOpenDev device, CanMsg msg)
        {

            UInt16 cob_id = (UInt16)msg.ID;
            switch (cob_id >> 7)
            {
                case SYNC:		// can be a SYNC or a EMCY message 
                    if (cob_id == 0x080)	// SYNC 
                    {
                        if (device.ComState.csSYNC == 1)
                            proceedSYNC(device);
                    }
                    else		// EMCY 
                        if (device.ComState.csEmergency == 1)
                            proceedEMCY(device, msg);
                    break;
                // case TIME_STAMP: 
                case PDO1tx:
                case PDO1rx:
                case PDO2tx:
                case PDO2rx:
                case PDO3tx:
                case PDO3rx:
                case PDO4tx:
                case PDO4rx:
                    if (device.ComState.csPDO == 1)
                        proceedPDO(device, msg);
                    break;
                case SDOtx:
                case SDOrx:
                    if (device.ComState.csSDO == 1)
                        proceedSDO(device, msg);
                    break;
                case NODE_GUARD:
                    if (device.ComState.csLifeGuard == 1)
                        proceedNODE_GUARD(device, msg);
                    break;
                case NMT:
                    if (device.iam_a_slave)
                        proceedNMTstateChange(device, msg);
                    break;
            }
        }
            */
        
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
        



        /*
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
            }*/

    }
}
