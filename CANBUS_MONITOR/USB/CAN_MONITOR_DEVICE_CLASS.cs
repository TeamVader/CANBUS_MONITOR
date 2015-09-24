//-----------------------------------------------------------------------------
//
//  CAN MONITOR DEVICE CLASS
//
//  
//
//  
//  Copyright (C) 2015 Alexander Stark
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//
//-----------------------------------------------------------------------------




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;
using usbGenericHidCommunications;


namespace CANBUS_MONITOR
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
    class CAN_Monitoring_Device : usbGenericHidCommunication
        {
        /// <summary>
        /// Class constructor - place any initialisation here
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public CAN_Monitoring_Device(int vid, int pid) : base(vid, pid)
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
                return true ;
            }
            return false;
        }

        /// <summary>
        /// Reset MCP 2515 over SPI
        /// </summary>
        public bool MCP_2515_SPI_Reset()
        {
            //bool success;
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;

            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.CAN_RESET; //SPI Read Command


            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Send USB Command 
        /// </summary>
        /// 
        public bool Send_USB_Command(Byte Command)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[CAN_USB_Packet.u_res] = Command; //Command

            if (senddata(USB_Packet))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Read Register of MCP 2515 over SPI
        /// </summary>
        /// 
        public Byte MCP_2515_SPI_Read(Byte Adress)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;

            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.CAN_READ; //SPI Read Command
            USB_Packet[CAN_USB_Packet.u_REG] = Adress; //Adress of Register

            if (senddata(USB_Packet))
            {
                USB_Packet = readdata();
                if (USB_Packet != null)
                {
                    Debug.WriteLine("SPI Read Received : {0}", Convert.ToString(USB_Packet[CAN_USB_Packet.u_DATA], 2).PadLeft(8, '0'));
                    return USB_Packet[CAN_USB_Packet.u_DATA];
                }
            }
            return 0xFF;
        }

        public Byte MCP_2515_SPI_Status()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;

            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.CAN_RD_STATUS; //SPI Status Command


            if (senddata(USB_Packet))
            {
                USB_Packet = readdata();
                if (USB_Packet != null)
                {
                    Debug.WriteLine("SPI Read Received : {0}", Convert.ToString(USB_Packet[CAN_USB_Packet.u_DATA], 2).PadLeft(8, '0'));
                    return USB_Packet[CAN_USB_Packet.u_DATA];
                }
            }
            return 0xFF;
        }

        /// <summary>
        /// Write Data to a Register MCP 2515 over SPI
        /// </summary>

        public Byte MCP_2515_SPI_Bit_Modify(Byte Data, Byte Adress)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;

            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.CAN_BIT_MODIFY; //SPI Read Command
            USB_Packet[CAN_USB_Packet.u_REG] = Adress; //Adress of Register
            USB_Packet[CAN_USB_Packet.u_DATA] = Data;
            if (senddata(USB_Packet))
            {
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// Write Data to a Register MCP 2515 over SPI
        /// </summary>

        public Byte MCP_2515_SPI_Write_Data(Byte Data,Byte Adress)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;

            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.CAN_WRITE; //SPI Read Command
            USB_Packet[CAN_USB_Packet.u_REG] = Adress; //Adress of Register
            USB_Packet[CAN_USB_Packet.u_DATA] = Data;
            if (senddata(USB_Packet))
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Return Operation Mode from MCP 2515 over SPI
        /// </summary>
        /// 
        public bool MCP_2515_Set_Operation_Mode(int Index)
        {
            Byte[] temparray = new Byte[65];

            Byte mask = 0x1F;
            Byte tempdata = MCP_2515_SPI_Read(MCP2515_Register.CANCTRL);
            Byte tempstat = 0;
            Debug.WriteLine("Mask : {0} Data : {1}", mask.ToString(), Convert.ToString(tempdata, 2).PadLeft(8, '0'));
            tempdata = (byte)(tempdata & mask);
            switch (Index)
            {
                case -1:
                    return false;
                case 0 :
                    tempdata = (byte)(tempdata | MCP_2515_Operation_Modes.Normal_Operation_Mode);
                    
                    break;
                case 1:
                    tempdata = (byte)(tempdata | MCP_2515_Operation_Modes.Sleep_Mode);

                    break;
                case 2:
                    tempdata = (byte)(tempdata | MCP_2515_Operation_Modes.Loopback_Mode);

                    break;
                case 3:
                    tempdata = (byte)(tempdata | MCP_2515_Operation_Modes.Listen_Only_Mode);

                    break;
                case 4:
                    tempdata = (byte)(tempdata | MCP_2515_Operation_Modes.Configuration_Mode);

                    break;

            }

            Debug.WriteLine("Mask : {0} Data : {1}", mask.ToString(),Convert.ToString(tempdata,2).PadLeft(8,'0'));


            tempstat =  MCP_2515_SPI_Write_Data(tempdata, MCP2515_Register.CANCTRL);
            //Debug.WriteLine("Starting");
            if (tempstat == 1)
            {
                //temparray= readdata();
                tempstat = MCP_2515_SPI_Read(MCP2515_Register.CANSTAT);
                //tempstat = MCP_2515_SPI_Read(MCP2515_Register.CANCTRL);
                
                Debug.WriteLine("Send : {0} Recevied : {1}", Convert.ToString(tempdata, 2).PadLeft(8, '0'),Convert.ToString(tempstat,2).PadLeft(8,'0') );
                if ((tempdata & 0xE0) == (tempstat & 0xE0))
                {
                    if ((tempdata & 0xE0) == 0x80)
                    {
                       // MCP_2515_SPI_Reset();
                    }
                    Debug.WriteLine("Configuration is Set");
                    return true;
                }
            }
            //MCP_2515_SPI_Write_Data(tempdata, MCP2515_Register.CANCTRL);
            return false;
        }

        public String MCP_2515_Get_Operation_Mode()
        {
           
           
            String OP_Mode ;
            //Byte Data = MCP_2515_SPI_Read(MCP2515_Register.CNF1);
            // Data = MCP_2515_SPI_Read(MCP2515_Register.CNF2);
            Byte Data = MCP_2515_SPI_Read(MCP2515_Register.CANSTAT);
            //Debug.WriteLine("Recevied : {0}", Convert.ToString(Data, 2).PadLeft(8, '0'));
             if (Data != 0xFF)
             {
                 Data = (byte)(Data & 0xe0); // bitwise And to check Bit 7-5 for Mode
                 switch (Data)
                 {
                     case MCP_2515_Operation_Modes.Normal_Operation_Mode :
                         OP_Mode = "Mode : Normal Operation Mode";
                         break;
                     case MCP_2515_Operation_Modes.Sleep_Mode:
                         OP_Mode = "Mode : Sleep Mode";
                         break;
                     case MCP_2515_Operation_Modes.Loopback_Mode:
                         OP_Mode = "Mode : Loopback Mode";
                         break;
                     case MCP_2515_Operation_Modes.Listen_Only_Mode:
                         OP_Mode = "Mode : Listen Only Mode";
                         break;
                     case MCP_2515_Operation_Modes.Configuration_Mode:
                         OP_Mode = "Mode : Configuration Mode";
                         break;
                    
                     default :
                         OP_Mode = "";
                         break;
                 }
                 return OP_Mode;
             }
             else
             {
                 Debug.WriteLine("CANCTRL Register read fails");
            return "";
             }
        }

        public String MCP_2515_Get_Bus_Speed()
        {


            String BusSpeed;
            
            Byte Data = MCP_2515_SPI_Read(MCP2515_Register.CNF1);
            //Debug.WriteLine("Recevied : {0}", Convert.ToString(Data, 2).PadLeft(8, '0'));
            if (Data != 0xFF)
            {
                Data = (byte)(Data & 0x07); // bitwise And to check Bit 7-5 for Mode
                switch (Data)
                {
                    case Bus_Speed.CAN_125kbps:
                        BusSpeed = "Bus Speed : 125 kbit/s";
                        break;
                    case Bus_Speed.CAN_250kbps:
                        BusSpeed = "Bus Speed : 250 kbit/s";
                        break;
                    case Bus_Speed.CAN_500kbps:
                        BusSpeed = "Bus Speed : 500 kbit/s";
                        break;
                    case Bus_Speed.CAN_1000kbps:
                        BusSpeed = "Bus Speed : 1000 kbit/s";
                        break;
                    

                    default:
                        BusSpeed = "Bus Speed not detected";
                        break;
                }
                return BusSpeed;
            }
            else
            {
                Debug.WriteLine("CANCNF1 Register read fails");
                return "";
            }
        }

        /// <summary>
        /// Set Bus Speed
        /// </summary>
        /// <param name="Speed"></param>
        /// <returns></returns>
        public bool MCP_2515_Set_Bus_Speed(Byte Speed)
        {
            MCP_2515_Set_Operation_Mode(4);
            Byte Data = MCP_2515_SPI_Write_Data(Speed,MCP2515_Register.CNF1);
            MCP_2515_Set_Operation_Mode(0);

            //Debug.WriteLine("Recevied : {0}", Convert.ToString(Data, 2).PadLeft(8, '0'));
            if (Data != 0x00)
            {
                Data = MCP_2515_SPI_Read(MCP2515_Register.CNF1);
                if (Data == Speed)
                {
                    return true;
                }
                return false;
            }
            else
            {
                Debug.WriteLine("CANCNF1 Register set fails");
                return false;
            }
        }
        /// <summary>
        /// Check Firmware Version of CAN Device
        /// </summary>

        public string Check_Firmware_Version()
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            //USB_Packet[60] = 208;
            USB_Packet[CAN_USB_Packet.u_SPI] = MCP2515_SPI_Commands.FIRMWARE_VER_RD;
            string firmware = "Firmwareversion : ";
            //208 Firmware Version
            //192 CAN Reset
            //USB_Packet[62] = 208;
            if (!senddata(USB_Packet))
            {
                return "";
               // MessageBox.Show("Send Fails");
            }
            else
            {

                USB_Packet = readdata();
                if (USB_Packet != null)
                {
                    for (int i = 60; i < 64; i++)
                    {
                        //MessageBox.Show(USB_Packet[i].ToString());
                        char ascii = Convert.ToChar(USB_Packet[i]);
                        firmware += ascii.ToString();
                        //MessageBox.Show(ascii.ToString());
                    }
                    return firmware;
                }

            }


            return "";

        }

        /// <summary>
        /// Send a CAN Message
        /// </summary>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        public bool Send_CAN_Message(CAN_MESSAGE.CAN_ID id, CAN_MESSAGE.CAN_DATA dataarray, byte dlc)
        {
            Byte[] USB_Packet = new Byte[65];
            USB_Packet[0] = 0;
            USB_Packet[CAN_USB_Packet.m1_SIDH] = id.SIDH;
            USB_Packet[CAN_USB_Packet.m1_SIDL] = id.SIDL;
            //Debug.Print(id.SIDL.ToString());
            USB_Packet[CAN_USB_Packet.m1_EID8] = id.EID8;
            USB_Packet[CAN_USB_Packet.m1_EID0] = id.EID0;

            USB_Packet[CAN_USB_Packet.m1_DLC] = (byte)(dlc);
            USB_Packet[CAN_USB_Packet.m1_D0] = dataarray.D0;
           // Debug.Print(dataarray.D0.ToString());
            USB_Packet[CAN_USB_Packet.m1_D1] = dataarray.D1;
            USB_Packet[CAN_USB_Packet.m1_D2] = dataarray.D2;
            USB_Packet[CAN_USB_Packet.m1_D3] = dataarray.D3;
            USB_Packet[CAN_USB_Packet.m1_D4] = dataarray.D4;
            USB_Packet[CAN_USB_Packet.m1_D5] = dataarray.D5;
            USB_Packet[CAN_USB_Packet.m1_D6] = dataarray.D6;
            USB_Packet[CAN_USB_Packet.m1_D7] = dataarray.D7;


            USB_Packet[CAN_USB_Packet.u_CANmsgs] = 0x01;
           
            if (senddata(USB_Packet))
            {
                return true;
                // MessageBox.Show("Send Fails");
            }
            


            return false;

        }
        
        }
    }
