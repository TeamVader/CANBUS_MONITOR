using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CANBUS_MONITOR
{
    public partial class CANopen
    {
        public class Node
        {
            public Byte Node_ID { get; set; }
            public Byte State { get; set; }
            public String State_Description { get; set; }
            public Brush Backgroundcolor { get; set; } 


            public void Set_State(Byte State)
            {
                this.State = State;
                this.State_Description = Default_NODE_STATES.GetDescription(State);
                this.Backgroundcolor = Get_State_Color(State);
            }
            public Brush Get_State_Color(Byte State)
            {
                switch (State)
                {
                    case (CANopen.Default_NODE_STATES.Boot_Up) :
                        return new SolidColorBrush(Colors.Blue);
                        
                    case (CANopen.Default_NODE_STATES.Operational) :
                        return new SolidColorBrush(Colors.LightGreen);
                    case (CANopen.Default_NODE_STATES.Stopped):
                        return new SolidColorBrush(Colors.Red);
                    case (CANopen.Default_NODE_STATES.Pre_operational):
                        return new SolidColorBrush(Colors.White);

                    default:
                        return new SolidColorBrush(Colors.White);
                }
            }

        }

        public class Nodedictionary
        {
            /// <summary>
            /// Dictionary of all Nodes during monitoring
            /// </summary>
            private Dictionary<int, Node> Nodedict = new Dictionary<int,Node>();
            private byte MAX_NODES = 0x7F;
            private byte MIN_NODE_ID = 0x01;


            /// <summary>
            /// Check if node exists in Dictionary
            /// </summary>
            /// <param name="NodeID"></param>
            /// <returns></returns>
            public bool NodeExists(int NodeID)
            {
                if (Nodedict.ContainsKey(NodeID))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// return node with at Node ID Key
            /// </summary>
            /// <param name="NodeID"></param>
            /// <returns></returns>
            public Node ReturnNode(int NodeID)
            {
                Node result;
                if (Nodedict.TryGetValue(NodeID, out result))
                {
                    return result;
                }
                return null;
            }


            /// <summary>
            /// Add a Node to the Dictionary
            /// </summary>
            /// <param name="newnode"></param>
            /// <returns></returns>
            public bool AddNode(Node newnode)
            {
                if (newnode.Node_ID > MAX_NODES || newnode.Node_ID < MIN_NODE_ID)
                {
                    Debug.Print("NodeID has to be in range between {0} to {1}",MIN_NODE_ID.ToString(), MAX_NODES.ToString());
                    return false;
                }
                if (!NodeExists(newnode.Node_ID))
                {
                    Nodedict.Add(newnode.Node_ID, newnode);
                    return true;
                }
                else
                {
                    Debug.Print("Node already Exists");
                    return false;
                }
            }

            public bool RemoveNode(int NodeID)
            {
                
                if (NodeExists(NodeID))
                {
                    Nodedict.Remove(NodeID);
                    return true;
                }
                else
                {
                    Debug.Print("Node is not existing");
                    return false;
                }
            }

            public int NodeCount()
            {
                return Nodedict.Count;
            }

        }


    }
}
