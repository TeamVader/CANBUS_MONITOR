using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CANBUS_MONITOR
{
    public class CAN_Monitor_Classes
    {
        #region BIND CAN DEVICE STATE TO CONTROLS
        public class Connection_Monitoring : INotifyPropertyChanged
        {

          /*  private Nodegroup _nodegroup = new Nodegroup();
            public Nodegroup Nodegroup
            {

                get { return _nodegroup; }
                set
                {
                    _nodegroup = value;
                }
            }*/

            private string _mode;
            public string Mode
            {
                get { return _mode; }
                set
                {
                    _mode = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Mode");
                }
            }

            private string _speed;
            public string Speed
            {
                get { return _speed; }
                set
                {
                    _speed = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Speed");
                }
            }


            private string _traffic;
            public string Traffic
            {
                get { return _traffic; }
                set
                {
                    _traffic = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Traffic");
                }
            }

            private string _firmware;
            public string Firmware
            {
                get { return _firmware; }
                set
                {
                    _firmware = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Firmware");
                }
            }

            private bool _isBusActive;
            public bool IsBusActive
            {
                get { return _isBusActive; }
                set
                {
                    _isBusActive = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("IsBusActive");
                }
            }

            private bool _isConfigMode;
            public bool IsConfigMode
            {
                get { return _isConfigMode; }
                set
                {
                    _isConfigMode = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("IsConfigMode");
                }
            }

            private bool _isNormalMode;
            public bool IsNormalMode
            {
                get { return _isNormalMode; }
                set
                {
                    _isNormalMode = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("IsNormalMode");
                }
            }

            private bool _isPrograming;
            public bool IsPrograming
            {
                get { return _isPrograming; }
                set
                {
                    _isPrograming = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("IsPrograming");
                }
            }

            private bool _recording;
            public bool Recording
            {
                get { return _recording; }
                set
                {
                    _recording = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Recording");
                }
            }
            private bool _deviceconnection;
            public bool Deviceconnection
            {
                get { return _deviceconnection; }
                set
                {
                    _deviceconnection = value;
                    //MessageBox.Show ("dd");
                    InvokePropertyChanged("Deviceconnection");
                }
            }

            private double _nodesize;

            public double nodesize
            {
                get { return _nodesize; }
                set
                {
                    _nodesize = value;
                    InvokePropertyChanged("nodesize");
                }
            }


            public void Setnodesize(Double newdouble)
            {

                nodesize = newdouble;
            }

            public void SetisPrograming(bool newbool)
            {

                IsPrograming = newbool;
            }

            public void SetRecording(bool newbool)
            {

                Recording = newbool;
            }
            public void SetConnection(bool newbool)
            {

                Deviceconnection = newbool;
            }

            public void SetisBusActive(bool newbool)
            {

                IsBusActive = newbool;
            }

            public void SetConfigMode(bool newbool)
            {

                IsConfigMode = newbool;
            }

            public void SetNormalMode(bool newbool)
            {

                IsNormalMode = newbool;
            }

            public void SetMode(string newstring)
            {

                Mode = newstring;
            }
            public void SetFirmware(string newstring)
            {

                Firmware = newstring;
            }

            public void SetTraffic(string newstring)
            {

                Traffic = newstring;
            }

            public void SetSpeed(string newstring)
            {

                Speed = newstring;
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
        #endregion

       

        
    }


}
