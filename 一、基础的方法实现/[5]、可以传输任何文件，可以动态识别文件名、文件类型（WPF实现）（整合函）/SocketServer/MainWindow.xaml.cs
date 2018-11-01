using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
         public MainWindow()
        {
            InitializeComponent();
            this.TxtBox_ReceivefileStatus.Text = "0/100";
            this.btn_StartServer.Content = "开启器服务器";
           
        }

        private void btn_StartServer_Click(object sender, RoutedEventArgs e)
        {
            this.btn_StartServer.Content = "监听中...";
            //点击后改变背景色
            this.btn_StartServer.Background = System.Windows.Media.Brushes.Red;
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }

        private void ReceiveMessage()
        {
            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            string path;//设置文件保存的路径，该路径包含了文件名、文件夹。
            byte[] fileContext = new byte[0];//以字节的形式，保存文件内容。
            byte[] b = new byte[1024]; //设置接收数据缓冲区的大小
            receiveSocket.Bind(hostIpEndPoint);
            receiveSocket.Listen(2);   //监听
            Socket hostSocket = receiveSocket.Accept();    //接受客户端连接
            MemoryStream fs = new MemoryStream();//内存流fs的初始容量大小为0，随着数据增长而扩展。
            int length = 0;
            this.Dispatcher.Invoke((Action)delegate ()
               {
                   while ((length = hostSocket.Receive(b)) > 0)  //每接受一次，只能读取小于等于缓冲区的大小1024个字节
                    {
                        fs.Write(b, 0, length);   //将接受到的数据b，按长度length放到内存流中。
                        if (progressBar_Rec.Value < 100)
                        {
                            //进度条的默认值为0
                            progressBar_Rec.Value++;
                            TxtBox_ReceivefileStatus.Text = "接收:" + progressBar_Rec.Value + "/100";
                        }
                    }
                    progressBar_Rec.Value = 100;
                    TxtBox_ReceivefileStatus.Text = "接收:" + progressBar_Rec.Value + "/100";
                }
            );
            fs.Flush();
            fs.Seek(0, SeekOrigin.Begin);
            GetPathAndFileContextByte(fs, out path, out fileContext); //读取从客户端传输过来的文件名、文件内容
            System.IO.File.WriteAllBytes(path, fileContext);//将文件内容写到该路径上
            fs.Close();
            hostSocket.Shutdown(SocketShutdown.Receive);
            hostSocket.Close();
            receiveSocket.Close();
        }
        /// <summary>
        /// 获取文件路径（包含了文件名与文件夹）、以及文件内容
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="path"></param>
        /// <param name="fileContext"></param>
        private void GetPathAndFileContextByte(MemoryStream fs, out string path, out byte[] fileContext)
        {
            string folderStr = "C:\\Users\\lanmage2\\Desktop\\AA\\";//文件夹的位置
            string fileNameStr;//从客户端传输过来的文件名
            int fileNameLong = 0;
            byte[] fileNameLongByte = new byte[4];
            byte[] fileNameByte = new byte[0];
            byte[] fileContextByte = new byte[0];
            int count = 0;
            while (count < fs.Length)
            {
                if (count < 4)
                {
                    fileNameLongByte[count] = Convert.ToByte(fs.ReadByte());
                    fileNameLong = BitConverter.ToInt32(fileNameLongByte, 0);//将字节转化为整数，该整数位文件名的长度。
                    fileNameByte = new byte[fileNameLong];
                    fileContextByte = new byte[fs.Length - 4 - fileNameByte.Length];
                }
                else if( (count >= 4) && (count < (fileNameByte.Length + 4)) )
                {
                    fileNameByte[count - 4] = Convert.ToByte(fs.ReadByte());//从内存流中，读取文件名放到字节数组中。
                }
                else
                {
                    fileContextByte[count - (fileNameByte.Length + 4) ] = Convert.ToByte(fs.ReadByte());//从内存流中，读取文件内容放到字节数组中。
                }
                count++;
            }
            fileNameStr = Encoding.UTF8.GetString(fileNameByte);//将字节数组转为字符串
            path = folderStr + fileNameStr;
            fileContext = fileContextByte;
        }
    }
}
