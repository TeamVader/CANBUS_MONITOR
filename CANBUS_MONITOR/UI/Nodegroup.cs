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
using System.Collections.Concurrent;


namespace CANBUS_MONITOR
{
    public class Nodegroup : ObservableConcurrentSortedDictionary<int, CANopen.Node> , INotifyPropertyChanged
    {
        


        private double _nodesize ;

        public double nodesize
        {
            get { return _nodesize; }
            set 
            { 
                _nodesize = value;
                InvokePropertyChanged("nodesize");
            }
        }

        

        private byte MAX_NODES = 0x7F;
        private byte MIN_NODE_ID = 0x01;
  
        public Nodegroup(): base(new KeyComparer()) //
        {
           
        }

        #region key comparer class
        
        private class KeyComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                int resultx = (int)entry1.Key;
                int resulty = (int)entry2.Key;
                return resultx.CompareTo(resulty);
            }
        }
        
        #endregion key comparer class


        public bool NodeExists(int NodeID)
        {
            if(this.ContainsKey(NodeID))
                return true;
            return false;
        }

        public void InvokePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                // Debug.Print("dAS");
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
 

        
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
