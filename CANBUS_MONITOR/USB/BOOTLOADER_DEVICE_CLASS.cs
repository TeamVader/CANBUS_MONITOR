using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using usbGenericHidCommunications;
using System.Windows;

namespace CANBUS_MONITOR
{
    class Bootloader_Device : usbGenericHidCommunication
    {

        

    /// <summary>
    /// This class performs several different tests against the 
    /// reference hardware/firmware to confirm that the USB
    /// communication library is functioning correctly.
    /// 
    /// It also serves as a demonstration of how to use the class
    /// library to perform different types of read and write
    /// operations.
    /// </summary>
    
        /// <summary>
        /// Class constructor - place any initialisation here
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public Bootloader_Device(int vid, int pid) : base(vid, pid)
            {
            }
     
        /// <summary>
        /// Read Data from CAN Device
        /// </summary>
        public Byte[] readdata()
        {
            bool success;
            Byte[] inputbuffer = new Byte[65];
            success = readSingleReportFromDevice(ref inputbuffer);
            if (success)
            {
                return inputbuffer;
            }
            return null;
        }

        /// <summary>
        /// Send Data to CAN Device
        /// </summary>

        public bool senddata(Byte[] outputbuffer)
        {
            bool success;

            success = writeRawReportToDevice(outputbuffer);
            if (success)
            {
                return true;
            }
            return false;
        }

        public bool Program_Data_Packet(Byte Command,Byte[] Data)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.PROGRAM_DEVICE; //Command


            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }

        public Byte[] Query_Device_Packet()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.QUERY_DEVICE; //Command


            if (senddata(USB_Packet))
            {
                USB_Packet = readdata();
                if (USB_Packet != null)
                {
                    
                    return USB_Packet;
                }
            }
            return null;
        }

        public Byte[] Extended_Query_Device_Packet()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.QUERY_EXTENDED_INFO; //Command


            if (senddata(USB_Packet))
            {
                USB_Packet = readdata();
                if (USB_Packet != null)
                {

                    return USB_Packet;
                }
            }
            return null;
        }


        public bool Erase_Device()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.ERASE_DEVICE; //Command


            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }

        public bool Reset_Device()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.RESET_DEVICE; //Command


            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }

        public bool Program_Complete_Device()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[1] = Bootloader_USB_Commands.PROGRAM_COMPLETE; //Command


            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }

    }
}
