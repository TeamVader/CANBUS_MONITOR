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



namespace CANBUS_MONITOR
{
    public class Nodegroup : ItemsControl
    {

        public class Nodeindex
        {
            public int index { get; set; }
            public CANopen.Node Node { get; set; }
        }
        /// <summary>
        /// List of all Nodes during monitoring
        /// 
        /// </summary>
        public ObservableCollection<CANopen.Node> Nodecollection { get; set; }

        

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

        public Nodegroup()
        {
            this.Nodecollection = new ObservableCollection<CANopen.Node>();
            
            // Debug.Print(this._Nodelist.Capacity.ToString());
            this.nodesize = 120;

            this.ItemsSource = this.Nodecollection;
        }

       
        


        /// <summary>
        /// Check if node exists in Dictionary
        /// </summary>
        /// <param name="NodeID"></param>
        /// <returns></returns>
        public bool NodeExists(int NodeID)
        {
            /*if (Nodecollection.Contains(NodeID))
            {
                return true;
            }
            return false;*/
            return true;
        }

        /// <summary>
        /// return node with at Node ID Key
        /// </summary>
        /// <param name="NodeID"></param>
        /// <returns></returns>
        public CANopen.Node ReturnNode(int NodeID)
        {
            CANopen.Node result;
           /* if (Nodecollection.TryGetValue(NodeID, out result))
            {
                return result;
            }
            return null;*/
            return null;
        }


        /// <summary>
        /// Add a Node to the Dictionary
        /// </summary>
        /// <param name="newnode"></param>
        /// <returns></returns>
        public bool AddNode(CANopen.Node newnode)
        {
            if (newnode.Node_ID > MAX_NODES) // || newnode.Node_ID < MIN_NODE_ID)
            {
                Debug.Print("NodeID has to be in range between {0} to {1}", MIN_NODE_ID.ToString(), MAX_NODES.ToString());
                return false;
            }
            if (NodeExists(newnode.Node_ID))
            {
                if (NodeCount() > 0)
                {
                    Nodecollection.AddSorted(newnode);
                }
                else
                {
                    
                    Nodecollection.Add(newnode);
                }
                //Nodecollection.Insert(newnode.Node_ID,newnode);
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
                Nodecollection.RemoveAt(NodeID);
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
            return Nodecollection.Count;
        }

       

    }

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
    }
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
