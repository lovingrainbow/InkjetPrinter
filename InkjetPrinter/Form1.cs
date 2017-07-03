using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Advantech.Motion;
using Basler.Pylon;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO.Ports;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using FtdAdapter;
using System.Threading;
using System.IO;

namespace InkjetPrinter
{
    public partial class InkJetPrinter : Form
    {
        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
        uint deviceCount = 0;
        uint AxisCount = 0;
        uint DeviceNum = 0;
        IntPtr Device_Hand = IntPtr.Zero;
        string pMotionDevCfg = @"C:\Users\nthu\Desktop\UI\UI\bin\Debug\MotionDev.cfg";
        IntPtr[] axis_Hand = new IntPtr[4];
        Axis[] axis = new Axis[3];
        Axis.AxisPara[] axisPara = new Axis.AxisPara[3];
        AxisUserControl[] axisUI = new AxisUserControl[3];

        System.Windows.Forms.Timer Timer_100ms = new System.Windows.Forms.Timer();

        DataTable dtPoint;
        int dataCount;

        bool RunPointStart = false;
        int RunPointStep = 0;
        int RunPointIndex;
        Point _MachinePoint = new Point();
        Point _SoftwarePoint = new Point();
        Point[] _RunPoint = new Point[3];
        PointConverter _PointConverter = new PointConverter();

        //Camera variable
        Camera camera = null;
        Bitmap bitmap;
        private PixelDataConverter converter = new PixelDataConverter();
        private Stopwatch stopWatch = new Stopwatch();
        string PhotoDir = "E:\\";

        //Modbus Port
        SerialPort ModbusPort = new SerialPort();
        IModbusSerialMaster ModbusMaster;
        byte slaveId = 1;
        bool heaterState = false;
        ushort[] temperature;
        double heaterTemp;
        BackgroundWorker ModbusPollingWork = new BackgroundWorker();
        //Auto Cycle
        bool Cycle_Auto;




        public InkJetPrinter()
        {
            InitializeComponent();

            ReadAxisXml();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //create file dir
            PhotoDir += DateTime.Now.ToString("yyyyMMddHHmm");
            Directory.CreateDirectory(PhotoDir);
            //open PCI board
            OpenMotionCard();
            //load AxisUI
            AddAxisUI();
            //axis Servo On
            for (int i = 0; i < axis.Length; i++)
            {
                axis[i].ServoOn();
            }
            //Brake ON
            uint Result = Motion.mAcm_DaqDoSetBit(Device_Hand, 0, 1);
            Log.log("Set D0 ON.");
            if (Result != (uint)ErrorCode.SUCCESS)
                Log.ErrorLog("Set D0 ON", (int)Result);
            //Init Modbus Port
            ReadModbusPortXml();
            ModbusPollingWork.DoWork += ModbusPollingWork_DoWork;
            ModbusPollingWork.RunWorkerAsync();
            //Init Serial Port for pump


            //Enable Refresh Timer
            Timer_100ms.Interval = 200;
            Timer_100ms.Tick += Timer_100ms_Tick;
            Timer_100ms.Enabled = true;
            //Init Datagridview Table
            dtPoint = new DataTable();
            dtPoint.Columns.Add("X", typeof(Double));
            dtPoint.Columns.Add("Y", typeof(Double));
            dtPoint.Columns.Add("Needle", typeof(int));
            //dtPoint.Columns.Add("Volume", typeof(int));
            DGPoint.DataSource = dtPoint;
            //Datagirdview width
            //DGPoint.Columns[0].Width = 130;
            //DGPoint.Columns[1].Width = 130;
            //DGPoint.Columns[2].Width = 100;
            //DGPoint.Columns[3].Width = 100;
            //Init BackgroundWorker
            //Init RunPointData
            for (int i = 0; i < _RunPoint.Length; i++)
            {
                _RunPoint[i] = new Point();
            }
            //Load Point Data
            LoadPointData();
            //Init Camera
            InitialCamera();

        }

        private void LoadPointData()
        {
            tbNeedle1X.Text = Para2Text(axis[0].Para.Needle1);
            tbNeedle1Y.Text = Para2Text(axis[1].Para.Needle1);
            tbNeedle1Z.Text = Para2Text(axis[2].Para.Needle1);
            tbNeedle2X.Text = Para2Text(axis[0].Para.Needle2);
            tbNeedle2Y.Text = Para2Text(axis[1].Para.Needle2);
            tbNeedle2Z.Text = Para2Text(axis[2].Para.Needle2);
            tbCameraX.Text = Para2Text(axis[0].Para.Camera);
            tbCameraY.Text = Para2Text(axis[1].Para.Camera);
            tbCameraZ.Text = Para2Text(axis[2].Para.Camera);
            tbSample1X.Text = Para2Text(axis[0].Para.Sample1);
            tbSample1Y.Text = Para2Text(axis[1].Para.Sample1);
            tbSample1Z.Text = Para2Text(axis[2].Para.Sample1);
            tbSample2X.Text = Para2Text(axis[0].Para.Sample2);
            tbSample2Y.Text = Para2Text(axis[1].Para.Sample2);
            tbSample2Z.Text = Para2Text(axis[2].Para.Sample2);
            tbSample3X.Text = Para2Text(axis[0].Para.Sample3);
            tbSample3Y.Text = Para2Text(axis[1].Para.Sample3);
            tbSample3Z.Text = Para2Text(axis[2].Para.Sample3);
            tbSample4X.Text = Para2Text(axis[0].Para.Sample4);
            tbSample4Y.Text = Para2Text(axis[1].Para.Sample4);
            tbSample4Z.Text = Para2Text(axis[2].Para.Sample4);

            //Manual UI Refresh
            tbORGR_3.Text = _PointConverter.dR.ToString();
            tbORGZ_3.Text = Para2Text(_PointConverter.dZ);
            tbPointX_3.Text = Para2Text(_RunPoint[2].X);
            tbPointY_3.Text = Para2Text(_RunPoint[2].Y);
            tbPointZ_3.Text = Para2Text(_RunPoint[2].Z);
            tbJogVelX.Text = Para2Text(axis[0].Para.JogVel);
            tbJogVelY.Text = Para2Text(axis[1].Para.JogVel);
            tbJogVelZ.Text = Para2Text(axis[2].Para.JogVel);
        }

        void Timer_100ms_Tick(object sender, EventArgs e)
        {
            //Refresh Axis and AxisUI
            for (int i = 0; i < axis.Length; i++)
            {
                axis[i].Refresh();
                axisUI[i].RefreshUI();
            }
            //Refresh Temperature Data
            if (ModbusPort.IsOpen)
            {
                try
                {
                    switch (temperature[0])
                    {
                        case 32770:
                            //輸入讀值尚未取得
                            lbTemperature.Text = "無輸入值";
                            break;
                        case 32771:
                            //未接感測器
                            lbTemperature.Text = "未接感測器";
                            break;
                        case 32772:
                            //感測器形式錯誤
                            lbTemperature.Text = "感測器種類錯誤";
                            break;
                        case 32773:
                            //輸入值無法量測
                            lbTemperature.Text = "無法量測";
                            break;
                        case 32774:
                            //記憶體無法讀寫
                            lbTemperature.Text = "記憶體錯誤";
                            break;
                        default:
                            //將值輸出到UI上
                            lbTemperature.Text = heaterTemp.ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorLog(ex.ToString(), 0);
                }

            }
            //Refresh PumpStatus
            UCPump.RefreshPumpUI();
            //Refresh Point Data
            _MachinePoint.X = axis[0].Para.CurCmd;
            _MachinePoint.Y = axis[1].Para.CurCmd;
            _MachinePoint.Z = axis[2].Para.CurCmd;
            _SoftwarePoint = Coordiante.CoordinateConveter(_MachinePoint, _PointConverter);

            switch (tcMain.SelectedTab.Name)
            {
                case "MotorPage":
                    {
                        lbPointX_1.Text = Para2Text(axis[0].Para.CurCmd);
                        lbPointY_1.Text = Para2Text(axis[1].Para.CurCmd);
                        lbPointZ_1.Text = Para2Text(axis[2].Para.CurCmd);
                        lbPointX_2.Text = String.Format("{0:0.###}", _SoftwarePoint.X / 1000);
                        lbPointY_2.Text = String.Format("{0:0.###}", _SoftwarePoint.Y / 1000);
                        lbPointZ_2.Text = String.Format("{0:0.###}", _SoftwarePoint.Z / 1000);
                        break;
                    }
                case "CameraPage":
                    break;
                case "ControlPage":
                    {
                        lbPointX_3.Text = String.Format("{0:0.###}", _SoftwarePoint.X / 1000);
                        lbPointY_3.Text = String.Format("{0:0.###}", _SoftwarePoint.Y / 1000);
                        lbPointZ_3.Text = String.Format("{0:0.###}", _SoftwarePoint.Z / 1000);
                        if (axis[0].State == 0 || axis[0].State == 3 || axis[1].State == 0
                            || axis[1].State == 3 || axis[2].State == 0 || axis[2].State == 3)
                        {
                            lbStageStatus.Text = "平台異常";
                            btnYJogN.Enabled = false;
                            btnYJogP.Enabled = false;
                            btnXJogP.Enabled = false;
                            btnXJogN.Enabled = false;
                            btnZJogN.Enabled = false;
                            btnZJogP.Enabled = false;
                            btnRun_3.Enabled = false;
                            btnRunSample1_3.Enabled = false;
                            btnRunSample2_3.Enabled = false;
                            btnRunSample3_3.Enabled = false;
                            btnRunSample4_3.Enabled = false;
                        }
                        else
                        {
                            lbStageStatus.Text = "可運作";
                            btnYJogN.Enabled = true;
                            btnYJogP.Enabled = true;
                            btnXJogP.Enabled = true;
                            btnXJogN.Enabled = true;
                            btnZJogN.Enabled = true;
                            btnZJogP.Enabled = true;
                            btnRun_3.Enabled = true;
                            btnRunSample1_3.Enabled = true;
                            btnRunSample2_3.Enabled = true;
                            btnRunSample3_3.Enabled = true;
                            btnRunSample4_3.Enabled = true;
                        }
                        break;
                    }
                default:
                    break;
            }

            if (Cycle_Auto)
            {
                for (int i = 0; i < axis.Length; i++)
                {
                    if (axis[i].State == 1)
                    {
                        axis[i].bMoveDone = true;
                    }
                }
            }
        }

        private void OpenMotionCard()
        {
            int Result;
            // Find DeviceNum
            Result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            if (Result != (int)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Find Device", Result);
                return;
            }
            //Get DeviceNum
            if (deviceCount > 0)
            {
                DeviceNum = CurAvailableDevs[0].DeviceNum;
            }
            //OpenDev
            Result = (int)Motion.mAcm_DevOpen(DeviceNum, ref Device_Hand);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Open Device", Result);
                return;
            }
            //Get AxisCount
            Result = (int)Motion.mAcm_GetU32Property(Device_Hand, (uint)PropertyID.FT_DevAxesCount, ref AxisCount);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Get AxisCount", Result);
                return;
            }
            //OpenAxis
            for (int i = 0; i < AxisCount; i++)
            {
                Result = (int)Motion.mAcm_AxOpen(Device_Hand, (UInt16)i, ref axis_Hand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    Log.ErrorLog("Open Axis " + i.ToString(), Result);
                    return;
                }
            }
            //load CFG
            Result = (int)Motion.mAcm_DevLoadConfig(Device_Hand, pMotionDevCfg);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                Log.ErrorLog("Load Configuration", Result);
                return;
            }
        }
        private void AddAxisUI()
        {
            for (int i = 0; i < axis.Length; i++)
            {
                axis[i] = new Axis(axis_Hand[i], axisPara[i]);
                axisUI[i] = new AxisUserControl(axis[i]);
                axisUI[i].Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                axisUI[i].Location = new System.Drawing.Point(423 * i + 600, 31);
                axisUI[i].Margin = new System.Windows.Forms.Padding(5);
                axisUI[i].Name = "Axis" + i.ToString();
                axisUI[i].Size = new System.Drawing.Size(400, 900);
                this.MotorPage.Controls.Add(axisUI[i]);
            }
            axisUI[0].Title = "AxisX";
            axisUI[1].Title = "AxisY";
            axisUI[2].Title = "AxisZ";
        }

        private void ReadAxisXml()
        {
            // Create xml document
            XmlDocument doc = new XmlDocument();
            // Load xml document
            doc.Load("AxisData.xml");
            XmlElement root = doc.DocumentElement;
            // select single xmlnode            
            // parse innertext to parameter
            for (int i = 0; i < axis.Length; i++)
            {
                axisPara[i] = new Axis.AxisPara();
                string node = "";
                switch (i)
                {
                    case 0:
                        node += "AxisX";
                        break;
                    case 1:
                        node += "AxisY";
                        break;
                    case 2:
                        node += "AxisZ";
                        break;
                    default:
                        break;
                }
                try
                {
                    axisPara[i].MaxVel = double.Parse(root.SelectSingleNode(node + "/MaxVel").InnerText);
                    axisPara[i].MaxAcc = double.Parse(root.SelectSingleNode(node + "/MaxAcc").InnerText);
                    axisPara[i].MaxDec = double.Parse(root.SelectSingleNode(node + "/MaxDec").InnerText);
                    axisPara[i].VelLow = double.Parse(root.SelectSingleNode(node + "/VelLow").InnerText);
                    axisPara[i].SoftLMTP = double.Parse(root.SelectSingleNode(node + "/SoftLMTP").InnerText);
                    axisPara[i].SoftLMTN = double.Parse(root.SelectSingleNode(node + "/SoftLMTN").InnerText);
                    axisPara[i].VelHigh = double.Parse(root.SelectSingleNode(node + "/VelHigh").InnerText);
                    axisPara[i].Acc = double.Parse(root.SelectSingleNode(node + "/Acc").InnerText);
                    axisPara[i].Dec = double.Parse(root.SelectSingleNode(node + "/Dec").InnerText);
                    axisPara[i].JogVel = double.Parse(root.SelectSingleNode(node + "/JogVel").InnerText);
                    axisPara[i].Distance = double.Parse(root.SelectSingleNode(node + "/Distance").InnerText);
                    axisPara[i].HomeVelLow = double.Parse(root.SelectSingleNode(node + "/HomeVelLow").InnerText);
                    axisPara[i].HomeVelHigh = double.Parse(root.SelectSingleNode(node + "/HomeVelHigh").InnerText);
                    axisPara[i].HomeAcc = double.Parse(root.SelectSingleNode(node + "/HomeAcc").InnerText);
                    axisPara[i].HomeDec = double.Parse(root.SelectSingleNode(node + "/HomeDec").InnerText);
                    axisPara[i].CurCmd = double.Parse(root.SelectSingleNode(node + "/CurCmd").InnerText);
                    axisPara[i].HomeMode = UInt32.Parse(root.SelectSingleNode(node + "/HomeMode").InnerText);
                    axisPara[i].HomeDir = UInt32.Parse(root.SelectSingleNode(node + "/HomeDir").InnerText);
                    axisPara[i].Needle1 = double.Parse(root.SelectSingleNode(node + "/Needle1").InnerText);
                    axisPara[i].Needle2 = double.Parse(root.SelectSingleNode(node + "/Needle2").InnerText);
                    axisPara[i].Camera = double.Parse(root.SelectSingleNode(node + "/Camera").InnerText);
                    axisPara[i].Sample1 = double.Parse(root.SelectSingleNode(node + "/Sample1").InnerText);
                    axisPara[i].Sample2 = double.Parse(root.SelectSingleNode(node + "/Sample2").InnerText);
                    axisPara[i].Sample3 = double.Parse(root.SelectSingleNode(node + "/Sample3").InnerText);
                    axisPara[i].Sample4 = double.Parse(root.SelectSingleNode(node + "/Sample4").InnerText);
                }
                catch (Exception e)
                {
                    Log.ErrorLog(e.ToString(), 0);
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private void ReadModbusPortXml()
        {
            try
            {
                // Create xml document
                XmlDocument doc = new XmlDocument();
                // Load xml document
                doc.Load("ModbusPort.xml");
                XmlElement root = doc.DocumentElement;
                // select single xmlnode            
                // parse innertext to parameter
                //Read modbusport protocols
                ModbusPort.PortName = root.SelectSingleNode("device").InnerText;
                ModbusPort.BaudRate = int.Parse(root.SelectSingleNode("BaudRate").InnerText);
                ModbusPort.DataBits = int.Parse(root.SelectSingleNode("DataBits").InnerText);
                switch (root.SelectSingleNode("Parity").InnerText)
                {
                    case "Even":
                        ModbusPort.Parity = Parity.Even;
                        break;
                    case "Odd":
                        ModbusPort.Parity = Parity.Odd;
                        break;
                    case "None":
                        ModbusPort.Parity = Parity.None;
                        break;
                    default:
                        break;
                }
                switch (root.SelectSingleNode("StopBits").InnerText)
                {
                    case "One":
                        ModbusPort.StopBits = StopBits.One;
                        break;
                    case "Two":
                        ModbusPort.StopBits = StopBits.Two;
                        break;
                    case "None":
                        ModbusPort.StopBits = StopBits.None;
                        break;
                    default:
                        break;
                }
                ModbusPort.Open();
                ModbusMaster = ModbusSerialMaster.CreateAscii(ModbusPort);
            }
            catch (Exception e)
            {
                Log.ErrorLog(e.ToString(), 0);
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close Temperature controller
            //Close Modbus Port
            /*
            try
            {
                ModbusMaster.WriteSingleCoil(slaveId, 2068, false);
            }
            catch(Exception ex)
            {
                Log.ErrorLog(ex.ToString(), 0);
            }
            */
            ModbusPort.Close();
            //Check Motor Stop, if no cancel close
            if (!axis[0].IO.RUN && !axis[1].IO.RUN && !axis[2].IO.RUN)
            {
                for (int i = 0; i < axis.Length; i++)
                {
                    if (axis[i].IO.SVON)
                        axis[i].ServoOn();
                }
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Wait for motor stop");
            }
            //Brake OFF
            uint Result = Motion.mAcm_DaqDoSetBit(Device_Hand, 0, 0);
            Log.log("Set D0 OFF.");
            if (Result != (uint)ErrorCode.SUCCESS)
                Log.ErrorLog("Set D0 OFF", (int)Result);
            //Stop Pump
            //Stop Pump Port
            UCPump.SavePumpParaXml();
            UCPump.PumpPortClose();
            //Close Camera
            DestroyCamera();
            //save parameter
            //save camera parameter
            //save axis parameter
            SaveAxisXml();
        }

        private void SaveAxisXml()
        {
            // Create xml document
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Axis");
            doc.AppendChild(root);
            for (int i = 0; i < axis.Length; i++)
            {
                string node = "";
                switch (i)
                {
                    case 0:
                        node += "AxisX";
                        break;
                    case 1:
                        node += "AxisY";
                        break;
                    case 2:
                        node += "AxisZ";
                        break;
                    default:
                        break;
                }
                XmlElement device = doc.CreateElement(node);
                root.AppendChild(device);

                XmlElement MaxVel = doc.CreateElement("MaxVel");
                MaxVel.InnerText = axis[i].Para.MaxVel.ToString();
                device.AppendChild(MaxVel);

                XmlElement MaxAcc = doc.CreateElement("MaxAcc");
                MaxAcc.InnerText = axis[i].Para.MaxAcc.ToString();
                device.AppendChild(MaxAcc);

                XmlElement MaxDec = doc.CreateElement("MaxDec");
                MaxDec.InnerText = axis[i].Para.MaxDec.ToString();
                device.AppendChild(MaxDec);

                XmlElement SoftLMTP = doc.CreateElement("SoftLMTP");
                SoftLMTP.InnerText = axis[i].Para.SoftLMTP.ToString();
                device.AppendChild(SoftLMTP);

                XmlElement SoftLMTN = doc.CreateElement("SoftLMTN");
                SoftLMTN.InnerText = axis[i].Para.SoftLMTN.ToString();
                device.AppendChild(SoftLMTN);

                XmlElement VelLow = doc.CreateElement("VelLow");
                VelLow.InnerText = axis[i].Para.VelLow.ToString();
                device.AppendChild(VelLow);

                XmlElement VelHigh = doc.CreateElement("VelHigh");
                VelHigh.InnerText = axis[i].Para.VelHigh.ToString();
                device.AppendChild(VelHigh);

                XmlElement Acc = doc.CreateElement("Acc");
                Acc.InnerText = axis[i].Para.Acc.ToString();
                device.AppendChild(Acc);

                XmlElement Dec = doc.CreateElement("Dec");
                Dec.InnerText = axis[i].Para.Dec.ToString();
                device.AppendChild(Dec);

                XmlElement Distance = doc.CreateElement("Distance");
                Distance.InnerText = axis[i].Para.Distance.ToString();
                device.AppendChild(Distance);

                XmlElement JogVel = doc.CreateElement("JogVel");
                JogVel.InnerText = axis[i].Para.JogVel.ToString();
                device.AppendChild(JogVel);

                XmlElement HomeVelLow = doc.CreateElement("HomeVelLow");
                HomeVelLow.InnerText = axis[i].Para.HomeVelLow.ToString();
                device.AppendChild(HomeVelLow);

                XmlElement HomeVelHigh = doc.CreateElement("HomeVelHigh");
                HomeVelHigh.InnerText = axis[i].Para.HomeVelHigh.ToString();
                device.AppendChild(HomeVelHigh);

                XmlElement HomeAcc = doc.CreateElement("HomeAcc");
                HomeAcc.InnerText = axis[i].Para.HomeAcc.ToString();
                device.AppendChild(HomeAcc);

                XmlElement HomeDec = doc.CreateElement("HomeDec");
                HomeDec.InnerText = axis[i].Para.HomeDec.ToString();
                device.AppendChild(HomeDec);

                XmlElement CurCmd = doc.CreateElement("CurCmd");
                CurCmd.InnerText = axis[i].Para.CurCmd.ToString();
                device.AppendChild(CurCmd);

                XmlElement HomeMode = doc.CreateElement("HomeMode");
                HomeMode.InnerText = axis[i].Para.HomeMode.ToString();
                device.AppendChild(HomeMode);

                XmlElement HomeDir = doc.CreateElement("HomeDir");
                HomeDir.InnerText = axis[i].Para.HomeDir.ToString();
                device.AppendChild(HomeDir);

                XmlElement Needle1 = doc.CreateElement("Needle1");
                Needle1.InnerText = axis[i].Para.Needle1.ToString();
                device.AppendChild(Needle1);

                XmlElement Needle2 = doc.CreateElement("Needle2");
                Needle2.InnerText = axis[i].Para.Needle2.ToString();
                device.AppendChild(Needle2);

                XmlElement Camera = doc.CreateElement("Camera");
                Camera.InnerText = axis[i].Para.Camera.ToString();
                device.AppendChild(Camera);

                XmlElement Sample1 = doc.CreateElement("Sample1");
                Sample1.InnerText = axis[i].Para.Sample1.ToString();
                device.AppendChild(Sample1);

                XmlElement Sample2 = doc.CreateElement("Sample2");
                Sample2.InnerText = axis[i].Para.Sample2.ToString();
                device.AppendChild(Sample2);

                XmlElement Sample3 = doc.CreateElement("Sample3");
                Sample3.InnerText = axis[i].Para.Sample3.ToString();
                device.AppendChild(Sample3);

                XmlElement Sample4 = doc.CreateElement("Sample4");
                Sample4.InnerText = axis[i].Para.Sample4.ToString();
                device.AppendChild(Sample4);
            }
            // save xml file
            doc.Save("AxisData.xml");
        }
        private void OpenPointData()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "csv Files (*.csv)|*.csv";
            ofd.InitialDirectory = ".\\";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] row_text = System.IO.File.ReadAllLines(ofd.FileName);
                string[] data_col = null;
                int x = 0;
                dtPoint.Clear();
                foreach (string text_line in row_text)
                {
                    data_col = text_line.Split(',');
                    if (x == 0)
                    {
                        x++;
                    }
                    else
                    {
                        try
                        {
                            dtPoint.Rows.Add(data_col);
                        }
                        catch (Exception e)
                        {
                            Log.ErrorLog(e.ToString(), 0);
                        }

                    }

                }
            }
            dataCount = dtPoint.Rows.Count;
        }

        private void btnOpenCSV_Click(object sender, EventArgs e)
        {
            OpenPointData();
        }

        private void btnRunPoint_Click(object sender, EventArgs e)
        {
            //Start Running
            //1.Check Point Data
            //2.If data OK, then Start Run
            if (dtPoint.Rows.Count > 0)
            {
                RunPointIndex = DGPoint.CurrentCell.RowIndex;
                Cycle_Auto = true;
                Thread AutoThread = new Thread(AutoCycle);
                AutoThread.Start();
                //disable ui
                ButtonControl(false);
            }
            

        }

        private void AutoCycle()
        {
            int step = 0;
            //camera open

            //AutoCycle
            while(Cycle_Auto)
            {
                switch(step)
                {
                    case 0:
                        {
                            //Check Pump Enable
                            if (cbPumpEnable.Checked)
                            {
                                step = 1;
                            }
                            else
                            {
                                step = 3;
                            }
                            break;
                        }                        
                    case 1:
                        {
                            //Check Needle
                            double x, y, z;
                            int needle = (int)dtPoint.Rows[RunPointIndex]["Needle"];
                            if (needle == 1)
                            {
                                x = (double)dtPoint.Rows[RunPointIndex]["X"] * 1000 + axis[0].Para.Needle1;
                                y = (double)dtPoint.Rows[RunPointIndex]["Y"] * 1000 + axis[1].Para.Needle1;
                                z = axis[2].Para.Needle1 - _PointConverter.dZ;
                                for (int i = 0; i < axis.Length; i++)
                                {
                                    axis[i].bMoveDone = false;
                                }
                                axis[0].MoveAbs(x);
                                axis[1].MoveAbs(y);
                                axis[2].MoveAbs(z);
                                step++;
                            }
                            else if (needle == 2)
                            {
                                x = (double)dtPoint.Rows[RunPointIndex]["X"] * 1000 + axis[0].Para.Needle2;
                                y = (double)dtPoint.Rows[RunPointIndex]["Y"] * 1000 + axis[1].Para.Needle2;
                                z = axis[2].Para.Needle2 - _PointConverter.dZ;
                                for (int i = 0; i < axis.Length; i++)
                                {
                                    axis[i].bMoveDone = false;
                                }
                                axis[0].MoveAbs(x);
                                axis[1].MoveAbs(y);
                                axis[2].MoveAbs(z);
                                step++;
                            }
                            else
                            {
                                RunPointIndex++;
                            }
                            break;
                        }                        
                    case 2:
                        {
                            //wait axis stop
                            if (axis[0].bMoveDone && axis[1].bMoveDone && axis[2].bMoveDone)
                            {
                                int needle = (int)dtPoint.Rows[RunPointIndex]["Needle"];
                                UCPump.AutoCycle(needle);
                                step++;
                            }
                            break;
                        }
                    case 3:
                        {
                            //wait pump stop
                            if (!UCPump.Cycle_Pipe)
                            {
                                double x, y, z;
                                x = (double)dtPoint.Rows[RunPointIndex]["X"] * 1000;
                                y = (double)dtPoint.Rows[RunPointIndex]["Y"] * 1000;
                                z = axis[2].Para.Camera - _PointConverter.dZ;
                                for (int i = 0; i < axis.Length; i++)
                                {
                                    axis[i].bMoveDone = false;
                                }
                                axis[0].MoveAbs(x);
                                axis[1].MoveAbs(y);
                                axis[2].MoveAbs(z);
                                step++;
                            }
                            break;
                        }
                    case 4:
                        {
                            //wait axis stop
                            if (axis[0].bMoveDone && axis[1].bMoveDone && axis[2].bMoveDone)
                            {
                                Thread.Sleep(1000);
                                if (cbCameraEnable.Checked)
                                {
                                    //Take photo                           
                                    Thread.Sleep(2000);
                                    OneShoot();
                                    string PhotoName = PhotoDir + '\\' + DateTime.Now.ToString("HHmmss_") + RunPointIndex + ".bmp";
                                    Thread.Sleep(1000);
                                    bitmap.Save(PhotoName);
                                }

                                RunPointIndex++;
                                if (RunPointIndex >= dataCount)
                                {
                                    Cycle_Auto = false;
                                    step = 0;
                                    ButtonControl(true);
                                }
                                else
                                {
                                    MethodInvoker miUpdate = new MethodInvoker(this.UpdateDataGirdViewSelect);
                                    this.BeginInvoke(miUpdate, null);
                                    step = 0;

                                }

                            }
                            break;
                        }
                    default:
                        Cycle_Auto = false;
                        break;
                }
            }
        }

        private void UpdateDataGirdViewSelect()
        {
            DGPoint.CurrentCell = DGPoint[0, RunPointIndex];
        }

        private void ModbusPollingWork_DoWork(object sender, DoWorkEventArgs e)
        {
            while (ModbusPort.IsOpen)
            {
                try
                {
                    temperature = ModbusMaster.ReadHoldingRegisters(slaveId, 4096, 1);
                    heaterTemp = (double)temperature[0] / 10;
                }
                catch (Exception ex)
                {
                    Log.ErrorLog(ex.ToString(), 0);
                }
                Thread.Sleep(1000);
            }
        }

        public void ButtonControl(bool Control)
        {
            //enable ui
            UCPump.ButtonControl(Control);
            btnOneShoot.Enabled = Control;
            btnContinuousShoot.Enabled = Control;
            btnSavePhoto.Enabled = Control;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            Cycle_Auto = false;

            UCPump.Stop();
            ButtonControl(true);
            RunPointStep = 0;
            for (int i = 0; i < axis.Length; i++)
            {
                axis[i].StopDec();
            }
        }
        private void int_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void double_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.')
                e.Handled = false;
            else
                e.Handled = true;
            if (e.KeyChar == '.' && tb.Text.IndexOf('.') >= 0)
                e.Handled = true;
        }

        private void doubleWithMinus_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-')
                e.Handled = false;
            else
                e.Handled = true;
            if (e.KeyChar == '.' && tb.Text.IndexOf('.') >= 0)
                e.Handled = true;
            if (e.KeyChar == '-' && tb.Text.Length > 0)
                e.Handled = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            Point _RunPointBuffer = new Point();

            switch (btn.Name)
            {
                case "btnRun_1":
                    {
                        //Check which device select
                        //1.camera don't change any point
                        //2.needle1 or needle2 change point position
                        _RunPointBuffer = _RunPoint[0].GetPoint();
                        if (rbtnNeedle1_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRun_2":
                    {
                        _RunPointBuffer = Coordiante.ReverseCoordinateConveter(_RunPoint[1], _PointConverter);
                        if (rbtnNeedle1_2.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_2.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRun_3":
                    {
                        _RunPointBuffer = Coordiante.ReverseCoordinateConveter(_RunPoint[2], _PointConverter);
                        if (rbtnNeedle1_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample1_1":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample1;
                        _RunPointBuffer.Y = axis[1].Para.Sample1;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample1_3":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample1;
                        _RunPointBuffer.Y = axis[1].Para.Sample1;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample2_1":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample2;
                        _RunPointBuffer.Y = axis[1].Para.Sample2;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample2_3":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample2;
                        _RunPointBuffer.Y = axis[1].Para.Sample2;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample3_1":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample3;
                        _RunPointBuffer.Y = axis[1].Para.Sample3;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample3_3":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample3;
                        _RunPointBuffer.Y = axis[1].Para.Sample3;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample4_1":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample4;
                        _RunPointBuffer.Y = axis[1].Para.Sample4;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_1.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                case "btnRunSample4_3":
                    {
                        _RunPointBuffer.X = axis[0].Para.Sample4;
                        _RunPointBuffer.Y = axis[1].Para.Sample4;
                        _RunPointBuffer.Z = 0;
                        if (rbtnNeedle1_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle1;
                            _RunPointBuffer.Y += axis[1].Para.Needle1;
                        }
                        else if (rbtnNeedle2_3.Checked)
                        {
                            _RunPointBuffer.X += axis[0].Para.Needle2;
                            _RunPointBuffer.Y += axis[1].Para.Needle2;
                        }
                        break;
                    }
                default:
                    return;
            }
            axis[0].MoveAbs(_RunPointBuffer.X);
            axis[1].MoveAbs(_RunPointBuffer.Y);
            axis[2].MoveAbs(_RunPointBuffer.Z);
        }

        public double Text2Para(string text)
        {
            return double.Parse(text) * 1000;
        }

        public string Para2Text(double para)
        {
            return Convert.ToString(para / 1000);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnModifyNeedle1":
                    axis[0].Para.Needle1 = Text2Para(tbNeedle1X.Text);
                    axis[1].Para.Needle1 = Text2Para(tbNeedle1Y.Text);
                    axis[2].Para.Needle1 = Text2Para(tbNeedle1Z.Text);
                    break;
                case "btnModifyNeedle2":
                    axis[0].Para.Needle2 = Text2Para(tbNeedle2X.Text);
                    axis[1].Para.Needle2 = Text2Para(tbNeedle2Y.Text);
                    axis[2].Para.Needle2 = Text2Para(tbNeedle2Z.Text);
                    break;
                case "btnModifyCamera":
                    axis[0].Para.Camera = Text2Para(tbCameraX.Text);
                    axis[1].Para.Camera = Text2Para(tbCameraY.Text);
                    axis[2].Para.Camera = Text2Para(tbCameraZ.Text);
                    break;
                case "btnModifySample1":
                    axis[0].Para.Sample1 = Text2Para(tbSample1X.Text);
                    axis[1].Para.Sample1 = Text2Para(tbSample1Y.Text);
                    axis[2].Para.Sample1 = Text2Para(tbSample1Z.Text);
                    break;
                case "btnModifySample2":
                    axis[0].Para.Sample2 = Text2Para(tbSample2X.Text);
                    axis[1].Para.Sample2 = Text2Para(tbSample2Y.Text);
                    axis[2].Para.Sample2 = Text2Para(tbSample2Z.Text);
                    break;
                case "btnModifySample3":
                    axis[0].Para.Sample3 = Text2Para(tbSample3X.Text);
                    axis[1].Para.Sample3 = Text2Para(tbSample3Y.Text);
                    axis[2].Para.Sample3 = Text2Para(tbSample3Z.Text);
                    break;
                case "btnModifySample4":
                    axis[0].Para.Sample4 = Text2Para(tbSample4X.Text);
                    axis[1].Para.Sample4 = Text2Para(tbSample4Y.Text);
                    axis[2].Para.Sample4 = Text2Para(tbSample4Z.Text);
                    break;
                default:
                    break;
            }
        }

        private void btnORGSet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnORGXY_2":
                case "btnORGXY_3":
                    {
                        _PointConverter.dX = _MachinePoint.X;
                        _PointConverter.dY = _MachinePoint.Y;
                        break;
                    }
                case "btnORGR_2":
                    {
                        if (tbORGR_2.Text.Length > 0 && tbORGR_2.Text.First() != '-')
                            _PointConverter.dR = double.Parse(tbORGR_2.Text);
                        else if (tbORGR_2.Text.Length > 1 && tbORGR_2.Text.First() == '-')
                            _PointConverter.dR = double.Parse(tbORGR_2.Text);
                        else
                            _PointConverter.dR = 0;
                        break;
                    }
                case "btnORGR_3":
                    {
                        if (tbORGR_3.Text.Length > 0 && tbORGR_3.Text.First() != '-')
                            _PointConverter.dR = double.Parse(tbORGR_3.Text);
                        else if (tbORGR_3.Text.Length > 1 && tbORGR_3.Text.First() == '-')
                            _PointConverter.dR = double.Parse(tbORGR_3.Text);
                        else
                            _PointConverter.dR = 0;
                        break;
                    }
                default:
                    break;
            }

        }

        private void tcMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //Refresh TabPage Data
            switch (e.TabPage.Name)
            {
                case "MotorPage":
                    {
                        //MotorPage UI Refresh
                        tbORGR_2.Text = _PointConverter.dR.ToString();
                        tbORGZ_2.Text = Para2Text(_PointConverter.dZ);
                        tbPointX_1.Text = Para2Text(_RunPoint[0].X);
                        tbPointY_1.Text = Para2Text(_RunPoint[0].Y);
                        tbPointZ_1.Text = Para2Text(_RunPoint[0].Z);
                        tbPointX_2.Text = Para2Text(_RunPoint[1].X);
                        tbPointY_2.Text = Para2Text(_RunPoint[1].Y);
                        tbPointZ_2.Text = Para2Text(_RunPoint[1].Z);
                        break;
                    }
                case "CameraPage":
                    {
                        break;
                    }
                case "ControlPage":
                    {
                        //Manual UI Refresh
                        tbORGR_3.Text = _PointConverter.dR.ToString();
                        tbORGZ_3.Text = Para2Text(_PointConverter.dZ);
                        tbPointX_3.Text = Para2Text(_RunPoint[2].X);
                        tbPointY_3.Text = Para2Text(_RunPoint[2].Y);
                        tbPointZ_3.Text = Para2Text(_RunPoint[2].Z);
                        tbJogVelX.Text = Para2Text(axis[0].Para.JogVel);
                        tbJogVelY.Text = Para2Text(axis[1].Para.JogVel);
                        tbJogVelZ.Text = Para2Text(axis[2].Para.JogVel);
                        break;
                    }
                default:
                    break;
            }
        }

        private void JogVel_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            switch (tb.Name)
            {
                case "tbJogVelX":
                    {
                        axis[0].Para.JogVel = Text2Para(tbJogVelX.Text);
                        break;
                    }
                case "tbJogVelY":
                    {
                        axis[1].Para.JogVel = Text2Para(tbJogVelY.Text);
                        break;
                    }
                case "tbJogVelZ":
                    {
                        axis[2].Para.JogVel = Text2Para(tbJogVelZ.Text);
                        break;
                    }
                case "tbPointX_1":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[0].X = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[0].X = Text2Para(tb.Text);
                        else
                            _RunPoint[0].X = 0;
                        break;
                    }
                case "tbPointY_1":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[0].Y = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[0].Y = Text2Para(tb.Text);
                        else
                            _RunPoint[0].Y = 0;
                        break;
                    }
                case "tbPointZ_1":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[0].Z = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[0].Z = Text2Para(tb.Text);
                        else
                            _RunPoint[0].Z = 0;
                        break;
                    }
                case "tbPointX_2":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[1].X = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[1].X = Text2Para(tb.Text);
                        else
                            _RunPoint[1].X = 0;
                        break;
                    }
                case "tbPointY_2":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[1].Y = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[1].Y = Text2Para(tb.Text);
                        else
                            _RunPoint[1].Y = 0;
                        break;
                    }
                case "tbPointZ_2":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[1].Z = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[1].Z = Text2Para(tb.Text);
                        else
                            _RunPoint[1].Z = 0;
                        break;
                    }
                case "tbPointX_3":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[2].X = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[2].X = Text2Para(tb.Text);
                        else
                            _RunPoint[2].X = 0;
                        break;
                    }
                case "tbPointY_3":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[2].Y = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[2].Y = Text2Para(tb.Text);
                        else
                            _RunPoint[2].Y = 0;
                        break;
                    }
                case "tbPointZ_3":
                    {
                        if (tb.Text.Length > 0 && tb.Text.First() != '-')
                            _RunPoint[2].Z = Text2Para(tb.Text);
                        else if (tb.Text.Length > 1 && tb.Text.First() == '-')
                            _RunPoint[2].Z = Text2Para(tb.Text);
                        else
                            _RunPoint[2].Z = 0;
                        break;
                    }
                case "tbORGZ_2":
                case "tbORGZ_3":
                    {
                        if (tb.Text.Length > 0)
                            _PointConverter.dZ = Text2Para(tb.Text);
                        else
                            _PointConverter.dZ = 0;
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnJog_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnXJogN":
                    {
                        axis[0].JogN();
                        break;
                    }
                case "btnXJogP":
                    {
                        axis[0].JogP();
                        break;
                    }
                case "btnYJogN":
                    {
                        axis[1].JogN();
                        break;
                    }
                case "btnYJogP":
                    {
                        axis[1].JogP();
                        break;
                    }
                case "btnZJogN":
                    {
                        axis[2].JogN();
                        break;
                    }
                case "btnZJogP":
                    {
                        axis[2].JogP();
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnJog_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnXJogN":
                case "btnXJogP":
                    {
                        axis[0].StopDec();
                        break;
                    }
                case "btnYJogN":
                case "btnYJogP":
                    {
                        axis[1].StopDec();
                        break;
                    }
                case "btnZJogN":
                case "btnZJogP":
                    {
                        axis[2].StopDec();
                        break;
                    }
                default:
                    break;
            }
        }

        private void pbCamera_Paint(object sender, PaintEventArgs e)
        {
            if (cbPositionLine.Checked)
            {
                int picBoxWidth = pbCamera.Size.Width;
                int picBoxHeight = pbCamera.Size.Height;
                int halfWidth = pbCamera.Size.Width / 2;
                int halfHeight = pbCamera.Size.Height / 2;
                Graphics objGraphic = e.Graphics;
                Pen pen = new Pen(Color.Blue, 3);
                //Draw line point create
                Point[] DrawPoint = new Point[4];
                for (int i = 0; i < DrawPoint.Length; i++)
                {
                    DrawPoint[i] = new Point();
                }
                PointConverter DrawConverter = new PointConverter();
                //point1 = 100,0
                //point2 = 0,100
                //point3 = -100,0
                //point4 = 0,-100 
                DrawPoint[0].X = 100;
                DrawPoint[0].Y = 0;
                DrawPoint[1].X = 0;
                DrawPoint[1].Y = 100;
                DrawPoint[2].X = -100;
                DrawPoint[2].Y = 0;
                DrawPoint[3].X = 0;
                DrawPoint[3].Y = -100;
                DrawConverter.dX = 0;
                DrawConverter.dY = 0;
                DrawConverter.dR = -1 * _PointConverter.dR;
                for (int i = 0; i < DrawPoint.Length; i++)
                {
                    DrawPoint[i] = Coordiante.CoordinateConveter(DrawPoint[i], DrawConverter);
                    DrawPoint[i].X += halfWidth;
                    DrawPoint[i].Y += halfHeight;
                }

                objGraphic.DrawLine(pen, (int)DrawPoint[0].X, (int)DrawPoint[0].Y, (int)DrawPoint[2].X, (int)DrawPoint[2].Y);
                objGraphic.DrawLine(pen, (int)DrawPoint[1].X, (int)DrawPoint[1].Y, (int)DrawPoint[3].X, (int)DrawPoint[3].Y);
            }
        }
        private void InitialCamera()
        {
            try
            {
                // Create a camera object that selects the first camera device found.
                // More constructors are available for selecting a specific camera device.
                camera = new Camera();
                // Print the model name of the camera.
                Console.WriteLine("Using camera {0}.", camera.CameraInfo[CameraInfoKey.ModelName]);

                // Set the acquisition mode to free running continuous acquisition when the camera is opened.
                camera.CameraOpened += Configuration.AcquireContinuous;

                // Register for the events of the image provider needed for proper operation.
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;
                // Open the connection to the camera device.
                camera.Open();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                // Comment the following two lines to disable waiting on exit.
                Console.Error.WriteLine("\nPress enter to exit.");
                Console.ReadLine();
            }
        }
        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnConnectionLost), sender, e);
                return;
            }

            // Close the camera object.
            DestroyCamera();
            // Because one device is gone, the list needs to be updated.
            //UpdateDeviceList();
        }
        private void DestroyCamera()
        {
            // Destroy the camera object.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch (Exception exception)
            {
                Log.ErrorLog(exception.ToString(), 0);
            }
        }
        // Occurs when the connection to a camera device is opened.
        private void OnCameraOpened(Object sender, EventArgs e)
        {

        }
        // Occurs when the connection to a camera device is closed.
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnCameraClosed), sender, e);
                return;
            }

            // The camera connection is closed. Disable all buttons.
            //EnableButtons(false, false);
        }
        // Occurs when a camera starts grabbing.
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnGrabStarted), sender, e);
                return;
            }

            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.

            stopWatch.Reset();

            // Do not update the device list while grabbing to reduce jitter. Jitter may occur because the GUI thread is blocked for a short time when enumerating.
            //updateDeviceListTimer.Stop();

            // The camera is grabbing. Disable the grab buttons. Enable the stop button.
            //EnableButtons(false, true);
        }
        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
                return;
            }

            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 100)
                    {
                        stopWatch.Restart();

                        bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                        bitmap.UnlockBits(bmpData);

                        // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
                        Bitmap bitmapOld = pbCamera.Image as Bitmap;
                        // Provide the display control with the new bitmap. This action automatically updates the display.
                        pbCamera.Image = bitmap;
                        if (bitmapOld != null)
                        {
                            // Dispose the bitmap.
                            bitmapOld.Dispose();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //ShowException(exception);
                Log.ErrorLog(exception.ToString(), 0);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }
        // Occurs when a camera has stopped grabbing.
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<GrabStopEventArgs>(OnGrabStopped), sender, e);
                return;
            }

            // Reset the stopwatch.
            stopWatch.Reset();

            // Re-enable the updating of the device list.
            //updateDeviceListTimer.Start();

            // The camera stopped grabbing. Enable the grab buttons. Disable the stop button.
            //EnableButtons(true, false);

            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCamera_Click(object sender, EventArgs e)
        {
            //Camera control button
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnOneShoot":
                    {
                        OneShoot();
                        break;
                    }
                case "btnContinuousShoot":
                    {
                        ContinuousShoot();
                        btnOneShoot.Enabled = false;
                        btnSavePhoto.Enabled = false;
                        break;
                    }
                case "btnStopShoot":
                    {
                        StopShoot();
                        btnOneShoot.Enabled = true;
                        btnSavePhoto.Enabled = true;
                        break;
                    }
                case "btnSavePhoto":
                    {
                        SavePhoto();
                        break;
                    }
                default:
                    break;
            }
        }
        private void OneShoot()
        {
            try
            {
                // Starts the grabbing of one image.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                Log.ErrorLog(exception.ToString(), 0);
            }
        }
        private void ContinuousShoot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                Log.ErrorLog(exception.ToString(), 0);
            }
        }
        private void StopShoot()
        {
            // Stop the grabbing.
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {
                Log.ErrorLog(exception.ToString(), 0);
            }
        }
        private void SavePhoto()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "bmp file|*.bmp";
            sfd.Title = "Save Photo";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK && sfd.FileName != "")
            {
                bitmap.Save(sfd.FileName);
            }
        }

        private void btnHeatStart_Click(object sender, EventArgs e)
        {
            try
            {
                double value = double.Parse(tbHeatTemperature.Text);
                ushort data = (ushort)(value * 10);
                ModbusMaster.WriteSingleRegister(slaveId, 4097, data);
                if (heaterState)
                {
                    ModbusMaster.WriteSingleCoil(slaveId, 2068, false);
                    btnHeatStart.Text = "開始加熱";
                    heaterState = false;
                }
                else
                {
                    ModbusMaster.WriteSingleCoil(slaveId, 2068, true);
                    btnHeatStart.Text = "停止加熱";
                    heaterState = true;
                }


            }
            catch (Exception ex)
            {
                Log.ErrorLog(ex.ToString(), 0);
            }
        }

        private void btnErrorReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < axis.Length; i++)
            {
                axis[i].ErrorReset();
            }
        }
    }
}
