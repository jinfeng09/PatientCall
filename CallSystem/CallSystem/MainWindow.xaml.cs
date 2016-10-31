using CallSystem.Class;
using Pharos.POS.Retailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CallSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void MyDelegate1(string text1);
        private delegate void MyDelegate2(string text2);

        public DBClass.DBMethod PDM;
        private bool IsBtn=true;
        System.ComponentModel.BackgroundWorker BGW_R;
        System.ComponentModel.BackgroundWorker BGW_L;
        private NetworkStream Lstream;
        private NetworkStream Rstream;
        private string Ldata;
        private string Rdata;
        private volatile bool LDo;
        private volatile bool RDo;
        private readonly object LlockObj = new object();
        private readonly object RlockObj = new object();

        private bool IsLDo
        {
            get
            {
                lock (this.LlockObj)
                    return this.LDo;
            }
            set
            {
                lock (this.LlockObj)
                    this.LDo = value;
            }
        }

        private bool IsRDo
        {
            get
            {
                lock (this.RlockObj)
                    return this.RDo;
            }
            set
            {
                lock (this.RlockObj)
                    this.RDo = value;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Config.LoadConfig();
            MultipScreenManager.ShowInScreen(this);
            this.WindowState = WindowState.Maximized;
            PDM = new DBClass.DBMethod();
            this.TbxLeftFont.Text = Config.FontLeft;
            this.TbxRightFont.Text = Config.FontRight;
        }

        void BGW_R_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IP_PortSetting ipPortSetting = new IP_PortSetting();
            ConConnection conConnection = new ConConnection();
            CallSystem.Class.Socket socket = new CallSystem.Class.Socket();
            try
            {
               // int port = (int)e.Argument;
                int port = Config.Rport;
                PublicClass.PortRight = port.ToString();
                TcpListener server = ipPortSetting.IP_PortSet(Config.IPAddress, port);
                while (true)
                {
                    using (this.Rstream = conConnection.ConConne(server,txtMsg))
                    {
                        this.Rdata = socket.Rsocket(this.Rstream, Config .WriteEncoder,txtMsg);
                        if (!this.IsRDo)
                        {
                            if (!string.IsNullOrWhiteSpace(this.Rdata))
                            {
                                this.IsRDo = true;
                                socket.Responsesocket(this.Rstream, Config.WriteEncoder);
                                if (this.Rdata.Split('_').Length == 2)
                                    PublicClass.RReceptionNo = this.Rdata.Split('_')[0];
                                if (this.Rdata.Contains("False"))
                                {
                                    DBClass.ListValue.RInfo = false;
                                   Thread.Sleep(Config .Delay);
                                }
                                if (this.Rdata.Contains("True"))
                                {
                                    DBClass.ListValue.RInfo = true;
                                }
                                this.R_ReceptionistNumber();
                                this.Rdata = string.Empty;
                                this.IsRDo = false;
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
               // if (!this.LErorrLog)
               //     return;
               //// Logger.Write(LogPriority.High, "例外", (object)ex);
               // this.LErorrLog = false;
                output(ex.ToString() + "\r\n");

            }

        }
        #region 调用输出文本
        private delegate void outputDelegate(string msg);
        private void output(string msg)
        {
            this.txtMsg.Dispatcher.Invoke(new outputDelegate(outputAction), msg);
        }

        private void outputAction(string msg)
        {
            this.txtMsg.AppendText(msg);
            this.txtMsg.AppendText("\n");
        }
        #endregion

        private void R_ReceptionistNumber()
        {
            IP_PortSetting ipPortSetting = new IP_PortSetting();
            ConConnection conConnection = new ConConnection();
            CallSystem.Class.Socket socket = new CallSystem.Class.Socket();
            try
            {
                //Logger.Write(LogPriority.High, "トレ\x30FCス", "採血２の受付番号表示", (object)"呼出音操作前");
                try
                {
                    this.Dispatcher.Invoke((Delegate)new MyDelegate1(this.RAddText), (object)PublicClass.RReceptionNo);
                }
                catch(Exception ex) { MessageBox.Show(ex.ToString()); }
                int reshowflog = 0;
                if (DBClass.ListValue.RInfo == false)
                {
                    reshowflog = 0;
                }
                if (DBClass.ListValue.RInfo == true)
                {
                    reshowflog = 1;
                }
                PDM.UpdateShowInfo(PublicClass.RReceptionNo, Config.RName, reshowflog.ToString());
                if (DBClass.ListValue.IsYes == false)
                {
                    MessageBox.Show("更新失败");
                }
                conConnection.conconne();
                //this.backgroundWorker2.RunWorkerAsync();
                //this.LErorrLog = true;
            }
            catch (Exception ex)
            {
                //if (!this.LErorrLog)
                //    return;
                //Logger.Write(LogPriority.High, "例外", (object)ex);
                //this.LErorrLog = false;
                output(ex.ToString() + "\r\n");
            }
        }

        void BGW_L_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IP_PortSetting ipPortSetting = new IP_PortSetting();
            ConConnection conConnection = new ConConnection();
            CallSystem.Class.Socket socket = new CallSystem.Class.Socket();
            try
            {
               // int port = (int)e.Argument;
                int port = Config.Lport;
                PublicClass.PortLeft = port.ToString();
                TcpListener server = ipPortSetting.IP_PortSet(Config.IPAddress, port);
                while (true)
                {
                    using (this.Lstream = conConnection.ConConne(server, txtMsg))
                    {
                        this.Ldata = socket.Lsocket(this.Lstream, Config.WriteEncoder, txtMsg);
                        if (!this.IsLDo)
                        {
                            if (!string.IsNullOrWhiteSpace(this.Ldata))
                            {
                                this.IsLDo = true;
                                socket.Responsesocket(this.Lstream, Config.WriteEncoder);
                                if (this.Ldata.Split('_').Length == 2)
                                    PublicClass.LReceptionNo = this.Ldata.Split('_')[0];
                                if (this.Ldata.Contains("False"))
                                {
                                    DBClass.ListValue.LInfo = false;
                                    Thread.Sleep(Config.Delay);
                                }
                                if (this.Ldata.Contains("True"))
                                {
                                    DBClass.ListValue.LInfo = true;
                                }
                                this.L_ReceptionistNumber();
                                this.Ldata = string.Empty;
                                this.IsLDo = false;
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                // if (!this.LErorrLog)
                //     return;
                //// Logger.Write(LogPriority.High, "例外", (object)ex);
                // this.LErorrLog = false;
                output(ex.ToString() + "\r\n");
            }
        }
        public void L_ReceptionistNumber()
        {
            IP_PortSetting ipPortSetting = new IP_PortSetting();
            ConConnection conConnection = new ConConnection();
             CallSystem.Class.Socket socket = new  CallSystem.Class.Socket();
            try
            {
                //Logger.Write(LogPriority.High, "トレ\x30FCス", "採血１の受付番号表示", (object)"呼出音操作前");
                this.Dispatcher.Invoke((Delegate)new MyDelegate2(this.LAddText), (object)PublicClass.LReceptionNo);
                int reshowflog=0;
                if(DBClass.ListValue.LInfo==false)
                {
                reshowflog=0;
                }
                if(DBClass.ListValue.LInfo==true)
                {
                reshowflog=1;
                }
                PDM.UpdateShowInfo(PublicClass.LReceptionNo, Config.LName, reshowflog.ToString());
                if (DBClass.ListValue.IsYes == false)
                {
                    MessageBox.Show("更新失败");
                }
                conConnection.conconne();
            }
            catch (Exception ex)
            {
                //if (!this.LErorrLog)
                //    return;
                //Logger.Write(LogPriority.High, "例外", (object)ex);
                //this.LErorrLog = false;
                output(ex.ToString() + "\r\n");
            }
        }
        internal void LAddText(string text)
        {
            this.LNumber.Text = text;
            string path = "..\\..\\Voice\\DingDong.wav";
            string path2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Voice\\DingDong.wav";
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path2);
            try
            {
                player.Play();
            }
            catch
            {
                string path3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Voice\\DingDong.wav";
                System.Media.SoundPlayer player3 = new System.Media.SoundPlayer(path3);
                player.Play();
            }
        }

        internal void RAddText(string text)
        {
            this.RNumber.Text = text;
            string path2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Voice\\DingDong.wav";
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path2);
            try
            {
                player.Play();
            }
            catch
            {
                string path3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Voice\\DingDong.wav";
                System.Media.SoundPlayer player3 = new System.Media.SoundPlayer(path3);
                player.Play();
            }
        }

        public void LabelFlicker()
        {
            for (int i = 0; i < 10; i++)
            {
                this.RNumber.FontSize = 80;
                this.RNumber.FontWeight = FontWeights.Thin;
                Thread.Sleep(500);
                this.RNumber.FontSize = 100;
                this.RNumber.FontWeight = FontWeights.Bold;
                Thread.Sleep(500);
            }          
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BGW_L = new System.ComponentModel.BackgroundWorker();
            BGW_R = new System.ComponentModel.BackgroundWorker();
            BGW_L.DoWork += BGW_L_DoWork;
            BGW_R.DoWork += BGW_R_DoWork;
            this.BGW_L.RunWorkerAsync((object)Config.Rport);
            this.BGW_R.RunWorkerAsync((object)Config.Lport);
        }

        private void txtMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtMsg.ScrollToEnd();
        }

        private void btnTxt_Click(object sender, RoutedEventArgs e)
        {
            //PDM.UpdateShowInfo("0113","");
            //txtMsg.Visibility = Visibility.Visible;
            //txtMsg.Visibility = Visibility.Hidden;
            if (IsBtn == true)
            {
                GridMsg.Visibility = Visibility.Visible;
                Gridone.Visibility = Visibility.Collapsed;
                GridTwo.Visibility = Visibility.Collapsed;
                IsBtn = false;
            }
            else
            {
                GridMsg.Visibility = Visibility.Collapsed;
                Gridone.Visibility = Visibility.Visible;
                GridTwo.Visibility = Visibility.Visible;
                IsBtn = true;
            }
        }
    }
}
