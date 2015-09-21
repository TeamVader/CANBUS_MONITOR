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
        public class Node : IComparable
        {
            public Byte Node_ID { get; set; }
            public Byte State { get; set; }
            public String State_Description { get; set; }
            public Brush Backgroundcolor { get; set; }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;
                Node othernode = obj as Node;
                if (othernode != null)
                    return this.Node_ID.CompareTo(othernode.Node_ID);
                else
                    throw new ArgumentException("Object is not a node");
            }

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

        


    }

    
}
