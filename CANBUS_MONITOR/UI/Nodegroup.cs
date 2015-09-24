using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;



namespace CANBUS_MONITOR
{
    public class Nodegroup : ObservableDictionary<int, CANopen.Node>
    {
        

        
        /// <summary>
        /// List of all Nodes during monitoring
        /// 
        /// </summary>
        

        /*
        

        private double _nodesize ;

        public double nodesize
        {
            get { return _nodesize; }
            set 
            { 
                _nodesize = value;
            }
        }

        

        private byte MAX_NODES = 0x7F;
        private byte MIN_NODE_ID = 0x01;
  */
        public Nodegroup(): base() //new KeyComparer()
        {
           
        }

        #region key comparer class
        /*
        private class KeyComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        */
        #endregion key comparer class

        


 

        
    }
    /*
    public static class Addclass
    {
        /// <summary>
        /// Add a Sorted Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        public static void AddSorted<T>(this IList<T> list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;
            int i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) > 0)
            {
                Debug.Print(comparer.Compare(list[i], item).ToString());
                i++;
            }
            list.Insert(i, item);

        }
    }*/
    /*
    public class IDSorter : IComparer<CANopen.Node>
    {
        public int Compare(object x, object y)
        {
            CANopen.Node nodex = x as CANopen.Node;
            CANopen.Node nodey = y as CANopen.Node;
            return nodex.Node_ID.CompareTo(nodex.Node_ID);

        }
    }*/
}
