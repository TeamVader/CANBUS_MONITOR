using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANBUS_MONITOR
{
  

    public class CAN_MESSAGE
    {

        /// <summary>
        /// CAN ID CLASS
        /// </summary>
        public class CAN_ID
        {
            public CAN_ID()
            {
            }
            public Byte SIDL { get; set; }
            public Byte SIDH { get; set; }
            public Byte EID0 { get; set; }
            public Byte EID8 { get; set; }
        }

        /// <summary>
        /// CAN DATA CLASS
        /// </summary>
        public class CAN_DATA
        {
            public CAN_DATA()
            {
            }
            public Byte D0 { get; set; }
            public Byte D1 { get; set; }
            public Byte D2 { get; set; }
            public Byte D3 { get; set; }
            public Byte D4 { get; set; }
            public Byte D5 { get; set; }
            public Byte D6 { get; set; }
            public Byte D7 { get; set; }
        }
    }
}
