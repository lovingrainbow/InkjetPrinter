using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Xml;
using System.Windows.Forms;

namespace InkjetPrinter
{
    public class PumpDriver
    {
        SerialPort PumpPort = new SerialPort();
        //check serial port is busy
        //Pump state
        public int PlungerPosition;
        public int ValvePosition;
        //counter for refresh
        int count;
        //Pump Status
        public int ErrorCode;
        public bool Pump_Busy;
        //Pump Parameter
        private int _Pump_PullVel;
        private int _Pump_OutputVel;
        public int Pump_PlungerMovement;
        public int Pump_ValveMovement;
        public int Pump_PullVel
        {
            get
            {
                return _Pump_PullVel;
            }
            set
            {
                if (value > 6000)
                    _Pump_PullVel = 6000;
                else
                    _Pump_PullVel = value;
            }
        }
        public int Pump_OutputVel
        {
            get
            {
                return _Pump_OutputVel;
            }
            set
            {
                if (value > 6000)
                    _Pump_OutputVel = 6000;
                else
                    _Pump_OutputVel = value;
            }
        }

        string Command;
        
        public PumpDriver()
        {
            ReadPumpPortXml();
            PumpPort.DataReceived += new SerialDataReceivedEventHandler(PumpPort_DataReceived);
            PumpPort.ReadTimeout = 10;
            try
            {
                PumpPort.Open();
            }
            catch (Exception e)
            {
                Log.ErrorLog(e.ToString(), 0);
            }
            
        }

        private void ReadPumpPortXml()
        {
            try
            {
                // Create xml document
                XmlDocument doc = new XmlDocument();
                // Load xml document
                doc.Load("PumpPort.xml");
                XmlElement root = doc.DocumentElement;
                // select single xmlnode            
                // parse innertext to parameter
                //Read modbusport protocols
                PumpPort.PortName = root.SelectSingleNode("device").InnerText;
                PumpPort.BaudRate = int.Parse(root.SelectSingleNode("BaudRate").InnerText);
                PumpPort.DataBits = int.Parse(root.SelectSingleNode("DataBits").InnerText);
                switch (root.SelectSingleNode("Parity").InnerText)
                {
                    case "Even":
                        PumpPort.Parity = Parity.Even;
                        break;
                    case "Odd":
                        PumpPort.Parity = Parity.Odd;
                        break;
                    case "None":
                        PumpPort.Parity = Parity.None;
                        break;
                    default:
                        break;
                }
                switch (root.SelectSingleNode("StopBits").InnerText)
                {
                    case "One":
                        PumpPort.StopBits = StopBits.One;
                        break;
                    case "Two":
                        PumpPort.StopBits = StopBits.Two;
                        break;
                    case "None":
                        PumpPort.StopBits = StopBits.None;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Log.ErrorLog(e.ToString(), 0);
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Refresh()
        {
            string str;
            if (count < 10)
            {
                count++;
            }
            
              
            switch (count)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 6:
                case 7:
                case 8:
                case 9:
                    //plunger position query
                    str = GetPlungerPosition();
                    break;
                case 5:
                    //Valve Position query
                    str = GetValvePosition();
                    break;
                case 10:
                    str = GetPumpStatus();
                    break;
                case 11:
                    //Command Mode
                    str = Command;
                    count = 1;
                    break;
                default:
                    str = GetPlungerPosition();
                    count = 1;
                    break;
            }
            if (PumpPort.IsOpen)
                PumpPort.Write(str);
        }
        public void PumpPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] buffer = new Byte[1024];
            Byte[] data;
            int Length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
            data = new Byte[Length];
            switch (count)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 6:
                case 7:
                case 8:
                case 9:
                    //check position status
                    if (Length > 5)
                    {                        
                        if (buffer[0] == 47 && buffer[1] == 48 && buffer[Length - 2] == 13 && buffer[Length - 1] == 10)
                            for(int i = 2; i < Length - 3; i++)
                            {
                                data[i - 2] = buffer[i];
                            }
                        PlungerPosition = DecodePlungerPosition(Encoding.Default.GetString(data));
                    }
                    break;
                case 5:
                    //check valve status
                    if (Length > 5)
                    {
                        if (buffer[0] == 47 && buffer[1] == 48 && buffer[Length - 2] == 13 && buffer[Length - 1] == 10)
                            for (int i = 2; i < Length - 3; i++)
                            {
                                data[i - 2] = buffer[i];
                            }
                        ValvePosition = DecodePlungerPosition(Encoding.Default.GetString(data));
                    }
                    break;
                case 10:
                    //check pump status
                    if (Length > 5)
                    {
                        if (buffer[0] == 47 && buffer[1] == 48 && buffer[Length - 2] == 13 && buffer[Length - 1] == 10)
                        {
                            ErrorCode = buffer[2] & 15;
                            if ((buffer[2] & 32) > 0)
                                Pump_Busy = true;
                            else
                                Pump_Busy = false;
                        }                            
                    }
                    if (Pump_Busy)
                        Console.WriteLine("The Pump is busy.");
                    else
                        Console.WriteLine("The Pump is not busy");

                    if (ErrorCode > 0)
                        Log.ErrorLog("Pump Error Code : ", ErrorCode);
                    break;
                case 11:
                    //Command mode result
                    break;
                default:
                    break;
            }

        }
        public string GetPumpStatus()
        {
            return "/1Q" + '\u000D';
        }
        public string GetPlungerPosition()
        {
            return "/1?4" + '\u000D';
        }
        public string GetValvePosition()
        {
            return "/1?6" + '\u000D';
        }
        public void PlungerMove(int position, int vel)
        {
            //Check Position
            int _position;
            if (position > 48000)
                _position = 48000;
            else if (position < 0)
                _position = 0;
            else
                _position = position;
            //Check Vel
            int _vel;
            if (vel > 6000)
                _vel = 6000;
            else if (vel < 5)
                _vel = 5;
            else
                _vel = vel;

            Command = "/1V" + _vel.ToString() + "A" + _position.ToString() + "R" + '\u000D';
            count = 11;
        }
        public void ValveMove(int valve)
        {
            int _valve;
            if (valve > 6)
                _valve = 6;
            else if (valve < 1)
                _valve = 1;
            else
                _valve = valve;

            Command = "/1I" + _valve.ToString() + "R" + '\u000D';
            count = 11;
        }
        public void PipeOut(int valve)
        {
            int _valve;
            if (valve > 6)
                _valve = 6;
            else if (valve < 1)
                _valve = 1;
            else
                _valve = valve;

            Command = "/1I" + _valve.ToString() + "V" + _Pump_OutputVel.ToString() + "A0R" + '\u000D';
            count = 11;
        }
        public void PipeIn(int valve)
        {
            int _valve;
            if (valve > 6)
                _valve = 6;
            else if (valve < 1)
                _valve = 1;
            else
                _valve = valve;
            Command = "/1I" + _valve.ToString() + "V" + _Pump_PullVel.ToString() + "A48000R" + '\u000D';
        }
        public void PipeInOut(int position, int vel, int valve)
        {
            //Check Position
            int _position;
            if (position > 48000)
                _position = 48000;
            else if (position < 0)
                _position = 0;
            else
                _position = position;
            //Check Vel
            int _vel;
            if (vel > 6000)
                _vel = 6000;
            else if (vel < 5)
                _vel = 5;
            else
                _vel = vel;
            //check valve
            int _valve;
            if (valve > 6)
                _valve = 6;
            else if (valve < 1)
                _valve = 1;
            else
                _valve = valve;
            //command
            Command = "/1I" + _valve.ToString() + "V" + _vel.ToString() + "A" + _position.ToString() + "R" + '\u000D';
            count = 11;
        }

        public void PumpInitialize()
        {
            Command = "/1ZR" + '\u000D';
            count = 11;
        }
        public void Pumpstop()
        {
            Command = "/1T" + '\u000D';
            count = 11;
        }
        public int DecodePlungerPosition(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception e)
            {
                return PlungerPosition;
            }
        }
        public int DecodeValvePosition(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception e)
            {
                return ValvePosition;
            }
        }
        public void ClosePump()
        {
            PumpPort.Close();
        }
    }
}
