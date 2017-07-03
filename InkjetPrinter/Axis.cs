using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantech.Motion;

namespace InkjetPrinter
{
    public class Axis
    {
        public class AxisIO
        {
            public bool RDY;
            public bool ALM;
            public bool LMTP;
            public bool LMTN;
            public bool EMG;
            public bool SVON;
            public bool OK;
            public bool RUN;
        }
        public class AxisPara
        {
            double _VelLow;
            double _VelHigh;
            double _Acc;
            double _Dec;
            double _JogVel;
            double _HomeVelLow;
            double _HomeVelHigh;
            double _HomeAcc;
            double _HomeDec;
            public double Needle1;
            public double Needle2;
            public double Camera;
            public double Sample1;
            public double Sample2;
            public double Sample3;
            public double Sample4;
            public double CurCmd;
            public double CurPos;
            public double Distance;
            public double MaxVel
            { get; set; }
            public double MaxAcc
            { get; set; }
            public double MaxDec
            { get; set; }
            public double SoftLMTP
            { get; set; }
            public double SoftLMTN
            { get; set; }
            public double VelLow
            {
                get { return _VelLow; }
                set
                {
                    if (value > MaxVel)
                        _VelLow = MaxVel;
                    else
                        _VelLow = value;
                }
            }
            public double VelHigh
            {
                get { return _VelHigh; }
                set
                {
                    if (value > MaxVel)
                        _VelHigh = MaxVel;
                    else
                        _VelHigh = value;
                }
            }
            public double Acc
            {
                get { return _Acc; }
                set
                {
                    if (value > MaxAcc)
                        _Acc = MaxAcc;
                    else
                        _Acc = value;
                }
            }
            public double Dec
            {
                get { return _Dec; }
                set
                {
                    if (value > MaxDec)
                        _Dec = MaxDec;
                    else
                        _Dec = value;
                }
            }
            public double JogVel
            {
                get { return _JogVel; }
                set
                {
                    if (value > MaxVel)
                        _JogVel = MaxVel;
                    else
                        _JogVel = value;
                }
            }
            public double HomeVelLow
            {
                get { return _HomeVelLow; }
                set
                {
                    if (value > MaxVel)
                        _HomeVelLow = MaxVel;
                    else
                        _HomeVelLow = value;
                }
            }
            public double HomeVelHigh
            {
                get { return _HomeVelHigh; }
                set
                {
                    if (value > MaxVel)
                        _HomeVelHigh = MaxVel;
                    else
                        _HomeVelHigh = value;
                }
            }
            public double HomeAcc
            {
                get { return _HomeAcc; }
                set
                {
                    if (value > MaxAcc)
                        _HomeAcc = MaxAcc;
                    else
                        _HomeAcc = value;
                }
            }
            public double HomeDec
            {
                get { return _HomeDec; }
                set
                {
                    if (value > MaxDec)
                        _HomeDec = MaxDec;
                    else
                        _HomeDec = value;
                }
            }
            public UInt32 HomeDir
            { get; set; }
            public UInt32 HomeMode
            { get; set; }
        }
        IntPtr _AxHand = IntPtr.Zero;
        public UInt16 State;
        UInt32 _AxIOState;
        public AxisIO IO = new AxisIO();
        public AxisPara Para = new AxisPara();
        public bool bInit;
        public bool bMoveDone;

        public Axis (IntPtr AxisHand , AxisPara Para)
        {
            _AxHand = AxisHand;
            this.Para = Para;
            SetCurCmd(Para);
        }

        private void SetCurCmd(AxisPara Para)
        {
            int Result;
            Result = (int)Motion.mAcm_AxSetCmdPosition(_AxHand, Para.CurCmd);
            Log.log("Set CurCmd");
            if (Result != (int)ErrorCode.SUCCESS)
                Log.ErrorLog("Set CurCmd", Result);
        }

        public void Refresh()
        {
            int Result;
            Result = (int)Motion.mAcm_AxGetState(_AxHand, ref State);
            Result = (int)Motion.mAcm_AxGetCmdPosition(_AxHand, ref Para.CurCmd);
            Result = (int)Motion.mAcm_AxGetActualPosition(_AxHand, ref Para.CurPos);
            Result = (int)Motion.mAcm_AxGetMotionIO(_AxHand, ref _AxIOState);
            
            //Log.log("Refresh");
            if (Result != (int)ErrorCode.SUCCESS)
                Log.ErrorLog("Refresh", Result);

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_RDY) > 0)
                IO.RDY = true;
            else
                IO.RDY = false;

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_ALM) > 0)
                IO.ALM = true;
            else
                IO.ALM = false;

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_LMTP) > 0)
                IO.LMTP = true;
            else
                IO.LMTP = false;

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_LMTN) > 0)
                IO.LMTN = true;
            else
                IO.LMTN = false;

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_EMG) > 0)
                IO.EMG = true;
            else
                IO.EMG = false;

            if ((_AxIOState & (uint)Ax_Motion_IO.AX_MOTION_IO_SVON) > 0)
                IO.SVON = true;
            else
                IO.SVON = false;

            if (State == 1 && IO.RDY && IO.ALM && !IO.EMG)
                IO.OK = true;
            else
                IO.OK = false;

            if (State == 2 || State == 4 || State == 5 || State == 6 || State == 7 || State == 8 || State == 9)
                IO.RUN = true;
            else
                IO.RUN = false;

        }

        public void ServoOn()
        {
            int Result;
            if (IO.SVON)
            {
                Result = (int)Motion.mAcm_AxSetSvOn(_AxHand, 0);
                Log.log("Servo Off");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Servo Off", Result);
                }
            }
            else
            {
                Result = (int)Motion.mAcm_AxSetSvOn(_AxHand, 1);
                Log.log("Servo On");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Servo On", Result);
                }
            }
        }
        public void MoveRel(double Distance)
        {
            int Result;
            double move = Distance;
            if (move > (Para.SoftLMTP - Para.CurCmd))
            {
                move = Para.SoftLMTP - Para.CurCmd;
                Log.warningLog("Move distance over max distance");
            }

            if (move < Para.SoftLMTN - Para.CurCmd)
            {
                move = Para.SoftLMTN - Para.CurCmd;
                Log.warningLog("Move distance over max distance");
            }


            if (IO.OK)
            {
                SetVel();
                Result = (int)Motion.mAcm_AxMoveRel(_AxHand, move);
                Log.log("Axis move relatively : " + Distance.ToString() + "mm");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Axis move relatively", Result);
                }
            }
            else
            {
                Log.warningLog("IO not ready");
            }
        }
        public void MoveAbs(double Distance)
        {
            int Result;
            double move = Distance;
            if (move > (Para.SoftLMTP))
            {
                move = Para.SoftLMTP;
                Log.warningLog("Move distance over max distance");
            }

            if (move < Para.SoftLMTN)
            {
                move = Para.SoftLMTN;
                Log.warningLog("Move distance over max distance");
            }


            if (IO.OK)
            {
                SetVel();
                Result = (int)Motion.mAcm_AxMoveAbs(_AxHand, move);
                Log.log("Axis move absolutely : " + move.ToString() + "mm");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Axis move absolutely", Result);
                }
            }
            else
            {
                Log.warningLog("IO not ready");
            }
        }
        public void JogP()
        {
            int Result;
            if (IO.OK)
            {
                SetJogVel();
                Result = (int)Motion.mAcm_AxMoveAbs(_AxHand, Para.SoftLMTP);
                Log.log("Jog Positive");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Jog Positive", Result);
                }
            }
            else
            {
                Log.warningLog("IO not ready");
            }
        }
        public void JogN()
        {
            int Result;
            if (IO.OK)
            {
                SetJogVel();
                Result = (int)Motion.mAcm_AxMoveAbs(_AxHand, Para.SoftLMTN);
                Log.log("Jog Negative");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Jog Negative", Result);
                }
            }
            else
            {
                Log.warningLog("IO not ready");
            }
        }
        
        public void SetVel()
        {
            int Result;
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelLow, Para.VelLow);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelHigh, Para.VelHigh);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxAcc, Para.Acc);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxDec, Para.Dec);
            Log.log("Axis set velocity parameter");
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Axis set velocity parameter", Result);
            }    
        }
        public void SetJogVel()
        {
            int Result;
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelLow, Para.VelLow);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelHigh, Para.JogVel);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxAcc, Para.Acc);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxDec, Para.Dec);
            Log.log("Axis set Jog parameter");
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Axis set Jog parameter", Result);
            }
        }
        public void SetHomeVel()
        {
            int Result;
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelLow, Para.HomeVelLow);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxVelHigh, Para.HomeVelHigh);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxAcc, Para.HomeAcc);
            Result = (int)Motion.mAcm_SetF64Property(_AxHand, (uint)PropertyID.PAR_AxDec, Para.HomeDec);
            Log.log("Axis set Home parameter");
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Axis set Home parameter", Result);
            }
        }
        public void Home()
        {
            int Result;
            if (IO.OK)
            {
                SetHomeVel();
                Result = (int)Motion.mAcm_AxHome(_AxHand, Para.HomeMode, Para.HomeDir);
                Log.log("Axis Home");
                if (Result != (int)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Axis Home", Result);
                }
            }
            else
                Log.warningLog("IO not ready");
        }
        public void ErrorReset()
        {
            int Result;

            Result = (int)Motion.mAcm_AxResetError(_AxHand);
            Log.log("Axis reset error");
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Axis reset error", Result);
            }
        }
        public void StopDec()
        {
            int Result;

            Result = (int)Motion.mAcm_AxStopDec(_AxHand);
            Log.log("Axis stop deceleration");
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Axis stop deceleration", Result);
            }
        }

        

    }
}
