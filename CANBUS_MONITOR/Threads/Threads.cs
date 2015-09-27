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

       public static void ListboxConsumer(BlockingCollection<Byte[]> bc, ListboxConsole lb,CancellationTokenSource ct)
        {
                string data = "";
                string CAN_Message = "CAN Message : ";
                int counter = 0;
                try
                {

                    foreach (var Canmessage in bc.GetConsumingEnumerable(ct.Token))
                    {
                         
                        
                        counter = (Canmessage[1] & 0x0F);
                        CAN_Message = "CAN Message : ";
            

                //MessageBox.Show(readBuffer[0].ToString());
                        data = " Data :";
                //ID extended or Standard
                        if ((Canmessage[1] & 0x20) == 0x20)
                         {
                          counter +=2;
                         }
                         


                        for (int i = 4; i < counter+4; i++)
                         {
                             data += Convert.ToString(Canmessage[i], 16).PadLeft(2, '0') + "  ";
                    
                         }
                
                           lb.Dispatcher.BeginInvoke(new Action(delegate()
                             {
                            lb.addItem(CAN_Message + string.Format("ID : {0}",((Canmessage[2] << 3) | (Canmessage[3] >> 5)).ToString("X3")) + data);
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

       public static void EmulateCanNetwork(int number, ListboxConsole lb)
       {

           BlockingCollection<Byte[]> bcoutput = new BlockingCollection<Byte[]>();

           Random rd = new Random();
           byte dlc = new byte();
           byte data = new byte();
           CancellationTokenSource src = new CancellationTokenSource();

           Task.Run(() => UserThreads.ListboxConsumer(bcoutput, lb , src));

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
                      testBuffer[2] = (byte)((key << 4) | (byte)(k >> 4));

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

          // src.Cancel(false);



           //Generate Messages
       }
    }
}
