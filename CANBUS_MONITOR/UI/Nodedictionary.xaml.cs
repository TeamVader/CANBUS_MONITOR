using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CANBUS_MONITOR.UI
{
    /// <summary>
    /// Interaction logic for Nodedictionary.xaml
    /// </summary>
    public partial class Nodedictionary : UserControl
    {

        // ************************************************************************
        // Private Fields
        // ************************************************************************
        #region Private Fields

        /// <summary>
        /// The list being tested
        /// </summary>
        private IDictionary<int, CANopen.Node> list;
        /// <summary>
        /// A dictionary mapping how many times a key has been updated
        /// </summary>
        private Dictionary<string, int> versions;
        /// <summary>
        /// Random number generator for generating list items
        /// </summary>
        private Random random;
        /// <summary>
        /// List of actions to be taken on the list being tested
        /// </summary>
        private BlockingCollection<Action> actions;
        /// <summary>
        /// Flag indicating if values should be added to the collection
        /// concurrently or not
        /// </summary>
        private bool concurrent = false;


        #endregion Private Fields

        // ************************************************************************
        // Public Methods
        // ************************************************************************
        #region Public Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        /// 
        public Nodedictionary()
        {
            InitializeComponent();
            actions = new BlockingCollection<Action>();
            Thread collectionUpdater = new Thread(
              delegate()
              {
                  while (true)
                  {
                      foreach (Action d in actions.GetConsumingEnumerable())
                      {
                          d();
                         // Debug.Print("ddd");
                      }
                  }
              }
            );
            collectionUpdater.IsBackground = true;
            collectionUpdater.Start();

        }

        public void AddNode(CANopen.Node node)
        {
            AddAction(delegate()
            {
                //MessageBox.Show(list.Count.ToString()); 

                list.Add(node.Node_ID, node);
            });
        }

        /// <summary>
        /// Initializes the control wih the observable list passed in, and randomly
        /// creates the number of values passed in to populate the list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="elementCount"></param>
        public void InitializeList(IDictionary<int, CANopen.Node> list)
        {
            this.list = list;
            this.random = new Random(100);
            this.versions = new Dictionary<string, int>();
            this.concurrent = true;
           
            Itemscontrol_Nodes.ItemsSource = list;
            //MessageBox.Show("ddd"); 
        }

        #endregion Public Methods

        // ************************************************************************
        // Private Methods
        // ************************************************************************
        #region Private Methods

        /// <summary>
        /// Adds a dictionary modification action to the list of actions if
        /// concurrent, or just executes the action if not.
        /// </summary>
        /// <param name="action"></param>
        private void AddAction(Action action)
        {
            if (concurrent)
            {
                actions.Add(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Creates a random key
        /// </summary>
        /// <returns></returns>
        private string GetRandomKey()
        {
            int index = random.Next(100);
            return "key" + index.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// Creates a unique random key
        /// </summary>
        /// <returns></returns>
        private string GetUniqueRandomKey()
        {
            string key = null;
            do
            {
                key = GetRandomKey();
            } while (versions.ContainsKey(key));
            return key;
        }

        /// <summary>
        /// Creates a value string for the key and version number passed in
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static string GetValue(string key, int version)
        {
            return key.Replace("key", "value") + "-" + version.ToString();
        }

        #endregion Private Methods


    }
}
