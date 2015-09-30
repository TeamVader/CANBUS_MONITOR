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
            public byte dlc = 0;
            public byte[] Data = new byte[8];
            public MsgTypes MsgType = MsgTypes.MSGTYPE_STANDARD;
        }





        public static void CAN_Frame_Dispatch(CANopen.Node node, CAN_FRAME frame)
        {

            UInt16 cob_id = (UInt16)frame.ID;
            
            switch (cob_id >> 7)
            {
                case Default_Identifier_Setup.SYNC:		// can be a SYNC or a EMCY message 
                    if (cob_id == 0x080)	// SYNC 
                    {
                        
                    }
                    else		// EMCY 
                    {
                         
                            proceedEMCY(node, frame);
                    }
                    break;
                case Default_Identifier_Setup.TIME_STAMP:
                case Default_Identifier_Setup.TPDO1:
                case Default_Identifier_Setup.RPDO1:
                case Default_Identifier_Setup.TPDO2:
                case Default_Identifier_Setup.RPDO2:
                case Default_Identifier_Setup.TPDO3:
                case Default_Identifier_Setup.RPDO3:
                case Default_Identifier_Setup.TPDO4:
                case Default_Identifier_Setup.RPDO4:
                   
                    break;
                case Default_Identifier_Setup.TSDO:
                case Default_Identifier_Setup.RSDO:
                    
                    break;
                case Default_Identifier_Setup.NMT_EC:
                   
                    proceedNMT_EC(node, frame);
                    break;
                case Default_Identifier_Setup.NMT:
                    if (frame.Data[1] != 0x00)
                    {
                       

                        proceedNMT(node, frame);
                    }
                    break;
            }
        }



        private static void proceedEMCY(CANopen.Node node, CAN_FRAME frame)
        {

        }

        private static void proceedNMT_EC(CANopen.Node node, CAN_FRAME frame)
        {
            if (frame.MsgType != MsgTypes.MSGTYPE_RTR)
            {
                node.Set_State((byte)(frame.Data[0] & 0x7F));
            }
        }

        private static void proceedNMT(CANopen.Node node, CAN_FRAME frame)
        {


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
