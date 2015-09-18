using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CANBUS_MONITOR
{
    
        public class HexFile
        {
            public enum eRecordType
            {
                DataRecord = 0,
                EndOfFileRecord = 1,
                ExtendedSegmentAddressRecord = 2,
                StartSegmentAddressRecord = 3,
                ExtendedLinearAddressRecord = 4,
                StartLinearAddressRecord = 5
            }

            private int _ErrorCounter = 0;
            public int ErrorCounter
            {
                get { return this._ErrorCounter; }
                set { _ErrorCounter = value; }
            }

            private int _filesize = 0;
            public int Filesize
            {
                get { return this._filesize; }
            }

            private string _Filename = "";
            public string Filename
            {
                get { return this._Filename; }
            }

            private List<HexFileLine> _HexFileLines;
            public List<HexFileLine> HexFileLines
            {
                get { return this._HexFileLines; }
            }

            // Konstruktor
            public HexFile()
            {
                this._HexFileLines = new List<HexFileLine>();
            }

            public void LoadFile(string vsFilename)
            {
                string sLine;
                FileStream fs;
                StreamReader sr;
                HexFileLine hexline;

                if (!System.IO.File.Exists(vsFilename))
                    throw new ArgumentException(String.Format("Die Datei '{0}' existiert nicht", vsFilename));
                this._Filename = vsFilename;

                // Datei öffnen
                try
                {
                    fs = new FileStream(vsFilename, FileMode.Open);
                    sr = new StreamReader(fs);
                }
                catch
                {
                    throw new Exception(string.Format("Fehler beim Öffnen der Datei '{0}'", vsFilename));
                }

                // Zeilen einlesen
                try
                {
                    while (!sr.EndOfStream)
                    {
                        sLine = sr.ReadLine();
                        //Trace.TraceInformation(this.GetType().Name + "." + MethodInfo.GetCurrentMethod().Name.ToString() + "(): "
                        //   + String.Format("Less HexLine {0}: '{1}'", this.HexFileLines.Count, sLine));
                        hexline = new HexFileLine(sLine);
                        this._HexFileLines.Add(hexline);
                        this._filesize += 5 + hexline.NumBytes; //1 Byte Number of Data, 2 Byte Address, 1 Byte type of Record , 1Byte Checksum
                        if (hexline.Errorflag == 1)
                        {
                            this._ErrorCounter++;
                        }
                        
                        Thread.Sleep(0);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    sr.Close();
                    fs.Close();
                }
            }
        }

        // Daten einer Zeile einer Hex-Datei
        public class HexFileLine
        {
            // Anzahl der Datenbytes
            private Byte _NumBytes = 0;
            public Byte NumBytes
            {
                get { return this._NumBytes; }
            }

            private int _errorflag = 0;
            public int Errorflag
            {
                get { return this._errorflag; }
            }

            // Recordtyp der Daten
            private HexFile.eRecordType _RecordType = HexFile.eRecordType.DataRecord;
            public HexFile.eRecordType RecordType
            {
                get { return this._RecordType; }
            }
            private System.UInt16 _32BitAdress_UpperBytes = 0;

            /*
            private Adress _Adress;
            public struct Adress
            {
                System.UInt16 _lowerBytes;
                public System.UInt16 LowerBytes
                {
                    get {return _lowerBytes ; }
                    set { _lowerBytes = value ; }
                }
                System.UInt16 _upperBytes;
                public System.UInt16 UpperBytes
                {
                    get { return _upperBytes; }
                    set { _upperBytes = value; }
                }

            };

             */
            // Zieladresse der Daten
            private System.UInt16 _Address = 0;
            public System.UInt16 Address
            {
                get { return this._Address; }
            }

            // Liste mit Daten
            private System.Byte[] _Data;
            public System.Byte[] Data
            {
                get { return this._Data; }
            }
            // Checksum
            private System.Byte _Checksum;
            public System.Byte Checksum
            {
                get { return this._Checksum; }
            }

            private const int DataOffset = 9;

            // Konstruktor
            // vsLine enthält aus Datei gelesene Textzeile
            public HexFileLine(string vsLine)
            {
                int i;
                uint sum = 0;
                Byte twoscompl = 0;
                bool Extended_Flag = false; 
                if (vsLine.Length < 9)
                    throw new Exception("HexZeile zu kurz");
                if (vsLine[0] != ':')
                    throw new Exception("Zeilenbeginn != ':'");
                // Anzahl Bytes
                this._NumBytes = Convert.ToByte(vsLine.Substring(1, 2), 16);

                //sum += (Byte)this._NumBytes;
                // Adresse.
                
                this._Address = Convert.ToUInt16(vsLine.Substring(3, 4), 16);
                
                //sum += this._Address;
                //MessageBox.Show(sum.ToString());

                // Recordtype
                i = Convert.ToByte(vsLine.Substring(7, 2), 16);
                
                if (Enum.IsDefined(this._RecordType.GetType(), i))
                {
                    this._RecordType = (HexFile.eRecordType)i;
                    //sum += (Byte)i;
                }
                else
                    throw new Exception("unrecognized Recordtype");
                //MessageBox.Show(this._RecordType.ToString());
                switch (this._RecordType)
                {
                    case HexFile.eRecordType.DataRecord:
                        // Datenbytes einlesen
                        this._Data = new System.Byte[this._NumBytes];
                        for (i = 0; i < this._NumBytes; i++)
                        {
                            this._Data[i] = Convert.ToByte(vsLine.Substring(HexFileLine.DataOffset + i * 2, 2), 16);
                            //sum += (Byte)this._Data[i];
                        }
                        for(i=0 ; i < (vsLine.Length - 3) / 2 ; i++)
                        {

                         sum += Convert.ToByte(vsLine.Substring((i * 2)+1 , 2), 16);
                        }
                        // Checksum einlesen

                        this._Checksum = Convert.ToByte(vsLine.Substring(HexFileLine.DataOffset + this._NumBytes * 2, 2), 16);


                        //Checksum prüfen

                        twoscompl = (Byte)(~sum + 1) ;
                        //MessageBox.Show(twoscompl.ToString() + "  " + this._Checksum.ToString());
                        if(twoscompl != this._Checksum)
                        {
                            this._errorflag = 1;
                            //MessageBox.Show(Convert.ToString(twoscompl, 2) + "  " + Convert.ToString(this._Checksum, 2));
                        }
                       
                        break;
                    case HexFile.eRecordType.EndOfFileRecord:
                        break;
                    //case HexFile.eRecordType.ExtendedSegmentAddressRecord:
                    //    this._Address = (System.UInt16)(Convert.ToUInt16(vsLine.Substring(HexFileLine.DataOffset, 4), 16) * 16);
                    //    break;
                    case HexFile.eRecordType.ExtendedLinearAddressRecord:
                        Extended_Flag = true;
                        this._32BitAdress_UpperBytes = (System.UInt16)(Convert.ToUInt16(vsLine.Substring(HexFileLine.DataOffset, 4), 16));
                        break;
                    default:
                        throw new Exception(string.Format("Recordtype '{0:X}' nicht unterstützt", this._RecordType));
                }

                /*
                if (Extended_Flag)
                {
                    this._Adress.UpperBytes = _32BitAdress_UpperBytes;
                }
                 * */
            }
        }
    
}
