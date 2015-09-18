
 // TO DO LIST
  Heartbeat implementieren in Firmware
  Nodes Initalisieren
  Network Manager
  Nodes registrieren/ Tabelle erstellen
  Request Funktion einbauen
   
   
   
   
   
   
   
   
   
    Vendor ID: 0x4d8
    Product ID: 0x0070
    Use configuration #1, and interface #0 with endpoints 0x01 (out) and 0x81 (in).
    All communication uses 64 byte interrupt transfers. 

USB CAN BUFFER STRUCTURE

    Bytes 0..51: CAN messages (see below for details). [IN/OUT]
    Byte 52: Number of CAN messages to send. I have only tried sending a single message at a time. [OUT]
    Byte 55: Transmit Error Counter. [IN]
    Byte 56: Receive Error Counter. [IN]
    Byte 57: CANSTAT register - see datasheet for details. [IN]
    Byte 58: CANCTRL register - see datasheet for details. [OUT]
    Byte 60: SPI command. For incoming messages, this echos back the command that was sent. Commands are 0xC0 (CAN reset), 0x03 (read register), 0x02 (write register), 0x80 (RTS - ?), 0xB0 (RD Status - ?) and 0xD0 (Read Firmware Version). [IN/OUT]
    Byte 61: Register. Set to the register to read or write. The response to a read request contains the same value. [IN/OUT]
    Byte 62: Data. The value to be written to a register for outgoing messages, or retrieved from a register for incoming messages. [IN/OUT]


CAN PROTOCOL


    Byte 0: Most significant bit (bit 7) indicates the presence of a CAN message. Bit 5 indicates an extended (29 bit) identifier. Bit 4 indicates a remote transmission request (RTR). Bits 0-3 contain the data length.
    [Standard ID] Bytes 1-2: Byte 1 and bits 5-7 of byte 2 contain the identifier, big endian.
    [Extended ID] Bytes 1-4: From most significant to least significant, the message ID is in byte 1, byte 2 (bits 5-7), byte 2 (bits 0-1), byte 3, and byte 4.
    Next [len] bytes: If this is not an RTR message, the content of the message follows (up to 8 bytes).

DATATABLE STRUCTURE

  CAN_CONFIGURATIONS
  keyid INT IDENTITY NOT NULL PRIMARY KEY, CANID INT , BinaryID Binary(4),Assembly NVARCHAR(40) ,Description NVARCHAR(40)

  CAN_Measuring

  (keyid INT IDENTITY NOT NULL PRIMARY KEY ,CANID INT,BYTE0 BINARY(1), BYTE1 BINARY(1),BYTE2 BINARY(1),BYTE3 BINARY(1),BYTE4 BINARY(1),BYTE5 BINARY(1),BYTE6 BINARY(1),BYTE7 BINARY(1))



  USB CAN BUFFER STRUCTURE

//USB Message Parse: Defines locations in 64 byte USB message
//CAN Message X
#define mx_SIDH   0
#define mx_SIDL   1
#define mx_EID8   2
#define mx_EID0   3
#define mx_DLC    4
#define mx_D0     5
#define mx_D1     6
#define mx_D2     7
#define mx_D3     8
#define mx_D4     9
#define mx_D5     10
#define mx_D6     11
#define mx_D7     12


//CAN Message 1
#define m1_SIDH   0
#define m1_SIDL   1
#define m1_EID8   2
#define m1_EID0   3
#define m1_DLC    4
#define m1_D0     5
#define m1_D1     6
#define m1_D2     7
#define m1_D3     8
#define m1_D4     9
#define m1_D5     10
#define m1_D6     11
#define m1_D7     12

//CAN Message 2
#define m2_SIDH   13
#define m2_SIDL   14
#define m2_EID8   15
#define m2_EID0   16
#define m2_DLC    17
#define m2_D0     18
#define m2_D1     19
#define m2_D2     20
#define m2_D3     21
#define m2_D4     22
#define m2_D5     23
#define m2_D6     24
#define m2_D7     25

//CAN Message 3
#define m3_SIDH   26
#define m3_SIDL   27
#define m3_EID8   28
#define m3_EID0   29
#define m3_DLC    30
#define m3_D0     31
#define m3_D1     32
#define m3_D2     33
#define m3_D3     34
#define m3_D4     35
#define m3_D5     36
#define m3_D6     37
#define m3_D7     38

//CAN Message 4
#define m4_SIDH   39
#define m4_SIDL   40
#define m4_EID8   41
#define m4_EID0   42
#define m4_DLC    43
#define m4_D0     44
#define m4_D1     45
#define m4_D2     46
#define m4_D3     47
#define m4_D4     48
#define m4_D5     49
#define m4_D6     50
#define m4_D7     51

//Registers, Status, Control
#define u_CANmsgs 52
#define u_CANINTF 53
#define u_EFLG    54
#define u_TEC     55
#define u_REC     56
#define u_CANSTAT 57
#define u_CANCTRL 58
#define u_STATUS  59
#define u_SPI     60
#define u_REG     61
#define u_DATA    62
#define u_res     63


/*******************************************************************
 *                   Register Mapping                          
 *******************************************************************/

/* Configuration Registers */
#define CANSTAT         0x0E
#define CANCTRL         0x0F
#define BFPCTRL         0x0C
#define TEC             0x1C
#define REC             0x1D
#define CNF3            0x28
#define CNF2            0x29
#define CNF1            0x2A
#define CANINTE         0x2B
#define CANINTF         0x2C
#define EFLG            0x2D
#define TXRTSCTRL       0x0D

/*  Recieve Filters */
#define RXF0SIDH        0x00
#define RXF0SIDL        0x01
#define RXF0EID8        0x02
#define RXF0EID0        0x03
#define RXF1SIDH        0x04
#define RXF1SIDL        0x05
#define RXF1EID8        0x06
#define RXF1EID0        0x07
#define RXF2SIDH        0x08
#define RXF2SIDL        0x09
#define RXF2EID8        0x0A
#define RXF2EID0        0x0B
#define RXF3SIDH        0x10
#define RXF3SIDL        0x11
#define RXF3EID8        0x12
#define RXF3EID0        0x13
#define RXF4SIDH        0x14
#define RXF4SIDL        0x15
#define RXF4EID8        0x16
#define RXF4EID0        0x17
#define RXF5SIDH        0x18
#define RXF5SIDL        0x19
#define RXF5EID8        0x1A
#define RXF5EID0        0x1B

/* Receive Masks */
#define RXM0SIDH        0x20
#define RXM0SIDL        0x21
#define RXM0EID8        0x22
#define RXM0EID0        0x23
#define RXM1SIDH        0x24
#define RXM1SIDL        0x25
#define RXM1EID8        0x26
#define RXM1EID0        0x27

/* Tx Buffer 0 */
#define TXB0CTRL        0x30
#define TXB0SIDH        0x31
#define TXB0SIDL        0x32
#define TXB0EID8        0x33
#define TXB0EID0        0x34
#define TXB0DLC         0x35
#define TXB0D0          0x36
#define TXB0D1          0x37
#define TXB0D2          0x38
#define TXB0D3          0x39
#define TXB0D4          0x3A
#define TXB0D5          0x3B
#define TXB0D6          0x3C
#define TXB0D7          0x3D
                         
/* Tx Buffer 1 */
#define TXB1CTRL        0x40
#define TXB1SIDH        0x41
#define TXB1SIDL        0x42
#define TXB1EID8        0x43
#define TXB1EID0        0x44
#define TXB1DLC         0x45
#define TXB1D0          0x46
#define TXB1D1          0x47
#define TXB1D2          0x48
#define TXB1D3          0x49
#define TXB1D4          0x4A
#define TXB1D5          0x4B
#define TXB1D6          0x4C
#define TXB1D7          0x4D

/* Tx Buffer 2 */
#define TXB2CTRL        0x50
#define TXB2SIDH        0x51
#define TXB2SIDL        0x52
#define TXB2EID8        0x53
#define TXB2EID0        0x54
#define TXB2DLC         0x55
#define TXB2D0          0x56
#define TXB2D1          0x57
#define TXB2D2          0x58
#define TXB2D3          0x59
#define TXB2D4          0x5A
#define TXB2D5          0x5B
#define TXB2D6          0x5C
#define TXB2D7          0x5D
                         
/* Rx Buffer 0 */
#define RXB0CTRL        0x60
#define RXB0SIDH        0x61
#define RXB0SIDL        0x62
#define RXB0EID8        0x63
#define RXB0EID0        0x64
#define RXB0DLC         0x65
#define RXB0D0          0x66
#define RXB0D1          0x67
#define RXB0D2          0x68
#define RXB0D3          0x69
#define RXB0D4          0x6A
#define RXB0D5          0x6B
#define RXB0D6          0x6C
#define RXB0D7          0x6D
                         
/* Rx Buffer 1 */
#define RXB1CTRL        0x70
#define RXB1SIDH        0x71
#define RXB1SIDL        0x72
#define RXB1EID8        0x73
#define RXB1EID0        0x74
#define RXB1DLC         0x75
#define RXB1D0          0x76
#define RXB1D1          0x77
#define RXB1D2          0x78
#define RXB1D3          0x79
#define RXB1D4          0x7A
#define RXB1D5          0x7B
#define RXB1D6          0x7C
#define RXB1D7          0x7D


//-------------MCP2515 SPI commands------------------------
#define CAN_RESET       0xC0  //Reset
#define CAN_READ        0x03  //Read
#define CAN_WRITE       0x02  //Write
#define CAN_RTS         0x80  //Request to Send
#define CAN_RTS_TXB0    0x81  //RTS TX buffer 0
#define CAN_RTS_TXB1    0x82  //RTS TX buffer 1
#define CAN_RTS_TXB2    0x84  //RTS TX buffer 2
#define CAN_RD_STATUS   0xA0  //Read Status
#define CAN_BIT_MODIFY  0x05  //Bit modify  
#define CAN_RX_STATUS   0xB0  //Receive status 
#define FIRMWARE_VER_RD		0xd0  //retrieve firmware version

#define CAN_RD_RX_BUFF        0x90  //Base command; requires pointer to RX buffer location
#define CAN_RD_START_RXB0SIDH 0x90  //Starts read at RXB0SIDH
#define CAN_RD_START_RXB0D0   0x92  //Starts read at RXB0D0
#define CAN_RD_START_RXB1SIDH 0x94  //Starts read at RXB1SIDH
#define CAN_RD_START_RXB1D0   0x96  //Starts read at RXB0D1

#define CAN_LOAD_TX     0xFF  //Used to let the function pick the buffer to load
#define CAN_LD_TXB0_ID  0x40  //Points to TXB0SIDH register
#define CAN_LD_TXB0_D0  0x41  //Points to TXB0D0 register
#define CAN_LD_TXB1_ID  0x42  //Points to TXB1SIDH register
#define CAN_LD_TXB1_D0  0x43  //Points to TXB1D0 register
#define CAN_LD_TXB2_ID  0x44  //Points to TXB2SIDH register
#define CAN_LD_TXB2_D0  0x45  //Points to TXB2D0 register