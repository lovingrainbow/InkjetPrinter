using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InkjetPrinter
{
    public partial class AxisUserControl : UserControl
    {
        Axis _Axis;
        string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                gbTitle.Text = _Title;
            }
        }
        public AxisUserControl(Axis axis)
        {
            InitializeComponent();
            _Axis = axis;
            InitPara();
        }

        private void InitPara()
        {
            tbVelLow.Text = Para2Text(_Axis.Para.VelLow);
            tbVelHigh.Text = Para2Text(_Axis.Para.VelHigh);
            tbAcc.Text = Para2Text(_Axis.Para.Acc);
            tbDec.Text = Para2Text(_Axis.Para.Dec);
            tbDistance.Text = Para2Text(_Axis.Para.Distance);
            tbJogVel.Text = Para2Text(_Axis.Para.JogVel);
            tbHomeVelLow.Text = Para2Text(_Axis.Para.HomeVelLow);
            tbHomeVelHigh.Text = Para2Text(_Axis.Para.HomeVelHigh);
            tbHomeAcc.Text = Para2Text(_Axis.Para.HomeAcc);
            tbHomeDec.Text = Para2Text(_Axis.Para.HomeDec);
        }
        public double Text2Para(string text)
        {
            return double.Parse(text) * 1000;
        }

        public string Para2Text(double para)
        {
            return Convert.ToString(para / 1000);
        }
        private string StateConvert(UInt16 AxState)
        {
            switch (AxState)
            {
                case 0:
                    return "AxDisable";
                case 1:
                    return "AxReady";
                case 2:
                    return "Stopping";
                case 3:
                    return "AxErrorStop";
                case 4:
                    return "AxHoming";
                case 5:
                    return "AxPtpMotion";
                case 6:
                    return "AxContiMotion";
                case 7:
                    return "AxSyncMotion";
                case 8:
                    return "AX_EXT_JOG";
                case 9:
                    return "AX_EXT_MPG";
                default:
                    return "Error";

            }
        }
        public void RefreshUI()
        {
            if (_Axis.IO.RDY)
                lbRDY.BackColor = Color.Green;
            else
                lbRDY.BackColor = Color.Gray;

            if (_Axis.IO.ALM)
                lbALM.BackColor = Color.Green;
            else
                lbALM.BackColor = Color.Gray;

            if (_Axis.IO.LMTP)
                lbLMTP.BackColor = Color.Green;
            else
                lbLMTP.BackColor = Color.Gray;

            if (_Axis.IO.LMTN)
                lbLMTN.BackColor = Color.Green;
            else
                lbLMTN.BackColor = Color.Gray;

            if (_Axis.IO.SVON)
                btnSVN.Text = "Servo ON";
            else
                btnSVN.Text = "Servo OFF";

            lbState.Text = StateConvert(_Axis.State);
            lbCurCmd.Text = Para2Text(_Axis.Para.CurCmd);
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            switch(tb.Name)
            {
                case "tbDistance":
                    {
                        if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-')
                            e.Handled = false;
                        else
                            e.Handled = true;
                        if (e.KeyChar == '.' && tbDistance.Text.IndexOf('.') >= 0)
                            e.Handled = true;
                        if (e.KeyChar == '-' && tbDistance.Text.Length > 0)
                            e.Handled = true;
                        break;
                    }
                default:
                    {
                        if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.')
                            e.Handled = false;
                        else
                            e.Handled = true;
                        if (e.KeyChar == '.' && tb.Text.IndexOf('.') >= 0)
                            e.Handled = true;
                        break;
                    }
            }
            
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            switch(tb.Name)
            {
                case "tbVelLow":
                    if (tbVelLow.TextLength > 0)
                        _Axis.Para.VelLow = Text2Para(tbVelLow.Text);
                    break;
                case "tbVelHigh":
                    if (tbVelHigh.TextLength > 0)
                        _Axis.Para.VelHigh = Text2Para(tbVelHigh.Text);
                    break;
                case "tbAcc":
                    if (tbAcc.TextLength > 0)
                        _Axis.Para.Acc = Text2Para(tbAcc.Text);
                    break;
                case "tbDec":
                    if (tbDec.TextLength > 0)
                        _Axis.Para.Dec = Text2Para(tbDec.Text);
                    break;
                case "tbDistance":
                    if (tbDistance.TextLength > 0)
                        _Axis.Para.Distance = Text2Para(tbDistance.Text);
                    break;
                case "tbJogVel":
                    if (tbJogVel.TextLength > 0)
                        _Axis.Para.JogVel = Text2Para(tbJogVel.Text);
                    break;
                case "tbHomeVelLow":
                    if (tbHomeVelLow.TextLength > 0)
                        _Axis.Para.HomeVelLow = Text2Para(tbHomeVelLow.Text);
                    break;
                case "tbHomeVelHigh":
                    if (tbHomeVelHigh.TextLength > 0)
                        _Axis.Para.HomeVelHigh = Text2Para(tbHomeVelHigh.Text);
                    break;
                case "tbHomeAcc":
                    if (tbHomeAcc.TextLength > 0)
                        _Axis.Para.HomeAcc = Text2Para(tbHomeAcc.Text);
                    break;
                case "tbHomeDec":
                    if (tbHomeDec.TextLength > 0)
                        _Axis.Para.HomeDec = Text2Para(tbHomeDec.Text);
                    break;
                default:
                    break;
            }
        }

        private void btnJogP_MouseDown(object sender, MouseEventArgs e)
        {
            _Axis.JogP();
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            _Axis.StopDec();
        }

        private void btnJogN_MouseDown(object sender, MouseEventArgs e)
        {
            _Axis.JogN();
        }

        private void btnSVN_Click(object sender, EventArgs e)
        {
            _Axis.ServoOn();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            _Axis.Home();
        }

        private void btnMoveRel_Click(object sender, EventArgs e)
        {
            _Axis.MoveRel(_Axis.Para.Distance);
        }

        private void btnMoveAbs_Click(object sender, EventArgs e)
        {
            _Axis.MoveAbs(_Axis.Para.Distance);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _Axis.StopDec();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _Axis.ErrorReset();
        }


    }
}
