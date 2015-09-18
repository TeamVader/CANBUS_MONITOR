using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
     class MCP2515_Register
    {
        /*******************************************************************
        *                   Register Mapping                          
        *******************************************************************/
         
             /* Configuration Registers */
             public const Byte CANSTAT       =  0x0E;
             public const Byte CANCTRL       =  0x0F;
             public const Byte BFPCTRL       =  0x0C;
             public const Byte TEC           =  0x1C;
             public const Byte REC           =  0x1D;
             public const Byte CNF3          =  0x28;
             public const Byte CNF2          =  0x29;
             public const Byte CNF1          =  0x2A;
             public const Byte CANINTE       =  0x2B;
             public const Byte CANINTF       =  0x2C;
             public const Byte EFLG          =  0x2D;
             public const Byte TXRTSCTRL     =  0x0D;
            
             /*  Recieve Filters */
             public const Byte RXF0SIDH      =  0x00;
             public const Byte RXF0SIDL      =  0x01;
             public const Byte RXF0EID8      =  0x02;
             public const Byte RXF0EID0      =  0x03;
             public const Byte RXF1SIDH      =  0x04;
             public const Byte RXF1SIDL      =  0x05;
             public const Byte RXF1EID8      =  0x06;
             public const Byte RXF1EID0      =  0x07;
             public const Byte RXF2SIDH      =  0x08;
             public const Byte RXF2SIDL      =  0x09;
             public const Byte RXF2EID8      =  0x0A;
             public const Byte RXF2EID0      =  0x0B;
             public const Byte RXF3SIDH      =  0x10;
             public const Byte RXF3SIDL      =  0x11;
             public const Byte RXF3EID8      =  0x12;
             public const Byte RXF3EID0      =  0x13;
             public const Byte RXF4SIDH      =  0x14;
             public const Byte RXF4SIDL      =  0x15;
             public const Byte RXF4EID8      =  0x16;
             public const Byte RXF4EID0      =  0x17;
             public const Byte RXF5SIDH      =  0x18;
             public const Byte RXF5SIDL      =  0x19;
             public const Byte RXF5EID8      =  0x1A;
             public const Byte RXF5EID0      =  0x1B;

             /* Receive Masks */
             public const Byte RXM0SIDH      =  0x20;
             public const Byte RXM0SIDL      =  0x21;
             public const Byte RXM0EID8      =  0x22;
             public const Byte RXM0EID0      =  0x23;
             public const Byte RXM1SIDH      =  0x24;
             public const Byte RXM1SIDL      =  0x25;
             public const Byte RXM1EID8      =  0x26;
             public const Byte RXM1EID0      =  0x27;

             /* Tx Buffer 0 */
             public const Byte TXB0CTRL      =  0x30;
             public const Byte TXB0SIDH      =  0x31;
             public const Byte TXB0SIDL      =  0x32;
             public const Byte TXB0EID8      =  0x33;
             public const Byte TXB0EID0      =  0x34;
             public const Byte TXB0DLC       =  0x35;
             public const Byte TXB0D0        =  0x36;
             public const Byte TXB0D1        =  0x37;
             public const Byte TXB0D2        =  0x38;
             public const Byte TXB0D3        =  0x39;
             public const Byte TXB0D4        =  0x3A;
             public const Byte TXB0D5        =  0x3B;
             public const Byte TXB0D6        =  0x3C;
             public const Byte TXB0D7        =  0x3D;
                         
             /* Tx Buffer 1 */
             public const Byte TXB1CTRL      =  0x40;
             public const Byte TXB1SIDH      =  0x41;
             public const Byte TXB1SIDL      =  0x42;
             public const Byte TXB1EID8      =  0x43;
             public const Byte TXB1EID0      =  0x44;
             public const Byte TXB1DLC       =  0x45;
             public const Byte TXB1D0        =  0x46;
             public const Byte TXB1D1        =  0x47;
             public const Byte TXB1D2        =  0x48;
             public const Byte TXB1D3        =  0x49;
             public const Byte TXB1D4        =  0x4A;
             public const Byte TXB1D5        =  0x4B;
             public const Byte TXB1D6        =  0x4C;
             public const Byte TXB1D7        =  0x4D;

             /* Tx Buffer 2 */
             public const Byte TXB2CTRL      =  0x50;
             public const Byte TXB2SIDH      =  0x51;
             public const Byte TXB2SIDL      =  0x52;
             public const Byte TXB2EID8      =  0x53;
             public const Byte TXB2EID0      =  0x54;
             public const Byte TXB2DLC       =  0x55;
             public const Byte TXB2D0        =  0x56;
             public const Byte TXB2D1        =  0x57;
             public const Byte TXB2D2        =  0x58;
             public const Byte TXB2D3        =  0x59;
             public const Byte TXB2D4        =  0x5A;
             public const Byte TXB2D5        =  0x5B;
             public const Byte TXB2D6        =  0x5C;
             public const Byte TXB2D7        =  0x5D;
                         
             /* Rx Buffer 0 */
             public const Byte RXB0CTRL      =  0x60;
             public const Byte RXB0SIDH      =  0x61;
             public const Byte RXB0SIDL      =  0x62;
             public const Byte RXB0EID8      =  0x63;
             public const Byte RXB0EID0      =  0x64;
             public const Byte RXB0DLC       =  0x65;
             public const Byte RXB0D0        =  0x66;
             public const Byte RXB0D1        =  0x67;
             public const Byte RXB0D2        =  0x68;
             public const Byte RXB0D3        =  0x69;
             public const Byte RXB0D4        =  0x6A;
             public const Byte RXB0D5        =  0x6B;
             public const Byte RXB0D6        =  0x6C;
             public const Byte RXB0D7        =  0x6D;
                         
             /* Rx Buffer 1 */
             public const Byte RXB1CTRL      =  0x70;
             public const Byte RXB1SIDH      =  0x71;
             public const Byte RXB1SIDL      =  0x72;
             public const Byte RXB1EID8      =  0x73;
             public const Byte RXB1EID0      =  0x74;
             public const Byte RXB1DLC       =  0x75;
             public const Byte RXB1D0        =  0x76;
             public const Byte RXB1D1        =  0x77;
             public const Byte RXB1D2        =  0x78;
             public const Byte RXB1D3        =  0x79;
             public const Byte RXB1D4        =  0x7A;
             public const Byte RXB1D5        =  0x7B;
             public const Byte RXB1D6        =  0x7C;
             public const Byte RXB1D7        =  0x7D;

    }

     class MCP2515_SPI_Commands
     {
         //-------------MCP2515 SPI commands------------------------
             public const Byte CAN_RESET     =  0xC0;  //Reset
             public const Byte CAN_READ      =  0x03;  //Read
             public const Byte CAN_WRITE     =  0x02;  //Write
             public const Byte CAN_RTS       =  0x80;  //Request to Send
             public const Byte CAN_RTS_TXB0  =  0x81;  //RTS TX buffer 0
             public const Byte CAN_RTS_TXB1  =  0x82;  //RTS TX buffer 1
             public const Byte CAN_RTS_TXB2  =  0x84;  //RTS TX buffer 2
             public const Byte CAN_RD_STATUS =  0xA0;  //Read Status
             public const Byte CAN_BIT_MODIFY=  0x05;  //Bit modify  
             public const Byte CAN_RX_STATUS =  0xB0;  //Receive status 
             public const Byte FIRMWARE_VER_RD	=	0xd0;  //retrieve firmware version

             public const Byte CAN_RD_RX_BUFF         = 0x90 ; //Base command; requires pointer to RX buffer location
             public const Byte CAN_RD_START_RXB0SIDH  = 0x90 ; //Starts read at RXB0SIDH
             public const Byte CAN_RD_START_RXB0D0    = 0x92 ; //Starts read at RXB0D0
             public const Byte CAN_RD_START_RXB1SIDH  = 0x94 ; //Starts read at RXB1SIDH
             public const Byte CAN_RD_START_RXB1D0    = 0x96 ; //Starts read at RXB0D1

             public const Byte CAN_LOAD_TX    = 0xFF;  //Used to let the function pick the buffer to load
             public const Byte CAN_LD_TXB0_ID = 0x40; //Points to TXB0SIDH register
             public const Byte CAN_LD_TXB0_D0 = 0x41; //Points to TXB0D0 register
             public const Byte CAN_LD_TXB1_ID = 0x42; //Points to TXB1SIDH register
             public const Byte CAN_LD_TXB1_D0 = 0x43; //Points to TXB1D0 register
             public const Byte CAN_LD_TXB2_ID = 0x44;//Points to TXB2SIDH register
             public const Byte CAN_LD_TXB2_D0 = 0x45; //Points to TXB2D0 register
         
     }

     class MCP_2515_Operation_Modes
     {
         // MCP2515 Operation Modes
         public const Byte Normal_Operation_Mode = 0x00;
         public const Byte Sleep_Mode            = 0x20;
         public const Byte Loopback_Mode         = 0x40;
         public const Byte Listen_Only_Mode      = 0x60;
         public const Byte Configuration_Mode    = 0x80;
         
     }

     class Bus_Speed
     {
         public const Byte CAN_1000kbps =  0x00;       //BRP setting in CNF1
         public const Byte CAN_500kbps  =  0x01;       //
         public const Byte CAN_250kbps  =  0x03;       //
         public const Byte CAN_125kbps  =  0x07;       //
     }
     class USB_User_Commands
     {
         // Send Command over USB for PIC18
         public const Byte Entry_Bootloader          = 0xFF;
         public const Byte Reset_Device              = 0x0F;
         public const Byte Start_USB_Communication   = 0xEF;
         public const Byte Stop_USB_Communication    = 0xDF;
         public const Byte Enable_Term_resistor      = 0x0E;
         public const Byte Disable_Term_resistor     = 0x0D;
     }

     class Bootloader_USB_Commands
     {
         public const Byte  QUERY_DEVICE            =   0x02;	//Command that the host uses to learn about the device (what regions can be programmed, and what type of memory is the region)
         public const Byte	UNLOCK_CONFIG			=	0x03;	//Note, this command is used for both locking and unlocking the config bits (see the "//Unlock Configs Command Definitions" below)
         public const Byte  ERASE_DEVICE			=	0x04;	//Host sends this command to start an erase operation.  Firmware controls which pages should be erased.
         public const Byte  PROGRAM_DEVICE			=	0x05;	//If host is going to send a full RequestDataBlockSize to be programmed, it uses this command.
         public const Byte	PROGRAM_COMPLETE		=	0x06;	//If host send less than a RequestDataBlockSize to be programmed, or if it wished to program whatever was left in the buffer, it uses this command.
         public const Byte  GET_DATA				=	0x07;	//The host sends this command in order to read out memory from the device.  Used during verify (and read/export hex operations)
         public const Byte	RESET_DEVICE			=	0x08;	//Resets the microcontroller, so it can update the config bits (if they were programmed, and so as to leave the bootloader (and potentially go back into the main application)
         public const Byte  SIGN_FLASH				=	0x09;	//The host PC application should send this command after the verify operation has completed successfully.  If checksums are used instead of a true verify (due to ALLOW_GET_DATA_COMMAND being commented), then the host PC application should send SIGN_FLASH command after is has verified the checksums are as exected. The firmware will then program the SIGNATURE_WORD into flash at the SIGNATURE_ADDRESS.
         public const Byte  QUERY_EXTENDED_INFO     =   0x0C;   //Used by host PC app to get additional info about the device, beyond the basic NVM layout provided by the query device command
     }
     class CAN_USB_Packet
     {
             //CAN Message 1
             public const int m1_SIDH    = 1;
             public const int m1_SIDL    = 2;
             public const int m1_EID8    = 3;
             public const int m1_EID0    = 4;
             public const int m1_DLC     = 5;
             public const int m1_D0      = 6;
             public const int m1_D1      = 7;
             public const int m1_D2      = 8;
             public const int m1_D3      = 9;
             public const int m1_D4      = 10;
             public const int m1_D5      = 11;
             public const int m1_D6      = 12;
             public const int m1_D7      = 13;

             //CAN Message 2
             public const int m2_SIDH    = 14;
             public const int m2_SIDL    = 15;
             public const int m2_EID8    = 16;
             public const int m2_EID0    = 17;
             public const int m2_DLC     = 18;
             public const int m2_D0      = 19;
             public const int m2_D1      = 20;
             public const int m2_D2      = 21;
             public const int m2_D3      = 22;
             public const int m2_D4      = 23;
             public const int m2_D5      = 24;
             public const int m2_D6      = 25;
             public const int m2_D7      = 26;

             //CAN Message 3
             public const int m3_SIDH    = 27;
             public const int m3_SIDL    = 28;
             public const int m3_EID8    = 29;
             public const int m3_EID0    = 30;
             public const int m3_DLC     = 31;
             public const int m3_D0      = 32;
             public const int m3_D1      = 33;
             public const int m3_D2      = 34;
             public const int m3_D3      = 35;
             public const int m3_D4      = 36;
             public const int m3_D5      = 37;
             public const int m3_D6      = 38;
             public const int m3_D7      = 39;

             //CAN Message 4
             public const int m4_SIDH    = 40;
             public const int m4_SIDL    = 41;
             public const int m4_EID8    = 42;
             public const int m4_EID0    = 43;
             public const int m4_DLC     = 44;
             public const int m4_D0      = 45;
             public const int m4_D1      = 46;
             public const int m4_D2      = 47;
             public const int m4_D3      = 48;
             public const int m4_D4      = 49;
             public const int m4_D5      = 50;
             public const int m4_D6      = 51;
             public const int m4_D7      = 52;

            //Registers, Status, Control
             public const int u_CANmsgs  = 53;
             public const int u_CANINTF  = 54;
             public const int u_EFLG     = 55;
             public const int u_TEC      = 56;
             public const int u_REC      = 57;
             public const int u_CANSTAT  = 58;
             public const int u_CANCTRL  = 59;
             public const int u_STATUS   = 60;
             public const int u_SPI      = 61;
             public const int u_REG      = 62;
             public const int u_DATA     = 63;
             public const int u_res      = 64;
     }
}
