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
            //创建负责通信的Socket
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            //获得要连接的远程服务器应用程序的IP地址和端口号
            Client.Connect(point);

            ShowMsg("连接成功");
            txtBox_Connect.Text = "已连接";
            //开启一个新的线程不停地接收服务器端发来的消息
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Start();
        }

        public delegate void delegate1(string str);//定义委托

        Socket Client;

        //private void btnConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //创建负责通信的Socket
        //        Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        IPAddress ip = IPAddress.Parse(txtServer.Text);
        //        IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
        //        //获得要连接的远程服务器应用程序的IP地址和端口号
        //        Client.Connect(point);

        //        ShowMsg("连接成功");
        //        txtBox_Connect.Text = "已连接";
        //       //开启一个新的线程不停地接收服务器端发来的消息
        //       Thread th = new Thread(Receive);
        //        th.IsBackground = true;
        //        th.Start();
        //    }
        //    catch { }
        //}

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
                    int r = Client.Receive(buffer);
                    if (r == 0)//未收到任何东西
                    {
                        break;
                    }
                    //第一个字节表示发送的是文字消息
                    else if (buffer[0] == 0)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 1, r-1);
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), "[接收]" + Client.RemoteEndPoint.ToString() + ":" + str);
                    }
                    //第二个字节表示发送的是图片消息
                    else if (buffer[0] == 1)
                    {
                        string savePath;
                        byte[] fileContext = new byte[0];
                        GetSavePathAndFileContextByte(r, buffer, out savePath, out fileContext); //读取从客户端传输过来的文件名、文件内容
                        System.IO.File.WriteAllBytes(savePath, fileContext);//将内容写入到制定的路径。
                        //MessageBox.Show("保存成功");
                        ShowImage(savePath);
                        string fileName = System.IO.Path.GetFileName(savePath);
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), "[接收]" + Client.RemoteEndPoint.ToString() + ":" + "接收的图片或文件：" + fileName);
                    }
                    else if (buffer[0] == 2)
                    {
                        //Shock();
                    }
                }
                catch
                {
                    //MessageBox.Show("你接收的是非法文件或非法消息");
                }
            }
        }

        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        /// <summary>
        /// 客户端显示图片
        /// </summary>
        /// <param name="path"></param>
        private void ShowImage(string path)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()//异步委托，直接更新UI线程
            {
                //string fileName = System.IO.Path.GetFileName(path);
                //Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), "[接收]" + Client.RemoteEndPoint.ToString() + ":" + "接收的图片或文件：" + fileName);
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
            //发送的消息，也放到了str中。
            string str = txtSendMsg.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            List<byte> list = new List<byte>();
            list.Add(0);
            list.AddRange(buffer);
            //将泛型集合转换为数组
            byte[] newBuffer = list.ToArray();
            Client.Send(newBuffer);
            txtSendMsg.Text = "";
            //发送时，也要在消息显示发送的消息。
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), "[发送]" + Client.RemoteEndPoint.ToString() + ":" + str);


        }

        private void btnOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\Users\\Administrator\\Desktop\\Client发";
            //ofd.Title = "请选择要发送的文件";
            //ofd.Filter = "所有文件|*.*";
            ofd.ShowDialog();
            txtFilePath.Text = ofd.FileName;
            ShowImage(txtFilePath.Text);
        }

        private void btnSendImage_Click(object sender, RoutedEventArgs e)
        {
            //获得要发送文件的路径
            string path = txtFilePath.Text;//文件的路径
            string fileName = System.IO.Path.GetFileName(path);
            Byte[] fileNameByte = GetFileNameByte(fileName);//文件名的字节数。
            Byte[] fileContentByte = GetfileContentByte(path);//文件内容。
            Byte[] combomArray = GetCombomFileByte(fileNameByte, fileContentByte);//总发送的内容。
            Client.Send(combomArray, 0, combomArray.Length, SocketFlags.None);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new delegate1(ShowMsg), "[发送]" + Client.RemoteEndPoint.ToString() + ":" + "已经发送图片或文件：" + fileName);
        }

        private Byte[] GetFileNameByte(string SendFileName_str)
        {
            byte[] fileNameByteVar = System.Text.Encoding.UTF8.GetBytes(SendFileName_str);
            return fileNameByteVar;
        }

        private Byte[] GetfileContentByte(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);//读取文件
            Byte[] fileContentByteVar = new Byte[fs.Length];
            try
            {
                fs.Read(fileContentByteVar, 0, fileContentByteVar.Length);
                fs.Close();
                //return fileContentByteVar;
            }
            catch
            {
                MessageBox.Show("获取文件内容字节失败");
                //return fileContentByteVar;
            }
            return fileContentByteVar;

        }

        /// <summary>
        /// 四部分，第一部分为1B，区分发送类型，即指令或文件。二部分为4B，保存文件名所占的字符串长度。三部分，保存文件名的内容。四部分，文件的正式内容。
        /// </summary>
        /// <param name="fileNameByte"></param>
        /// <param name="fileContentByte"></param>
        /// <returns></returns>
        private Byte[] GetCombomFileByte(Byte[] fileNameByte, Byte[] fileContentByte)
        {
            int fileNameLong = fileNameByte.Length;//将文件的长度保存为一个整数。
            byte[] fileNameLongByte = new byte[4];//将整数放在4个字节中传输。
            fileNameLongByte = BitConverter.GetBytes(fileNameLong);//一个int等于四个字节。
            int combomFileLong = 1 + fileNameByte.Length + fileNameLongByte.Length + fileContentByte.Length;//总字节长度
            Byte[] combomFileByteTemple = new byte[combomFileLong];
            for (int i = 0; i < combomFileLong; i++)
            {
                if (i == 0)//区分发送类型，即指令或文件
                {
                    combomFileByteTemple[0] = 1;// 1表示发送文件
                }
                else if ((i > 0) && (i < (fileNameLongByte.Length + 1)))//确认文件名的长度，四个字节保存。
                {
                    combomFileByteTemple[i] = fileNameLongByte[i - 1];
                }
                else if ((i >= (fileNameLongByte.Length + 1)) && (i < (fileNameByte.Length + fileNameLongByte.Length + 1)))//文件名的内容
                {
                    combomFileByteTemple[i] = fileNameByte[i - fileNameLongByte.Length - 1];//从第0号位置读取fileNameByte
                }
                else
                {
                    combomFileByteTemple[i] = fileContentByte[(i - fileNameByte.Length - fileNameLongByte.Length - 1)];//从第0号位置读取fileContentByte
                }
            }
            return combomFileByteTemple;
        }

        /// <summary>
        /// 获取文件路径path(path包含了文件夹、文件名、文明类型)、文件内容
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="path"></param>
        /// <param name="fileContext"></param>
        private void GetSavePathAndFileContextByte(int r, Byte[] fs, out string savePath, out byte[] fileContext)
        {
            string folderStr = "C:\\Users\\lanmage2\\Desktop\\Client收\\";//文件夹的位置
            string fileNameStr;//从客户端传输过来的文件名
            int fileNameLong = 0;
            int fileLong = 0;
            fileLong = r;
            byte[] fileNameLongByte = new byte[4];//固定四个字节，用来保存名字的长度。
            byte[] fileNameByte = new byte[0];//初始化为0
            byte[] fileContextByte = new byte[0];//初始化为0
            int count = 0;
            //while (count < fs.Length)//花费太长时间，必须修改。
            while (count < fileLong)
            {
                if (count == 0)
                {
                    //不做任何处理,方便逻辑的理解
                }
                else if ((count > 0) && (count < 4 + 1))
                {
                    fileNameLongByte[count - 1] = fs[count];
                    fileNameLong = BitConverter.ToInt32(fileNameLongByte, 0);//将字节转化为整数，该整数位文件名的长度。
                    fileNameByte = new byte[fileNameLong];
                    //fileContextByte = new byte[fs.Length - 1 - 4 - fileNameByte.Length];
                    fileContextByte = new byte[fileLong - 1 - 4 - fileNameByte.Length];
                }
                else if ((count >= (4 + 1)) && (count < (fileNameByte.Length + 4 + 1)))
                {

                    fileNameByte[count - 4 - 1] = fs[count];
                }
                else
                {
                    fileContextByte[count - (fileNameByte.Length + 4 + 1)] = fs[count];
                }
                count++;
            }
            fileNameStr = Encoding.UTF8.GetString(fileNameByte);//将字节数组转为字符串
            savePath = folderStr + fileNameStr;
            fileContext = fileContextByte;
        }

        private void BtnSysShutDown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
