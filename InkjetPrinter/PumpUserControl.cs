using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.IO.Ports;

namespace InkjetPrinter
{
    public partial class PumpUserControl : UserControl
    {
        public PumpUserControl()
        {
            InitializeComponent();
        }

        private void PumpUserControl_Load(object sender, EventArgs e)
        {
            //Load data from xml
            ReadPumpPortXml();
            ReadPumpParaXml();
            //Init Pump Port

            try
            {
                PumpPort.Open();
            }
            catch (Exception ex)
            {
                //TODO: write log
            }
            //Init UI Parameter
            //Valve Control
            //ComboBox
            cbValve.SelectedIndex = 0;
            //Plunger Control
            //tbPlunger
            tbPlunger.Text = "0";
            //tbPipeInVel
            tbPipeInVel.Text = _PipeInVel.ToString();
            //tbPipeOutVel
            tbPipeOutVel.Text = _PipeOutVel.ToString();
            //Sample
            //tbNeedle1PipeIn
            tbNeedle1PipeIn.Text = "0";
            //tbNeedle1PipeOut
            tbNeedle1PipeOut.Text = "0";
            //tbNeedle2PipeIn
            tbNeedle2PipeIn.Text = "0";
            //tbNeedle2PipeOut
            tbNeedle2PipeOut.Text = "0";
            //Pipe Control
            //tbNeedle1Pipe
            tbNeedle1Pipe.Text = _Needle1Pipe.ToString();
            //tbNeedle2Pipe
            tbNeedle2Pipe.Text = _Needle2Pipe.ToString();
            //radiobutton init
            rbtnAir.Checked = true;
        }
        private void IntText_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void Pump_TextChange(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
                switch (tb.Name)
                {
                    case "tbPlunger":
                        PlungerMove = int.Parse(tb.Text);
                        break;
                    case "tbPipeInVel":
                        PipeInVel = int.Parse(tb.Text);
                        break;
                    case "tbPipeOutVel":
                        PipeOutVel = int.Parse(tb.Text);
                        break;
                    case "tbNeedle1PipeIn":
                        Needle1PipeIn = int.Parse(tb.Text);
                        break;
                    case "tbNeedle1PipeOut":
                        Needle1PipeOut = int.Parse(tb.Text);
                        break;
                    case "tbNeedle2PipeIn":
                        Needle2PipeIn = int.Parse(tb.Text);
                        break;
                    case "tbNeedle2PipeOut":
                        Needle2PipeOut = int.Parse(tb.Text);
                        break;
                    case "tbNeedle1Pipe":
                        Needle1Pipe = int.Parse(tb.Text);
                        break;
                    case "tbNeedle2Pipe":
                        Needle2Pipe = int.Parse(tb.Text);
                        break;
                    default:
                        break;
                }
            }
        }
        public void ButtonControl(bool control)
        {
            btnValve.Enabled = control;
            btnPlunger.Enabled = control;
            btnNeedle1PipeIn.Enabled = control;
            btnNeedle1PipeOut.Enabled = control;
            btnNeedle2PipeIn.Enabled = control;
            btnNeedle2PipeOut.Enabled = control;
        }
        public void RefreshPumpUI()
        {
            if (PumpPort.IsOpen)
            {
                string str;

                if (count < 10)
                    count++;
                else
                    count = 1;

                switch (count)
                {
                    case 0:
                        //Command Mode
                        str = Command;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        //plunger position query
                        str = "/1?4" + '\u000D';
                        break;
                    case 5:
                        //Valve Position query
                        str = "/1?6" + '\u000D';
                        break;
                    case 10:
                        str = "/1Q" + '\u000D';
                        break;
                    default:
                        str = "/1?4" + '\u000D';
                        count = 1;
                        break;
                }
                tbSendData.Text += str + Environment.NewLine;
                ParameterizedThreadStart refreshRun = new ParameterizedThreadStart(SerialCommunication);
                Thread refresh = new Thread(refreshRun);
                refresh.Start(str);
            }
            else
            {
                tbSendData.Text += "Serial port can't open." + Environment.NewLine;
            }

            //tbNeedle1 tbNeedle2 refresh
            lbNeedle1.Text = _Needle1.ToString();
            lbNeedle2.Text = _Needle2.ToString();

            if (SendCount > 100)
            {
                tbSendData.Clear();
                tbReceiveData.Clear();
                SendCount = 0;
            }
            SendCount++;
        }
        public void SerialCommunication(object o)
        {
            string command = o as string;
            PumpPort.DiscardInBuffer();
            PumpPort.DiscardOutBuffer();
            PumpPort.Write(command);
            Thread.Sleep(50);
            DataReceived();
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
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void DataReceived()
        {
            try
            {
                Byte[] buffer = new Byte[1024];
                Byte[] data;
                int Length = PumpPort.Read(buffer, 0, buffer.Length);
                data = new Byte[Length];
                switch (count)
                {
                    case 0:
                        //Command mode result
                        break;
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
                                for (int i = 3; i < Length - 4; i++)
                                {
                                    data[i - 2] = buffer[i];
                                }
                            try
                            {
                                Plunger = int.Parse(Encoding.Default.GetString(data));
                            }
                            catch (Exception ex)
                            {
                                //TODO: write log
                            }
                        }
                        break;
                    case 5:
                        //check valve status
                        if (Length > 5)
                        {
                            if (buffer[0] == 47 && buffer[1] == 48 && buffer[Length - 2] == 13 && buffer[Length - 1] == 10)
                                for (int i = 3; i < Length - 4; i++)
                                {
                                    data[i - 2] = buffer[i];
                                }
                            try
                            {
                                Valve = int.Parse(Encoding.Default.GetString(data));
                            }
                            catch (Exception ex)
                            {
                                //TODO: write log
                            }
                        }
                        break;
                    case 10:
                        //check pump status
                        if (Length > 5)
                        {
                            if (buffer[0] == 47 && buffer[1] == 48 && buffer[Length - 2] == 13 && buffer[Length - 1] == 10)
                            {
                                Pump = buffer[2];
                            }
                        }
                        break;
                    default:
                        break;
                }
                //display data to textbox

                Display d = new Display(DisplayReceiveData);
                this.Invoke(d, Encoding.Default.GetString(buffer) + "(" + Length.ToString() + ")");
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }
        public void DisplayReceiveData(string str)
        {
            tbReceiveData.Text += str;
            tbReceiveData.Text += Environment.NewLine;
        }
        public void Stop()
        {
            Cycle_Pipe = false;
            Command = "/1T" + '\u000D';
            count = -1;
        }
        public void Pipe(int valve, int velocity, int plunger)
        {
            Command = "/1I" + valve.ToString() + "V" + velocity.ToString() + "A" + plunger.ToString() + "R" + '\u000D';
            count = -1;
        }
        public void AutoCycle(int Needle)
        {
            Cycle_Pipe = true;
            if (Needle == 1)
            {
                Thread Needle1Pipe = new Thread(Needle1PipeCycle);
                Needle1Pipe.Start();
            }
            else if (Needle == 2)
            {
                Thread Needle2Pipe = new Thread(Needle2PipeCycle);
                Needle2Pipe.Start();
            }
            else
            {
                Thread Needle1Pipe = new Thread(Needle1PipeCycle);
                Needle1Pipe.Start();
            }
        }
        public void Needle1PipeCycle()
        {
            int step = 0;
            int RemainingVolume = Needle1Pipe;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {
                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(3, PipeInVel, 0);
                        else
                            Pipe(3, PipeInVel, (Plunger - RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume >= (PlungerBuffer) && Plunger == 0)
                        {
                            _Needle1 -= (PlungerBuffer);
                            RemainingVolume -= (PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume < (PlungerBuffer) && Plunger == (PlungerBuffer - RemainingVolume))
                        {
                            _Needle1 -= RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 48000);
                        else
                            Pipe(2, PipeOutVel, 48000);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 48000)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        public void Needle2PipeCycle()
        {
            int step = 0;
            int RemainingVolume = Needle2Pipe;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {

                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(4, PipeInVel, 0);
                        else
                            Pipe(4, PipeInVel, (Plunger - RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume >= (PlungerBuffer) && Plunger == 0)
                        {
                            _Needle2 -= (PlungerBuffer);
                            RemainingVolume -= (PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume < (PlungerBuffer) && Plunger == (PlungerBuffer - RemainingVolume))
                        {
                            _Needle2 -= RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 48000);
                        else
                            Pipe(2, PipeOutVel, 48000);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 48000)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        public void Needle1PipeInCycle()
        {
            int step = 0;
            int RemainingVolume = Needle1PipeIn;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {

                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeIn
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > (48000 - Plunger))
                            Pipe(3, PipeInVel, 48000);
                        else
                            Pipe(3, PipeInVel, (Plunger + RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume > (48000 - PlungerBuffer) && Plunger == 48000)
                        {
                            _Needle1 += (48000 - PlungerBuffer);
                            RemainingVolume -= (48000 - PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume <= (48000 - PlungerBuffer) && Plunger == (PlungerBuffer + RemainingVolume))
                        {
                            _Needle1 += RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 0);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 0);
                        else
                            Pipe(2, PipeOutVel, 0);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 0)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        public void Needle1PipeOutCycle()
        {
            int step = 0;
            int RemainingVolume = Needle1PipeOut;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {

                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(3, PipeInVel, 0);
                        else
                            Pipe(3, PipeInVel, (Plunger - RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume >= (PlungerBuffer) && Plunger == 0)
                        {
                            _Needle1 -= (PlungerBuffer);
                            RemainingVolume -= (PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume < (PlungerBuffer) && Plunger == (PlungerBuffer - RemainingVolume))
                        {
                            _Needle1 -= RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 48000);
                        else
                            Pipe(2, PipeOutVel, 48000);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 48000)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        public void Needle2PipeInCycle()
        {
            int step = 0;
            int RemainingVolume = Needle2PipeIn;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {

                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeIn
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > (48000 - Plunger))
                            Pipe(4, PipeInVel, 48000);
                        else
                            Pipe(4, PipeInVel, (Plunger + RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume > (48000 - PlungerBuffer) && Plunger == 48000)
                        {
                            _Needle2 += (48000 - PlungerBuffer);
                            RemainingVolume -= (48000 - PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume <= (48000 - PlungerBuffer) && Plunger == (PlungerBuffer + RemainingVolume))
                        {
                            _Needle2 += RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 0);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 0);
                        else
                            Pipe(2, PipeOutVel, 0);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 0)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        public void Needle2PipeOutCycle()
        {
            int step = 0;
            int RemainingVolume = Needle2PipeOut;
            int PlungerBuffer = 0;
            while (Cycle_Pipe)
            {

                switch (step)
                {
                    case 0:
                        //check volume
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(4, PipeInVel, 0);
                        else
                            Pipe(4, PipeInVel, (Plunger - RemainingVolume));
                        step++;
                        break;
                    case 2:
                        //wait pipein finish
                        if (RemainingVolume >= (PlungerBuffer) && Plunger == 0)
                        {
                            _Needle2 -= (PlungerBuffer);
                            RemainingVolume -= (PlungerBuffer);
                            step++;
                        }
                        else if (RemainingVolume < (PlungerBuffer) && Plunger == (PlungerBuffer - RemainingVolume))
                        {
                            _Needle2 -= RemainingVolume;
                            RemainingVolume = 0;
                            step++;
                        }
                        break;
                    case 3:
                        //Check still need work?
                        if (RemainingVolume > 0)
                            step++;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 48000);
                        else
                            Pipe(2, PipeOutVel, 48000);
                        step++;
                        break;
                    case 5:
                        //wait empty
                        if (Plunger == 48000)
                            step = 1;
                        break;
                    default:
                        //other step, error and return
                        Cycle_Pipe = false;
                        break;
                }
            }
        }
        private void btnPlunger_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Command = "/1V";
                if (PlungerMove > Plunger)
                    Command += PipeInVel.ToString();
                else
                    Command += PipeOutVel.ToString();

                Command += "A" + PlungerMove.ToString() + "R" + '\u000D';
                count = -1;
            }            
        }

        private void btnValve_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Command = "/1I" + Convert.ToString(cbValve.SelectedIndex + 1) + "R" + '\u000D';
                count = -1;
            }            
        }

        private void btnPumpInit_Click(object sender, EventArgs e)
        {
            Needle1 = 0;
            Needle2 = 0;
            Cycle_Pipe = false;
            Command = "/1ZR" + '\u000D';
            count = -1;
        }

        private void btnPumpStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnNeedle1PipeIn_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Cycle_Pipe = true;
                Thread Needle1PipeIn = new Thread(Needle1PipeInCycle);
                Needle1PipeIn.Start();
            }
            
        }

        private void btnNeedle1PipeOut_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Cycle_Pipe = true;
                Thread Needle1PipeOut = new Thread(Needle1PipeOutCycle);
                Needle1PipeOut.Start();
            }            
        }

        private void btnNeedle2PipeIn_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Cycle_Pipe = true;
                Thread Needle2PipeIn = new Thread(Needle2PipeInCycle);
                Needle2PipeIn.Start();
            }            
        }

        private void btnNeedle2PipeOut_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Cycle_Pipe = true;
                Thread Needle2PipeOut = new Thread(Needle2PipeOutCycle);
                Needle2PipeOut.Start();
            }            
        }

        private void tbSendData_TextChanged(object sender, EventArgs e)
        {
            //scroll down
            tbSendData.SelectionStart = tbSendData.Text.Length;
            tbSendData.ScrollToCaret();
        }

        private void tbReceiveData_TextChanged(object sender, EventArgs e)
        {
            //scroll down
            tbReceiveData.SelectionStart = tbReceiveData.Text.Length;
            tbReceiveData.ScrollToCaret();
        }

        public void PumpPortClose()
        {
            PumpPort.Close();
        }
        public void ReadPumpParaXml()
        {
            try
            {
                // Create xml document
                XmlDocument doc = new XmlDocument();
                // Load xml document
                doc.Load("PumpPara.xml");
                XmlElement root = doc.DocumentElement;
                // select single xmlnode            
                // parse innertext to parameter
                //Read pump para
                PipeInVel = int.Parse(root.SelectSingleNode("PipeInVel").InnerText);
                PipeOutVel = int.Parse(root.SelectSingleNode("PipeOutVel").InnerText);
                Needle1Pipe = int.Parse(root.SelectSingleNode("Needle1Pipe").InnerText);
                Needle2Pipe = int.Parse(root.SelectSingleNode("Needle2Pipe").InnerText);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void SavePumpParaXml()
        {
            // Create xml document
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("PumpPara");
            doc.AppendChild(root);

            XmlElement PipeInVel = doc.CreateElement("PipeInVel");
            PipeInVel.InnerText = this.PipeInVel.ToString();
            root.AppendChild(PipeInVel);

            XmlElement PipeOutVel = doc.CreateElement("PipeOutVel");
            PipeOutVel.InnerText = this.PipeOutVel.ToString();
            root.AppendChild(PipeOutVel);

            XmlElement Needle1Pipe = doc.CreateElement("Needle1Pipe");
            Needle1Pipe.InnerText = this.Needle1Pipe.ToString();
            root.AppendChild(Needle1Pipe);

            XmlElement Needle2Pipe = doc.CreateElement("Needle2Pipe");
            Needle2Pipe.InnerText = this.Needle2Pipe.ToString();
            root.AppendChild(Needle2Pipe);
            // save xml file
            doc.Save("PumpPara.xml");
        }
    }
}
