namespace InkjetPrinter
{
    partial class CoordinateConverter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbPointZ = new System.Windows.Forms.Label();
            this.lbPointY = new System.Windows.Forms.Label();
            this.lbPointX = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRunZ = new System.Windows.Forms.TextBox();
            this.tbRunY = new System.Windows.Forms.TextBox();
            this.tbRunX = new System.Windows.Forms.TextBox();
            this.tbORGR = new System.Windows.Forms.TextBox();
            this.btnORGRSet = new System.Windows.Forms.Button();
            this.btnORGXYSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbPointZ
            // 
            this.lbPointZ.AutoSize = true;
            this.lbPointZ.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPointZ.Location = new System.Drawing.Point(355, 5);
            this.lbPointZ.Name = "lbPointZ";
            this.lbPointZ.Size = new System.Drawing.Size(0, 20);
            this.lbPointZ.TabIndex = 35;
            // 
            // lbPointY
            // 
            this.lbPointY.AutoSize = true;
            this.lbPointY.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPointY.Location = new System.Drawing.Point(255, 5);
            this.lbPointY.Name = "lbPointY";
            this.lbPointY.Size = new System.Drawing.Size(0, 20);
            this.lbPointY.TabIndex = 34;
            // 
            // lbPointX
            // 
            this.lbPointX.AutoSize = true;
            this.lbPointX.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPointX.Location = new System.Drawing.Point(155, 5);
            this.lbPointX.Name = "lbPointX";
            this.lbPointX.Size = new System.Drawing.Size(0, 20);
            this.lbPointX.TabIndex = 33;
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStop.Location = new System.Drawing.Point(405, 50);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 29);
            this.btnStop.TabIndex = 32;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRun.Location = new System.Drawing.Point(5, 50);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(100, 29);
            this.btnRun.TabIndex = 31;
            this.btnRun.Text = "移動至";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(35, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "座標";
            // 
            // tbRunZ
            // 
            this.tbRunZ.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbRunZ.Location = new System.Drawing.Point(305, 50);
            this.tbRunZ.Name = "tbRunZ";
            this.tbRunZ.Size = new System.Drawing.Size(100, 29);
            this.tbRunZ.TabIndex = 29;
            this.tbRunZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRunY
            // 
            this.tbRunY.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbRunY.Location = new System.Drawing.Point(205, 50);
            this.tbRunY.Name = "tbRunY";
            this.tbRunY.Size = new System.Drawing.Size(100, 29);
            this.tbRunY.TabIndex = 28;
            this.tbRunY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRunX
            // 
            this.tbRunX.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbRunX.Location = new System.Drawing.Point(105, 50);
            this.tbRunX.Name = "tbRunX";
            this.tbRunX.Size = new System.Drawing.Size(100, 29);
            this.tbRunX.TabIndex = 27;
            this.tbRunX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbORGR
            // 
            this.tbORGR.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbORGR.Location = new System.Drawing.Point(305, 100);
            this.tbORGR.Name = "tbORGR";
            this.tbORGR.Size = new System.Drawing.Size(100, 29);
            this.tbORGR.TabIndex = 26;
            this.tbORGR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnORGRSet
            // 
            this.btnORGRSet.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnORGRSet.Location = new System.Drawing.Point(155, 100);
            this.btnORGRSet.Name = "btnORGRSet";
            this.btnORGRSet.Size = new System.Drawing.Size(100, 29);
            this.btnORGRSet.TabIndex = 25;
            this.btnORGRSet.Text = "設定角度";
            this.btnORGRSet.UseVisualStyleBackColor = true;
            this.btnORGRSet.Click += new System.EventHandler(this.btnORGRSet_Click);
            // 
            // btnORGXYSet
            // 
            this.btnORGXYSet.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnORGXYSet.Location = new System.Drawing.Point(5, 100);
            this.btnORGXYSet.Name = "btnORGXYSet";
            this.btnORGXYSet.Size = new System.Drawing.Size(100, 29);
            this.btnORGXYSet.TabIndex = 24;
            this.btnORGXYSet.Text = "設定原點";
            this.btnORGXYSet.UseVisualStyleBackColor = true;
            this.btnORGXYSet.Click += new System.EventHandler(this.btnORGXYSet_Click);
            // 
            // CoordinateConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbPointZ);
            this.Controls.Add(this.lbPointY);
            this.Controls.Add(this.lbPointX);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbRunZ);
            this.Controls.Add(this.tbRunY);
            this.Controls.Add(this.tbRunX);
            this.Controls.Add(this.tbORGR);
            this.Controls.Add(this.btnORGRSet);
            this.Controls.Add(this.btnORGXYSet);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CoordinateConverter";
            this.Size = new System.Drawing.Size(550, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPointZ;
        private System.Windows.Forms.Label lbPointY;
        private System.Windows.Forms.Label lbPointX;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRunZ;
        private System.Windows.Forms.TextBox tbRunY;
        private System.Windows.Forms.TextBox tbRunX;
        private System.Windows.Forms.TextBox tbORGR;
        private System.Windows.Forms.Button btnORGRSet;
        private System.Windows.Forms.Button btnORGXYSet;


    }
}
