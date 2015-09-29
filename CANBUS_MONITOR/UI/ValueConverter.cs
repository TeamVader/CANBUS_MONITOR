using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CANBUS_MONITOR
{
    public class StateToBackgroundColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo
         culture)
        {
            byte state = (byte)value;
            switch (state)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
