using System.IO.Ports;

namespace InkjetPrinter
{
    partial class PumpUserControl
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbPump = new System.Windows.Forms.GroupBox();
            this.gbReceiveData = new System.Windows.Forms.GroupBox();
            this.tbReceiveData = new System.Windows.Forms.TextBox();
            this.gbSendData = new System.Windows.Forms.GroupBox();
            this.tbSendData = new System.Windows.Forms.TextBox();
            this.gbPipeControl = new System.Windows.Forms.GroupBox();
            this.rbtnAir = new System.Windows.Forms.RadioButton();
            this.rbtnWater = new System.Windows.Forms.RadioButton();
            this.tbPipeIntro = new System.Windows.Forms.TextBox();
            this.tbNeedle2Pipe = new System.Windows.Forms.TextBox();
            this.lbNeedle2PipeTitle = new System.Windows.Forms.Label();
            this.tbNeedle1Pipe = new System.Windows.Forms.TextBox();
            this.lbNeedle1PipeTitle = new System.Windows.Forms.Label();
            this.btnPumpStop = new System.Windows.Forms.Button();
            this.btnPumpInit = new System.Windows.Forms.Button();
            this.lbPump = new System.Windows.Forms.Label();
            this.lbPumpTitle = new System.Windows.Forms.Label();
            this.gbNeedle = new System.Windows.Forms.GroupBox();
            this.tbNeedle2PipeOut = new System.Windows.Forms.TextBox();
            this.btnNeedle2PipeOut = new System.Windows.Forms.Button();
            this.tbNeedle2PipeIn = new System.Windows.Forms.TextBox();
            this.btnNeedle2PipeIn = new System.Windows.Forms.Button();
            this.lbNeedle2 = new System.Windows.Forms.Label();
            this.lbNeedle2Title = new System.Windows.Forms.Label();
            this.tbNeedle1PipeOut = new System.Windows.Forms.TextBox();
            this.btnNeedle1PipeOut = new System.Windows.Forms.Button();
            this.tbNeedle1PipeIn = new System.Windows.Forms.TextBox();
            this.btnNeedle1PipeIn = new System.Windows.Forms.Button();
            this.lbNeedle1 = new System.Windows.Forms.Label();
            this.lbNeedle1Title = new System.Windows.Forms.Label();
            this.gbPlunger = new System.Windows.Forms.GroupBox();
            this.tbPipeOutVel = new System.Windows.Forms.TextBox();
            this.tbPipeInVel = new System.Windows.Forms.TextBox();
            this.lbPipeOutVelTitle = new System.Windows.Forms.Label();
            this.lbPipeInVelTitle = new System.Windows.Forms.Label();
            this.tbPlunger = new System.Windows.Forms.TextBox();
            this.btnPlunger = new System.Windows.Forms.Button();
            this.lbPlunger = new System.Windows.Forms.Label();
            this.lbPlungerTitle = new System.Windows.Forms.Label();
            this.gbValve = new System.Windows.Forms.GroupBox();
            this.cbValve = new System.Windows.Forms.ComboBox();
            this.tbValveIntro = new System.Windows.Forms.TextBox();
            this.btnValve = new System.Windows.Forms.Button();
            this.lbValve = new System.Windows.Forms.Label();
            this.lbValveTitle = new System.Windows.Forms.Label();
            this.gbPump.SuspendLayout();
            this.gbReceiveData.SuspendLayout();
            this.gbSendData.SuspendLayout();
            this.gbPipeControl.SuspendLayout();
            this.gbNeedle.SuspendLayout();
            this.gbPlunger.SuspendLayout();
            this.gbValve.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPump
            // 
            this.gbPump.Controls.Add(this.gbReceiveData);
            this.gbPump.Controls.Add(this.gbSendData);
            this.gbPump.Controls.Add(this.gbPipeControl);
            this.gbPump.Controls.Add(this.btnPumpStop);
            this.gbPump.Controls.Add(this.btnPumpInit);
            this.gbPump.Controls.Add(this.lbPump);
            this.gbPump.Controls.Add(this.lbPumpTitle);
            this.gbPump.Controls.Add(this.gbNeedle);
            this.gbPump.Controls.Add(this.gbPlunger);
            this.gbPump.Controls.Add(this.gbValve);
            this.gbPump.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.gbPump.Location = new System.Drawing.Point(0, 0);
            this.gbPump.Name = "gbPump";
            this.gbPump.Size = new System.Drawing.Size(930, 410);
            this.gbPump.TabIndex = 1;
            this.gbPump.TabStop = false;
            this.gbPump.Text = "Pump Control";
            // 
            // gbReceiveData
            // 
            this.gbReceiveData.Controls.Add(this.tbReceiveData);
            this.gbReceiveData.Location = new System.Drawing.Point(670, 210);
            this.gbReceiveData.Name = "gbReceiveData";
            this.gbReceiveData.Size = new System.Drawing.Size(250, 190);
            this.gbReceiveData.TabIndex = 10;
            this.gbReceiveData.TabStop = false;
            this.gbReceiveData.Text = "Receive Data";
            // 
            // tbReceiveData
            // 
            this.tbReceiveData.Location = new System.Drawing.Point(10, 30);
            this.tbReceiveData.Multiline = true;
            this.tbReceiveData.Name = "tbReceiveData";
            this.tbReceiveData.ReadOnly = true;
            this.tbReceiveData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReceiveData.Size = new System.Drawing.Size(230, 150);
            this.tbReceiveData.TabIndex = 1;
            this.tbReceiveData.TextChanged += new System.EventHandler(this.tbReceiveData_TextChanged);
            // 
            // gbSendData
            // 
            this.gbSendData.Controls.Add(this.tbSendData);
            this.gbSendData.Location = new System.Drawing.Point(670, 20);
            this.gbSendData.Name = "gbSendData";
            this.gbSendData.Size = new System.Drawing.Size(250, 190);
            this.gbSendData.TabIndex = 9;
            this.gbSendData.TabStop = false;
            this.gbSendData.Text = "Send Data";
            // 
            // tbSendData
            // 
            this.tbSendData.Location = new System.Drawing.Point(10, 30);
            this.tbSendData.Multiline = true;
            this.tbSendData.Name = "tbSendData";
            this.tbSendData.ReadOnly = true;
            this.tbSendData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSendData.Size = new System.Drawing.Size(230, 150);
            this.tbSendData.TabIndex = 0;
            this.tbSendData.TextChanged += new System.EventHandler(this.tbSendData_TextChanged);
            // 
            // gbPipeControl
            // 
            this.gbPipeControl.Controls.Add(this.rbtnAir);
            this.gbPipeControl.Controls.Add(this.rbtnWater);
            this.gbPipeControl.Controls.Add(this.tbPipeIntro);
            this.gbPipeControl.Controls.Add(this.tbNeedle2Pipe);
            this.gbPipeControl.Controls.Add(this.lbNeedle2PipeTitle);
            this.gbPipeControl.Controls.Add(this.tbNeedle1Pipe);
            this.gbPipeControl.Controls.Add(this.lbNeedle1PipeTitle);
            this.gbPipeControl.Location = new System.Drawing.Point(450, 20);
            this.gbPipeControl.Name = "gbPipeControl";
            this.gbPipeControl.Size = new System.Drawing.Size(210, 380);
            this.gbPipeControl.TabIndex = 8;
            this.gbPipeControl.TabStop = false;
            this.gbPipeControl.Text = "Pipe Control";
            // 
            // rbtnAir
            // 
            this.rbtnAir.AutoSize = true;
            this.rbtnAir.Location = new System.Drawing.Point(110, 200);
            this.rbtnAir.Name = "rbtnAir";
            this.rbtnAir.Size = new System.Drawing.Size(91, 24);
            this.rbtnAir.TabIndex = 13;
            this.rbtnAir.TabStop = true;
            this.rbtnAir.Text = "抽取空氣";
            this.rbtnAir.UseVisualStyleBackColor = true;
            // 
            // rbtnWater
            // 
            this.rbtnWater.AutoSize = true;
            this.rbtnWater.Location = new System.Drawing.Point(10, 200);
            this.rbtnWater.Name = "rbtnWater";
            this.rbtnWater.Size = new System.Drawing.Size(91, 24);
            this.rbtnWater.TabIndex = 12;
            this.rbtnWater.TabStop = true;
            this.rbtnWater.Text = "抽取清水";
            this.rbtnWater.UseVisualStyleBackColor = true;
            // 
            // tbPipeIntro
            // 
            this.tbPipeIntro.Location = new System.Drawing.Point(10, 110);
            this.tbPipeIntro.Multiline = true;
            this.tbPipeIntro.Name = "tbPipeIntro";
            this.tbPipeIntro.ReadOnly = true;
            this.tbPipeIntro.Size = new System.Drawing.Size(190, 70);
            this.tbPipeIntro.TabIndex = 11;
            this.tbPipeIntro.Text = "項目：自動噴灑量\r\n單位：PumpPulse\r\n換算：960 = 1ul";
            // 
            // tbNeedle2Pipe
            // 
            this.tbNeedle2Pipe.Location = new System.Drawing.Point(110, 70);
            this.tbNeedle2Pipe.Name = "tbNeedle2Pipe";
            this.tbNeedle2Pipe.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle2Pipe.TabIndex = 10;
            this.tbNeedle2Pipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle2Pipe.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle2Pipe.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // lbNeedle2PipeTitle
            // 
            this.lbNeedle2PipeTitle.Location = new System.Drawing.Point(10, 75);
            this.lbNeedle2PipeTitle.Name = "lbNeedle2PipeTitle";
            this.lbNeedle2PipeTitle.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle2PipeTitle.TabIndex = 7;
            this.lbNeedle2PipeTitle.Text = "Needle 2";
            this.lbNeedle2PipeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbNeedle1Pipe
            // 
            this.tbNeedle1Pipe.Location = new System.Drawing.Point(110, 30);
            this.tbNeedle1Pipe.Name = "tbNeedle1Pipe";
            this.tbNeedle1Pipe.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle1Pipe.TabIndex = 4;
            this.tbNeedle1Pipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle1Pipe.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle1Pipe.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // lbNeedle1PipeTitle
            // 
            this.lbNeedle1PipeTitle.Location = new System.Drawing.Point(10, 35);
            this.lbNeedle1PipeTitle.Name = "lbNeedle1PipeTitle";
            this.lbNeedle1PipeTitle.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle1PipeTitle.TabIndex = 1;
            this.lbNeedle1PipeTitle.Text = "Needle 1";
            this.lbNeedle1PipeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPumpStop
            // 
            this.btnPumpStop.BackColor = System.Drawing.Color.Red;
            this.btnPumpStop.Location = new System.Drawing.Point(240, 110);
            this.btnPumpStop.Name = "btnPumpStop";
            this.btnPumpStop.Size = new System.Drawing.Size(190, 40);
            this.btnPumpStop.TabIndex = 7;
            this.btnPumpStop.Text = "幫浦停止動作";
            this.btnPumpStop.UseVisualStyleBackColor = false;
            this.btnPumpStop.Click += new System.EventHandler(this.btnPumpStop_Click);
            // 
            // btnPumpInit
            // 
            this.btnPumpInit.BackColor = System.Drawing.Color.Lime;
            this.btnPumpInit.Location = new System.Drawing.Point(240, 60);
            this.btnPumpInit.Name = "btnPumpInit";
            this.btnPumpInit.Size = new System.Drawing.Size(190, 40);
            this.btnPumpInit.TabIndex = 6;
            this.btnPumpInit.Text = "幫浦初始化";
            this.btnPumpInit.UseVisualStyleBackColor = false;
            this.btnPumpInit.Click += new System.EventHandler(this.btnPumpInit_Click);
            // 
            // lbPump
            // 
            this.lbPump.Location = new System.Drawing.Point(340, 30);
            this.lbPump.Name = "lbPump";
            this.lbPump.Size = new System.Drawing.Size(90, 20);
            this.lbPump.TabIndex = 5;
            this.lbPump.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPumpTitle
            // 
            this.lbPumpTitle.Location = new System.Drawing.Point(240, 30);
            this.lbPumpTitle.Name = "lbPumpTitle";
            this.lbPumpTitle.Size = new System.Drawing.Size(90, 20);
            this.lbPumpTitle.TabIndex = 4;
            this.lbPumpTitle.Text = "Status";
            this.lbPumpTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbNeedle
            // 
            this.gbNeedle.Controls.Add(this.tbNeedle2PipeOut);
            this.gbNeedle.Controls.Add(this.btnNeedle2PipeOut);
            this.gbNeedle.Controls.Add(this.tbNeedle2PipeIn);
            this.gbNeedle.Controls.Add(this.btnNeedle2PipeIn);
            this.gbNeedle.Controls.Add(this.lbNeedle2);
            this.gbNeedle.Controls.Add(this.lbNeedle2Title);
            this.gbNeedle.Controls.Add(this.tbNeedle1PipeOut);
            this.gbNeedle.Controls.Add(this.btnNeedle1PipeOut);
            this.gbNeedle.Controls.Add(this.tbNeedle1PipeIn);
            this.gbNeedle.Controls.Add(this.btnNeedle1PipeIn);
            this.gbNeedle.Controls.Add(this.lbNeedle1);
            this.gbNeedle.Controls.Add(this.lbNeedle1Title);
            this.gbNeedle.Location = new System.Drawing.Point(230, 150);
            this.gbNeedle.Name = "gbNeedle";
            this.gbNeedle.Size = new System.Drawing.Size(210, 250);
            this.gbNeedle.TabIndex = 2;
            this.gbNeedle.TabStop = false;
            this.gbNeedle.Text = "Sample";
            // 
            // tbNeedle2PipeOut
            // 
            this.tbNeedle2PipeOut.Location = new System.Drawing.Point(110, 210);
            this.tbNeedle2PipeOut.Name = "tbNeedle2PipeOut";
            this.tbNeedle2PipeOut.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle2PipeOut.TabIndex = 36;
            this.tbNeedle2PipeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle2PipeOut.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle2PipeOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // btnNeedle2PipeOut
            // 
            this.btnNeedle2PipeOut.Location = new System.Drawing.Point(10, 210);
            this.btnNeedle2PipeOut.Name = "btnNeedle2PipeOut";
            this.btnNeedle2PipeOut.Size = new System.Drawing.Size(90, 30);
            this.btnNeedle2PipeOut.TabIndex = 35;
            this.btnNeedle2PipeOut.Text = "排出";
            this.btnNeedle2PipeOut.UseVisualStyleBackColor = true;
            this.btnNeedle2PipeOut.Click += new System.EventHandler(this.btnNeedle2PipeOut_Click);
            // 
            // tbNeedle2PipeIn
            // 
            this.tbNeedle2PipeIn.Location = new System.Drawing.Point(110, 170);
            this.tbNeedle2PipeIn.Name = "tbNeedle2PipeIn";
            this.tbNeedle2PipeIn.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle2PipeIn.TabIndex = 34;
            this.tbNeedle2PipeIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle2PipeIn.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle2PipeIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // btnNeedle2PipeIn
            // 
            this.btnNeedle2PipeIn.Location = new System.Drawing.Point(10, 170);
            this.btnNeedle2PipeIn.Name = "btnNeedle2PipeIn";
            this.btnNeedle2PipeIn.Size = new System.Drawing.Size(90, 30);
            this.btnNeedle2PipeIn.TabIndex = 33;
            this.btnNeedle2PipeIn.Text = "抽取";
            this.btnNeedle2PipeIn.UseVisualStyleBackColor = true;
            this.btnNeedle2PipeIn.Click += new System.EventHandler(this.btnNeedle2PipeIn_Click);
            // 
            // lbNeedle2
            // 
            this.lbNeedle2.Location = new System.Drawing.Point(110, 140);
            this.lbNeedle2.Name = "lbNeedle2";
            this.lbNeedle2.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle2.TabIndex = 32;
            this.lbNeedle2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbNeedle2Title
            // 
            this.lbNeedle2Title.Location = new System.Drawing.Point(10, 140);
            this.lbNeedle2Title.Name = "lbNeedle2Title";
            this.lbNeedle2Title.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle2Title.TabIndex = 31;
            this.lbNeedle2Title.Text = "Needle 2";
            this.lbNeedle2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbNeedle1PipeOut
            // 
            this.tbNeedle1PipeOut.Location = new System.Drawing.Point(110, 100);
            this.tbNeedle1PipeOut.Name = "tbNeedle1PipeOut";
            this.tbNeedle1PipeOut.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle1PipeOut.TabIndex = 6;
            this.tbNeedle1PipeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle1PipeOut.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle1PipeOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // btnNeedle1PipeOut
            // 
            this.btnNeedle1PipeOut.Location = new System.Drawing.Point(10, 100);
            this.btnNeedle1PipeOut.Name = "btnNeedle1PipeOut";
            this.btnNeedle1PipeOut.Size = new System.Drawing.Size(90, 30);
            this.btnNeedle1PipeOut.TabIndex = 5;
            this.btnNeedle1PipeOut.Text = "排出";
            this.btnNeedle1PipeOut.UseVisualStyleBackColor = true;
            this.btnNeedle1PipeOut.Click += new System.EventHandler(this.btnNeedle1PipeOut_Click);
            // 
            // tbNeedle1PipeIn
            // 
            this.tbNeedle1PipeIn.Location = new System.Drawing.Point(110, 60);
            this.tbNeedle1PipeIn.Name = "tbNeedle1PipeIn";
            this.tbNeedle1PipeIn.Size = new System.Drawing.Size(90, 29);
            this.tbNeedle1PipeIn.TabIndex = 4;
            this.tbNeedle1PipeIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbNeedle1PipeIn.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbNeedle1PipeIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // btnNeedle1PipeIn
            // 
            this.btnNeedle1PipeIn.Location = new System.Drawing.Point(10, 60);
            this.btnNeedle1PipeIn.Name = "btnNeedle1PipeIn";
            this.btnNeedle1PipeIn.Size = new System.Drawing.Size(90, 30);
            this.btnNeedle1PipeIn.TabIndex = 3;
            this.btnNeedle1PipeIn.Text = "抽取";
            this.btnNeedle1PipeIn.UseVisualStyleBackColor = true;
            this.btnNeedle1PipeIn.Click += new System.EventHandler(this.btnNeedle1PipeIn_Click);
            // 
            // lbNeedle1
            // 
            this.lbNeedle1.Location = new System.Drawing.Point(110, 30);
            this.lbNeedle1.Name = "lbNeedle1";
            this.lbNeedle1.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle1.TabIndex = 2;
            this.lbNeedle1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbNeedle1Title
            // 
            this.lbNeedle1Title.Location = new System.Drawing.Point(10, 30);
            this.lbNeedle1Title.Name = "lbNeedle1Title";
            this.lbNeedle1Title.Size = new System.Drawing.Size(90, 20);
            this.lbNeedle1Title.TabIndex = 1;
            this.lbNeedle1Title.Text = "Needle 1";
            this.lbNeedle1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbPlunger
            // 
            this.gbPlunger.Controls.Add(this.tbPipeOutVel);
            this.gbPlunger.Controls.Add(this.tbPipeInVel);
            this.gbPlunger.Controls.Add(this.lbPipeOutVelTitle);
            this.gbPlunger.Controls.Add(this.lbPipeInVelTitle);
            this.gbPlunger.Controls.Add(this.tbPlunger);
            this.gbPlunger.Controls.Add(this.btnPlunger);
            this.gbPlunger.Controls.Add(this.lbPlunger);
            this.gbPlunger.Controls.Add(this.lbPlungerTitle);
            this.gbPlunger.Location = new System.Drawing.Point(10, 230);
            this.gbPlunger.Name = "gbPlunger";
            this.gbPlunger.Size = new System.Drawing.Size(210, 170);
            this.gbPlunger.TabIndex = 1;
            this.gbPlunger.TabStop = false;
            this.gbPlunger.Text = "Plunger Control";
            // 
            // tbPipeOutVel
            // 
            this.tbPipeOutVel.Location = new System.Drawing.Point(110, 130);
            this.tbPipeOutVel.Name = "tbPipeOutVel";
            this.tbPipeOutVel.Size = new System.Drawing.Size(90, 29);
            this.tbPipeOutVel.TabIndex = 8;
            this.tbPipeOutVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbPipeOutVel.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbPipeOutVel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // tbPipeInVel
            // 
            this.tbPipeInVel.Location = new System.Drawing.Point(110, 95);
            this.tbPipeInVel.Name = "tbPipeInVel";
            this.tbPipeInVel.Size = new System.Drawing.Size(90, 29);
            this.tbPipeInVel.TabIndex = 7;
            this.tbPipeInVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbPipeInVel.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbPipeInVel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // lbPipeOutVelTitle
            // 
            this.lbPipeOutVelTitle.Location = new System.Drawing.Point(10, 130);
            this.lbPipeOutVelTitle.Name = "lbPipeOutVelTitle";
            this.lbPipeOutVelTitle.Size = new System.Drawing.Size(90, 30);
            this.lbPipeOutVelTitle.TabIndex = 6;
            this.lbPipeOutVelTitle.Text = "排出速度";
            this.lbPipeOutVelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPipeInVelTitle
            // 
            this.lbPipeInVelTitle.Location = new System.Drawing.Point(10, 95);
            this.lbPipeInVelTitle.Name = "lbPipeInVelTitle";
            this.lbPipeInVelTitle.Size = new System.Drawing.Size(90, 30);
            this.lbPipeInVelTitle.TabIndex = 5;
            this.lbPipeInVelTitle.Text = "抽取速度";
            this.lbPipeInVelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbPlunger
            // 
            this.tbPlunger.Location = new System.Drawing.Point(110, 60);
            this.tbPlunger.Name = "tbPlunger";
            this.tbPlunger.Size = new System.Drawing.Size(90, 29);
            this.tbPlunger.TabIndex = 4;
            this.tbPlunger.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbPlunger.TextChanged += new System.EventHandler(this.Pump_TextChange);
            this.tbPlunger.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntText_KeyPress);
            // 
            // btnPlunger
            // 
            this.btnPlunger.Location = new System.Drawing.Point(10, 60);
            this.btnPlunger.Name = "btnPlunger";
            this.btnPlunger.Size = new System.Drawing.Size(90, 30);
            this.btnPlunger.TabIndex = 2;
            this.btnPlunger.Text = "位置移動";
            this.btnPlunger.UseVisualStyleBackColor = true;
            this.btnPlunger.Click += new System.EventHandler(this.btnPlunger_Click);
            // 
            // lbPlunger
            // 
            this.lbPlunger.Location = new System.Drawing.Point(110, 30);
            this.lbPlunger.Name = "lbPlunger";
            this.lbPlunger.Size = new System.Drawing.Size(90, 20);
            this.lbPlunger.TabIndex = 1;
            this.lbPlunger.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPlungerTitle
            // 
            this.lbPlungerTitle.Location = new System.Drawing.Point(10, 30);
            this.lbPlungerTitle.Name = "lbPlungerTitle";
            this.lbPlungerTitle.Size = new System.Drawing.Size(90, 20);
            this.lbPlungerTitle.TabIndex = 0;
            this.lbPlungerTitle.Text = "Status";
            this.lbPlungerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbValve
            // 
            this.gbValve.Controls.Add(this.cbValve);
            this.gbValve.Controls.Add(this.tbValveIntro);
            this.gbValve.Controls.Add(this.btnValve);
            this.gbValve.Controls.Add(this.lbValve);
            this.gbValve.Controls.Add(this.lbValveTitle);
            this.gbValve.Location = new System.Drawing.Point(10, 20);
            this.gbValve.Name = "gbValve";
            this.gbValve.Size = new System.Drawing.Size(210, 210);
            this.gbValve.TabIndex = 0;
            this.gbValve.TabStop = false;
            this.gbValve.Text = "Valve Control";
            // 
            // cbValve
            // 
            this.cbValve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValve.FormattingEnabled = true;
            this.cbValve.Items.AddRange(new object[] {
            "清水入",
            "空氣入",
            "針頭1",
            "針頭2"});
            this.cbValve.Location = new System.Drawing.Point(110, 60);
            this.cbValve.Name = "cbValve";
            this.cbValve.Size = new System.Drawing.Size(90, 28);
            this.cbValve.TabIndex = 5;
            // 
            // tbValveIntro
            // 
            this.tbValveIntro.Location = new System.Drawing.Point(10, 100);
            this.tbValveIntro.Multiline = true;
            this.tbValveIntro.Name = "tbValveIntro";
            this.tbValveIntro.ReadOnly = true;
            this.tbValveIntro.Size = new System.Drawing.Size(190, 100);
            this.tbValveIntro.TabIndex = 4;
            this.tbValveIntro.Text = "Valve 1：清水入\r\nValve 2：空氣入\r\nValve 3：針頭1\r\nValve 4：針頭2";
            // 
            // btnValve
            // 
            this.btnValve.Location = new System.Drawing.Point(10, 60);
            this.btnValve.Name = "btnValve";
            this.btnValve.Size = new System.Drawing.Size(90, 30);
            this.btnValve.TabIndex = 2;
            this.btnValve.Text = "閥門切換";
            this.btnValve.UseVisualStyleBackColor = true;
            this.btnValve.Click += new System.EventHandler(this.btnValve_Click);
            // 
            // lbValve
            // 
            this.lbValve.Location = new System.Drawing.Point(110, 30);
            this.lbValve.Name = "lbValve";
            this.lbValve.Size = new System.Drawing.Size(80, 20);
            this.lbValve.TabIndex = 1;
            this.lbValve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbValveTitle
            // 
            this.lbValveTitle.Location = new System.Drawing.Point(10, 30);
            this.lbValveTitle.Name = "lbValveTitle";
            this.lbValveTitle.Size = new System.Drawing.Size(90, 20);
            this.lbValveTitle.TabIndex = 0;
            this.lbValveTitle.Text = "Status";
            this.lbValveTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PumpUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPump);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "PumpUserControl";
            this.Size = new System.Drawing.Size(930, 410);
            this.Load += new System.EventHandler(this.PumpUserControl_Load);
            this.gbPump.ResumeLayout(false);
            this.gbReceiveData.ResumeLayout(false);
            this.gbReceiveData.PerformLayout();
            this.gbSendData.ResumeLayout(false);
            this.gbSendData.PerformLayout();
            this.gbPipeControl.ResumeLayout(false);
            this.gbPipeControl.PerformLayout();
            this.gbNeedle.ResumeLayout(false);
            this.gbNeedle.PerformLayout();
            this.gbPlunger.ResumeLayout(false);
            this.gbPlunger.PerformLayout();
            this.gbValve.ResumeLayout(false);
            this.gbValve.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //Manual Define
        private int _Valve;
        public int Valve
        {
            get
            {
                return _Valve;
            }
            set
            {
                if (value > 6)
                    _Valve = 6;
                else if (value < 1)
                    _Valve = 1;
                else
                    _Valve = value;

                //lbValve.Text = _Valve.ToString();
            }
        }
        private int _Plunger;
        public int Plunger
        {
            get
            {
                return _Plunger;
            }
            set
            {
                if (value > 48000)
                    _Plunger = 48000;
                else if (value < 0)
                    _Plunger = 0;
                else
                    _Plunger = value;

                //lbPlunger.Text = _Plunger.ToString();
            }
        }
        private int _PlungerMove;
        public int PlungerMove
        {
            get
            {
                return _PlungerMove;
            }
            set
            {
                if (value > 48000)
                    _PlungerMove = 48000;
                else if (value < 0)
                    _PlungerMove = 0;
                else
                    _PlungerMove = value;

                tbPlunger.Text = _PlungerMove.ToString();
            }
        }
        private int _PipeInVel;
        public int PipeInVel
        {
            get
            {
                return _PipeInVel;
            }
            set
            {
                if (value > 6000)
                    _PipeInVel = 6000;
                else if (value < 0)
                    _PipeInVel = 0;
                else
                    _PipeInVel = value;

                tbPipeInVel.Text = _PipeInVel.ToString();
            }
        }
        private int _PipeOutVel;
        public int PipeOutVel
        {
            get
            {
                return _PipeOutVel;
            }
            set
            {
                if (value > 6000)
                    _PipeOutVel = 6000;
                else if (value < 0)
                    _PipeOutVel = 0;
                else
                    _PipeOutVel = value;

                tbPipeOutVel.Text = _PipeOutVel.ToString();
            }
        }
        private byte _Pump;
        public byte Pump
        {
            get
            {
                return _Pump;
            }
            set
            {
                _Pump = value;
                if ((value & 32) > 0)
                {
                    Busy = false;
                }
                else
                {
                    Busy = true;
                }
                if ((value & 15) > 0)
                {
                    ErrorCode = (value & 15);
                    //lbPump.Text = "Error : " + ErrorCode.ToString();
                }

            }
        }
        private int _ErrorCode;
        public int ErrorCode
        {
            get
            {
                return _ErrorCode;
            }
            set
            {
                _ErrorCode = value;
                /*
                if (value > 0)
                {
                    Stop();
                    ButtonControl(false);
                }
                else
                    ButtonControl(true);
                    */
            }
        }
        private bool _Busy;
        public bool Busy
        {
            get
            {
                return _Busy;
            }
            set
            {
                _Busy = value;
            }
        }
        public bool SendingCommand
        { get; set; }
        private int _Needle1;
        public int Needle1
        {
            get
            {
                return _Needle1;
            }
            set
            {
                _Needle1 = value;
            }
        }
        private int _Needle1PipeIn;
        public int Needle1PipeIn
        {
            get
            {
                return _Needle1PipeIn;
            }
            set
            {
                _Needle1PipeIn = value;
                tbNeedle1PipeIn.Text = _Needle1PipeIn.ToString();
            }
        }
        private int _Needle1PipeOut;
        public int Needle1PipeOut
        {
            get
            {
                return _Needle1PipeOut;
            }
            set
            {
                _Needle1PipeOut = value;
                tbNeedle1PipeOut.Text = _Needle1PipeOut.ToString();
            }
        }
        private int _Needle2;
        public int Needle2
        {
            get
            {
                return _Needle2;
            }
            set
            {
                _Needle2 = value;
            }
        }
        private int _Needle2PipeIn;
        public int Needle2PipeIn
        {
            get
            {
                return _Needle2PipeIn;
            }
            set
            {
                _Needle2PipeIn = value;
                tbNeedle2PipeIn.Text = _Needle2PipeIn.ToString();
            }
        }
        private int _Needle2PipeOut;
        public int Needle2PipeOut
        {
            get
            {
                return _Needle2PipeOut;
            }
            set
            {
                _Needle2PipeOut = value;
                tbNeedle2PipeOut.Text = _Needle2PipeOut.ToString();
            }
        }
        private int _Needle1Pipe;
        public int Needle1Pipe
        {
            get
            {
                return _Needle1Pipe;
            }
            set
            {
                if (value > 48000)
                    _Needle1Pipe = 48000;
                else if (value < 1)
                    _Needle1Pipe = 1;
                else
                    _Needle1Pipe = value;

                tbNeedle1Pipe.Text = _Needle1Pipe.ToString();
            }
        }
        private int _Needle2Pipe;
        public int Needle2Pipe
        {
            get
            {
                return _Needle2Pipe;
            }
            set
            {
                if (value > 48000)
                    _Needle2Pipe = 48000;
                else if (value < 1)
                    _Needle2Pipe = 1;
                else
                    _Needle2Pipe = value;

                tbNeedle2Pipe.Text = _Needle2Pipe.ToString();
            }
        }
        public bool Cycle_Pipe;
        private int SendCount;
        //Pump Serial Port
        public SerialPort PumpPort = new SerialPort();
        private int count;
        private string Command;
        private System.Windows.Forms.GroupBox gbPump;
        private System.Windows.Forms.GroupBox gbReceiveData;
        private System.Windows.Forms.TextBox tbReceiveData;
        private System.Windows.Forms.GroupBox gbSendData;
        private System.Windows.Forms.TextBox tbSendData;
        private System.Windows.Forms.GroupBox gbPipeControl;
        private System.Windows.Forms.RadioButton rbtnAir;
        private System.Windows.Forms.RadioButton rbtnWater;
        private System.Windows.Forms.TextBox tbPipeIntro;
        private System.Windows.Forms.TextBox tbNeedle2Pipe;
        private System.Windows.Forms.Label lbNeedle2PipeTitle;
        private System.Windows.Forms.TextBox tbNeedle1Pipe;
        private System.Windows.Forms.Label lbNeedle1PipeTitle;
        private System.Windows.Forms.Button btnPumpStop;
        private System.Windows.Forms.Button btnPumpInit;
        private System.Windows.Forms.Label lbPump;
        private System.Windows.Forms.Label lbPumpTitle;
        private System.Windows.Forms.GroupBox gbNeedle;
        private System.Windows.Forms.TextBox tbNeedle2PipeOut;
        private System.Windows.Forms.Button btnNeedle2PipeOut;
        private System.Windows.Forms.TextBox tbNeedle2PipeIn;
        private System.Windows.Forms.Button btnNeedle2PipeIn;
        private System.Windows.Forms.Label lbNeedle2;
        private System.Windows.Forms.Label lbNeedle2Title;
        private System.Windows.Forms.TextBox tbNeedle1PipeOut;
        private System.Windows.Forms.Button btnNeedle1PipeOut;
        private System.Windows.Forms.TextBox tbNeedle1PipeIn;
        private System.Windows.Forms.Button btnNeedle1PipeIn;
        private System.Windows.Forms.Label lbNeedle1;
        private System.Windows.Forms.Label lbNeedle1Title;
        private System.Windows.Forms.GroupBox gbPlunger;
        private System.Windows.Forms.TextBox tbPipeOutVel;
        private System.Windows.Forms.TextBox tbPipeInVel;
        private System.Windows.Forms.Label lbPipeOutVelTitle;
        private System.Windows.Forms.Label lbPipeInVelTitle;
        private System.Windows.Forms.TextBox tbPlunger;
        private System.Windows.Forms.Button btnPlunger;
        private System.Windows.Forms.Label lbPlunger;
        private System.Windows.Forms.Label lbPlungerTitle;
        private System.Windows.Forms.GroupBox gbValve;
        private System.Windows.Forms.ComboBox cbValve;
        private System.Windows.Forms.TextBox tbValveIntro;
        private System.Windows.Forms.Button btnValve;
        private System.Windows.Forms.Label lbValve;
        private System.Windows.Forms.Label lbValveTitle;

        delegate void Display(string str);
    }
}
