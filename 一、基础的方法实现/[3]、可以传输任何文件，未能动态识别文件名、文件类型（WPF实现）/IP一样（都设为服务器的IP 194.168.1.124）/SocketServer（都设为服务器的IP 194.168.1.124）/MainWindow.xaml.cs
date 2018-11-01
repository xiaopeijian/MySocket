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
            IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("194.168.1.124"), 8888);
            //设置接收数据缓冲区的大小
            byte[] b = new byte[10];
            receiveSocket.Bind(hostIpEndPoint);
            //监听
            receiveSocket.Listen(2);
            //接受客户端连接
            Socket hostSocket = receiveSocket.Accept();
            //内存流fs的初始容量大小为0，随着数据增长而扩展。
            MemoryStream fs = new MemoryStream();
            int length = 0;
           
            this.Dispatcher.Invoke((Action)delegate ()
               { 
                   //每接受一次，只能读取小于等于缓冲区的大小4096个字节
                   while ((length = hostSocket.Receive(b)) > 0)
                    {
                        //将接受到的数据b，按长度length放到内存流中。
                        fs.Write(b, 0, length);

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
            byte[] byteArray = new byte[fs.Length];
            int count = 0;
            while (count < fs.Length)
            {
                byteArray[count] = Convert.ToByte(fs.ReadByte());
                count++;
            }
            string Path = "C:\\Users\\lanmage2\\Desktop\\AA\\文件1.txt";
            //FileStream filestream = new FileStream(Path + "\\文件1.txt", FileMode.OpenOrCreate);
            //FileStream filestream = File.Create(Path);
            //filestream.Write(byteArray, 0, byteArray.Length);//不能用
            System.IO.File.WriteAllBytes(Path, byteArray);//能用

            /*Bitmap类，可以将*/
            //Bitmap Img = new Bitmap(fs);
            //Img.Save(@"reveive.jpg", ImageFormat.Png);
            //关闭写文件流
            fs.Close();
            //关闭接收数据的Socket
            hostSocket.Shutdown(SocketShutdown.Receive);
            hostSocket.Close();
            //关闭发送连接
            receiveSocket.Close();
        } 
    }
}
