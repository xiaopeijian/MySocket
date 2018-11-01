using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void delegate1(string str);//定义委托

        Socket socketSend;

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(txtServer.Text);
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(point);

                ShowMsg("连接成功");
                btnConnect.Content = "已连接";
               //开启一个新的线程不停地接收服务器端发来的消息
               Thread th = new Thread(Receive);
                th.IsBackground = true;
                th.Start();
            }
            catch { }
        }

        /// <summary>
        /// 不停地接收服务器发来的消息
        /// </summary>
        void Receive()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    //实际接收到的有效字节数
                    int r = socketSend.Receive(buffer);
                    //
                    if (r == 0)
                    {
                        break;
                    }
                    //第一个字节表示发送的是文字消息
                    if (buffer[0] == 0)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 1, r - 1);
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), socketSend.RemoteEndPoint.ToString() + ":" + str);
                    }
                    //第二个字节表示发送的是图片消息
                    else if (buffer[0] == 1)
                    {

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.InitialDirectory = @"C:\Users\Administrator\Desktop";
                        sfd.Title = "请选择要保存的文件";
                        sfd.Filter = "所有文件|*.*";
                        sfd.ShowDialog();
                        string path = sfd.FileName;
                        //建立一个文件流，将buffer按字节保存到path中。其中，path包含了文件目录、文件名、文件属性
                        using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            //必须从第二个位置开始读写。
                            fsWrite.Write(buffer, 1, r - 1);
                        }
                        MessageBox.Show("保存图片成功");
                        DisPlay(path);
                    }
                    else if (buffer[0] == 2)
                    {
                        //Shock();
                    }
                }
                catch
                {
                    MessageBox.Show("你发送的是非法文件或非法消息");
                }
            }
        }

        /// <summary>
        /// 震动
        /// </summary>
        void Shock()
        {
            for (int i = 0; i < 500; i++)
            {

            }
        }

        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
            MessageBox.Show("客户端刚收到的消息是：" + str);
        }

        /// <summary>
        /// 客户端显示图片
        /// </summary>
        /// <param name="path"></param>
        private void DisPlay(string path)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()//异步委托，直接更新UI线程
            {
                //TxtBox_ReceiveFilePath.Text = path;
                //string ReceiveFileName_str = System.IO.Path.GetFileName(path);
                //TxtBox_ReceivefileName.Text = ReceiveFileName_str;
                string ReceiveFileNameProperty_str = System.IO.Path.GetExtension(path);    //获取后缀。
                //必须判断，是图片执行，并显示。
                if ((ReceiveFileNameProperty_str == ".jpg")
                    || (ReceiveFileNameProperty_str == ".png")
                    || (ReceiveFileNameProperty_str == ".bmp")
                    || (ReceiveFileNameProperty_str == ".jpeg")
                    || (ReceiveFileNameProperty_str == ".JPG")
                    || (ReceiveFileNameProperty_str == ".PNG")
                    || (ReceiveFileNameProperty_str == ".BMP")
                    || (ReceiveFileNameProperty_str == ".JPEG"))
                {
                    Image_PlayImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));//绝对路径
                }
                //btn_StartServer.Content = "接收完成";
            }
          );
        }

        /// <summary>
        /// 客户端给服务器发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void btnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            string str = txtSendMsg.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            socketSend.Send(buffer);
        }

        private void btnOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\Administrator\Desktop";
            ofd.Title = "请选择要发送的文件";
            ofd.Filter = "所有文件|*.*";
            ofd.ShowDialog();
            txtFilePath.Text = ofd.FileName;
            DisPlay(txtFilePath.Text);
        }

        private void btnSendImage_Click(object sender, RoutedEventArgs e)
        {
            //获得要发送文件的路径
            string path = txtFilePath.Text;
            using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                int r = fsRead.Read(buffer, 0, buffer.Length);
                List<byte> list = new List<byte>();
                list.Add(1);
                list.AddRange(buffer);
                byte[] newBuffer = list.ToArray();
                //socketSend.Send(buffer);
                socketSend.Send(newBuffer, 0, r + 1, SocketFlags.None);
            }

        }
    }
}
