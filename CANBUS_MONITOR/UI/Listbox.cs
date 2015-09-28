using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CANBUS_MONITOR
{
    public class ListboxPIC : ListBox
    {

        public void addItem(string newitem)
        {
            this.Items.Add(newitem);
            if (this.Items.Count > 10)
            {
                // MessageBox.Show(this.Items.Count.ToString());
                this.Items.RemoveAt(0);
            }
        }
    }

    public class ListboxConsole : ListBox
    {

        public void addItem(string newitem)
        {
            this.Items.Add(newitem);
            if (this.Items.Count > 6)
            {
                // MessageBox.Show(this.Items.Count.ToString());
                this.Items.RemoveAt(0);
            }
        }
    }
}
