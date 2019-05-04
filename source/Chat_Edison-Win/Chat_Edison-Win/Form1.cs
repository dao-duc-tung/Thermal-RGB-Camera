using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Edison_Win
{
    public partial class fClient : Form
    {
        /// <summary>
        /// IP Edison Server List
        /// </summary>
        /// 192.168.42.1 - AP mode in Edison
        /// 192.168.1.117 - Wifi TeamHUST (LinkSys EA4500)
        /// 192.168.4.176 - Wifi cslab (B1-505)
        /// 192.168.2.15 - Ethernet over USB
        const int THERMAL_IMAGE = 0;
        const int RGB_IMAGE = 1;
        const int BOTH_IMAGE = 2;
        int MODE = THERMAL_IMAGE;
        bool AUTO_CAPTURE = true;
        bool AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD = false;
        bool IsPressCaptureButton = false;

        char[] ThermalCmd = { 'A', '#' };
        char[] RGBCmd = { '5', '#' };
        char[] BothCmd = { 'A', '5' };
        const int delayNotice = 300;
        Socket client;
        IPEndPoint ipe;
        Thread ConnectThread, ListenDataThread, CheckSystemFault;
        bool connect = false;
        NotifyIcon notifyIcon = new NotifyIcon();

        public static double temperature = 0;

        int numImage = 0;
        //byte[] ThermalImage = new byte[60 * 80 * 2];
        byte[] ImageBuffer = new byte[32768];

        public fClient()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void fClient_Load(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
            notifyIcon.BalloonTipTitle = "VMIG 2016";

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            cbIPServer.SelectedIndex = 0;
            ImageProcessing.InitIP();
        }

        #region Code for NETWORK

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //ipe = new IPEndPoint(IPAddress.Parse(txtIPServer.Text), Convert.ToInt32(txtPort.Text));
                ipe = new IPEndPoint(IPAddress.Parse(cbIPServer.SelectedItem.ToString()), Convert.ToInt32(txtPort.Text));
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                connect = true;

                ConnectThread = new Thread(new ThreadStart(ConnectServer));
                ConnectThread.IsBackground = true;
                ConnectThread.Start();

                ShowConnect();
            }
            catch (Exception ex)
            {
                notifyIcon.BalloonTipText = "Loi: " + ex.Message;
                notifyIcon.ShowBalloonTip(delayNotice);
            }
        }

        private void ConnectServer()
        {
            while (true)
            {
                try
                {
                    client.Connect(ipe);

                    if (client.Connected)
                    {
                        ListenDataThread = new Thread(ListenData);
                        ListenDataThread.IsBackground = true;
                        ListenDataThread.Start(client);

                        txtMain.AppendText("Connected to " + ipe.Address.ToString());
                        txtMain.AppendText("\n");

                        ConnectThread.Abort();
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

        }

        private bool IsConnect(Socket sk)
        {
            bool part1 = sk.Poll(1000, SelectMode.SelectRead);
            bool part2 = (sk.Available == 0);
            if (part1 && part2)
            {
                return false;
            }
            return true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                connect = false;
                DisposeSocket();
                ConnectThread.Abort();
                ListenDataThread.Abort();
            }
            catch
            {
                connect = false;
                DisposeSocket();
                ConnectThread.Abort();
                //ListenDataThread.Abort();
                notifyIcon.BalloonTipText = "Chua ket noi";
                notifyIcon.ShowBalloonTip(delayNotice);
            }
        }

        private void DisposeSocket()
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                ipe = null;
                client = null;
                ShowConnect();
            }
            catch (Exception)
            {
                ShowConnect();
                ConnectThread.Abort();
                notifyIcon.BalloonTipText = "Chua ket noi";
                notifyIcon.ShowBalloonTip(delayNotice);
            }
        }

        private void ShowConnect()
        {
            if (!connect)
            {
                btnConnect.Text = "Start";
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                //btnSend.Enabled = false;
                cbIPServer.Enabled = true;
                txtPort.Enabled = true;
                //txtMsg.Enabled = false;
            }
            else
            {
                btnConnect.Text = "Connected";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                //btnSend.Enabled = true;
                cbIPServer.Enabled = false;
                txtPort.Enabled = false;
                //txtMsg.Enabled = true;
            }
        }

        private bool SendMsg(byte[] msg)
        {
            try
            {
                if (!IsConnect(client))
                {
                    notifyIcon.BalloonTipText = "Chua ket noi";
                    notifyIcon.ShowBalloonTip(delayNotice);
                    return false;
                }
                else
                {
                    client.Send(msg);
                    txtMain.AppendText("Client: " + Encoding.ASCII.GetString(msg));
                    txtMain.AppendText("\n");
                    txtMain.ScrollToCaret();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (client.Connected)
                {
                    notifyIcon.BalloonTipText = "Loi Send: " + ex.Message;
                    notifyIcon.ShowBalloonTip(delayNotice);
                }
                else
                {
                    notifyIcon.BalloonTipText = "Chua co ket noi voi Server";
                    notifyIcon.ShowBalloonTip(delayNotice);
                }
                return false;
            }
        }

        #endregion

        private void ListenData(object obj)
        {
            while (true)
            {
                try
                {
                    if (client.Connected)
                    {
                        if (AUTO_CAPTURE)
                        {
                            //continue capture
                        }
                        else
                        {
                            //waiting for button Capture thermal or Both image is clicked
                            while (!IsPressCaptureButton) ;
                            IsPressCaptureButton = false;
                        }
                        int mode = MODE;
                        SendCommand(mode);
                        lblMODE.Text = mode.ToString();
                        byte[] nNumByteTemp = new byte[2];
                        int numBytes = 0;
                        client.Receive(nNumByteTemp, 2, SocketFlags.None);
                        numBytes = nNumByteTemp[0] + (nNumByteTemp[1] << 8);
                        int nByte = 0, readByte = 0;
                        do
                        {
                            nByte = client.Receive(ImageBuffer, readByte, numBytes - readByte, SocketFlags.None);
                            readByte += nByte;
                        } while (readByte < numBytes);

                        if (numBytes > 0)
                        {
                            ReceiveFromEdison(numBytes, mode);
                            //CheckSystemFault = new Thread(new ThreadStart(IsSystemFault));
                            //CheckSystemFault.IsBackground = true;
                            //CheckSystemFault.Start();
                        }
                        else
                        {
                            connect = false;
                            DisposeSocket();
                            ListenDataThread.Abort();
                        }
                    }
                    else
                    {
                        connect = false;
                        DisposeSocket();
                        ListenDataThread.Abort();
                    }
                }
                catch (Exception ex)
                {
                    connect = false;
                    DisposeSocket();

                    notifyIcon.BalloonTipText = "Mat ket noi: " + ex.Message;
                    notifyIcon.ShowBalloonTip(delayNotice);

                    ListenDataThread.Abort();
                }

            }
        }

        private bool ReceiveFromEdison(int numBytes, int mode)
        {
            bool done = false;
            if (mode == THERMAL_IMAGE)
            {
                if (!ImageProcessing.CheckThermalImage(ImageBuffer))
                {
                    txtMain.AppendText("Failed to load Thermal Image");
                    txtMain.AppendText("\n");
                    txtMain.ScrollToCaret();
                    Thread.Sleep(500);
                    return false;
                }
                done = true;
            }
            else if (mode == RGB_IMAGE)
            {
                //received JPG image
                //using OpenCV to encoding
                if (!ImageProcessing.CheckRGBImage(ImageBuffer, numBytes))
                {
                    txtMain.AppendText("Failed to load RGB Image");
                    txtMain.AppendText("\n");
                    txtMain.ScrollToCaret();
                    Thread.Sleep(500);
                    return false;
                }
                done = true;
            }
            else if (mode == BOTH_IMAGE)
            {
                //received both thermal and rgb image
                //do the mixed image algorithm
                //display

                done = true;
            }

            if (done)
            {
                DisplayImage(numBytes, mode);
                txtMain.AppendText("Server: " + (++numImage).ToString());
                txtMain.AppendText("\n");
                txtMain.ScrollToCaret();
            }

            return done;
        }

        private void DisplayImage(int numBytes, int mode)
        {
            if (mode == THERMAL_IMAGE)
            {
                byte[] zoomImg = ImageProcessing.ZoomIn();
                Bitmap tempbmp = ImageProcessing.CreateBitmapFromBytes(zoomImg, ImageProcessing.NEW_WIDTH, ImageProcessing.NEW_HEIGHT);
                pictureBox1.Image = tempbmp;
                lblTemperature.Text = CalTemperature().ToString("F2");
                //btnNote.Text = ImageProcessing.MaxVal.ToString();
            }
            else if (mode == RGB_IMAGE)
            {
                Mat tempMat = ImageProcessing.UndistortImage();
                pictureBox3.Image = tempMat.Bitmap;
            }
            else if (mode == BOTH_IMAGE)
            {
                //test
                byte[] thermal = new byte[9600];
                Array.Copy(ImageBuffer, 0, thermal, 0, 9600);
                byte[] rgb = new byte[numBytes - 9600];
                Array.Copy(ImageBuffer, 9600, rgb, 0, numBytes - 9600);
                if (!ImageProcessing.CheckThermalImage(thermal) || !ImageProcessing.CheckRGBImage(rgb, numBytes - 9600))
                {
                    txtMain.AppendText("Failed to load Image");
                    txtMain.AppendText("\n");
                    txtMain.ScrollToCaret();
                }

                byte[] zoomImg = ImageProcessing.ZoomIn();
                Bitmap tempbmp = ImageProcessing.CreateBitmapFromBytes(zoomImg, ImageProcessing.NEW_WIDTH, ImageProcessing.NEW_HEIGHT);
                pictureBox1.Image = tempbmp;
                //Mat tempMat = ImageProcessing.UndistortImage();
                //pictureBox3.Image = tempMat.Bitmap;

                pictureBox3.Image = MixImage.MixBothImage();
                lblTemperature.Text = CalTemperature().ToString("F2");
                //btnNote.Text = ImageProcessing.MaxVal.ToString();
            }
        }

        private double CalTemperature()
        {
            temperature = (((double)(ImageProcessing.MaxVal - 8100) / (double)100 + 36.5 + Convert.ToDouble(numBalanceTemperature.Value)));
            if (temperature < 34) temperature += 2;
            else if (temperature < 35) temperature += 1.2;
            else if (temperature < 36) temperature += 1;
            //else if (temperature <= 35) temperature += 1;
            if (ImageProcessing.MaxVal == 9600) return temperature;
            if (temperature >= Double.Parse(txtTemperatureThreshold.Text))
            {
                lblWarning.Visible = true;
                lblTemperature.ForeColor = Color.Red;
                if (AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD)
                {
                    MODE = 2;
                }
            }
            else
            {
                lblWarning.Visible = false;
                lblTemperature.ForeColor = Color.Blue;
                if (AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD)
                {
                    MODE = 0;
                }
            }
            return temperature;
        }

        private void SendCommand(int mode)
        {
            if(mode == 0)
            {
                if (!SendMsg(Encoding.ASCII.GetBytes(ThermalCmd)))
                {

                }
            }
            else if(mode == 1)
            {
                if (!SendMsg(Encoding.ASCII.GetBytes(RGBCmd)))
                {

                }
            }
            else if(mode == 2)
            {
                if (!SendMsg(Encoding.ASCII.GetBytes(BothCmd)))
                {

                }
            }
        }

        private void btnSendThermalCmd_Click(object sender, EventArgs e)
        {
            MODE = 0;
            IsPressCaptureButton = true;    //just use this var when AUTO_CAPTURE = false;
        }

        private void btnSendRGBCmd_Click(object sender, EventArgs e)
        {
            //MODE = 1;
        }

        private void btnSendBothImageCmd_Click(object sender, EventArgs e)
        {
            MODE = 2;
            IsPressCaptureButton = true;    //just use this var when AUTO_CAPTURE = false;
        }

        private void btnAutoCapture_Click(object sender, EventArgs e)
        {
            AUTO_CAPTURE = !AUTO_CAPTURE;
            lblAutoCapture.Text = AUTO_CAPTURE ? "1" : "0";
        }

        private void btnAutoChangeMode_Click(object sender, EventArgs e)
        {
            AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD = !AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD;
            lblAutoMode.Text = AUTO_CHANGE_MODE_WHEN_OVER_THRESHOLD ? "1" : "0";
        }

        private void btnAddIPEdison_Click(object sender, EventArgs e)
        {
            AddIPEdison f = new AddIPEdison(this);
            f.FormBorderStyle = FormBorderStyle.FixedSingle;
            f.Show();
        }

        public void AddIPIntoCb(string ip)
        {
            cbIPServer.Items.Add(ip);
        }

        private void btnNote_Click(object sender, EventArgs e)
        {
            string note =
@"IP of Edison
    192.168.42.1    - AP mode in Edison
    192.168.1.117   - Wifi TeamHUST (LinkSys EA4500)
    192.168.4.176    - Wifi cslab (B1-505)
    192.168.2.15    - Ethernet over USB
Power of Edison
    J2 is the battery connector. The right pin (with the nearby number 2) is GND
Wire of Thermal Camera
    CS:     red     (Connect to pin 10 in Due)
    MOSI:   no wire 
    MISO:   white   (Connect to pin 74 in Due)
    CLK:    yellow  (Connect to pin 76 in Due)
    GND:    black
    VCC:    red with white stripe   (Connect to pin 10 in Due)
Wire of RGB Camera
    Black:  5V
    Green:  GND
    Yellow: RX (Connect to TX3 in Due)
    Red:    TX (Connect to RX3 in Due)
UART btw Edison and Due
    Yellow: TX (Connect to RX1 in Due)
    Red:    RX (Connect to TX1 in Due)
TFT LCD
    Power 3.3V
    Pin RD to 3.3V
    Light to 3.3V
Note about suspending of system
    If PC could not receive image, press Disconnect, and Connect again
    If it still do not work, Reset Due
    If it still do not work, Reset System: Turn off all, turn on Edison first, then Due and Software on PC
";
            MessageBox.Show(note, "NOTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void IsSystemFault()
        {
            int oldValue = numImage;
            Thread.Sleep(8000);
            if (numImage == oldValue)//loi
            {
                try
                {
                    connect = false;
                    DisposeSocket();
                    ConnectThread.Abort();
                    ListenDataThread.Abort();
                }
                catch
                {
                    connect = false;
                    DisposeSocket();
                    ConnectThread.Abort();
                    //ListenDataThread.Abort();
                    notifyIcon.BalloonTipText = "Chua ket noi";
                    notifyIcon.ShowBalloonTip(delayNotice);
                }

                try
                {
                    ipe = new IPEndPoint(IPAddress.Parse(cbIPServer.SelectedItem.ToString()), Convert.ToInt32(txtPort.Text));
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    connect = true;

                    ConnectThread = new Thread(new ThreadStart(ConnectServer));
                    ConnectThread.IsBackground = true;
                    ConnectThread.Start();

                    ShowConnect();
                }
                catch (Exception ex)
                {
                    notifyIcon.BalloonTipText = "Loi: " + ex.Message;
                    notifyIcon.ShowBalloonTip(delayNotice);
                }
            }
            else//khong loi
            {

            }
            CheckSystemFault.Abort();
        }

    }
}
