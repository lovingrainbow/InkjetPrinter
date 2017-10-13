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
            //Init UI Parameter
            //Valve Control
            //ComboBox
            cbValve.SelectedIndex = 0;
            cbClean.SelectedIndex = 0;
            CleanValve = 3;
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
            rbtnWater.Checked = true;
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
            //tbNeedle1 tbNeedle2 refresh
            lbNeedle1.Text = _Needle1.ToString();
            lbNeedle2.Text = _Needle2.ToString();
            lbValve.Text = _Valve.ToString();
            lbPlunger.Text = _Plunger.ToString();
            if ((Pump & 32) > 0)
            {
                lbPump.Text = "Ready";

            }
            else
            {
                lbPump.Text = "Busy";
            }
            if ((Pump & 15) > 0)
            {
                ErrorCode = (Pump & 15);
                lbPump.Text = "Error : " + ErrorCode.ToString();
            }
            /*
            if (ErrorCode > 0)
            {
                Stop();
                ButtonControl(false);
            }
            else
                ButtonControl(true);
                
            if (Busy)
                ButtonControl(false);
            else
                ButtonControl(true);
                */

            if (SendCount > 100)
            {
                tbSendData.Clear();
                tbReceiveData.Clear();
                SendCount = 0;
            }
            SendCount++;
        }

        public void DisplaySendData(string str)
        {
            tbSendData.Text += str + Environment.NewLine;
        }
        public void DisplayReceiveData(string str)
        {
            tbReceiveData.Text += str;
        }
        public void Stop()
        {
            Cycle_Pipe = false;
            Command = "/1T" + '\u000D';
            SendingCommand = true;
        }
        public void Pipe(int valve, int velocity, int plunger)
        {
            Command = "/1I" + valve.ToString() + "V" + velocity.ToString() + "A" + plunger.ToString() + "R" + '\u000D';
            SendingCommand = true;
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
                            if (Plunger > 0)
                                step++;
                            else
                                step = 4;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(3, PipeOutVel, 0);
                        else
                            Pipe(3, PipeOutVel, (Plunger - RemainingVolume));
                        
                        if (Busy)
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
                            Pipe(2, PipeInVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeInVel, 48000);
                        else
                            Pipe(2, PipeInVel, 48000);
                        if (Busy)
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
                            if (Plunger > 0)
                                step++;
                            else
                                step = 4;
                        else
                            Cycle_Pipe = false;
                        break;
                    case 1:
                        //PipeOut
                        PlungerBuffer = Plunger;
                        if (RemainingVolume > Plunger)
                            Pipe(4, PipeOutVel, 0);
                        else
                            Pipe(4, PipeOutVel, (Plunger - RemainingVolume));
                        if (Busy)
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
                            Pipe(2, PipeInVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeInVel, 48000);
                        else
                            Pipe(2, PipeInVel, 48000);
                        if (Busy)
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
                        if (Busy)
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
                        {
                            Cycle_Pipe = false;
                            MessageBox.Show("針1抽取動作完成");
                        }
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 0);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 0);
                        else
                            Pipe(2, PipeOutVel, 0);
                        if (Busy)
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
                            Pipe(3, PipeOutVel, 0);
                        else
                            Pipe(3, PipeOutVel, (Plunger - RemainingVolume));
                        if (Busy)
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
                        {
                            Cycle_Pipe = false;
                            MessageBox.Show("針1排出動作完成");
                        }
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeInVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeInVel, 48000);
                        else
                            Pipe(2, PipeInVel, 48000);
                        if (Busy)
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
                        if (Busy)
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
                        {
                            Cycle_Pipe = false;
                            MessageBox.Show("針2抽取動作完成");
                        }                            
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeOutVel, 0);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeOutVel, 0);
                        else
                            Pipe(2, PipeOutVel, 0);
                        if (Busy)
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
                            Pipe(4, PipeOutVel, 0);
                        else
                            Pipe(4, PipeOutVel, (Plunger - RemainingVolume));
                        if (Busy)
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
                        {
                            Cycle_Pipe = false;
                            MessageBox.Show("針2排出動作完成");
                        }                            
                        break;
                    case 4:
                        //empty air or water
                        if (rbtnAir.Checked)
                            Pipe(2, PipeInVel, 48000);
                        else if (rbtnWater.Checked)
                            Pipe(1, PipeInVel, 48000);
                        else
                            Pipe(2, PipeInVel, 48000);
                        if (Busy)
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
        public void CleanCycle()
        {
            int step = 0;
            int CycleCount = 0;
            while(Cycle_Pipe)
            {
                switch(step)
                {
                    case 0:
                        if (Plunger > 0)
                        {
                            step++;
                        }                            
                        else
                            step = 3;
                        break;
                    case 1:
                        {
                            Pipe(CleanValve, PipeOutVel, 0);
                            if (Busy)
                                step++;
                            break;
                        }
                    case 2:
                        if (Plunger == 0)
                        {
                            if (CycleCount >= CleanTime)
                            {
                                Cycle_Pipe = false;
                                MessageBox.Show("清洗動作完成");
                            }                                
                            else
                                step++;                
                        }                            
                        break;
                    case 3:
                        {
                            if (rbtnWater.Checked)
                                Pipe(1, PipeInVel, 48000);
                            else
                                Pipe(2, PipeInVel, 48000);

                            if (Busy)
                                step++;
                            break;
                        }
                    case 4:
                        if (Plunger == 48000)
                        {
                            CycleCount++;
                            step = 0;
                        }
                        break;
                    default:
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
                SendingCommand = true;
            }            
        }

        private void btnValve_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Command = "/1I" + Convert.ToString(cbValve.SelectedIndex + 1) + "R" + '\u000D';
                SendingCommand = true;
            }            
        }

        private void btnPumpInit_Click(object sender, EventArgs e)
        {
            Needle1 = 0;
            Needle2 = 0;
            Cycle_Pipe = false;
            Command = "/1ZN2R" + '\u000D';
            SendingCommand = true;
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

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (!Cycle_Pipe)
            {
                Cycle_Pipe = true;
                Thread CleanThread = new Thread(CleanCycle);
                CleanThread.Start();
            }
        }

        private void cbClean_SelectedIndexChanged(object sender, EventArgs e)
        {
            CleanValve = cbClean.SelectedIndex + 3;
        }

        private void tbCleanTimes_TextChanged(object sender, EventArgs e)
        {
            if (tbCleanTimes.Text.Length > 0)
                CleanTime = int.Parse(tbCleanTimes.Text);
            else
                CleanTime = 0;
        }
    }
}
