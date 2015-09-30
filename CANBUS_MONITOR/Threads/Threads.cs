using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CANBUS_MONITOR
{
    class UserThreads
    {

        public static void ListboxConsumer(BlockingCollection<CANopen.CAN_FRAME> bc, ListboxConsole lb, CancellationTokenSource ct)
        {
                string data = "";
                string CAN_Message = "CAN Message : ";
                int counter = 0;
                try
                {

                    foreach (var Canmessage in bc.GetConsumingEnumerable(ct.Token))
                    {
                         
                     
                        CAN_Message = "CAN Message : ";

                        data = " Data :";

                        for (int i = 0 ; i < Canmessage.dlc; i++)
                         {
                             data += Convert.ToString(Canmessage.Data[i], 16).PadLeft(2, '0') + "  ";
                    
                         }
                
                           lb.Dispatcher.BeginInvoke(new Action(delegate()
                             {
                            lb.addItem(CAN_Message + string.Format("ID : {0}",Canmessage.ID.ToString("X3")) + data);
                             }));
                        
                    }

                }

                catch (OperationCanceledException)
                {

                    lb.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        lb.addItem("Task canceled");
                    }));
                    
                }

                Debug.WriteLine("\r\nNo more items to take. Press the Enter key to exit.");
        }

        public static void NodeDispatcher(BlockingCollection<CANopen.CAN_FRAME> bc, Nodegroup nodegroup, CancellationToken ct)
        {
           
            CANopen.Node newnode;
           
            try
            {

                foreach (var Canmessage in bc.GetConsumingEnumerable(ct))
                {

                   

                    if(!nodegroup.NodeExists(Canmessage.ID & 0x7F))
                    {
                    newnode = new CANopen.Node();
                    newnode.Node_ID = (byte)(Canmessage.ID & 0x7F);
                    //newnode.Set_State(0x05);
                    nodegroup.AddNode(newnode);
                    }

                    CANopen.CAN_Frame_Dispatch(nodegroup[(int)(Canmessage.ID & 0x7F)], (CANopen.CAN_FRAME)Canmessage);
                }

            }

            catch (OperationCanceledException)
            {

               

            }

        }


        /// <summary>
        /// Converts USB Bytes to CAN_FRAME class
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="ct"></param>
        public static void USB_To_CAN_FRAME_Converter(BlockingCollection<Byte[]> bc, BlockingCollection<CANopen.CAN_FRAME> bcframes, BlockingCollection<CANopen.CAN_FRAME> bcnodes, CancellationToken ct)
        {
            
            CANopen.CAN_FRAME newframe = new CANopen.CAN_FRAME();
           int dlc = 0;

            try
            {

                foreach (var Canmessage in bc.GetConsumingEnumerable(ct))
                {


                    newframe.dlc = (byte)(Canmessage[1] & 0x0F);

                    dlc = (Canmessage[1] & 0x0F);

                     //ID extended or Standard
                    if ((Canmessage[1] & 0x20) == 0x20)
                    {
                        dlc += 2;
                        newframe.ID = (UInt32)((Canmessage[2] << 3) | (Canmessage[3] >> 5) | (Canmessage[4] << 19) | (Canmessage[5] << 11) | ((Canmessage[3] & 0x03) << 27));
                        newframe.MsgType = CANopen.MsgTypes.MSGTYPE_EXTENDED;
                    }
                    else
                    {
                        newframe.ID = (UInt32)((Canmessage[2] << 3) | (Canmessage[3] >> 5));
                        newframe.MsgType = CANopen.MsgTypes.MSGTYPE_STANDARD;
                    }

                    //RTR Message?
                    if ((Canmessage[1] & 0x10) == 0x10)
                    {
                        newframe.MsgType = CANopen.MsgTypes.MSGTYPE_RTR;
                    }

                    for (int i = 0; i < dlc; i++)
                    {
                       newframe.Data[i] = Canmessage[i+4];

                    }

                    bcframes.Add(newframe);
                    bcnodes.Add(newframe);
                    /*
                    if (!nodegroup.NodeExists(id))
                    {
                        newnode = new CANopen.Node();
                        newnode.Node_ID = id;
                        //newnode.Set_State(0x05);
                        nodegroup.AddNode(newnode);
                    }
                    */
                }

            }

            catch (OperationCanceledException)
            {



            }


        }

       public static void EmulateCanNetwork(int number, ListboxConsole lb, Nodegroup nodegroup)
       {

           
           BlockingCollection<Byte[]> bcoutput = new BlockingCollection<Byte[]>();

           BlockingCollection<CANopen.CAN_FRAME> bcframes = new BlockingCollection<CANopen.CAN_FRAME>();
           BlockingCollection<CANopen.CAN_FRAME> bcnodes = new BlockingCollection<CANopen.CAN_FRAME>();
           Random rd = new Random();
           byte dlc = new byte();
           byte data = new byte();
           CancellationTokenSource src = new CancellationTokenSource();
           CancellationToken ct = src.Token;

           
           
           
           Task frames = Task.Run(() => UserThreads.USB_To_CAN_FRAME_Converter(bcoutput, bcframes,bcnodes, ct));
           Task lbconsole = Task.Run(() => UserThreads.ListboxConsumer(bcframes, lb , src));
            Task node = Task.Run(() => UserThreads.NodeDispatcher(bcnodes, nodegroup, ct));

           byte[] testBuffer = new byte[64];

           testBuffer[0] = 0;
           for (int i = 0; i < number; i++)
           {

               foreach (var key in CANopen.Default_Identifier_Setup.Default_Identifier_Setup_Dict.Keys)
               {
                   

                  for (Byte k = 1; k < 128; k++)
                  {
                      dlc = (byte)(rd.Next(1, 8));
                      testBuffer[1] = dlc;
                      data = (byte)(rd.Next(0, 255));
                      testBuffer[2] = (byte)((key << 4) | (byte)(k >> 3));

                      testBuffer[3] = (byte)(k << 5) ; 
                      
                      for (byte l = 0; l < dlc; l++)
                      {
                          testBuffer[4+l] = data;
                      }
                      bcoutput.Add(testBuffer);
                      

                      Thread.Sleep(20);
                  }
               }
           
           }

           foreach (var key in CANopen.Default_NODE_STATES.Default_NODES_STATES_Dict.Keys)
           {
               for (Byte k = 1; k < 128; k++)
               {
                   dlc = 1;
                   testBuffer[1] = dlc;
                   data = (byte)(rd.Next(0, 255));
                   testBuffer[2] = (byte)((CANopen.Default_Identifier_Setup.NMT_EC << 4) | (byte)(k >> 3));

                   testBuffer[3] = (byte)(k << 5);

                   for (byte l = 0; l < dlc; l++)
                   {
                       testBuffer[4 + l] = key;
                   }
                   bcoutput.Add(testBuffer);


                   Thread.Sleep(20);
               }
           }

          // src.Cancel(false);



           //Generate Messages
       }
    }
}
