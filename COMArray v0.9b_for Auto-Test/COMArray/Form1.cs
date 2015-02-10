using System;
using System.Diagnostics;               //high resolution stopwatch
using System.Drawing;
//using System.IO;                      //StreamReader
using System.IO.Ports;                  //SerialPort
using System.Windows.Forms;
using System.Collections;               //ArrayList
using Utility;                          //HexEncoding
using Microsoft.VisualBasic;
using System.Text;                      //Encoding
using System.Threading;               //Dispatcher timer
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using MicroLibrary;
using System.IO;                     //Microsecond Timer

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public class CustomMSCom : SerialPort
        {
            private short _socketID;
            public short Tag
            {
                get { return _socketID; }
                set { _socketID = value; }
            }
        } 

        public class CustomButton : Button
        {
            private short _socketID;
            public short jTag
            {
                get { return _socketID; }
                set { _socketID = value; }
            }
        } 

        #region Declaration
        private const short max = 16;
        private const uint SETXOFF = 1;
        private const uint SETXON = 2;
        private const uint SETRTS = 3;
        private const uint CLRRTS = 4;
        private const uint SETDTR = 5;
        private const uint CLRDTR = 6;
        private const uint RESETDEV = 7;
        private const uint SETBREAK = 8;
        private const uint CLRBREAK = 9;

        private Panel[] Panel = new Panel[max];                              //one panel is one COM port
        private Label[] lbRx = new Label[max];
        private Label[] lbTx = new Label[max];
        private Label[] lbLoss = new Label[max];
        private Label[] lbError = new Label[max];
        private Label[] lbLatency = new Label[max];
        private Label[] lbAvgLatency = new Label[max];
        private Button[] Open = new Button[max];
        private Button[] Send = new Button[max];
        private Button[] Stop = new Button[max];
        private Button[] Clear = new Button[max];
        private Button[] CTS = new Button[max];
        private Button[] DSR = new Button[max];
        private RadioButton[] NoFlowCon = new RadioButton[max];
        private RadioButton[] RTSCTS = new RadioButton[max];
        private RadioButton[] XonXoff = new RadioButton[max];
        private TextBox[] Loss = new TextBox[max];
        private TextBox[] Error = new TextBox[max];
        private TextBox[] Rx = new TextBox[max];
        private TextBox[] Tx = new TextBox[max];
        private TextBox[] Setting = new TextBox[max];
        private TextBox[] CurLatency = new TextBox[max];                       //show current latency
        private TextBox[] AvgLatency = new TextBox[max];                       //show average latency
        private ComboBox[] Port = new ComboBox[max];
        private CustomButton[,] MSComEscape;
        private CustomButton[,] SetFlow;
        private CustomMSCom[] MSCom = new CustomMSCom[max];
        private Stopwatch[] Latencytick = new Stopwatch[max];                    //for latency calculation
        private MicroTimer[] Send_Timer = new MicroTimer[max];
        private MicroTimer[] Delay_Timer = new MicroTimer[max];
        private ArrayList[] loopback_buffer = new ArrayList[max];                //array to store loopback data    
        private ToolTip TTip = new ToolTip();                               //for tooltip

        private bool[] all_back = new bool[max];                            //stores if all data is received
        private bool[] xoffed = new bool[max];                               //xon can send, xoff cannot send
        private byte[] custom_str;
        private short[] wait_count = new short[max];                         //count for how many wait loops
        private int[] Read_Buffer = new int[max];                          //count for one set of data
        private int[] ReceiveCount = new int[max];                         //count for Rx
        private int[] SendCount = new int[max];                            //count for Tx
        private int[] ErrorCount = new int[max];
        private int[] LossCount = new int[max];
        private long[] ElaspedTime = new long[max];
        private double[] Latency = new double[max];
        private double[] DurationCount = new double[max];                     //for latency add up
        private string[] PortName = new string[max];
        private long[] skeyprocess = new long[max];                         //current key position for sending
        private long[] rkeyprocess = new long[max];                         //current key position for reading
        private int sendlength;
        private int startport;
        private int sendinterval;
        private int delayinterval;
        private long runtime;
        private double sendtotal;

        // Creates and initializes a new ArrayList for "COMArray_setting"
        private ArrayList listSetting = new ArrayList();

        public Form1()
        {
            this.SuspendLayout();
            InitializeComponent();
                                                            //adjust according to screen resolution
            Rectangle screensize = SystemInformation.VirtualScreen;
            if (screensize.Height < 900)
                this.Height = screensize.Height -380;
            if (screensize.Width < 1300)
                this.Width = screensize.Width -200;

            short i = 0, x = 5, y = 55;                       //initial position of the panel
            //max = Convert.ToInt16(txTotalPort.Text);

            MSComEscape = new CustomButton[max,9];
            SetFlow = new CustomButton[max, 4];
     
            sendlength = Convert.ToInt32(txSendLen.Text);
            startport = Convert.ToInt32(txStartPort.Text);
            sendinterval = Convert.ToInt32(txSendInterval.Text);
            delayinterval = Convert.ToInt32(txLoopBDelay.Text);
            sendtotal = (double)Convert.ToDouble(txSendTotal.Text) * 1024 * 1024;
            runtime = Convert.ToInt64(txDuration.Text) * 60 * 1000;

            TTip.SetToolTip(ckSkipErrCk, "Skip Error checking");
            TTip.SetToolTip(ckWaitnSend, "Wait for data to come back before sending again");
            TTip.SetToolTip(ckLoopback, "Loopback mode is in purple");
            TTip.SetToolTip(txSendInterval, "Resolution up to 3ms");

            for (i = 0; i < max; i++)               //draw panels
            {
                Panel[i] = new Panel();
                Panel[i].SuspendLayout();
                Panel[i].Left = x;
                Panel[i].Top = y;
                Panel[i].Width = 146;
                Panel[i].Height = 318;
                Panel[i].Tag = i;
                Panel[i].BackColor = Color.Transparent;
                Panel[i].Click += new EventHandler(Panel_Click);
                Panel[i].BorderStyle = BorderStyle.FixedSingle;
                x += 154;

                if (i == 7)                                         //switch to second raw
                {
                    y = 376;
                    x = 5;
                }
            }

            for (i = 0; i < max; i++)                               //draw each component in the Panel
            {
                Setting[i] = new TextBox();                         //build up Settings Textbox
                Setting[i].Size = new Size(80,25);
                Setting[i].Text = "9600,n,8,1";
                Setting[i].Location = new Point(3, 5);
                Setting[i].Tag = i;                                 //index of the component
                TTip.SetToolTip(Setting[i], "Baudrate,Parity(N-O-E-M-S),Databits,Stopbits");
                Panel[i].Controls.Add(Setting[i]);                  //add Settings on the Panel
                
                Port[i] = new ComboBox();
                Port[i].Size = new Size(50,25);
                Port[i].Location = new Point(88,5);
                Port[i].Text = (startport + i).ToString();
                Port[i].Tag = i;
                Port[i].TextChanged += new EventHandler(Port_TextChanged);

                for (short j = 0; j < 8; j++)
                    Port[i].Items.Add(i + j + startport);

                Panel[i].Controls.Add(Port[i]);
                
                Open[i] = new Button();
                Open[i].Text = "Open";
                Open[i].Size = new Size(69, 25); 
                Open[i].Location = new Point(3, 32);
                Open[i].Tag = i;
                Open[i].Click += new EventHandler(Open_Click);
                Panel[i].Controls.Add(Open[i]);

                Clear[i] = new Button();
                Clear[i].Text = "Clear";
                Clear[i].Size = Open[i].Size;
                Clear[i].Location = new Point(73, 32);
                Clear[i].Tag = i;
                Clear[i].Click += new EventHandler(Clear_Click);
                Panel[i].Controls.Add(Clear[i]);

                Send[i] = new Button();
                Send[i].Text = "Send";
                Send[i].Size = new Size(66, 25);
                Send[i].Location = new Point(3, 221);
                Send[i].Tag = i;
                Send[i].Enabled = false;
                Send[i].Click += new EventHandler(Send_Click);
                Panel[i].Controls.Add(Send[i]);

                Stop[i] = new Button();
                Stop[i].Text = "Stop";
                Stop[i].Size = Send[i].Size;
                Stop[i].Location = new Point(73, 221);
                Stop[i].Tag = i;
                Stop[i].Enabled = false;
                Stop[i].Click += new EventHandler(Stop_Click);
                Panel[i].Controls.Add(Stop[i]); 

                Tx[i] = new TextBox();
                Tx[i].Size = new Size(74, 23);
                Tx[i].Location = new Point(64, 79);
                Tx[i].Text = "0";
                Tx[i].Tag = i;
                Tx[i].Enabled = false;
                Tx[i].BackColor = Color.White;
                Panel[i].Controls.Add(Tx[i]);

                Rx[i] = new TextBox();
                Rx[i].Size = Tx[i].Size;
                Rx[i].Location = new Point(64, 104);
                Rx[i].Text = "0";
                Rx[i].Tag = i;
                Rx[i].Enabled = false;
                Rx[i].BackColor = Color.White;
                Panel[i].Controls.Add(Rx[i]);

                Loss[i] = new TextBox();
                Loss[i].Size = Rx[i].Size;
                Loss[i].Location = new Point(64, 129);
                Loss[i].Text = "0";
                Loss[i].Tag = i;
                Loss[i].Enabled = false;
                Loss[i].BackColor = Color.White;
                Panel[i].Controls.Add(Loss[i]);

                Error[i] = new TextBox();
                Error[i].Size = Rx[i].Size;
                Error[i].Location = new Point(64, 154);
                Error[i].Text = "0";
                Error[i].Tag = i;
                Error[i].Enabled = false;
                Error[i].BackColor = Color.White;
                Panel[i].Controls.Add(Error[i]);

                CurLatency[i] = new TextBox();
                CurLatency[i].Size = new Size(60,25);
                CurLatency[i].Location = new Point(6, 196);
                CurLatency[i].Text = "0";
                CurLatency[i].Tag = i;
                CurLatency[i].Enabled = false;
                CurLatency[i].BackColor = Color.White;
                Panel[i].Controls.Add(CurLatency[i]);

                AvgLatency[i] = new TextBox();
                AvgLatency[i].Size = CurLatency[i].Size;
                AvgLatency[i].Location = new Point(74, 196);
                AvgLatency[i].Text = "0";
                AvgLatency[i].Tag = i;
                AvgLatency[i].Enabled = false;
                AvgLatency[i].BackColor = Color.White;
                Panel[i].Controls.Add(AvgLatency[i]);

                MSCom[i] = new CustomMSCom();                                                        //create SerialPort array
                MSCom[i].DataReceived += new SerialDataReceivedEventHandler(MSCom_DataReceived);    //create handle when data is received
                MSCom[i].PinChanged += new SerialPinChangedEventHandler(MSCom_PinChanged);          //create handle when pin state changed
                MSCom[i].ErrorReceived += new SerialErrorReceivedEventHandler(MSCom_ErrorReceived);
                MSCom[i].Tag = i;

                Latencytick[i] = new Stopwatch();                           //create Stopwatch array for latency calculation

                Send_Timer[i] = new MicroTimer();           //create a timer array to control timing for sending
                Send_Timer[i].Enabled = false;
                Send_Timer[i].Tag = i;
                Send_Timer[i].Interval = sendinterval*1000;
                Send_Timer[i].MicroTimerElapsed += new MicroTimer.MicroTimerElapsedEventHandler(Send_Timer_Tick);    //handle when timer is triggered everytime

                Delay_Timer[i] = new MicroTimer();           //create a timer array to control timing for sending
                Delay_Timer[i].Enabled = false;
                Delay_Timer[i].Tag = i;
                //Delay_Timer[i].Interval = delayinterval;
                Delay_Timer[i].MicroTimerElapsed += new MicroTimer.MicroTimerElapsedEventHandler(Delay_Timer_Tick);    //handle when timer is triggered everytime

                ReceiveCount[i] = 0;
                SendCount[i] = 0;
                LossCount[i] = 0;
                ErrorCount[i] = 0;
                Latency[i] = 0;
                DurationCount[i] = 0;
                PortName[i] = Port[i].Text;
                Read_Buffer[i] = 0;
                skeyprocess[i] = 0;
                rkeyprocess[i] = 0;
                all_back[i] = true;
                loopback_buffer[i] = new ArrayList();

                lbTx[i] = new Label();
                lbTx[i].Location = new Point(10, 82);
                lbTx[i].Text = "Tx";
                lbTx[i].Size = new Size(25, 16);
                Panel[i].Controls.Add(lbTx[i]);

                lbRx[i] = new Label();
                lbRx[i].Location = new Point(10, 107);
                lbRx[i].Text = "Rx";
                lbRx[i].Size = lbTx[i].Size;
                Panel[i].Controls.Add(lbRx[i]);

                lbLoss[i] = new Label();
                lbLoss[i].Location = new Point(10, 132);
                lbLoss[i].Text = "Loss";
                lbLoss[i].Size = new Size(40, 16);
                Panel[i].Controls.Add(lbLoss[i]);

                lbError[i] = new Label();
                lbError[i].Location = new Point(10, 157);
                lbError[i].Text = "Error";
                lbError[i].Size = lbLoss[i].Size;
                Panel[i].Controls.Add(lbError[i]);

                lbLatency[i] = new Label();
                lbLatency[i].Location = new Point(6, 177);
                lbLatency[i].Text = "Latency";
                lbLatency[i].Size = new Size(60, 20);
                Panel[i].Controls.Add(lbLatency[i]);

                lbAvgLatency[i] = new Label();
                lbAvgLatency[i].Location = new Point(66, 177);
                lbAvgLatency[i].Text = "AvgLatency";
                lbAvgLatency[i].Size = new Size(80, 20);
                Panel[i].Controls.Add(lbAvgLatency[i]);

                CTS[i] = new Button();
                CTS[i].Location = new Point(3, 292);
                CTS[i].Text = "CTS";
                CTS[i].Size = new Size(66, 22);
                CTS[i].Enabled = false;
                Panel[i].Controls.Add(CTS[i]);

                DSR[i] = new Button();
                DSR[i].Location = new Point(73, 292);
                DSR[i].Text = "DSR";
                DSR[i].Size = CTS[i].Size;
                DSR[i].Enabled = false;
                TTip.SetToolTip(DSR[i], "DSR signal will get lost when using USB");
                Panel[i].Controls.Add(DSR[i]);

                for (short j = 0; j < 9; j++)
                {
                    MSComEscape[i, j] = new CustomButton();
                    MSComEscape[i, j].Location = new Point(j*16, 270);
                    MSComEscape[i, j].Text = Convert.ToString(j + 1);
                    MSComEscape[i, j].Size = new Size(17, 22);
                    MSComEscape[i, j].Tag = i;
                    MSComEscape[i, j].jTag = j;
                    MSComEscape[i, j].Enabled = false;
                    MSComEscape[i, j].Click += new EventHandler(MSComEscape_Click);

                    switch (j)
                    {
                        case 0: TTip.SetToolTip(MSComEscape[i, j], "SETXOFF"); break;
                        case 1: TTip.SetToolTip(MSComEscape[i, j], "SETXON"); break;
                        case 2: TTip.SetToolTip(MSComEscape[i, j], "SETRTS"); break;
                        case 3: TTip.SetToolTip(MSComEscape[i, j], "CLRRTS"); break;
                        case 4: TTip.SetToolTip(MSComEscape[i, j], "SETDTR"); break;
                        case 5: TTip.SetToolTip(MSComEscape[i, j], "CLRDTR"); break;
                        case 6: TTip.SetToolTip(MSComEscape[i, j], "RESETDEV"); break;
                        case 7: TTip.SetToolTip(MSComEscape[i, j], "SETBREAK"); break;
                        case 8: TTip.SetToolTip(MSComEscape[i, j], "CLRBREAK"); break;
                    }
                    Panel[i].Controls.Add(MSComEscape[i,j]);
                }

                for (short j = 0; j < 4; j++)
                {
                    SetFlow[i, j] = new CustomButton();
                    SetFlow[i, j].Location = new Point(3 + j * 34, 247);
                    //SetFlow[i, j].Text = Convert.ToString(j + 1);
                    SetFlow[i, j].Size = new Size(35, 22);
                    SetFlow[i, j].Tag = i;
                    SetFlow[i, j].jTag = j;
                    SetFlow[i, j].Enabled = false;
                    SetFlow[i, j].Click += new EventHandler(SetFlow_Click);

                    switch (j)
                    {
                        case 0: 
                            TTip.SetToolTip(SetFlow[i, j], "Xon");
                            SetFlow[i, j].Text = "Xn";
                            break;
                        case 1: 
                            TTip.SetToolTip(SetFlow[i, j], "Xoff");
                            SetFlow[i, j].Text = "Xf";
                            break;
                        case 2: 
                            TTip.SetToolTip(SetFlow[i, j], "RTS");
                            SetFlow[i, j].Text = "RT";
                            break;
                        case 3:
                            TTip.SetToolTip(SetFlow[i, j], "DTR");
                            SetFlow[i, j].Text = "DT";
                            break;
                    }
                    Panel[i].Controls.Add(SetFlow[i, j]);
                }

                NoFlowCon[i] = new RadioButton();
                NoFlowCon[i].Location = new Point(6, 59);
                NoFlowCon[i].Text = "N";
                NoFlowCon[i].Size = new Size(30, 17);
                NoFlowCon[i].Checked = true;
                NoFlowCon[i].Tag = i;
                NoFlowCon[i].CheckedChanged += new EventHandler(NoFlowCon_CheckedChanged);
                TTip.SetToolTip(NoFlowCon[i], "No Flow Control");
                Panel[i].Controls.Add(NoFlowCon[i]);

                XonXoff[i] = new RadioButton();
                XonXoff[i].Location = new Point(40, 59);
                XonXoff[i].Text = "X";
                XonXoff[i].Size = new Size(30, 17);
                XonXoff[i].Tag = i;
                TTip.SetToolTip(XonXoff[i], "Xon/Xoff");
                XonXoff[i].CheckedChanged += new EventHandler(XonXoff_CheckedChanged);
                Panel[i].Controls.Add(XonXoff[i]);

                RTSCTS[i] = new RadioButton();
                RTSCTS[i].Location = new Point(75, 59);
                RTSCTS[i].Text = "R";
                RTSCTS[i].Size = new Size(30, 17);
                RTSCTS[i].Tag = i;
                RTSCTS[i].CheckedChanged += new EventHandler(RTSCTS_CheckedChanged);
                TTip.SetToolTip(RTSCTS[i], "RTS/CTS");
                Panel[i].Controls.Add(RTSCTS[i]);
            }
            this.Controls.AddRange(Panel);                          //add all panels onto the GUI

        }
        #endregion

        private void Panel_Click(object sender, EventArgs e)        //triggers when clicks on the blank area of the panel
        {
            short n = Convert.ToInt16(((Panel)sender).Tag);         //find which panel it is

            if (!Send_Timer[n].Enabled)                             //change color of the panel when not sending
            {                                                       //color switches between Transparent, LightGreen, and Violet
                if (Panel[n].BackColor == Color.Transparent)        //switch from Transparent to LightGreen
                    Panel[n].BackColor = Color.LightGreen;
                else if (Panel[n].BackColor == Color.LightGreen)    //switch from LightGreen to Violet
                {
                    Panel[n].BackColor = Color.Violet;
                    Send[n].Enabled = false;
                }
                else
                {                                                   //siwtch from Violet to Tranparent
                    Panel[n].BackColor = Color.Transparent;

                    if (MSCom[n].IsOpen)                            //close COM when switch to transparent
                    {
                        MSCom[n].Close();
                        Send[n].Enabled = false;
                        Open[n].Text = "Open";
                    }
                }
                                                                    //resets CTS and DSR's color to match the panel
                if (CTS[n].BackColor != Color.Red || Panel[n].BackColor == Color.Transparent)
                    CTS[n].BackColor = Panel[n].BackColor;              
                if (DSR[n].BackColor != Color.Red || Panel[n].BackColor == Color.Transparent)
                    DSR[n].BackColor = Panel[n].BackColor;
            }
        }

        private void Send_Click(object sender, EventArgs e)         //when send button is clicked
        {
            short n = Convert.ToInt16(((Button)sender).Tag);

            Send_Timer[n].Enabled = true;                           //call send timer to start looping
            Send[n].Enabled = false;
            Stop[n].Enabled = true;
            Open[n].Enabled = false;
                                                                    //record start time in debug window
            DebugBox.Items.Add(DateTime.Now + ": COM" + Port[n].Text + " Started");
        }

        private void Stop_Click(object sender, EventArgs e)         //disable send timer when stop button is clicked
        {
            short n = Convert.ToInt16(((Button)sender).Tag);

            Send_Timer[n].Enabled = false;
            Stop[n].Enabled = false;

            if (Panel[n].BackColor == Color.LightGreen)
                Send[n].Enabled = true;
            Open[n].Enabled = true;
        }

        private void Clear_Click(object sender, EventArgs e)        //clears GUI and variables
        {
            short n = Convert.ToInt16(((Button)sender).Tag);

            Tx[n].Text = "0";
            Rx[n].Text = "0";
            Loss[n].Text = "0";
            Error[n].Text = "0";
            CurLatency[n].Text = "0";
            AvgLatency[n].Text = "0";
            TAvgLatency.Text = "0";
            //txWaitCount.Text = "0";

            SendCount[n] = 0;
            Read_Buffer[n] = 0;
            ReceiveCount[n] = 0;
            ErrorCount[n] = 0;
            DurationCount[n] = 0;
            ElaspedTime[n] = 0;
            LossCount[n] = 0;
            loopback_buffer[n].Clear();
            
            skeyprocess[n] = 0;
            rkeyprocess[n] = 0;
            Latency[n] = 0;
            Latencytick[n].Reset();


            if (MSCom[n].IsOpen)                                    //clears In/Out buffers in COM
            {
                MSCom[n].DiscardInBuffer();
                MSCom[n].DiscardOutBuffer();
            }

            DebugBox.Items.Clear();
        }

        private void Open_Click(object sender, EventArgs e)         //Opens or Close COM port when clicked
        {
            short n = Convert.ToInt16(((Button)sender).Tag);
            string[] a = SerialPort.GetPortNames();
                                                                    //if COM is already open or closed unexpectly
            if ((MSCom[n].IsOpen) || (!MSCom[n].IsOpen && Open[n].Text == "Close"))  
            {
                if (MSCom[n].IsOpen)
                    MSCom[n].Close();                                //close it                                  
                
                Send_Timer[n].Enabled = false;
                Send[n].Enabled = false;
                Open[n].Enabled = true;
                Open[n].Text = "Open";

                for (short j = 0; j < 4; j++)
                    SetFlow[n, j].Enabled = false;

                for (short j = 0; j<9; j++)
                    MSComEscape[n,j].Enabled = false;
                
                //bSETXON[n].Enabled = false;
                CTS[n].BackColor = Panel[n].BackColor;              //jun15
                DSR[n].BackColor = Panel[n].BackColor;              
            }
            else
            {                                                       //otherwise open it and set settings
                try
                {
                    string[] stemp;
                    stemp = Setting[n].Text.Split(',');             //split settings with ','
                    //stemp = SerialPort.GetPortNames();
                    MSCom[n].PortName = "COM" + (Port[n].Text);
                    MSCom[n].WriteTimeout = sendinterval;
                    MSCom[n].BaudRate = Convert.ToInt32(stemp[0]);
                    MSCom[n].WriteBufferSize = 100000;
                    MSCom[n].ReadBufferSize = 100000;                //sets the read buffer size  
                    //MSCom[n].ReceivedBytesThreshold = 2;          //controls how many bytes to store in buffer before DataReceived will be triggered
                    
                    switch (stemp[1].ToUpper())
                    {
                        case "N":
                            MSCom[n].Parity = Parity.None; break;
                        case "O":
                            MSCom[n].Parity = Parity.Odd; break;
                        case "E":
                            MSCom[n].Parity = Parity.Even; break;
                        case "M":
                            MSCom[n].Parity = Parity.Mark; break;
                        case "S":
                            MSCom[n].Parity = Parity.Space; break;
                    }

                    MSCom[n].DataBits = Convert.ToInt16(stemp[2]);

                    switch (stemp[3])
                    {
                        case "1":
                            MSCom[n].StopBits = StopBits.One; break;
                        case "O":
                            MSCom[n].StopBits = StopBits.None; break;
                        case "2":
                            MSCom[n].StopBits = StopBits.Two; break;
                        case "1.5":
                            MSCom[n].StopBits = StopBits.OnePointFive; break;
                    }

                    MSCom[n].Open();

                    if (Panel[n].BackColor == Color.Transparent)    //Open with differnt panel color according to send or loopback
                    {
                        if (ckLoopback.Checked)
                            Panel[n].BackColor = Color.Violet;
                        else
                            Panel[n].BackColor = Color.LightGreen;
                    }

                    if (Panel[n].BackColor == Color.LightGreen)
                        Send[n].Enabled = true;
                    
                    Open[n].Text = "Close";

                    for (short j = 0; j < 4; j++)
                        SetFlow[n, j].Enabled = true;

                    for (short j = 0; j < 9; j++)
                        MSComEscape[n, j].Enabled = true;

                    if (MSCom[n].CtsHolding)                        //show the state of the pin (CTS and DSR)
                        CTS[n].BackColor = Color.Red;
                    else
                        CTS[n].BackColor = Panel[n].BackColor;

                    if (MSCom[n].DsrHolding)
                        DSR[n].BackColor = Color.Red;
                    else
                        DSR[n].BackColor = Panel[n].BackColor;

                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                /*catch (UnauthorizedAccessException er)
                catch (IOException er)
                catch (FormatException er)*/
            }
        }

        private void MSComEscape_Click(object sender, EventArgs e)
        {
            short n = Convert.ToInt16(((CustomButton)sender).Tag);
            uint j = Convert.ToUInt32(((CustomButton)sender).jTag);
            MSCom[n].SETXONXOFF(j + 1);
        }

        private void SetFlow_Click(object sender, EventArgs e)
        {
            short n = Convert.ToInt16(((CustomButton)sender).Tag);
            uint j = Convert.ToUInt32(((CustomButton)sender).jTag);
            byte[] t = new byte[2];

            switch (j)
            {
                case 0:
                    t[0] = byte.Parse(txXon.Text, System.Globalization.NumberStyles.HexNumber);
                    MSCom[n].Write(t, 0, 1);
                    break;
                case 1:
                    t[0] = byte.Parse(txXoff.Text, System.Globalization.NumberStyles.HexNumber);
                    MSCom[n].Write(t, 0, 1);
                    break;
                case 2:
                    if (MSCom[n].RtsEnable)
                        MSCom[n].RtsEnable = false;
                    else
                        MSCom[n].RtsEnable = true;
                    break;
                case 3:
                    if (MSCom[n].DtrEnable)
                        MSCom[n].DtrEnable = false;
                    else
                        MSCom[n].DtrEnable = true;
                    break;
            }
        }

        private void Port_TextChanged(object sender, EventArgs e)
        {
            short n = Convert.ToInt16(((ComboBox)sender).Tag);
            PortName[n] = Port[n].Text;
        }

        private void Send_Timer_Tick(object sender, MicroTimerEventArgs e)    //Timer for sending data
        {
            short n = Convert.ToInt16(((MicroTimer)sender).Tag);
            int k = 0;

            if (!MSCom[n].CtsHolding && MSCom[n].Handshake == Handshake.RequestToSend)
                return;

            //if (SendCount[n] > 100)
            //{
            //    MSCom[n].SETXONXOFF(1);
            //    OnDisableTimer(n);
            //    return;
            //}

            if (!MSCom[n].IsOpen)                                   //VirtualCOM will close port sometimes
                return;
            //MSCom[n].BaseStream.
            //if (xoffed[n] && MSCom[n].Handshake == Handshake.XOnXOff)
            //    return;

            //if (MSCom[n].BytesToWrite > 0 || MSCom[n].BytesToRead>0)
            //    return;

            if (ckStopOnErr.Checked && ErrorCount[n] > 0)           //if Stop on Error is enabled and has error, stop
            {
                Send_Timer[n].Enabled = false;
                Stop[n].Enabled = false;
                Send[n].Enabled = true;
                Open[n].Enabled = true;
            }
            else
            {
                if (ckWaitnSend.Checked && !all_back[n])            //if wait_n_send mode and not all data is back, wait
                {
                    wait_count[n]++;

                    if (wait_count[n] > 20)                         //resend if wait too long
                        all_back[n] = true;

                    return;
                }
                else
                {
                    try
                    {
                        byte[] send_buffer = new byte[sendlength];      //create a buffer send array
                        //send_buffer = new byte[sendlength];      //create a buffer send array

                        wait_count[n] = 0;

                        if (ckCustomStr.Checked)
                        {
                            //int discarded;
                            //string custom_str = "1B42FFFFFFFFFFFFFFFFFFFFFFFF";

                            send_buffer = custom_str;
                            sendlength = send_buffer.Length;
                            txSendLen.Text = Convert.ToString(sendlength);
                        }
                        else
                        {
                            for (k = 0; k < sendlength; k++)            //fill in items in send array one by one
                            {
                                byte btemp = 0;

                                switch (MSCom[n].DataBits)
                                {
                                    case 8:
                                        btemp = (byte)skeyprocess[n]; break;
                                    case 7:
                                        btemp = (byte)(skeyprocess[n] % 128); break;
                                    case 6:
                                        btemp = (byte)(skeyprocess[n] % 64); break;
                                    case 5:
                                        btemp = (byte)(skeyprocess[n] % 32); break;
                                }

                                if (XonXoff[n].Checked)
                                {
                                    switch (btemp)                          //skip 0x11-14 in XonXoff mode
                                    {
                                        case 17:
                                            btemp += 4;
                                            skeyprocess[n] += 4;
                                            break;
                                        case 18:
                                            btemp += 3;
                                            skeyprocess[n] += 3;
                                            break;
                                        case 19:
                                            btemp += 2;
                                            skeyprocess[n] += 2;
                                            break;
                                        case 20:
                                            btemp++;
                                            skeyprocess[n]++;
                                            break;
                                    }
                                    //if (btemp >= 17 && btemp <= 19)
                                    //{
                                    //    btemp++;
                                    //    skeyprocess[n]++;
                                    //}
                                }

                                send_buffer[k] = btemp;
                                skeyprocess[n]++;
                            }
                        }
                        //MSCom[n].Write("Section 1390 Received. (76/4542) Section 1391 Received. (77/4542)Section 1390 Received. (76/4542) Section 1391 Received. (77/4542)");

                        //for (int j = 0; j < Convert.ToInt32(txWaitCount.Text); j++)
                        //{
                        //    //MSCom[n].DiscardOutBuffer();
                        //    MSCom[n].DiscardInBuffer();
                        //}
                        if (ckHalf.Checked)
                        {
                            MSCom[n].DiscardOutBuffer();
                            MSCom[n].DiscardInBuffer();
                        }
                        MSCom[n].Write(send_buffer, 0, sendlength); //writes to COM

                        if (ckWaitnSend.Checked)
                            Latencytick[n].Start();                     //starts Stopwatch to calculate latency

                        all_back[n] = false;
                        SendCount[n] += sendlength;
                        //SendCount[n] += "Section 1390 Received. (76/4542) Section 1391 Received. (77/4542)Section 1390 Received. (76/4542) Section 1391 Received. (77/4542)".Length;

                    }
                    catch (Exception)
                    {
                        OnDisableTimer(n);

                        //if (MSCom[n].IsOpen)
                        //{
                        //    MSCom[n].Close();
                        //    MSCom[n].Open();
                        //}

                        //Stop[n].Enabled = false;

                        //MessageBox.Show(er.Message);

                        //    Send_Timer[n].Enabled = false;
                        //    Stop[n].Enabled = false;

                        //    if (Panel[n].BackColor == Color.LightGreen)
                        //        Send[n].Enabled = true;
                        //    Open[n].Enabled = true;
                        //    MessageBox.Show(er.Message);
                        //}

                        //if (SendCount[n] >= sendtotal || ElaspedTime[n]>=runtime)    //if all bytes sent or passed duration, stop
                        //    Send_Timer[n].Enabled = false;
                    }
                }
            }
            ElaspedTime[n] += (long)Send_Timer[n].Interval/1000;               //count duration
        }

        private void Delay_Timer_Tick(object sender, MicroTimerEventArgs e)    //Timer for sending data
        {
            short n = Convert.ToInt16(((MicroTimer)sender).Tag);

            try
            {
                byte[] arr = (byte[])loopback_buffer[n].ToArray(typeof(byte));
                MSCom[n].Write(arr, 0, loopback_buffer[n].Count);
                SendCount[n] += loopback_buffer[n].Count;
                Read_Buffer[n] = 0;                     //clear bufffer after send
                loopback_buffer[n].Clear();
                Delay_Timer[n].Enabled = false;
            }
            catch { }
        }

        private void MSCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            short n = Convert.ToInt16(((CustomMSCom)sender).Tag);
            int len_in_buffer = 0;

            byte[] readtemp = new byte[MSCom[n].BytesToRead];       //create read buffer array according to the bytes stored in COM
            len_in_buffer = readtemp.Length;                        //BytesToRead changes very fast, should use only once
            MSCom[n].Read(readtemp, 0, len_in_buffer);

            ReceiveCount[n] += len_in_buffer;
            Read_Buffer[n] += len_in_buffer;

            //if (ReceiveCount[n] >= 10000)
            //{
            //    byte[] a = new byte[1];
            //    a[0] = 19;
            //    MSCom[n].Write(a, 0, 1);        //software
            //    //MSCom[n].RtsEnable = false;     //hardware
            //    return;
            //}

            //string uu = Encoding.ASCII.GetString(readtemp);
            //Debug.WriteLine(Convert.ToString(len_in_buffer));
            //Debug.WriteLine(uu);

            try
            {                                                        //if all data in one send is received
                if (Read_Buffer[n] >= sendlength && Panel[n].BackColor == Color.LightGreen && ckWaitnSend.Checked)
                {                                                   //calculate Latency
                    Latency[n] = Latencytick[n].ElapsedTicks * 1000D / Stopwatch.Frequency;
                    DurationCount[n] += Latency[n];
                    Latencytick[n].Reset();
                    all_back[n] = true;
                }

                if (!ckSkipErrCk.Checked)                           //do error checking if not skipping
                {
                    //bool hasError = false;
                    string debugitem = "";

                    for (int j = 0; j < len_in_buffer; j++)         //loopthrough read array
                    {                                               //if incoming byte does not equal to the index stored
                        byte curIndex = 0;

                        if (ckCustomStr.Checked)
                            curIndex = custom_str[rkeyprocess[n] % custom_str.Length];
                        else
                        {
                            switch (MSCom[n].DataBits)
                            {
                                case 8:
                                    curIndex = (byte)rkeyprocess[n]; break;
                                case 7:
                                    curIndex = (byte)(rkeyprocess[n] % 128); break;
                                case 6:
                                    curIndex = (byte)(rkeyprocess[n] % 64); break;
                                case 5:
                                    curIndex = (byte)(rkeyprocess[n] % 32); break;
                            }
                        }

                        if (XonXoff[n].Checked)                     //skip characters that Xonoff is using
                        {
                            //if (curIndex == 17 || curIndex == 19)
                            //{
                            //    curIndex++;
                            //    rkeyprocess[n]++;
                            //}
                            switch (curIndex)
                            {
                                case 17:
                                    curIndex += 4;
                                    rkeyprocess[n] += 4;
                                    break;
                                case 18:
                                    curIndex += 3;
                                    rkeyprocess[n] += 3;
                                    break;
                                case 19:
                                    curIndex += 2;
                                    rkeyprocess[n] += 2;
                                    break;
                                case 20:
                                    curIndex++;
                                    rkeyprocess[n]++;
                                    break;
                            }
                        }

                        if (readtemp[j] != curIndex)
                        {
                            //hasError = true;                        //found error

                            //string a = ((byte)rkeyprocess[n]).ToString("X");
                            //string b = readtemp[j].ToString("X");

                            //print in debug list
                            string a = String.Format("{0:X}", rkeyprocess[n]);
                            string b = String.Format("{0:X}", readtemp[j]);

                            if (a.Length == 1)
                                a = "0" + a;
                            if (b.Length == 1)
                                b = "0" + b;

                            debugitem += DateTime.Now + ": " + a + " -> " + b + " " + (j + 1) + " " + len_in_buffer + "\t";
                            UpdateDebugBox(debugitem);
                            debugitem = "";

                            ErrorCount[n]++;
                            rkeyprocess[n] = (int)readtemp[j];      //reindex according to the incoming byte
                        }
                        rkeyprocess[n]++;
                    }

                    if (debugitem != "")                            //empty debug buffer
                    {
                        UpdateDebugBox(debugitem);
                        debugitem = "";
                    }

                    //if (hasError == true)      //print all data if has error
                    //{
                    //    foreach (byte b in readtemp)
                    //    {
                    //        String.Format("{0:X}", b);
                    //        debugitem += b.ToString() + " ";

                    //        if (debugitem.Length > 120)
                    //        {
                    //            UpdateDebugBox(debugitem);
                    //            debugitem = "";
                    //        }
                    //    }
                    //}

                    if (debugitem != "")                            //empty debug buffer
                        UpdateDebugBox(debugitem);
                }

                if (Panel[n].BackColor == Color.Violet)             //for loopback
                {
                    if (ckWaitnSend.Checked)                        //if needs to wait for data
                    {
                        foreach (byte b in readtemp)                //add data in receive buffer to loopback buffer
                            loopback_buffer[n].Add(b);

                        if (Read_Buffer[n] >= sendlength)           //if all received, loopback
                        {                                           //convert ArrayList to regular array
                            if (delayinterval <= 0)
                            {
                                try
                                {
                                    byte[] arr = (byte[])loopback_buffer[n].ToArray(typeof(byte));
                                    MSCom[n].Write(arr, 0, loopback_buffer[n].Count);
                                    SendCount[n] += loopback_buffer[n].Count;
                                    Read_Buffer[n] = 0;             //clear buffer after send
                                    loopback_buffer[n].Clear();
                                }
                                catch { }
                            }
                            else
                                Delay_Timer[n].Enabled = true;

                        }
                    }
                    else                                            //loopback directly if no need to wait
                    {
                        MSCom[n].Write(readtemp, 0, len_in_buffer);
                        SendCount[n] += len_in_buffer;
                    }
                    //Debug.WriteLine(len_in_buffer);
                }
                else
                {
                    if (Read_Buffer[n] >= sendlength)               //if all recevied, clear read buffer of the sender
                        Read_Buffer[n] = 0;
                }
            }
            catch (Exception er) //FormatException and TimeoutException
            {
                MessageBox.Show(er.Message);
            }
               
        }

        private void MSCom_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (e.EventType == SerialPinChange.Break)
                return;

            short n = Convert.ToInt16(((CustomMSCom)sender).Tag);
           
            if (MSCom[n].IsOpen)
            {
                if (MSCom[n].DsrHolding)                //show the state of the pin
                    DSR[n].BackColor = Color.Red;
                else
                    DSR[n].BackColor = Panel[n].BackColor;

                if (MSCom[n].CtsHolding)
                    CTS[n].BackColor = Color.Red;
                else
                    CTS[n].BackColor = Panel[n].BackColor;
            }
            //if (MSCom[n].CDHolding)
            //    DCD[n].BackColor = Color.Red;
            //else
            //    DCD[n].BackColor = Panel[n].BackColor;
        }

        private void MSCom_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            short n = Convert.ToInt16(((CustomMSCom)sender).Tag);

            if (e.EventType == SerialError.Frame || e.EventType == SerialError.RXParity)
                return;
            if (MSCom[n].IsOpen)                                //close error COM
                MSCom[n].Close();                                     
            OnDisableTimer(n);
            //MessageBox.Show("COM #" + (n+1).ToString() + " Closed Due to Error");
        }
        
        private void GUITimer_Tick(object sender, EventArgs e)   //this timer updates the GUI
        {
            short port_on = 0;
            double iLatency = 0, TLatency = 0;

            for (short i = 0; i < max; i++)
            {
                if (Panel[i].BackColor == Color.LightGreen || Panel[i].BackColor == Color.Violet)
                {
                    if (Panel[i].BackColor == Color.LightGreen)
                    {
                        port_on++;
                        LossCount[i] = SendCount[i] - ReceiveCount[i];
                    }

                    Tx[i].Text = SendCount[i].ToString();
                    Loss[i].Text = LossCount[i].ToString();
                    Error[i].Text = ErrorCount[i].ToString();
                    Rx[i].Text = ReceiveCount[i].ToString();
                    CurLatency[i].Text = Latency[i].ToString();

                    if (SendCount[i] > 0 && ReceiveCount[i] > 0 && Panel[i].BackColor == Color.LightGreen)
                    {
                        iLatency = DurationCount[i] / (ReceiveCount[i] / sendlength);
                        TLatency += iLatency;
                        AvgLatency[i].Text = iLatency.ToString();
                    }
                }
            }

            if (port_on > 0)
                TAvgLatency.Text = (TLatency / port_on).ToString();
        }

        private void butOpenSelect_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Please make sure all ports are not sending \n and press \"Clear All\" after open", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
                for (short i = 0; i < max; i++)
                {
                    if (Panel[i].BackColor == Color.LightGreen || Panel[i].BackColor == Color.Violet)
                    {
                        ((Button)sender).Tag = i;
                        Open_Click(sender, e);
                    }
                }
            //}
        }

        private void butSendSelect_Click(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
            {
                if (Panel[i].BackColor == Color.LightGreen && MSCom[i].IsOpen && !Send_Timer[i].Enabled)
                {
                    ((Button)sender).Tag = i;
                    Send_Click(sender, e);
                }
            }
        }

        private void butClearAll_Click(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
            {
                ((Button)sender).Tag = i;
                Clear_Click(sender, e);
            }
        }

        private void butStopSelect_Click(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
            {
                ((Button)sender).Tag = i;
                Stop_Click(sender, e);
            }
        }

        private void butDebug_Click(object sender, EventArgs e)
        {
            if (DebugBox.Visible)
                DebugBox.Visible = false;
            else
                DebugBox.Visible = true;
        }

        private void ckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAll.Checked)
            {
                for (short i = 0; i < max; i++)
                {
                    if (ckLoopback.Checked)
                        Panel[i].BackColor = Color.Violet;
                    else
                        Panel[i].BackColor = Color.LightGreen;
                }
            }
            else
                for (short i = 0; i < max; i++)
                    if (!MSCom[i].IsOpen)
                        Panel[i].BackColor = Color.Transparent;
        }

        private void ckOdd_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOdd.Checked)
            {
                for (short i = 0; i < max; i++)
                {
                    if (ckLoopback.Checked)
                        Panel[i].BackColor = Color.Violet;
                    else
                        Panel[i].BackColor = Color.LightGreen;

                    i++;
                }
            }
            else
            {
                for (short i = 0; i < max; i++)
                {
                    if (!MSCom[i].IsOpen)
                        Panel[i].BackColor = Color.Transparent;
                        
                    i++;
                }
            }
        }

        private void ckEven_CheckedChanged(object sender, EventArgs e)
        {
            if (ckEven.Checked)
            {
                for (short i = 1; i < max; i++)
                {
                    if (ckLoopback.Checked)
                        Panel[i].BackColor = Color.Violet;
                    else
                        Panel[i].BackColor = Color.LightGreen;
                    i++;
                }
            }
            else
            {
                for (short i = 1; i < max; i++)
                {
                    if (!MSCom[i].IsOpen)
                        Panel[i].BackColor = Color.Transparent;
                    i++;
                }
            }
        }

        private void ckWaitnSend_CheckedChanged(object sender, EventArgs e)
        {
            if (ckWaitnSend.Checked)                        //cannot do error checking if send too fast
            {                                               //so force disable
                ckSkipErrCk.Enabled = true;
                ckSkipErrCk.Checked = false;
            }
            else
            {
                ckSkipErrCk.Enabled = false;
                ckSkipErrCk.Checked = true;
            }
        }

        private void OnEnableTimer(short n)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                Send_Timer[n].Enabled = true;
            }, new object[] { });
        }

        private void OnDisableTimer(short n)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                //Panel[n].BackColor = Color.Transparent;

                if (MSCom[n].IsOpen)                            //close COM when switch to transparent
                {
                    //MSCom[n].Close();
                    //Send[n].Enabled = false;
                    //Open[n].Text = "Open";
                    Send_Timer[n].Enabled = false;
                    Stop[n].Enabled = false;

                    if (Panel[n].BackColor == Color.LightGreen)
                        Send[n].Enabled = true;
                    Open[n].Enabled = true;
                }
                                                                //resets CTS and DSR's color to match the panel
                //if (CTS[n].BackColor != Color.Red || Panel[n].BackColor == Color.Transparent)
                //    CTS[n].BackColor = Panel[n].BackColor;              
                //if (DSR[n].BackColor != Color.Red || Panel[n].BackColor == Color.Transparent)
                //    DSR[n].BackColor = Panel[n].BackColor;

            }, new object[] { });
        }

        private void UpdateDebugBox(string text)
        {
            if (this.DebugBox.InvokeRequired)
                this.DebugBox.BeginInvoke(new MethodInvoker(delegate() { UpdateDebugBox(text); }));
            else
            {
                DebugBox.Items.Add(text);
                DebugBox.SelectedIndex = DebugBox.Items.Count - 1;  //autoscroll
            }
        }

        private void txSettingAll_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (short i = 0; i < max; i++)
                    Setting[i].Text = txSettingAll.Text;
            }
            catch (FormatException)
            { }
        }

        private void txSendLen_TextChanged(object sender, EventArgs e)
        {
            try
            { sendlength = Convert.ToInt32(txSendLen.Text); }
            catch (FormatException)
            { }
        }

        private void txSendTotal_TextChanged(object sender, EventArgs e)
        {
            try
            { sendtotal = Convert.ToDouble(txSendTotal.Text) * 1024 * 1024; }
            catch (FormatException)
            { }
        }

        private void txTotalPort_TextChanged(object sender, EventArgs e)        //sets how many ports to work on
        {
            //try
            //{
            //    short n = Convert.ToInt16(txTotalPort.Text);

            //    if (n > 16)
            //    {
            //        txTotalPort.Undo();
            //        throw new IndexOutOfRangeException("Can't set larger than 16");
            //    }
            //    else
            //        max = Convert.ToInt16(txTotalPort.Text);
            //}
            //catch (IndexOutOfRangeException er)
            //{
            //    MessageBox.Show(er.Message);
            //}
            //catch (FormatException)
            //{ }
        }

        private void txStartPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
                startport = Convert.ToInt32(txStartPort.Text);

                for (short i = 0; i < max; i++)
                {
                    Port[i].Text = (startport + i).ToString();
                    Port[i].Items.Clear();
                }
            }
            catch (FormatException)
            {
                txStartPort.Undo();
            }
        }

        private void txStartPort_Leave(object sender, EventArgs e)
        {
            //for (short i = 0; i < max; i++)                           //nested loop cause GUI to lag, can fix by mutithread
            //    for (short j = 0; j < 8; j++)
            //        txPort[i].Items.Add(startport + i + j);
        }

        private void txSendInterval_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (short i = 0; i < max; i++)
                {
                    sendinterval = Convert.ToInt32(txSendInterval.Text);
                    //MSCom[i].WriteTimeout = sendinterval;
                    Send_Timer[i].Interval = sendinterval*1000;
                }
            }
            catch (Exception)
            { }
        }

        private void rbN_CheckedChanged(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
                NoFlowCon[i].Checked = rbN.Checked;
        }

        private void rbRTSCTS_CheckedChanged(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
                RTSCTS[i].Checked = rbRTSCTS.Checked;
        }

        private void rbXonXoff_CheckedChanged(object sender, EventArgs e)
        {
            for (short i = 0; i < max; i++)
                XonXoff[i].Checked = rbXonXoff.Checked;
        }

        private void NoFlowCon_CheckedChanged(object sender, EventArgs e)
        {
            short n = Convert.ToInt16(((RadioButton)sender).Tag);

            if (NoFlowCon[n].Checked)
            {
                MSCom[n].Handshake = Handshake.None;
                //RTS[n].Enabled = true;
                SetFlow[n, 2].Enabled = true; 
            }
        }

        private void XonXoff_CheckedChanged(object sender, EventArgs e) //set to Xon Xoff mode
        {
            short n = Convert.ToInt16(((RadioButton)sender).Tag);

            if (XonXoff[n].Checked)
            {
                MSCom[n].Handshake = Handshake.XOnXOff;
                //RTS[n].Enabled = true;
            }
        }

        private void RTSCTS_CheckedChanged(object sender, EventArgs e)  //set to RTS/CTS mode
        {
            short n = Convert.ToInt16(((RadioButton)sender).Tag);

            if (RTSCTS[n].Checked)
            {
                MSCom[n].Handshake = Handshake.RequestToSend;
                SetFlow[n, 2].Enabled = false; 
            }
        }

        private void txDuration_TextChanged(object sender, EventArgs e)
        {
            try
            { runtime = Convert.ToInt64(txDuration.Text) * 60 * 1000; }
            catch (FormatException)
            { }
        }

        private void ckCustomStr_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckCustomStr.Checked)                       //cannot do error checking if custom string
            {                                               //so force disable
                //ckSkipErrCk.Enabled = true;
                //ckSkipErrCk.Checked = false;
            }
            else
            {
                //using (StreamReader reader = new StreamReader("custom.txt"))
                //{
                //    custom_str = reader.ReadToEnd();
                //}
                string strtmp;
                int discarded;

                strtmp = Interaction.InputBox("Custom Send String", "Please enter the string in hex", "FF");

                if (strtmp == "")
                {
                    //ckSkipErrCk.Enabled = true;
                    //ckSkipErrCk.Checked = false;
                    ckCustomStr.Checked = false;
                }
                else
                {
                    //ckSkipErrCk.Enabled = false;
                    //ckSkipErrCk.Checked = true;
                }

                custom_str = HexEncoding.GetBytes(strtmp, out discarded);
                txSendLen.Text = Convert.ToString(custom_str.Length);
            }
        }

        private void ckHalf_CheckedChanged(object sender, EventArgs e)
        {
            //max = 7;
            ////Form1.siz = new System.Drawing.Size(100, 100);
            //this.Size = new System.Drawing.Size(1090, 390);
        }

        private void txLoopBDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (short i = 0; i < max; i++)
                {
                    delayinterval = Convert.ToInt32(txLoopBDelay.Text);
                    //MSCom[i].WriteTimeout = sendinterval;
                    Delay_Timer[i].Interval = delayinterval*1000;
                }
            }
            catch (Exception)
            { }
        }

        private void txWaitCount_TextChanged(object sender, EventArgs e)
        {
            //short n = Convert.ToInt16(((CustomMSCom)sender).Tag);

            MSCom[0].WriteTimeout = Convert.ToInt32(txWaitCount.Text);
            MSCom[0].ReadTimeout = Convert.ToInt32(txWaitCount.Text);
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            short i;
            for (i = 0; i < max; i++)
            {
                Send_Timer[i].Enabled = false;
                MSCom[i].Dispose();
            }
            GUITimer.Enabled = false;

            Thread.Sleep(1000);
            for (i = 0; i < max; i++)
            {
                if (Convert.ToUInt32(Loss[i].Text) > 0 || Convert.ToUInt32(Error[i].Text) > 0)
                {
                    try
                    {
                        File.Create("debug_dog");
                        break;
                    }
                    catch (IOException ex)
                    {
                        DebugBox.Items.Add(DateTime.Now + ": " + ex.Message);
                        break;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        DebugBox.Items.Add(DateTime.Now + ": " + ex.Message);
                        break;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String line;
            int i = 0;
            if (File.Exists("setting_dog.txt"))
            {
                using (StreamReader sr = new StreamReader("setting_dog.txt", Encoding.ASCII))
                {
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        listSetting.Add(line);
                        line = sr.ReadLine();
                        i++;
                    }
                }
                txSendLen.Text = listSetting[0].ToString();
                txSendInterval.Text = listSetting[1].ToString();
                txStartPort.Text = listSetting[2].ToString();
                txSettingAll.Text = listSetting[3].ToString();
                for (i = 4; i < listSetting.Count; i++)
                {
                    if (listSetting[i].ToString().ToUpper() == "G")
                    {
                        Panel[i - 4].BackColor = Color.LightGreen;
                    }
                    else if (listSetting[i].ToString().ToUpper() == "R")
                    {
                        Panel[i - 4].BackColor = Color.Violet;
                    }
                    else
                    {
                        Panel[i - 4].BackColor = Color.Transparent;
                    }
                }
            }

            if (File.Exists("debug_dog"))
            {
                File.Delete("debug_dog");            
            }

            if (File.Exists("Auto_Test_dog"))
            {
                butOpenSelect_Click(butOpenSelect, null);
                butSendSelect_Click(butSendSelect, null);
            }
        }
    }

    internal static class SerialPortExtensions
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        
        public static void SETXONXOFF(this SerialPort port, uint mode)
        {
            if (port == null)
                throw new NullReferenceException();
            if (port.BaseStream == null)
                throw new InvalidOperationException("Cannot change X chars until after the port has been opened.");

            try
            {
                // Get the base stream and its type which is System.IO.Ports.SerialStream
                object baseStream = port.BaseStream;
                Type baseStreamType = baseStream.GetType();

                // Get the Win32 file handle for the port
                SafeFileHandle portFileHandle = (SafeFileHandle)baseStreamType.GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(baseStream);

                // Get the value of the private DCB field (a value type)
                FieldInfo dcbFieldInfo = baseStreamType.GetField("dcb", BindingFlags.NonPublic | BindingFlags.Instance);
                object dcbValue = dcbFieldInfo.GetValue(baseStream);

                EscapeCommFunction(portFileHandle, mode);
            }
            catch (SecurityException) { throw; }
            catch (OutOfMemoryException) { throw; }
            catch (Win32Exception) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("SETXONXoffChars has failed due to incorrect assumptions about System.IO.Ports.SerialStream which is an internal type.", ex);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool EscapeCommFunction(SafeFileHandle hFile, uint dwFunc);
    }
}