namespace WindowsFormsApplication1
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txStartPort = new System.Windows.Forms.TextBox();
            this.txSendInterval = new System.Windows.Forms.TextBox();
            this.txSendLen = new System.Windows.Forms.TextBox();
            this.GUITimer = new System.Windows.Forms.Timer(this.components);
            this.butOpenSelect = new System.Windows.Forms.Button();
            this.butSendSelect = new System.Windows.Forms.Button();
            this.butClearAll = new System.Windows.Forms.Button();
            this.lbStartPort = new System.Windows.Forms.Label();
            this.lbLength = new System.Windows.Forms.Label();
            this.lbInterval = new System.Windows.Forms.Label();
            this.ckAll = new System.Windows.Forms.CheckBox();
            this.ckOdd = new System.Windows.Forms.CheckBox();
            this.ckEven = new System.Windows.Forms.CheckBox();
            this.txSendTotal = new System.Windows.Forms.TextBox();
            this.lblSendTotal = new System.Windows.Forms.Label();
            this.ckLoopback = new System.Windows.Forms.CheckBox();
            this.DebugBox = new System.Windows.Forms.ListBox();
            this.butDebug = new System.Windows.Forms.Button();
            this.txSettingAll = new System.Windows.Forms.TextBox();
            this.TAvgLatency = new System.Windows.Forms.TextBox();
            this.ckWaitnSend = new System.Windows.Forms.CheckBox();
            this.txWaitCount = new System.Windows.Forms.TextBox();
            this.butStopSelect = new System.Windows.Forms.Button();
            this.lbTotalPort = new System.Windows.Forms.Label();
            this.txTotalPort = new System.Windows.Forms.TextBox();
            this.ckStopOnErr = new System.Windows.Forms.CheckBox();
            this.ckSkipErrCk = new System.Windows.Forms.CheckBox();
            this.txXon = new System.Windows.Forms.TextBox();
            this.txXoff = new System.Windows.Forms.TextBox();
            this.lbXon = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbN = new System.Windows.Forms.RadioButton();
            this.rbRTSCTS = new System.Windows.Forms.RadioButton();
            this.rbDSRDTR = new System.Windows.Forms.RadioButton();
            this.rbXonXoff = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txDuration = new System.Windows.Forms.TextBox();
            this.ckCustomStr = new System.Windows.Forms.CheckBox();
            this.ckHalf = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txLoopBDelay = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // txStartPort
            // 
            this.txStartPort.Location = new System.Drawing.Point(70, 29);
            this.txStartPort.Name = "txStartPort";
            this.txStartPort.Size = new System.Drawing.Size(40, 22);
            this.txStartPort.TabIndex = 1;
            this.txStartPort.Text = "7";
            this.txStartPort.TextChanged += new System.EventHandler(this.txStartPort_TextChanged);
            this.txStartPort.Leave += new System.EventHandler(this.txStartPort_Leave);
            // 
            // txSendInterval
            // 
            this.txSendInterval.Location = new System.Drawing.Point(190, 3);
            this.txSendInterval.Name = "txSendInterval";
            this.txSendInterval.Size = new System.Drawing.Size(41, 22);
            this.txSendInterval.TabIndex = 2;
            this.txSendInterval.Text = "200";
            this.txSendInterval.TextChanged += new System.EventHandler(this.txSendInterval_TextChanged);
            // 
            // txSendLen
            // 
            this.txSendLen.Location = new System.Drawing.Point(69, 3);
            this.txSendLen.Name = "txSendLen";
            this.txSendLen.Size = new System.Drawing.Size(41, 22);
            this.txSendLen.TabIndex = 3;
            this.txSendLen.Text = "30";
            this.txSendLen.TextChanged += new System.EventHandler(this.txSendLen_TextChanged);
            // 
            // GUITimer
            // 
            this.GUITimer.Enabled = true;
            this.GUITimer.Interval = 50;
            this.GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // butOpenSelect
            // 
            this.butOpenSelect.Location = new System.Drawing.Point(838, 3);
            this.butOpenSelect.Name = "butOpenSelect";
            this.butOpenSelect.Size = new System.Drawing.Size(88, 47);
            this.butOpenSelect.TabIndex = 4;
            this.butOpenSelect.Text = "Open/Close";
            this.butOpenSelect.UseVisualStyleBackColor = true;
            this.butOpenSelect.Click += new System.EventHandler(this.butOpenSelect_Click);
            // 
            // butSendSelect
            // 
            this.butSendSelect.Location = new System.Drawing.Point(927, 1);
            this.butSendSelect.Name = "butSendSelect";
            this.butSendSelect.Size = new System.Drawing.Size(62, 25);
            this.butSendSelect.TabIndex = 5;
            this.butSendSelect.Text = "Send";
            this.butSendSelect.UseVisualStyleBackColor = true;
            this.butSendSelect.Click += new System.EventHandler(this.butSendSelect_Click);
            // 
            // butClearAll
            // 
            this.butClearAll.Location = new System.Drawing.Point(990, 2);
            this.butClearAll.Name = "butClearAll";
            this.butClearAll.Size = new System.Drawing.Size(71, 49);
            this.butClearAll.TabIndex = 6;
            this.butClearAll.Text = "Clear All";
            this.butClearAll.UseVisualStyleBackColor = true;
            this.butClearAll.Click += new System.EventHandler(this.butClearAll_Click);
            // 
            // lbStartPort
            // 
            this.lbStartPort.AutoSize = true;
            this.lbStartPort.Location = new System.Drawing.Point(2, 31);
            this.lbStartPort.Name = "lbStartPort";
            this.lbStartPort.Size = new System.Drawing.Size(65, 16);
            this.lbStartPort.TabIndex = 7;
            this.lbStartPort.Text = "Start Port:";
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.Location = new System.Drawing.Point(2, 5);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(68, 16);
            this.lbLength.TabIndex = 8;
            this.lbLength.Text = "Length(B):";
            // 
            // lbInterval
            // 
            this.lbInterval.AutoSize = true;
            this.lbInterval.Location = new System.Drawing.Point(111, 7);
            this.lbInterval.Name = "lbInterval";
            this.lbInterval.Size = new System.Drawing.Size(80, 16);
            this.lbInterval.TabIndex = 9;
            this.lbInterval.Text = "Interval(ms):";
            // 
            // ckAll
            // 
            this.ckAll.AutoSize = true;
            this.ckAll.Location = new System.Drawing.Point(744, 0);
            this.ckAll.Name = "ckAll";
            this.ckAll.Size = new System.Drawing.Size(83, 20);
            this.ckAll.TabIndex = 10;
            this.ckAll.Text = "Check All";
            this.ckAll.UseVisualStyleBackColor = true;
            this.ckAll.CheckedChanged += new System.EventHandler(this.ckAll_CheckedChanged);
            // 
            // ckOdd
            // 
            this.ckOdd.AutoSize = true;
            this.ckOdd.Location = new System.Drawing.Point(745, 16);
            this.ckOdd.Name = "ckOdd";
            this.ckOdd.Size = new System.Drawing.Size(94, 20);
            this.ckOdd.TabIndex = 11;
            this.ckOdd.Text = "Check Odd";
            this.ckOdd.UseVisualStyleBackColor = true;
            this.ckOdd.CheckedChanged += new System.EventHandler(this.ckOdd_CheckedChanged);
            // 
            // ckEven
            // 
            this.ckEven.AutoSize = true;
            this.ckEven.Location = new System.Drawing.Point(745, 33);
            this.ckEven.Name = "ckEven";
            this.ckEven.Size = new System.Drawing.Size(99, 20);
            this.ckEven.TabIndex = 12;
            this.ckEven.Text = "Check Even";
            this.ckEven.UseVisualStyleBackColor = true;
            this.ckEven.CheckedChanged += new System.EventHandler(this.ckEven_CheckedChanged);
            // 
            // txSendTotal
            // 
            this.txSendTotal.Location = new System.Drawing.Point(300, 29);
            this.txSendTotal.Name = "txSendTotal";
            this.txSendTotal.Size = new System.Drawing.Size(41, 22);
            this.txSendTotal.TabIndex = 16;
            this.txSendTotal.Text = "99";
            this.txSendTotal.TextChanged += new System.EventHandler(this.txSendTotal_TextChanged);
            // 
            // lblSendTotal
            // 
            this.lblSendTotal.AutoSize = true;
            this.lblSendTotal.Location = new System.Drawing.Point(232, 33);
            this.lblSendTotal.Name = "lblSendTotal";
            this.lblSendTotal.Size = new System.Drawing.Size(70, 16);
            this.lblSendTotal.TabIndex = 15;
            this.lblSendTotal.Text = "Total(MB):";
            // 
            // ckLoopback
            // 
            this.ckLoopback.AutoSize = true;
            this.ckLoopback.Location = new System.Drawing.Point(644, 1);
            this.ckLoopback.Name = "ckLoopback";
            this.ckLoopback.Size = new System.Drawing.Size(88, 20);
            this.ckLoopback.TabIndex = 17;
            this.ckLoopback.Text = "Loopback";
            this.ckLoopback.UseVisualStyleBackColor = true;
            // 
            // DebugBox
            // 
            this.DebugBox.FormattingEnabled = true;
            this.DebugBox.ItemHeight = 16;
            this.DebugBox.Location = new System.Drawing.Point(5, 348);
            this.DebugBox.Name = "DebugBox";
            this.DebugBox.Size = new System.Drawing.Size(1200, 308);
            this.DebugBox.TabIndex = 18;
            this.DebugBox.Visible = false;
            // 
            // butDebug
            // 
            this.butDebug.Location = new System.Drawing.Point(1063, 3);
            this.butDebug.Name = "butDebug";
            this.butDebug.Size = new System.Drawing.Size(57, 47);
            this.butDebug.TabIndex = 19;
            this.butDebug.Text = "Debug";
            this.butDebug.UseVisualStyleBackColor = true;
            this.butDebug.Click += new System.EventHandler(this.butDebug_Click);
            // 
            // txSettingAll
            // 
            this.txSettingAll.AcceptsReturn = true;
            this.txSettingAll.Location = new System.Drawing.Point(347, 29);
            this.txSettingAll.Name = "txSettingAll";
            this.txSettingAll.Size = new System.Drawing.Size(83, 22);
            this.txSettingAll.TabIndex = 20;
            this.txSettingAll.Text = "9600,n,8,1";
            this.txSettingAll.TextChanged += new System.EventHandler(this.txSettingAll_TextChanged);
            // 
            // TAvgLatency
            // 
            this.TAvgLatency.Enabled = false;
            this.TAvgLatency.Location = new System.Drawing.Point(738, 79);
            this.TAvgLatency.Name = "TAvgLatency";
            this.TAvgLatency.Size = new System.Drawing.Size(46, 22);
            this.TAvgLatency.TabIndex = 21;
            this.TAvgLatency.Text = "0";
            this.TAvgLatency.Visible = false;
            // 
            // ckWaitnSend
            // 
            this.ckWaitnSend.AutoSize = true;
            this.ckWaitnSend.Checked = true;
            this.ckWaitnSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckWaitnSend.Location = new System.Drawing.Point(644, 17);
            this.ckWaitnSend.Name = "ckWaitnSend";
            this.ckWaitnSend.Size = new System.Drawing.Size(99, 20);
            this.ckWaitnSend.TabIndex = 22;
            this.ckWaitnSend.Text = "Wait n Send";
            this.ckWaitnSend.UseVisualStyleBackColor = true;
            this.ckWaitnSend.CheckedChanged += new System.EventHandler(this.ckWaitnSend_CheckedChanged);
            // 
            // txWaitCount
            // 
            this.txWaitCount.Location = new System.Drawing.Point(414, 4);
            this.txWaitCount.Name = "txWaitCount";
            this.txWaitCount.Size = new System.Drawing.Size(47, 22);
            this.txWaitCount.TabIndex = 23;
            this.txWaitCount.Text = "1500";
            this.txWaitCount.TextChanged += new System.EventHandler(this.txWaitCount_TextChanged);
            // 
            // butStopSelect
            // 
            this.butStopSelect.Location = new System.Drawing.Point(927, 26);
            this.butStopSelect.Name = "butStopSelect";
            this.butStopSelect.Size = new System.Drawing.Size(62, 24);
            this.butStopSelect.TabIndex = 24;
            this.butStopSelect.Text = "Stop";
            this.butStopSelect.UseVisualStyleBackColor = true;
            this.butStopSelect.Click += new System.EventHandler(this.butStopSelect_Click);
            // 
            // lbTotalPort
            // 
            this.lbTotalPort.AutoSize = true;
            this.lbTotalPort.Location = new System.Drawing.Point(601, 127);
            this.lbTotalPort.Name = "lbTotalPort";
            this.lbTotalPort.Size = new System.Drawing.Size(69, 16);
            this.lbTotalPort.TabIndex = 25;
            this.lbTotalPort.Text = "Total Port:";
            this.lbTotalPort.Visible = false;
            // 
            // txTotalPort
            // 
            this.txTotalPort.Enabled = false;
            this.txTotalPort.Location = new System.Drawing.Point(676, 126);
            this.txTotalPort.Name = "txTotalPort";
            this.txTotalPort.Size = new System.Drawing.Size(37, 22);
            this.txTotalPort.TabIndex = 26;
            this.txTotalPort.Text = "16";
            this.txTotalPort.Visible = false;
            this.txTotalPort.TextChanged += new System.EventHandler(this.txTotalPort_TextChanged);
            // 
            // ckStopOnErr
            // 
            this.ckStopOnErr.AutoSize = true;
            this.ckStopOnErr.Location = new System.Drawing.Point(644, 34);
            this.ckStopOnErr.Name = "ckStopOnErr";
            this.ckStopOnErr.Size = new System.Drawing.Size(105, 20);
            this.ckStopOnErr.TabIndex = 27;
            this.ckStopOnErr.Text = "Stop on Error";
            this.ckStopOnErr.UseVisualStyleBackColor = true;
            // 
            // ckSkipErrCk
            // 
            this.ckSkipErrCk.AutoSize = true;
            this.ckSkipErrCk.Location = new System.Drawing.Point(549, 1);
            this.ckSkipErrCk.Name = "ckSkipErrCk";
            this.ckSkipErrCk.Size = new System.Drawing.Size(100, 20);
            this.ckSkipErrCk.TabIndex = 28;
            this.ckSkipErrCk.Text = "Skip Err Chk";
            this.ckSkipErrCk.UseVisualStyleBackColor = true;
            // 
            // txXon
            // 
            this.txXon.Location = new System.Drawing.Point(501, 4);
            this.txXon.Name = "txXon";
            this.txXon.Size = new System.Drawing.Size(41, 22);
            this.txXon.TabIndex = 29;
            this.txXon.Text = "11";
            // 
            // txXoff
            // 
            this.txXoff.Location = new System.Drawing.Point(501, 30);
            this.txXoff.Name = "txXoff";
            this.txXoff.Size = new System.Drawing.Size(41, 22);
            this.txXoff.TabIndex = 30;
            this.txXoff.Text = "13";
            // 
            // lbXon
            // 
            this.lbXon.AutoSize = true;
            this.lbXon.Location = new System.Drawing.Point(436, 6);
            this.lbXon.Name = "lbXon";
            this.lbXon.Size = new System.Drawing.Size(66, 16);
            this.lbXon.TabIndex = 31;
            this.lbXon.Text = "Xon(Hex):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 32;
            this.label1.Text = "Xoff(Hex):";
            // 
            // rbN
            // 
            this.rbN.AutoSize = true;
            this.rbN.Checked = true;
            this.rbN.Location = new System.Drawing.Point(546, 34);
            this.rbN.Name = "rbN";
            this.rbN.Size = new System.Drawing.Size(36, 20);
            this.rbN.TabIndex = 33;
            this.rbN.TabStop = true;
            this.rbN.Text = "N";
            this.rbN.UseVisualStyleBackColor = true;
            this.rbN.CheckedChanged += new System.EventHandler(this.rbN_CheckedChanged);
            // 
            // rbRTSCTS
            // 
            this.rbRTSCTS.AutoSize = true;
            this.rbRTSCTS.Location = new System.Drawing.Point(608, 34);
            this.rbRTSCTS.Name = "rbRTSCTS";
            this.rbRTSCTS.Size = new System.Drawing.Size(36, 20);
            this.rbRTSCTS.TabIndex = 34;
            this.rbRTSCTS.Text = "R";
            this.rbRTSCTS.UseVisualStyleBackColor = true;
            this.rbRTSCTS.CheckedChanged += new System.EventHandler(this.rbRTSCTS_CheckedChanged);
            // 
            // rbDSRDTR
            // 
            this.rbDSRDTR.AutoSize = true;
            this.rbDSRDTR.Location = new System.Drawing.Point(763, 127);
            this.rbDSRDTR.Name = "rbDSRDTR";
            this.rbDSRDTR.Size = new System.Drawing.Size(36, 20);
            this.rbDSRDTR.TabIndex = 35;
            this.rbDSRDTR.Text = "D";
            this.rbDSRDTR.UseVisualStyleBackColor = true;
            this.rbDSRDTR.Visible = false;
            // 
            // rbXonXoff
            // 
            this.rbXonXoff.AutoSize = true;
            this.rbXonXoff.Location = new System.Drawing.Point(578, 34);
            this.rbXonXoff.Name = "rbXonXoff";
            this.rbXonXoff.Size = new System.Drawing.Size(34, 20);
            this.rbXonXoff.TabIndex = 36;
            this.rbXonXoff.Text = "X";
            this.rbXonXoff.UseVisualStyleBackColor = true;
            this.rbXonXoff.CheckedChanged += new System.EventHandler(this.rbXonXoff_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "Duration(m):";
            // 
            // txDuration
            // 
            this.txDuration.Location = new System.Drawing.Point(190, 30);
            this.txDuration.Name = "txDuration";
            this.txDuration.Size = new System.Drawing.Size(41, 22);
            this.txDuration.TabIndex = 38;
            this.txDuration.Text = "9999";
            this.txDuration.TextChanged += new System.EventHandler(this.txDuration_TextChanged);
            // 
            // ckCustomStr
            // 
            this.ckCustomStr.AutoSize = true;
            this.ckCustomStr.Location = new System.Drawing.Point(549, 17);
            this.ckCustomStr.Name = "ckCustomStr";
            this.ckCustomStr.Size = new System.Drawing.Size(91, 20);
            this.ckCustomStr.TabIndex = 39;
            this.ckCustomStr.Text = "Custom Str";
            this.ckCustomStr.UseVisualStyleBackColor = true;
            this.ckCustomStr.CheckedChanged += new System.EventHandler(this.ckCustomStr_CheckedChanged);
            // 
            // ckHalf
            // 
            this.ckHalf.AutoSize = true;
            this.ckHalf.Location = new System.Drawing.Point(414, 16);
            this.ckHalf.Name = "ckHalf";
            this.ckHalf.Size = new System.Drawing.Size(51, 20);
            this.ckHalf.TabIndex = 41;
            this.ckHalf.Text = "Half";
            this.ckHalf.UseVisualStyleBackColor = true;
            this.ckHalf.Visible = false;
            this.ckHalf.CheckedChanged += new System.EventHandler(this.ckHalf_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 16);
            this.label3.TabIndex = 42;
            this.label3.Text = "Loopback Delay(ms):";
            // 
            // txLoopBDelay
            // 
            this.txLoopBDelay.Location = new System.Drawing.Point(367, 2);
            this.txLoopBDelay.Name = "txLoopBDelay";
            this.txLoopBDelay.Size = new System.Drawing.Size(41, 22);
            this.txLoopBDelay.TabIndex = 43;
            this.txLoopBDelay.Text = "0";
            this.txLoopBDelay.TextChanged += new System.EventHandler(this.txLoopBDelay_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1133, 378);
            this.Controls.Add(this.txLoopBDelay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ckHalf);
            this.Controls.Add(this.txDuration);
            this.Controls.Add(this.txSendInterval);
            this.Controls.Add(this.txSendLen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbDSRDTR);
            this.Controls.Add(this.txTotalPort);
            this.Controls.Add(this.lbTotalPort);
            this.Controls.Add(this.butStopSelect);
            this.Controls.Add(this.TAvgLatency);
            this.Controls.Add(this.txSettingAll);
            this.Controls.Add(this.butDebug);
            this.Controls.Add(this.DebugBox);
            this.Controls.Add(this.txSendTotal);
            this.Controls.Add(this.lblSendTotal);
            this.Controls.Add(this.lbInterval);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.lbStartPort);
            this.Controls.Add(this.butClearAll);
            this.Controls.Add(this.butSendSelect);
            this.Controls.Add(this.butOpenSelect);
            this.Controls.Add(this.txStartPort);
            this.Controls.Add(this.txWaitCount);
            this.Controls.Add(this.ckEven);
            this.Controls.Add(this.ckOdd);
            this.Controls.Add(this.ckAll);
            this.Controls.Add(this.ckWaitnSend);
            this.Controls.Add(this.ckLoopback);
            this.Controls.Add(this.ckStopOnErr);
            this.Controls.Add(this.ckCustomStr);
            this.Controls.Add(this.txXon);
            this.Controls.Add(this.rbRTSCTS);
            this.Controls.Add(this.rbXonXoff);
            this.Controls.Add(this.rbN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbXon);
            this.Controls.Add(this.txXoff);
            this.Controls.Add(this.ckSkipErrCk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "COM Array[16] v0.9c for Auto-test (2015/2/)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txStartPort;
        private System.Windows.Forms.TextBox txSendInterval;
        private System.Windows.Forms.TextBox txSendLen;
        private System.Windows.Forms.Timer GUITimer;
        private System.Windows.Forms.Button butOpenSelect;
        private System.Windows.Forms.Button butSendSelect;
        private System.Windows.Forms.Button butClearAll;
        private System.Windows.Forms.Label lbStartPort;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Label lbInterval;
        private System.Windows.Forms.CheckBox ckAll;
        private System.Windows.Forms.CheckBox ckOdd;
        private System.Windows.Forms.CheckBox ckEven;
        private System.Windows.Forms.TextBox txSendTotal;
        private System.Windows.Forms.Label lblSendTotal;
        private System.Windows.Forms.CheckBox ckLoopback;
        private System.Windows.Forms.ListBox DebugBox;
        private System.Windows.Forms.Button butDebug;
        private System.Windows.Forms.TextBox txSettingAll;
        private System.Windows.Forms.TextBox TAvgLatency;
        private System.Windows.Forms.CheckBox ckWaitnSend;
        private System.Windows.Forms.TextBox txWaitCount;
        private System.Windows.Forms.Button butStopSelect;
        private System.Windows.Forms.Label lbTotalPort;
        private System.Windows.Forms.TextBox txTotalPort;
        private System.Windows.Forms.CheckBox ckStopOnErr;
        private System.Windows.Forms.CheckBox ckSkipErrCk;
        private System.Windows.Forms.TextBox txXon;
        private System.Windows.Forms.TextBox txXoff;
        private System.Windows.Forms.Label lbXon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbN;
        private System.Windows.Forms.RadioButton rbRTSCTS;
        private System.Windows.Forms.RadioButton rbDSRDTR;
        private System.Windows.Forms.RadioButton rbXonXoff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txDuration;
        private System.Windows.Forms.CheckBox ckCustomStr;
        private System.Windows.Forms.CheckBox ckHalf;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txLoopBDelay;
        private System.IO.Ports.SerialPort serialPort1;

    }
}

