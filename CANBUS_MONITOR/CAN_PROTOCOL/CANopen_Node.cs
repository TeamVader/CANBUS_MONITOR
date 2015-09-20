using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
    public partial class CANopen
    {
        public class Node
        {
            public Byte Node_ID { get; set; }
            public Byte State { get; set; }
            public String State_Description { get; set; }

            public void Set_State(Byte State)
            {
                this.State = State;
                this.State_Description = Default_NODE_STATES.GetDescription(State);
            }

        }
    }
}
